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
					Console.WriteLine ("Creating test publisher");
					using (Publisher<test_msgs.msg.Dummy> test_pub = new Publisher<test_msgs.msg.Dummy> (test_node, "TestTopic", rmw_qos_profile_t.rmw_qos_profile_sensor_data)) {
						Console.WriteLine ("Creating test subscription");
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

							Console.Write("string Array: ");
							foreach (var arritem in e.Message.thisisastringarray) {
								Console.WriteLine(arritem + " ,");
							}
							Console.WriteLine();
							Console.WriteLine ("Seconds: " + e.Message.thisisatime.sec);

						};
						Console.WriteLine ("Creating executor");
						Executor executor = new SingleThreadedExecutor ();
						executor.AddNode (test_node);
						Console.WriteLine ("Spinning");
						executor.Spin (new TimeSpan (0, 0, 0, 0, 10));
						Console.WriteLine ("Creating new test_msgs.Dummy");
						using (test_msgs.msg.Dummy test_msg = new test_msgs.msg.Dummy ()) {
							
							test_msg.thisiauint8 = 10;
							test_msg.thisisabool = 1;
							test_msg.thisisaint16 = 15;
							test_msg.thisisfloat64 = 10.0f;
							test_msg.thisisastring = "test_test_test";
							test_msg.thisisanotherstring = "test2";
							test_msg.thisafloat32array = new float[]{ 10.0f, 1.1f };

							test_msg.thisisastringarray =  (new string[] {
								"test1",
								"test2",
								"test3"
							});
							test_msg.thisisaint8array = (new byte[]{ 100, 102, 200 });
							test_msg.thisisfloat64array = new double[]{ 10.4, 100.1, 100.10 };
							test_msg.thisisatime.sec = 10;

							Console.WriteLine ("Test seconds: "+ test_msg.thisisatime.sec);
							Console.WriteLine ("Test_2 seconds: " + test_msg.Data.thisisatime.sec);
							Console.WriteLine ("Publishing message");
							if (test_pub.Publish (test_msg)) {
								Console.WriteLine ("Publish was successfull");
							}

						
						}

						Console.WriteLine ("####################################################");
						Console.WriteLine ("Creating service");
						Service<test_msgs.srv.DummySrv_Request,test_msgs.srv.DummySrv_Response> test_service = test_node.CreateService<test_msgs.srv.DummySrv_Request,test_msgs.srv.DummySrv_Response> ("TestService");
						test_service.RequestRecieved += (object sender, ServiceRecievedRequestEventArgs<test_msgs.srv.DummySrv_Request,test_msgs.srv.DummySrv_Response> e) => {
							Console.WriteLine ("Recieved new request");
							using(test_msgs.srv.DummySrv_Response response = new test_msgs.srv.DummySrv_Response())
							{
								response.sum = e.Request.a + e.Request.b;
								e.SendResponseFunc(response);
							}

						};
						Client<test_msgs.srv.DummySrv_Request,test_msgs.srv.DummySrv_Response> test_client = test_node.CreateClient<test_msgs.srv.DummySrv_Request,test_msgs.srv.DummySrv_Response> ("TestService");
						test_client.RecievedResponse += (object sender, ClientRecievedResponseEventArgs<test_msgs.srv.DummySrv_Response> e) => {
							Console.WriteLine ("Client recived response : sum:  " + e.Response.sum);
						};
						using (test_msgs.srv.DummySrv_Request testRequest = new test_msgs.srv.DummySrv_Request ()) {
							testRequest.a = 10;
							testRequest.b = 100;
							test_client.SendRequest (testRequest);
						}
						Console.WriteLine ("Press any key to exit");
						Console.ReadKey ();
						executor.Cancel ();

					}

				}

			}
		
		}
	}
}
