# rclcs_testing_ws

This is a complete testing workspace setup for the [rclcs](https://github.com/firesurfer/rclcs/).
Please make sure you did the steps described in the rclcs documentation before using this workspace.

## What does this workspace consist of?

* src/test_msgs   -  A simple test message which will be translated to c/c++ and c# code
* src/test_cs	  -  Test and demonstration that shows how to use the rclcs in order to communicate via ROS2 (This is the C# code)
* src/test_cpp    - Cpp listener for the message
* src/test_cpp_pub - Cpp publisher for the message 

## How to build and run

Just do an `ament build` in the root directory.
Then do

```
source install/local_setup.bash
cd install/bin
export MONO_PATH=../lib:${AMENT_PREFIX_PATH}/lib>
mono test_cs.exe
```

This will start the Main function defined in Program.cs

For commented examples have a look at `test_cs/test_cs/Examples`.
There are three examples at the moment.

* BasicNodeExample - demonstrates how to initialize the rcl and how to create/spin a node
* PublisherExample - create a publisher an publish the dummy message
* SubscriptionExample - subscribe on a TestTopic an recieve the Dummy message

By changing the line `"-main:test_cs.MainClass"` in the CMakeLists.txt you can also build the other examples.
