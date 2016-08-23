using System;
using ROS2Sharp;
namespace test_cs
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Init RCL");
			RCL.rcl_init (args.Length, args, Allocator.rcl_get_default_allocator ());
			Console.WriteLine ("Creating node");
			Node test_node = new Node ("test_node");
			Console.WriteLine ("Creating test publisher");
			Publisher<test_msgs.Dummy> test_pub = new Publisher<test_msgs.Dummy> (test_node, "TestTopic");
			Console.WriteLine ("Creating test subscription");
			Subscription<test_msgs.Dummy> test_subscription = test_node.CreateSubscription<test_msgs.Dummy> ("TestTopic");
			test_subscription.MessageRecieved += (object sender, MessageRecievedEventArgs<test_msgs.Dummy> e) => {
				Console.WriteLine("Recieved message on test topic: " + e.Message.thisiauint8);
			};
			Console.WriteLine ("Creating executor");
			Executor executor = new SingleThreadedExecutor ();
			executor.AddNode (test_node);
			Console.WriteLine ("Spinning");
			executor.Spin (new TimeSpan (0, 0, 0, 0, 10));
			Console.WriteLine ("Creating new test_msgs.Dummy");
			test_msgs.Dummy test_msg = new test_msgs.Dummy ();
			test_msg.thisiauint8 = 10;
			//test_msg.thisisastring = new rosidl_generator_c__String ("test");
			//Console.WriteLine (test_msg.thisisastring.Data );

			Console.WriteLine ("Publishing message");
			if (test_pub.Publish (test_msg)) {
				Console.WriteLine ("Publish was successfull");
			}
			Console.WriteLine ("Press any key to exit");
			Console.ReadKey ();
			executor.Cancel ();

			RCL.rcl_shutdown ();

		}
	}
}
