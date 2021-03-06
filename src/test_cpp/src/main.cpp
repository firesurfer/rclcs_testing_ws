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
void chatterCallback(const cs_msgs::msg::Dummy::SharedPtr msg)
{
 	std::cout << (int)msg->thisisaint8 << ", " << (int)msg->thisiauint8 << ", " << msg->thisisastring << (int)msg->thisisaint8array[0]<< std::endl;
}

int main(int argc, char *argv[])
{
   
    rclcpp::init(argc, argv);
    
    node = rclcpp::Node::make_shared("Cube_Hardware_Simulation_");
    std::thread spinner(&spin);
    auto sub = node->create_subscription<cs_msgs::msg::Dummy>("TestTopic", chatterCallback, rmw_qos_profile_default);

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
