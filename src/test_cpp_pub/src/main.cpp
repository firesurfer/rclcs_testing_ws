#include <iostream>
#include "rclcpp/rclcpp.hpp"
#include <memory>
#include "cs_msgs/msg/dummy.hpp"
#include "cs_msgs/msg/dummy.h"
rclcpp::Node::SharedPtr node;
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
 
    rclcpp::init(argc, argv);
    
    node = rclcpp::Node::make_shared("Cube_Hardware_Simulation_");
    std::thread spinner(&spin);
    auto pub = node->create_publisher<cs_msgs::msg::Dummy>("TestTopic",rmw_qos_profile_default);
    cs_msgs::msg::Dummy::SharedPtr msg = std::make_shared<cs_msgs::msg::Dummy>();
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
