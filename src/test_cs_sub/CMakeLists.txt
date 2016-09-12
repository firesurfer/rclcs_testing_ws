cmake_minimum_required(VERSION 2.8.3)

project(test_cs_sub)



find_package(ament_cmake REQUIRED)


add_custom_target(test_cs_dll ALL
	COMMAND 
	xbuild test_cs_sub.csproj /property:OutputPath="${CMAKE_BINARY_DIR}"
	WORKING_DIRECTORY
	${CMAKE_SOURCE_DIR}/test_cs
)

install(FILES ${CMAKE_BINARY_DIR}/test_cs_sub.exe 
	DESTINATION bin)

ament_package()
