//using LiteApi.Contracts.Models;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace LiteApi.Services
//{
//    public class CompiledActionInvokerCache
//    {
//        private static readonly Dictionary<Guid, RuntimeCompiledActionInvokerProxy> _cache = new Dictionary<Guid, RuntimeCompiledActionInvokerProxy>();

//        public RuntimeCompiledActionInvokerProxy GetProxy(Guid methodGuid)
//        {
//            if (methodGuid == default(Guid)) throw new ArgumentException($"{nameof(methodGuid)} has default value");

//            if (!_cache.ContainsKey(methodGuid))
//            {
//                throw new KeyNotFoundException();
//            }

//            return _cache[methodGuid];
//        }
        
//        public void GenerateProxiesForControllerConstructors(ControllerContext[] ctrls)
//        {
//            var constructors = new Dictionary<Guid, ConstructorInfo>();
//            foreach (var ctrl in ctrls)
//            {
//                constructors[ctrl.ControllerGuid] = ControllerBuilder.GetConstructor(ctrl.ControllerType);
//            }

//            Assembly assembly = ProxyCompiler.CompileForConstructors(constructors);

//            Type type = assembly.GetTypes().Single(x => x.Name.StartsWith("ProxyCalls", StringComparison.Ordinal));
//            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);

//            foreach (var ctrl in ctrls)
//            {
//                _cache[ctrl.ControllerGuid] = GetProxyForConstructor(ctrl, methods);
//            }
//        }

//        public void GenerateProxiesForActions(ActionContext[] actions)
//        {
//            Dictionary<Guid, MethodInfo> dict = new Dictionary<Guid, MethodInfo>();
//            foreach (ActionContext action in actions)
//            {
//                dict[action.ActionGuid] = action.Method;
//            }
//            Assembly assembly = ProxyCompiler.CompileForMethods(dict);
//            Type type = assembly.GetTypes().Single(x => x.Name.StartsWith("ProxyCalls", StringComparison.Ordinal));
//            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);

//            foreach (ActionContext action in actions)
//            {
//                _cache[action.ActionGuid] = GetProxyForAction(action, methods);
//            }
//        }

//        private RuntimeCompiledActionInvokerProxy GetProxyForConstructor(ControllerContext ctrl, MethodInfo[] methods)
//        {
//            string methodName = $"ProxyCall_{ctrl.ControllerGuid.ToString("N").ToLower()}";
//            var method = methods.Single(x => x.Name == methodName);
//            object proxyCall = method.Invoke(null, new object[0]);
//            return new RuntimeCompiledActionInvokerProxy
//            {
//                IsAsync = false,
//                IsVoid = false,
//                InvokeConstructor = (Func<object[], object>)proxyCall
//            };
//        }

//        private RuntimeCompiledActionInvokerProxy GetProxyForAction(ActionContext action, MethodInfo[] methods)
//        {
//            string methodName = $"ProxyCall_{action.ActionGuid.ToString("N").ToLower()}";
//            var method = methods.Single(x => x.Name == methodName);
//            var retType = method.ReturnType;

//            bool isVoid = retType == typeof(void) || retType == typeof(Task);

//            bool isAsync = retType == typeof(Task);

//            if (!isAsync)
//            {
//                var typeInfo = retType.GetTypeInfo();
//                isAsync = retType.IsConstructedGenericType && retType.GetGenericTypeDefinition() == typeof(Task<>);
//            }

//            var proxy = new RuntimeCompiledActionInvokerProxy
//            {
//                IsAsync = isAsync,
//                IsVoid = isVoid
//            };
//            object proxyCall = method.Invoke(null, new object[0]);
//            if (isAsync)
//            {
//                if (isVoid)
//                {
//                    proxy.InvokeVoidTask = (Func<object, object[], Task>)proxyCall;
//                }
//                else
//                {
//                    proxy.InvokeNotVoidTask = (Func<object, object[], Task<object>>)proxyCall;
//                }
//            }
//            else
//            {
//                if (isVoid)
//                {
//                    proxy.InvokeVoid = (Action<object, object[]>)proxyCall;
//                }
//                else
//                {
//                    proxy.InvokeNotVoid = (Func<object, object[], object>)proxyCall;
//                }
//            }

//            return proxy;
//        }
//    }

//    public class RuntimeCompiledActionInvokerProxy
//    {
//        public bool IsVoid { get; set; }
//        public bool IsAsync { get; set; }
//        public Func<object, object[], Task<object>> InvokeNotVoidTask { get; set; }
//        public Func<object, object[], Task> InvokeVoidTask { get; set; }
//        public Action<object, object[]> InvokeVoid { get; set; }
//        public Func<object, object[], object> InvokeNotVoid { get; set; }
//        public Func<object[], object> InvokeConstructor { get; set; }
//    }
    
//}
