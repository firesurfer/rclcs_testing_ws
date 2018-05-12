using System;
using rclcs;
namespace test_cs
{
	/**
	 * This example shows how to create a subscription
	 */
	public class SubscriptionExample
	{
		public static void Main (string[] args)
		{
			
			//Create instance of RCL class which handles functions from rcl.h
			//RCL implements IDisposable so the using statement makes sure rcl_shutdown will be called after usages
			using (RCL rcl = new RCL ()) {
				//Initialise RCL with default allocator
				rcl.Init (args);

				//Create an executor that will task our node
				Executor demoExecutor = new SingleThreadedExecutor();
				//Let the executor task all nodes with the given timespan
				demoExecutor.Spin (new TimeSpan (0, 0, 0, 0, 10));
				//Create a Node
				using (Node testNode = new Node ("BasicNodeExample")) {
					//Add node to executor
					demoExecutor.AddNode(testNode);

					//Create subscription on TestTopic
                    using (Subscription<cs_msgs.msg.Dummy> testSub = testNode.CreateSubscription<cs_msgs.msg.Dummy> ("TestTopic")) {
						//Register on MessageRecieved event
                        testSub.MessageRecieved += (object sender, MessageRecievedEventArgs<cs_msgs.msg.Dummy> e) => 
						{
							//Simply print all message items
							foreach (var item in e.Message.GetType().GetFields()) {
								Console.Write (item.Name + "      :" + item.GetValue (e.Message));
								Console.WriteLine ();

							}
						};
						//Call readkey so the program won't close instantly
						Console.ReadKey ();
					}
				}
				//Remember to stop the executor - you could start it again afterwards
				demoExecutor.Cancel ();

			}
			//rcl_shutdown gets called automatically
		}
	}
}

