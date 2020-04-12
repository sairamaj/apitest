namespace ApiManager
{
	internal static class ServiceLocator
	{
		public static void Initialize(Wpf.Util.Core.Registration.IServiceLocator locator)
		{
			Locator = locator;
		}

		public static Wpf.Util.Core.Registration.IServiceLocator Locator { get; private set; }
	}
}
