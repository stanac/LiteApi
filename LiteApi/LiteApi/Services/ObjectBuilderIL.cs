using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace LiteApi.Services
{
    public class ObjectBuilderIL: ObjectBuilder
    {
        private static readonly IDictionary<Type, Func<object[], object>> _cache = new ConcurrentDictionary<Type, Func<object[], object>>();

        public ObjectBuilderIL(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override object BuildObject(Type objectType)
        {
            ConstructorInfo ctor = GetConstructor(objectType);
            var parameters = GetConstructorParameterValues(ctor.GetParameters());
            Func<object[], object> delegat = _cache[objectType];
            if (delegat == null)
            {
                delegat = CreateDelegate(ctor);
                _cache[objectType] = delegat;
            }

            return delegat(parameters);
        }

        private Func<object[], object> CreateDelegate(ConstructorInfo ctor)
        {
            // setup
            Type objectType = ctor.DeclaringType;
            Type[] prms = ctor.GetParameters().Select(x => x.ParameterType).ToArray();
            DynamicMethod dm = new DynamicMethod($"DelegateCtor:{objectType.FullName}", objectType, new Type[] { typeof(object[]) }, typeof(ObjectBuilderIL).Module);
            var ilGen = dm.GetILGenerator();

            if (prms.Length > 127)
            {
                throw new ArgumentOutOfRangeException("Only methods/constructors with less than 128 parameters are supported.");
            }

            // cast all parameters and put them on evaluation stack
            void EmitCast(Type type)
            {
                if (type.IsPrimitive)
                {
                    ilGen.Emit(OpCodes.Unbox, type);
                }
                else if (type.IsClass || type.IsInterface)
                {
                    ilGen.Emit(OpCodes.Isinst, type);
                }
                else
                {
                    ilGen.Emit(OpCodes.Unbox_Any, type);
                }
            }

            void LoadValueParamArray(int index)
            {
                ilGen.Emit(OpCodes.Ldarg_0); // load array from parameters

                switch (index)               // set index integer on stack
                {
                    case 0:
                        ilGen.Emit(OpCodes.Ldc_I4_0);
                        break;
                    case 1:
                        ilGen.Emit(OpCodes.Ldc_I4_1);
                        break;
                    case 2:
                        ilGen.Emit(OpCodes.Ldc_I4_2);
                        break;
                    case 3:
                        ilGen.Emit(OpCodes.Ldc_I4_3);
                        break;
                    case 4:
                        ilGen.Emit(OpCodes.Ldc_I4_4);
                        break;
                    case 5:
                        ilGen.Emit(OpCodes.Ldc_I4_5);
                        break;
                    case 6:
                        ilGen.Emit(OpCodes.Ldc_I4_6);
                        break;
                    case 7:
                        ilGen.Emit(OpCodes.Ldc_I4_7);
                        break;
                    case 8:
                        ilGen.Emit(OpCodes.Ldc_I4_8);
                        break;
                    default:
                        ilGen.Emit(OpCodes.Ldc_I4_S, index);
                        break;
                }

                ilGen.Emit(OpCodes.Ldelem_Ref); // load value from index
            }

            for (int i = 0; i < prms.Length; i++)
            {
                LoadValueParamArray(i);
                EmitCast(prms[i]);
            }

            ilGen.Emit(OpCodes.Newobj, ctor);
            ilGen.Emit(OpCodes.Ret);

            return (Func<object[], object>)dm.CreateDelegate(typeof(Func<object[], object>));
        }
    }
}
