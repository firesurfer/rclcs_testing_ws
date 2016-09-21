using System;
using System.Reflection;
using System.IO;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Runtime.InteropServices;
namespace test_cs
{
	/// <summary>
	/// Windows assembly finder.
	/// This class is needed to find the rclcs.dll assembly at program start
	/// </summary>
	public static class WindowsAssemblyLoader
	{
		//See: https://stackoverflow.com/questions/6089083/currentdomain-assemblyresolve-does-not-fire-when-assembly-is-used-as-a-subclass
		public static void Main(string[] args)
		{
			RegisterAssemlyLoadEvent();
			StartMain(args);
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void StartMain(string[] args)
		{
			test_cs.BasicNodeExampleWithUsing.Main(args);
		}
		/// <summary>
		/// Registers the assemly load event.
		/// </summary>
		public static void RegisterAssemlyLoadEvent()
		{

			if (!RunningOnWindows())
				return;

			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.AssemblyLoad += new AssemblyLoadEventHandler (OnAssemblyLoad);
			currentDomain.AssemblyResolve += OnAssemblyResolve;

			string ament_prefix_path_raw = Environment.GetEnvironmentVariable("AMENT_PREFIX_PATH");
			string[] ament_prefix_paths = ament_prefix_path_raw.Split(new char[] { ';' });
			for (int i = 0; i < ament_prefix_paths.Length; i++)
			{
				ament_prefix_paths[i] = Path.Combine(ament_prefix_paths[i], "bin");
			}            
			//See https://stackoverflow.com/questions/2864673/specify-the-search-path-for-dllimport-in-net
			var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };
			string newPath = string.Join(Path.PathSeparator.ToString(), path.Concat(ament_prefix_paths));
			Environment.SetEnvironmentVariable("PATH", newPath);
			Console.WriteLine(newPath);
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
		public static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
		{
			Console.WriteLine("AssemblyReloveEvent");
			string ament_prefix_path_raw = Environment.GetEnvironmentVariable ("AMENT_PREFIX_PATH");
			string[] ament_prefix_paths = ament_prefix_path_raw.Split (new char[]{';'});

			foreach (var item in ament_prefix_paths) {
				string searchPath = Path.Combine (item, "lib");

				if (Directory.Exists (searchPath)) {
					foreach (var file in Directory.GetFiles(searchPath)) {
						string pureName = args.Name.Split(new char[] { ',' })[0];

						if (Path.GetFileName (file) == pureName + ".dll") {
							Console.WriteLine("Found: " + pureName + " at: " + file);
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

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern bool SetDllDirectory(string lpPathName);
	}
}

