using System;
using rclcs;
namespace test_cs
{
	public class ServiceExample
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

						using (Service<test_msgs.srv.DummySrv_Request,test_msgs.srv.DummySrv_Response> DummyService = new Service<test_msgs.srv.DummySrv_Request, test_msgs.srv.DummySrv_Response> (testNode, "TestService")) {
							DummyService.RequestRecieved += (object sender, ServiceRecievedRequestEventArgs<test_msgs.srv.DummySrv_Request, test_msgs.srv.DummySrv_Response> e) => 
							{
								Console.WriteLine("Test service recieved request");
								using(test_msgs.srv.DummySrv_Response response = new test_msgs.srv.DummySrv_Response())
								{
									e.SendResponseFunc( response);
								}
							};
						}

						//Keeps app from closing instantly
						Console.ReadKey ();
					}
					Console.WriteLine ("Disposed node");
				}


			}
			//rcl_shutdown gets called automatically
		}
	}
	public class ClientExample
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

						using (Client<test_msgs.srv.DummySrv_Request,test_msgs.srv.DummySrv_Response> DummyClient = new Client<test_msgs.srv.DummySrv_Request, test_msgs.srv.DummySrv_Response> (testNode, "TestService")) {
							DummyClient.RecievedResponse += (object sender, ClientRecievedResponseEventArgs<test_msgs.srv.DummySrv_Response> e) => 
							{
								Console.WriteLine("Dummy client recieved response");
							};
							using (test_msgs.srv.DummySrv_Request testRequest = new test_msgs.srv.DummySrv_Request ()) {
								testRequest.a = 10;
								testRequest.b = 100;
								DummyClient.SendRequest (testRequest);
							}
						}

						//Keeps app from closing instantly
						Console.ReadKey ();
					}
					Console.WriteLine ("Disposed node");
				}


			}
			//rcl_shutdown gets called automatically
		}
	}
}

