using System;
using System.Reflection;
using System.IO;
namespace test_cs
{
	/// <summary>
	/// Windows assembly finder.
	/// This class is needed to find the rclcs.dll assembly at program start
	/// </summary>
	public static class WindowsAssemblyFinder
	{
		/// <summary>
		/// Registers the assemly load event.
		/// </summary>
		public static void RegisterAssemlyLoadEvent()
		{
			if (!RunningOnWindows())
				return;
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.AssemblyLoad += new AssemblyLoadEventHandler (OnAssemblyLoad);
		}
		private static void OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
		{

		}
		/// <summary>
		/// This event is risen in case the runtime is looking for a certain assembly that isn't found in the assembly cache
		/// By manually handling this event we look in the AMENT_PREFIX_PATH/lib for the dlls we are looking for
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
		{
			string ament_prefix_path_raw = Environment.GetEnvironmentVariable ("AMENT_PREFIX_PATH");
			string[] ament_prefix_paths = ament_prefix_path_raw.Split (new char[]{';'});

			foreach (var item in ament_prefix_paths) {
				string searchPath = Path.Combine (item, "lib");
				if (Directory.Exists (searchPath)) {
					foreach (var file in Directory.GetFiles(searchPath)) {
						if (Path.GetFileName (file) == args.Name) {
							return Assembly.LoadFrom (file);
						}
					}
				}
			}
			throw new DllNotFoundException ("Could not find assembly with name: " + args.Name);
		}
		/// <summary>
		/// Determines if the application is running on windows (By this it means not running under mono)
		/// </summary>
		/// <returns><c>true</c>, if running on windows, <c>false</c> otherwise.</returns>
		public static bool RunningOnWindows()
		{
			bool isMono = false;
			if (Type.GetType ("Mono.Runtime") != null) {
				isMono = true;
			}
			return !isMono;
		}
	}
}

