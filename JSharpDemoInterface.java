package CenterCLR;

public interface JSharpDemoInterface
{
	public static final String DemoValue = "Here is J# string in constant!";

	public final class InnerClass
	{
		public static String GetDemoString()
		{
			return "Here is J# string in inner class static method!";
		}
	}
}
