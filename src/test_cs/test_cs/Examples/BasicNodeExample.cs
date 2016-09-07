using System;
using rclcs;
namespace test_cs
{
	/**
	 * This basic example demonstrates how to create managed ROS2 node 
	 * and how to spin it. It furthermore demonstrates how to make sure that unmanaged memory will be freed afterwards.
	 * 
	 */ 
	public class BasicNodeExampleWithUsing
	{
		public static void Main (string[] args)
		{
			//Create instance of RCL class which handles functions from rcl.h
			//RCL implements IDisposable so the using statement makes sure rcl_shutdown will be called after usages
			using (RCL rcl = new RCL ()) {
				//Initialise RCL with default allocator
				rcl.Init (args);
				Console.WriteLine ("RCL Init was successfull");
				//Create an executor that will task our node
				using (Executor demoExecutor = new SingleThreadedExecutor ()) {
					//Let the executor task all nodes with the given timespan
					demoExecutor.Spin (new TimeSpan (0, 0, 0, 0, 10));
					//Create a Node
					using (Node testNode = new Node ("BasicNodeExample")) {
						Console.WriteLine ("Creation of node was successfull");
						//Add node to executor
						demoExecutor.AddNode (testNode);

						//Now do some fancy stuff

						//Keeps app from closing instantly
						Console.ReadKey ();
					}
					Console.WriteLine ("Disposed node");
				}


			}
			//rcl_shutdown gets called automatically
		}

	}
	/**
	 * This basic example demonstrates how to create managed ROS2 node 
	 * and how to spin it. It demonstrates how to free memory by hand
	 */
	public class BasicNodeExampleWithManualDispose
	{
		public static void Main (string[] args)
		{
			//Create instance of RCL class which handles functions from rcl.h
			RCL rcl = new RCL();
			//Initialise RCL with default allocator
			rcl.Init (args);

			//Create an executor that will task our node
			Executor demoExecutor = new SingleThreadedExecutor();
			//Let the executor task all nodes with the given timespan
			demoExecutor.Spin (new TimeSpan (0, 0, 0, 0, 10));
			//Create a Node
			Node testNode = new Node ("BasicNodeExample");
			//Add node to executor
			demoExecutor.AddNode(testNode);

			//Do some fancy stuff

			//Dispose the node so there won't be any unmanaged resources
			testNode.Dispose ();
			//Remember to stop the executor - you could start it again afterwards
			demoExecutor.Cancel ();
			//Dispose the rcl object - this will call rcl_shutdown
			rcl.Dispose ();
		}

	}

}

