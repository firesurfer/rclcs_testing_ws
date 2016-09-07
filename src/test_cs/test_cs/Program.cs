using System;
using rclcs;
namespace test_cs
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Init RCL");

			using (RCL rcl = new RCL ()) 
			{
				rcl.Init (args);
			

				Console.WriteLine ("Creating node");
				using (Node test_node = new Node ("test_node")) {
					Console.WriteLine ("Creating test publisher");
					using (Publisher<test_msgs.msg.Dummy> test_pub = new Publisher<test_msgs.msg.Dummy> (test_node, "TestTopic")) {
						Console.WriteLine ("Creating test subscription");
						Subscription<test_msgs.msg.Dummy> test_subscription = test_node.CreateSubscription<test_msgs.msg.Dummy> ("TestTopic");
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
							foreach (var arritem in e.Message.thisisaint8array.Array) {
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
						using(test_msgs.msg.Dummy test_msg = new test_msgs.msg.Dummy())
						{
						//test_msgs.msg.Dummy test_msg = new test_msgs.msg.Dummy ();
						test_msg.thisiauint8 = 10;
						test_msg.thisisabool = 1;
						test_msg.thisisaint16 = 15;
						test_msg.thisisfloat64 = 10.0f;
						test_msg.thisisastring = new rosidl_generator_c__String ("test_test_test");
						test_msg.thisisanotherstring = new rosidl_generator_c__String ("test2");
						test_msg.thisafloat32array = new float[]{ 10.0f, 1.1f };

						test_msg.thisisaint8array = new rosidl_generator_c__primitive_array_int8 (new byte[]{ 100, 102, 200 });
						test_msg.thisisfloat64array =  new double[]{ 10.4, 100.1, 100.10 };
						//test_msg.thisisatime.sec = 10;
						Console.WriteLine ();

						Console.WriteLine ("Publishing message");
						if (test_pub.Publish (test_msg)) {
							Console.WriteLine ("Publish was successfull");
						}

						
						}
						Console.WriteLine ("####################################################");
						/*Console.WriteLine ("Creating service");
						Service<test_msgs.srv.DummySrv_Request,test_msgs.srv.DummySrv_Response> test_service = test_node.CreateService<test_msgs.srv.DummySrv_Request,test_msgs.srv.DummySrv_Response> ("TestService");
						test_service.RequestRecieved += (object sender, ServiceRecievedRequestEventArgs<test_msgs.srv.DummySrv_Request> e) => {
							Console.WriteLine ("Recieved new request");
							test_service.SendResponse (new test_msgs.srv.DummySrv_Response ());
						};
						Client<test_msgs.srv.DummySrv_Request,test_msgs.srv.DummySrv_Response> test_client = test_node.CreateClient<test_msgs.srv.DummySrv_Request,test_msgs.srv.DummySrv_Response> ("TestService");
						test_client.RecievedResponse += (object sender, ClientRecievedResponseEventArgs<test_msgs.srv.DummySrv_Response> e) => {
							Console.WriteLine ("Client recived response");
						};
						test_client.SendRequest (new test_msgs.srv.DummySrv_Request ());*/
						Console.WriteLine ("Press any key to exit");
						Console.ReadKey ();
						executor.Cancel ();

					}

				}

			}
		
		}
	}
}
