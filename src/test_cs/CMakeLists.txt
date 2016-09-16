cmake_minimum_required(VERSION 2.8.3)

project(test_cs)



find_package(ament_cmake REQUIRED)

if(NOT WIN32)
add_custom_target(test_cs_dll ALL
	COMMAND 
	xbuild test_cs.csproj /property:OutputPath="${CMAKE_BINARY_DIR}"
	WORKING_DIRECTORY
	${CMAKE_SOURCE_DIR}/test_cs
)
else()
add_custom_target(test_cs_dll ALL
	COMMAND 
	xbuild test_cs.csproj /property:OutputPath="${CMAKE_BINARY_DIR}"
	WORKING_DIRECTORY
	${CMAKE_SOURCE_DIR}/test_cs
)
endif()

install(FILES ${CMAKE_BINARY_DIR}/test_cs.exe 
	DESTINATION bin)

ament_package()
