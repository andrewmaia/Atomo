//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Reflection.Emit;
//using System.Text;

//namespace Atomo.Data
//{
//    public class GenericFactory
//    {
//        private readonly ModuleBuilder module;

//        public GenericFactory()
//        {
//            AssemblyName name = new AssemblyName("DynamicClasses");
//            AssemblyBuilder assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
//            module = assembly.DefineDynamicModule("Module");

//        }

//        public Type CreateDynamicClass(DynamicProperty[] properties, string Name)
//        {
//            string typeName = "DynamicClass" + Name;
//            TypeBuilder tb = module.DefineType(typeName, TypeAttributes.Class |
//                TypeAttributes.Public, typeof(DynamicClass));
//            FieldInfo[] fields = GenerateProperties(tb, properties);
//            GenerateEquals(tb, fields);
//            GenerateGetHashCode(tb, fields);
//            Type result = tb.CreateType();
//            return result;
//        }

//        static FieldInfo[] GenerateProperties(TypeBuilder tb, DynamicProperty[] properties)
//        {
//            FieldInfo[] fields = new FieldBuilder[properties.Length];
//            for (int i = 0; i < properties.Length; i++)
//            {
//                DynamicProperty dp = properties[i];
//                FieldBuilder fb = tb.DefineField("_" + dp.Name, dp.Type, FieldAttributes.Private);
//                PropertyBuilder pb = tb.DefineProperty(dp.Name, PropertyAttributes.HasDefault, dp.Type, null);
//                MethodBuilder mbGet = tb.DefineMethod("get_" + dp.Name,
//                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
//                    dp.Type, Type.EmptyTypes);
//                ILGenerator genGet = mbGet.GetILGenerator();
//                genGet.Emit(OpCodes.Ldarg_0);
//                genGet.Emit(OpCodes.Ldfld, fb);
//                genGet.Emit(OpCodes.Ret);
//                MethodBuilder mbSet = tb.DefineMethod("set_" + dp.Name,
//                    MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
//                    null, new Type[] { dp.Type });
//                ILGenerator genSet = mbSet.GetILGenerator();
//                genSet.Emit(OpCodes.Ldarg_0);
//                genSet.Emit(OpCodes.Ldarg_1);
//                genSet.Emit(OpCodes.Stfld, fb);
//                genSet.Emit(OpCodes.Ret);
//                pb.SetGetMethod(mbGet);
//                pb.SetSetMethod(mbSet);
//                fields[i] = fb;
//            }
//            return fields;
//        }

//        static void GenerateEquals(TypeBuilder tb, FieldInfo[] fields)
//        {
//            MethodBuilder mb = tb.DefineMethod("Equals",
//                MethodAttributes.Public | MethodAttributes.ReuseSlot |
//                MethodAttributes.Virtual | MethodAttributes.HideBySig,
//                typeof(bool), new Type[] { typeof(object) });
//            ILGenerator gen = mb.GetILGenerator();
//            LocalBuilder other = gen.DeclareLocal(tb);
//            Label next = gen.DefineLabel();
//            gen.Emit(OpCodes.Ldarg_1);
//            gen.Emit(OpCodes.Isinst, tb);
//            gen.Emit(OpCodes.Stloc, other);
//            gen.Emit(OpCodes.Ldloc, other);
//            gen.Emit(OpCodes.Brtrue_S, next);
//            gen.Emit(OpCodes.Ldc_I4_0);
//            gen.Emit(OpCodes.Ret);
//            gen.MarkLabel(next);
//            foreach (FieldInfo field in fields)
//            {
//                Type ft = field.FieldType;
//                Type ct = typeof(EqualityComparer<>).MakeGenericType(ft);
//                next = gen.DefineLabel();
//                gen.EmitCall(OpCodes.Call, ct.GetMethod("get_Default"), null);
//                gen.Emit(OpCodes.Ldarg_0);
//                gen.Emit(OpCodes.Ldfld, field);
//                gen.Emit(OpCodes.Ldloc, other);
//                gen.Emit(OpCodes.Ldfld, field);
//                gen.EmitCall(OpCodes.Callvirt, ct.GetMethod("Equals", new Type[] { ft, ft }), null);
//                gen.Emit(OpCodes.Brtrue_S, next);
//                gen.Emit(OpCodes.Ldc_I4_0);
//                gen.Emit(OpCodes.Ret);
//                gen.MarkLabel(next);
//            }
//            gen.Emit(OpCodes.Ldc_I4_1);
//            gen.Emit(OpCodes.Ret);
//        }

//        static void GenerateGetHashCode(TypeBuilder tb, FieldInfo[] fields)
//        {
//            MethodBuilder mb = tb.DefineMethod("GetHashCode",
//                MethodAttributes.Public | MethodAttributes.ReuseSlot |
//                MethodAttributes.Virtual | MethodAttributes.HideBySig,
//                typeof(int), Type.EmptyTypes);
//            ILGenerator gen = mb.GetILGenerator();
//            gen.Emit(OpCodes.Ldc_I4_0);
//            foreach (FieldInfo field in fields)
//            {
//                Type ft = field.FieldType;
//                Type ct = typeof(EqualityComparer<>).MakeGenericType(ft);
//                gen.EmitCall(OpCodes.Call, ct.GetMethod("get_Default"), null);
//                gen.Emit(OpCodes.Ldarg_0);
//                gen.Emit(OpCodes.Ldfld, field);
//                gen.EmitCall(OpCodes.Callvirt, ct.GetMethod("GetHashCode", new Type[] { ft }), null);
//                gen.Emit(OpCodes.Xor);
//            }
//            gen.Emit(OpCodes.Ret);
//        }

//    }
//    public abstract class DynamicClass
//    {
//        public override string ToString()
//        {
//            PropertyInfo[] props = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
//            StringBuilder sb = new StringBuilder();
//            sb.Append("{");
//            for (int i = 0; i < props.Length; i++)
//            {
//                if (i > 0) sb.Append(", ");
//                sb.Append(props[i].Name);
//                sb.Append("=");
//                sb.Append(props[i].GetValue(this, null));
//            }
//            sb.Append("}");
//            return sb.ToString();
//        }
//    }

//    public class DynamicProperty
//    {
//        private readonly string name;
//        private readonly Type type;

//        public DynamicProperty(string name, Type type)
//        {
//            if (name == null) throw new ArgumentNullException("name");
//            if (type == null) throw new ArgumentNullException("type");
//            this.name = name;
//            this.type = type;
//        }

//        public string Name
//        {
//            get { return name; }
//        }

//        public Type Type
//        {
//            get { return type; }
//        }
//    }
//}