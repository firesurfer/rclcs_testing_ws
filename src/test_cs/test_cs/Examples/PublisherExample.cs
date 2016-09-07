using System;
using rclcs;

namespace test_cs
{
	/**
	 * This example shows how to create a publisher
	 */
	public class PublisherExample
	{
		public static void Main (string[] args)
		{
			//Create instance of RCL class which handles functions from rcl.h
			//RCL implements IDisposable so the using statement makes sure rcl_shutdown will be called after usages
			using (RCL rcl = new RCL ()) {
				//Initialise RCL with default allocator
				rcl.Init (args);

				//Create an executor that will task our node
				Executor demoExecutor = new SingleThreadedExecutor ();
				//Let the executor task all nodes with the given timespan
				demoExecutor.Spin (new TimeSpan (0, 0, 0, 0, 10));
				//Create a Node
				using (Node testNode = new Node ("BasicNodeExample")) {
					//Add node to executor
					demoExecutor.AddNode (testNode);

					//Now we're creating a publisher with the Dummy message
					//TODO show alternative to Node.CreatePublisher<T>
					using (Publisher<test_msgs.msg.Dummy> testPublisher = testNode.CreatePublisher<test_msgs.msg.Dummy> ("TestTopic")) {
						//Create a message //TODO let message implement IDisposable
						test_msgs.msg.Dummy testMsg = new test_msgs.msg.Dummy (); 
						//Fill the message fields
						testMsg.thisafloat32 = 0.4f;
						//Fill a string //TODO -> Make strings better usable
						testMsg.thisisastring = new rosidl_generator_c__String ("TestString");
						//Fill an array
						testMsg.thisafloat32array = new rosidl_generator_c__primitive_array_float32 (new float[]{ 1.3f, 100000.4f });

						//And now publish the message
						testPublisher.Publish (testMsg);
						//Free unmanaged memory
						testMsg.Free ();
					}

				}
				//Remember to stop the executor - you could start it again afterwards
				demoExecutor.Cancel ();

			}
			//rcl_shutdown gets called automatically
		}
	}
}

