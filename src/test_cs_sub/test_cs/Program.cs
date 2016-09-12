using System;
using rclcs;

namespace test_cs
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Init RCL");

			using (RCL rcl = new RCL ()) {
				rcl.Init (args);
			

				Console.WriteLine ("Creating node");
				using (Node test_node = new Node ("test_node")) {
					
						Subscription<test_msgs.msg.Dummy> test_subscription = test_node.CreateSubscription<test_msgs.msg.Dummy> ("TestTopic",rmw_qos_profile_t.rmw_qos_profile_sensor_data);
						test_subscription.MessageRecieved += (object sender, MessageRecievedEventArgs<test_msgs.msg.Dummy> e) => {
							
							Console.WriteLine ("Recieved message on test topic: ");
							foreach (var item in e.Message.GetType().GetProperties()) {
								Console.Write (item.Name + "      :" + item.GetValue (e.Message));
								Console.WriteLine ();

							}
							Console.Write ("Float32 Array: ");
							foreach (var arritem in e.Message.thisafloat32array) {
								Console.Write (arritem + " ,");
							}
							Console.WriteLine ();

							Console.Write ("Double Array: ");
							foreach (var arritem in e.Message.thisisfloat64array) {
								Console.Write (arritem + " ,");
							}
							Console.WriteLine ();

							Console.Write ("int8 Array: ");
							foreach (var arritem in e.Message.thisisaint8array) {
								Console.Write (arritem + " ,");
							}
							Console.WriteLine ();


							Console.WriteLine ("Seconds: " + e.Message.thisisatime.sec);

						};
						Console.WriteLine ("Creating executor");
						Executor executor = new SingleThreadedExecutor ();
						executor.AddNode (test_node);
						Console.WriteLine ("Spinning");
						executor.Spin (new TimeSpan (0, 0, 0, 0, 10));
						Console.WriteLine ("Creating new test_msgs.Dummy");
						
						Console.WriteLine ("Press any key to exit");
						Console.ReadKey ();
						executor.Cancel ();

					}

				}

			}
		
		}
	}

