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
				foreach (var item in e.Message.GetType().GetFields()) {
					Console.WriteLine(item.Name + " " + item.GetValue(e.Message));
				}
				Console.WriteLine("String size,capacity: " + e.Message.thisisastring.Size + ", " + e.Message.thisisastring.Capacity);
				Console.WriteLine("String: " + e.Message.thisisastring.Data);
				Console.WriteLine("Float32 Array size,capacity,pointer: " + e.Message.thisafloat32array.ArraySize+ ", " + e.Message.thisafloat32array.ArrayCapacity + ", " + e.Message.thisafloat32array.Data);
				Console.WriteLine("Int8 Array size,capacity,pointer: " + e.Message.thisisaint8array.ArraySize+ ", " + e.Message.thisisaint8array.ArrayCapacity + ", " + e.Message.thisisaint8array.Array);
				// + " " + e.Message.thisafloat32array.Array[0]);
			};
			Console.WriteLine ("Creating executor");
			Executor executor = new SingleThreadedExecutor ();
			executor.AddNode (test_node);
			Console.WriteLine ("Spinning");
			executor.Spin (new TimeSpan (0, 0, 0, 0, 10));
			Console.WriteLine ("Creating new test_msgs.Dummy");
			test_msgs.Dummy test_msg = new test_msgs.Dummy ();
			test_msg.thisiauint8 = 10;
			test_msg.thisisabool = false;
			test_msg.thisisaint16 = 15;
			test_msg.thisisfloat64 = 10.0f;
			test_msg.thisisastring = new rosidl_generator_c__String ("test_test_test");
			test_msg.thisafloat32array = new rosidl_generator_c__primitive_array_float32 (new float[]{ 10.0f, 1.1f });

			test_msg.thisisaint8array = new rosidl_generator_c__primitive_array_int8 (new byte[]{ 0, 10, 120, 244 });
			Console.WriteLine("Float32 Array size,capacity,pointer: " + test_msg.thisafloat32array.ArraySize+ ", " + test_msg.thisafloat32array.ArrayCapacity + ", " + test_msg.thisafloat32array.Data);
			Console.Write("Float32 Array content: ");
			foreach (var item in test_msg.thisafloat32array.Array) {
				Console.Write (item +"; ");
			}
			Console.WriteLine ();
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
