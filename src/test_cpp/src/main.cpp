#include <iostream>
#include "rclcpp/rclcpp.hpp"
#include <memory>
#include "test_msgs/msg/dummy.hpp"
#include "test_msgs/msg/dummy.h"
rclcpp::node::Node::SharedPtr node;
void spin()
{
    rclcpp::executors::SingleThreadedExecutor exec;
    std::cout << "spinning" << std::endl;
    rclcpp::WallRate loop_rate(20);
    while(rclcpp::ok())
    {
        exec.spin_node_some(node);
        loop_rate.sleep();
    }
}
void chatterCallback(const test_msgs::msg::Dummy::SharedPtr msg)
{
 	std::cout << (int)msg->thisisaint8 << ", " << (int)msg->thisiauint8 << ", " << msg->thisisastring << std::endl;
}

int main(int argc, char *argv[])
{
    std::cout << "Dummy struct size is: " << sizeof(test_msgs__msg__Dummy) << " Size of bool: " << sizeof(bool) << " Size of float: " << sizeof(float) << " Size of double: " << sizeof(double) << " String size: " << sizeof(rosidl_generator_c__String) << " Array size: " << sizeof(rosidl_generator_c__int8__Array)<< std::endl;
    rclcpp::init(argc, argv);
    
    node = rclcpp::node::Node::make_shared("Cube_Hardware_Simulation_");
    std::thread spinner(&spin);
    auto sub = node->create_subscription<test_msgs::msg::Dummy>("TestTopic", chatterCallback, rmw_qos_profile_default);
    rclcpp::WallRate loop_rate(30);
    std::string input = "";
    while(input != "exit" && rclcpp::ok())
    {   std::getline (std::cin,input);
        if(input == "exit")
            exit(0);
        loop_rate.sleep();
    }
    spinner.join();
    return 0;
}
