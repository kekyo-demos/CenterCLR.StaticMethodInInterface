using System;
using System.Reflection;
using System.Reflection.Emit;

namespace CenterCLR.StaticMethodInInterface
{
	class Program
	{
		private static MethodBuilder DefineDemoMethod(TypeBuilder typeBuilder, string methodName, string returnValue)
		{
			// スタティックメソッドを定義
			var methodBuilder = typeBuilder.DefineMethod(
				methodName,
				MethodAttributes.Public | MethodAttributes.Static,
				CallingConventions.Standard,
				typeof(string),
				Type.EmptyTypes);

			// return "<returnValue>";
			var ilGenerator = methodBuilder.GetILGenerator();
			ilGenerator.Emit(OpCodes.Ldstr, returnValue);
			ilGenerator.Emit(OpCodes.Ret);

			return methodBuilder;
		}

		private static PropertyBuilder DefineDemoProperty(TypeBuilder typeBuilder, string propertyName, string returnValue)
		{
			var methodBuilder = DefineDemoMethod(typeBuilder, "get_" + propertyName, returnValue);

			// プロパティを定義
			var propertyBuilder = typeBuilder.DefineProperty(
				propertyName,
				PropertyAttributes.None,
				typeof(string),
				Type.EmptyTypes);

			// スタティックメソッドを割り当て
			propertyBuilder.SetGetMethod(methodBuilder);

			return propertyBuilder;
		}

		private static Type CreateInterfaceWithStaticMethod()
		{
			///////////////////////////////////////////////////////////////////////////////////////////
			// ダイナミックアセンブリとモジュールを生成

			var assemblyName = new AssemblyName("StaticMethodInInterface");
			var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Save);
			var moduleBuilder = assemblyBuilder.DefineDynamicModule("StaticMethodInInterface", "StaticMethodInInterface.dll");

			///////////////////////////////////////////////////////////////////////////////////////////
			// ダイナミックインターフェイスを定義

			var typeBuilder = moduleBuilder.DefineType(
				"CenterCLR.IDemoInterface",
				TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Interface);

			///////////////////////////////////////////////////////////////////////////////////////////
			// ダイナミックインターフェイスにスタティックメソッドを定義

			DefineDemoMethod(typeBuilder, "GetDemoString", "Here is string in interface static method!");

			///////////////////////////////////////////////////////////////////////////////////////////
			// ダイナミックインターフェイスにスタティックプロパティを定義し、メソッドをバインド

			DefineDemoProperty(typeBuilder, "DemoString", "Here is string in interface static property!");

			///////////////////////////////////////////////////////////////////////////////////////////
			// ダイナミックインターフェイスにインナークラスを定義

			var innerClassBuilder = typeBuilder.DefineNestedType(
				"InnerClass",
				TypeAttributes.NestedPublic | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.Class);

			///////////////////////////////////////////////////////////////////////////////////////////
			// インナークラスにスタティックメソッドを定義

			DefineDemoMethod(innerClassBuilder, "GetDemoString", "Here is string in inner class static method!");

			///////////////////////////////////////////////////////////////////////////////////////////
			// インナークラスにスタティックプロパティを定義

			DefineDemoProperty(innerClassBuilder, "DemoString", "Here is string in inner class static property!");

			///////////////////////////////////////////////////////////////////////////////////////////
			// ダイナミックインターフェイスを確定

			innerClassBuilder.CreateType();
			var type = typeBuilder.CreateType();

			///////////////////////////////////////////////////////////////////////////////////////////
			// ダイナミックアセンブリを保存

			assemblyBuilder.Save("StaticMethodInInterface.dll");

			return type;
		}

		static void Main(string[] args)
		{
			var type = CreateInterfaceWithStaticMethod();
		}
	}
}
