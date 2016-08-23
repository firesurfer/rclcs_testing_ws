cmake_minimum_required(VERSION 2.8.3)

project(test_msgs)

if(NOT WIN32)
  set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++11 -Wall -Wextra")
endif()

find_package(ament_cmake REQUIRED)

#Message generation
find_package(builtin_interfaces REQUIRED)
find_package(rosidl_default_generators REQUIRED)

file(GLOB MESSAGE_FILES
    "msg/*.msg"
)

file(GLOB SERVICE_FILES
    "srv/*.srv"
)

rosidl_generate_interfaces(${PROJECT_NAME}
  ${MESSAGE_FILES}
  ${SERVICE_FILES}
  DEPENDENCIES builtin_interfaces
)

ament_package()
