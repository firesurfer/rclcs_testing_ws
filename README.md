# rclcs_testing_ws

This is a complete testing workspace setup for the [rclcs](https://github.com/firesurfer/rclcs/).
Please make sure you did the steps described in the rclcs documentation before using this workspace.

## What does this workspace consist of?

* src/test_msgs   -  A simple test message which will be translated to c/c++ and c# code
* src/test_cs	  -  Test and demonstration that shows how to use the rclcs in order to communicate via ROS2
* src/test_cpp    - Cpp listener for the message
* src/test_cpp_pub - Cpp publisher for the message 

## How to build and run

Just do an `ament build` in the root directory.
Then do

```
source install/local_setup.bash
cd install/bin
export MONO_PATH=../lib:<Path to ros2 workspace/install/lib>
mono test_cs.exe
```
