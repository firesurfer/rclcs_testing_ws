using System;
using rclcs;
namespace test_cs
{
	public class RCLExample
	{
		public static void Main (string[] args)
		{
			
			//Create instance of RCL class which handles functions from rcl.h
			//RCL implements IDisposable so the using statement makes sure rcl_shutdown will be called after usages
			using (RCL rcl = new RCL ()) {
				//Initialise RCL with default allocator
				rcl.Init (args);
				Console.WriteLine ("RCL init was successfull - Press any key to exit");
				Console.ReadKey ();
			}
			//rcl_shutdown gets called automatically
			Console.WriteLine("If you can read this exit was successfull ");
		}
	}
}

