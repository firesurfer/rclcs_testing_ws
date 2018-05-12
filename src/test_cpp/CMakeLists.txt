cmake_minimum_required(VERSION 3.5)



project(test_cpp)

if(NOT WIN32)
  set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++11 -Wall -Wextra -g")
endif()



find_package(ament_cmake REQUIRED)
find_package(builtin_interfaces REQUIRED)
find_package(rclcpp REQUIRED)
find_package(rmw REQUIRED)
find_package(rmw_implementation_cmake REQUIRED)
find_package(std_msgs REQUIRED)
find_package(cs_msgs)



include_directories(
    	${rclcpp_INCLUDE_DIRS}
    	${rmw_INCLUDE_DIRS}
    	${rmw_implementation_INCLUDE_DIRS}
    	${std_msgs_INCLUDE_DIRS}
    	${cs_msgs_INCLUDE_DIRS}
	${CMAKE_CURRENT_SOURCE_DIR}/src/
	
)

add_executable(test_cpp${target_suffix}
		${CMAKE_CURRENT_SOURCE_DIR}/src/main.cpp)

target_link_libraries(test_cpp
    	${rclcpp_LIBRARIES}
    	${rmw_LIBRARIES}
    	${rmw_implementation_LIBRARIES}
    	${std_msgs_LIBRARIES}
	${cs_msgs_LIBRARIES}
    	-pthread

)






install(TARGETS test_cpp DESTINATION bin)





ament_package()
