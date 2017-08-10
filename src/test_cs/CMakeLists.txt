cmake_minimum_required(VERSION 3.5)

project(test_cs)



find_package(ament_cmake REQUIRED)
find_package(ament_cmake_python REQUIRED)

find_package(dotnet_cmake_module REQUIRED)
find_package(DotNETExtra MODULE)

file(GLOB_RECURSE CS_SOURCES ${CMAKE_CURRENT_SOURCE_DIR}/src/*.cs)
 

#add_assemblies(test_cs
#    ${CS_SOURCES}
#    OUTPUT_NAME
#    "test_cs"
#    OUTPUT_TYPE 
 #   "Exe"
#   COMPILER_ARGS
 #   "-main:test_cs.MainClass"
#    REFERENCE_ASSEMBLIES
 #   "System,System.Xml,System.Xml.Linq,rclcs,test_msgs,builtin_interfaces"
#)
add_msbuild(test_cs
    EXECUTABLE
    CSPROJ
    ${CMAKE_CURRENT_SOURCE_DIR}/src/test_cs.csproj)
install_assemblies(test_cs COPY_DEPENDENCIES DESTINATION bin)
#install(FILES ${CMAKE_BINARY_DIR}/test_cs.exe 	DESTINATION bin)

ament_package()
