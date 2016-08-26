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


int main(int argc, char *argv[])
{
    std::cout << "Dummy struct size is: " << sizeof(test_msgs__msg__Dummy) << " Size of bool: " << sizeof(bool) << " Size of float: " << sizeof(float) << " Size of double: " << sizeof(double) << " String size: " << sizeof(rosidl_generator_c__String) << " Array size: " << sizeof(rosidl_generator_c__int8__Array)<< std::endl;
    rclcpp::init(argc, argv);
    
    node = rclcpp::node::Node::make_shared("Cube_Hardware_Simulation_");
    std::thread spinner(&spin);
    auto pub = node->create_publisher<test_msgs::msg::Dummy>("TestTopic",rmw_qos_profile_default);
    test_msgs::msg::Dummy::SharedPtr msg = std::make_shared<test_msgs::msg::Dummy>();
    std::vector<int8_t> test;
    test.push_back(100);
    test.push_back(110);
    msg->thisisaint8array = test;
    msg->thisisastring = "test";
   
    rclcpp::WallRate loop_rate(30);
    std::string input = "";
    while(input != "exit" && rclcpp::ok())
    {    pub->publish(msg);
        loop_rate.sleep();
    }
    spinner.join();
    return 0;
}
