using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace LiteApi.Services
{
    public static class ProxyCompiler
    {
        public static Assembly CompileForConstructors(Dictionary<Guid, ConstructorInfo> constructors)
        {
            if (constructors == null) throw new ArgumentNullException(nameof(constructors));
            if (constructors.Count == 0) throw new ArgumentException("no method was supplied for compilation", nameof(constructors));


            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"using System;

namespace LiteApi.DynamicallyCompiledProxy
{
public static class ProxyCalls
{
");
            foreach (var ctor in constructors)
            {
                sb.AppendLine(GenerateCallCodeForProxyConstructor(ctor.Key, ctor.Value));
            }

            sb.AppendLine(" }  } ");

            List<Assembly> referencedAssemblies =
                constructors
                .Select(x => x.Value.DeclaringType.GetTypeInfo().Assembly)
                .ToList();

            foreach (Type type in constructors.SelectMany(x => x.Value.GetParameters()).Select(x => x.ParameterType))
            {
                var assembly = type.GetTypeInfo().Assembly;
                referencedAssemblies.Add(assembly);
            }

            referencedAssemblies = referencedAssemblies
                .GroupBy(x => x.FullName)
                .Select(x => x.First())
                .ToList();

            return CompileToAssembly(sb.ToString(), referencedAssemblies);
        }

        private static string GenerateCallCodeForProxyConstructor(Guid controllerGuid, ConstructorInfo ctor)
        {
            string ctorArgs = GenerateParameterValues(ctor.GetParameters(), "args");
            string returnType = GetFullTypeName(ctor.DeclaringType);

            string code = $@"

public static Func<object[], object> ProxyCall_{controllerGuid.ToString("N").ToLower()}()
{{ 
    return new Func<object[], object>((args) => new {ctor.DeclaringType.FullName}({ctorArgs}));
}}

";
            return code;
        }

        public static Assembly CompileForMethods(Dictionary<Guid, MethodInfo> methods)
        {
            if (methods == null) throw new ArgumentNullException(nameof(methods));
            if (methods.Count == 0) throw new ArgumentException("no method was supplied for compilation", nameof(methods));

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"using System;

namespace LiteApi.DynamicallyCompiledProxy
{
public static class ProxyCalls
{
");
            foreach (var method in methods)
            {
                sb.AppendLine(GenerateCallCodeForProxyMethod(method.Key, method.Value));
            }

            sb.AppendLine(" }  } ");

            List<Assembly> referencedAssemblies =
                methods
                .Select(x => x.Value.DeclaringType.GetTypeInfo().Assembly)
                .ToList();

            foreach (Type type in methods.SelectMany(x => x.Value.GetParameters()).Select(x => x.ParameterType))
            {
                var assembly = type.GetTypeInfo().Assembly;
                referencedAssemblies.Add(assembly);
            }

            referencedAssemblies = referencedAssemblies
                .GroupBy(x => x.FullName)
                .Select(x => x.First())
                .ToList();

            return CompileToAssembly(sb.ToString(), referencedAssemblies);
        }

        private static string GenerateCallCodeForProxyMethod(Guid methodGuid, MethodInfo method) 
            => method.ReturnType == typeof(void)
                ? GenerateCodeForVoid(methodGuid, method)
                : GenerateCodeForNotVoid(methodGuid, method);

        private static string GenerateCodeForNotVoid(Guid methodGuid, MethodInfo method)
        {
            string methodArgs = GenerateParameterValues(method.GetParameters(), "args");
            string returnType = GetFullTypeName(method.ReturnType);

            string code = $@"

public static Func<object, object[], object> ProxyCall_{methodGuid.ToString("N").ToLower()}()
{{ 
    return new Func<object, object[], object>((inst, args) => (inst as {method.DeclaringType.FullName}).{method.Name}({methodArgs}));
}}

";
            return code;
        }

        private static string GenerateCodeForVoid(Guid methodGuid, MethodInfo method)
        {
            string methodArgs = GenerateParameterValues(method.GetParameters(), "args");

            string code = $@"

public static Action<object, object[]> ProxyCall_{methodGuid.ToString("N").ToLower()}()
{{
    return new Action(() => (instance as {method.DeclaringType.FullName}).{method.Name}({methodArgs}));
}}

";
            return code;
        }

        //private class TestClass
        //{
        //    public Task Do(int a, int b) { throw new NotImplementedException(); }
        //    public void Do2(int a, int b) { throw new NotImplementedException(); }
        //}

        //public static Action<object, object[]> ProxyCall()
        //{
        //    var a = new Func<object, object[], Task>((inst, args) => (inst as TestClass).Do((int)args[0], (int)args[1]));
        //    return new Action<object, object[]>((inst, args) => (inst as TestClass).Do2((int)args[0], (int)args[1]));
        //}

        private static string GenerateParameterValues(ParameterInfo[] parameters, string parametersArrayName)
        {
            var paramTypes = parameters.Select(x => x.ParameterType).ToArray();
            string[] parameterValues = new string[parameters.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                var paramType = paramTypes[i];
                parameterValues[i] = $"({GetFullTypeName(paramType)}){parametersArrayName}[{i}]";
            }
            return string.Join(", ", parameterValues);
        }

        private static string GetFullTypeName(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (!typeInfo.IsGenericType) return type.FullName;

            var arguments = string.Join(", ", type.GetGenericArguments().Select(GetFullTypeName));
            string name = type.FullName.Substring(0, type.FullName.IndexOf('`'));
            name += $"<{arguments}>";

            return name;
        }

        private static Assembly CompileToAssembly(string code, IEnumerable<Assembly> referencedAssemblies)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

            string file = "_" + Guid.NewGuid().ToString("N").ToLower();
            var dir = Path.GetDirectoryName(typeof(Func<>).GetTypeInfo().Assembly.Location);
            var bclReferences = Directory.GetFiles(dir, "*.dll")
                .Where(x => char.IsUpper(Path.GetFileName(x).First()) && !Path.GetFileName(x).ToLower().StartsWith("api"))
                .Select(x => MetadataReference.CreateFromFile(x))
                .ToList();
            bclReferences.Add(MetadataReference.CreateFromFile(Path.Combine(dir, "mscorlib.dll")));

            MetadataReference[] liteAppiRefs = 
            {
                MetadataReference.CreateFromFile(typeof(LiteController).GetTypeInfo().Assembly.Location)
            };

            var refs =
                referencedAssemblies.Select(x => x.Location)
                .Select(x => MetadataReference.CreateFromFile(x))
                .Union(bclReferences)
                .Union(liteAppiRefs);

            MetadataReference[] references = refs
                .GroupBy(x => x.Display)
                .Select(x => x.First())
                .ToArray();

            CSharpCompilation compilation = CSharpCompilation.Create(file, new[] { syntaxTree }, references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Release)
                );

            using (Stream ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);
                if (result.Success)
                {
                    ms.Position = 0;

                    var assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
                    return assembly;
                }
                var ex = new Exception("failed to compile");
                ex.Data.Add("result", result);
                throw ex;
            }
        }
    }
}
