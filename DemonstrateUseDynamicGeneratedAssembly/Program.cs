using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace CenterCLR.DemonstrateUseDynamicGeneratedAssembly
{
	class Program
	{
		static void Main(string[] args)
		{
			///////////////////////////////////////////////////////////////////////////////////////////
			// スタティックメソッドへのアクセスはコンパイルできない

			// error CS0117: 'CenterCLR.IDemoInterface' に 'GetDemoString' の定義がありません。
			// var result = IDemoInterface.GetDemoString();
			// Debug.Assert(result == "Here is string in interface static method!");


			///////////////////////////////////////////////////////////////////////////////////////////
			// スタティックプロパティへのアクセスはコンパイルできない

			// error CS0117: 'CenterCLR.IDemoInterface' に 'DemoString' の定義がありません。
			// var result = IDemoInterface.DemoString;
			// Debug.Assert(result == "Here is string in interface static property!");


			///////////////////////////////////////////////////////////////////////////////////////////
			// スタティックメソッドへのリフレクションを使ったアクセスは可能

			var type = typeof(IDemoInterface);
			var method = type.GetMethod("GetDemoString");
			var result1 = (string)method.Invoke(null, null);
			Debug.Assert(result1 == "Here is string in interface static method!");


			///////////////////////////////////////////////////////////////////////////////////////////
			// スタティックプロパティへのリフレクションを使ったアクセスは可能

			var property = type.GetProperty("DemoString");
			var result2 = (string)property.GetValue(null);
			Debug.Assert(result2 == "Here is string in interface static property!");


			///////////////////////////////////////////////////////////////////////////////////////////
			// スタティックメソッドからデリゲートを作ることは可能

			var func = (Func<string>)Delegate.CreateDelegate(typeof(Func<string>), method);
			var result3 = func();
			Debug.Assert(result3 == "Here is string in interface static method!");


			///////////////////////////////////////////////////////////////////////////////////////////
			// スタティックメソッドを式木経由で呼び出すデリゲートをコンパイルすることは可能

			var expression = Expression.Lambda<Func<string>>(Expression.Call(null, method));
			var compiled = expression.Compile();
			var result4 = compiled();
			Debug.Assert(result4 == "Here is string in interface static method!");


			///////////////////////////////////////////////////////////////////////////////////////////
			// J#のスタティック定数へのアクセスはコンパイルできない

			// error CS0117: 'JSharpDemo.JSharpDemoInterface' に 'getDemoValue' の定義がありません。
			// var result5 = JSharpDemoInterface.DemoValue;
			// Debug.Assert(result5 == "Here is J# string in constant!");


			///////////////////////////////////////////////////////////////////////////////////////////
			// J#のインナークラスのメソッドへのアクセスは可能

			var result6 = JSharpDemoInterface.InnerClass.GetDemoString();
			Debug.Assert(result6 == "Here is J# string in inner class static method!");


			///////////////////////////////////////////////////////////////////////////////////////////
			// インナークラスのメソッドへのアクセスは可能

			var result7 = IDemoInterface.InnerClass.GetDemoString();
			Debug.Assert(result7 == "Here is string in inner class static method!");


			///////////////////////////////////////////////////////////////////////////////////////////
			// インナークラスのメソッドへのアクセスは可能

			var result8 = IDemoInterface.InnerClass.DemoString;
			Debug.Assert(result8 == "Here is string in inner class static property!");
		}
	}
}
