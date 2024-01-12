cmake_minimum_required(VERSION 3.20)
cmake_policy(SET CMP0091 NEW)

# Custom platforms (external)
if(TARGET_SYSTEM_NAME STREQUAL "ps4")
  message("Build corehost/CMakeLists.txt for PS4")
  include(../../ps4/PS4Toolchain.txt)
elseif(TARGET_SYSTEM_NAME STREQUAL "ps5")
  message("Build corehost/CMakeLists.txt for PS5")
  include(../../ps5/PS5Toolchain.txt)
elseif(TARGET_SYSTEM_NAME STREQUAL "switch")
  message("Build corehost/CMakeLists.txt for Switch")
  include(../../switch/SwitchToolchain.cmake)
endif()

project(corehost)

include(../../../eng/native/configurepaths.cmake)
include(${CLR_ENG_NATIVE_DIR}/configurecompiler.cmake)

# Include external platform files (eg. console implementation)
if(TARGET_SYSTEM_NAME STREQUAL "ps4")
  include_directories("${CMAKE_CURRENT_SOURCE_DIR}/../../ps4")
  set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -fexceptions")
elseif(TARGET_SYSTEM_NAME STREQUAL "ps5")
  include_directories("${CMAKE_CURRENT_SOURCE_DIR}/../../ps5")
  set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -fexceptions")
elseif(TARGET_SYSTEM_NAME STREQUAL "switch")
  include_directories("${CMAKE_CURRENT_SOURCE_DIR}/../../switch")
  set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -fexceptions")
endif()

if (MSVC)
    # Host components don't try to handle asynchronous exceptions
    add_compile_options($<$<COMPILE_LANGUAGE:CXX>:/EHsc>)

    # Explicitly re-enable warnings about declaration hiding (currently disabled by configurecompiler.cmake)
    add_compile_options($<$<COMPILE_LANGUAGE:C,CXX>:/we4456>) # declaration of 'identifier' hides previous local declaration
    add_compile_options($<$<COMPILE_LANGUAGE:C,CXX>:/we4457>) # declaration of 'identifier' hides function parameter
    add_compile_options($<$<COMPILE_LANGUAGE:C,CXX>:/we4458>) # declaration of 'identifier' hides class member
    add_compile_options($<$<COMPILE_LANGUAGE:C,CXX>:/we4459>) # declaration of 'identifier' hides global declaration
elseif (CMAKE_CXX_COMPILER_ID MATCHES GNU)
    # Prevents libc from calling pthread_cond_destroy on static objects in
    # dlopen()'ed library which we dlclose() in pal::unload_library.
    add_compile_options($<$<COMPILE_LANGUAGE:CXX>:-fno-use-cxa-atexit>)
endif()

add_subdirectory(hostcommon)
add_subdirectory(nethost)
add_subdirectory(fxr)

if(CLR_CMAKE_TARGET_STATIC_LIBS_ONLY)
else()
    add_subdirectory(hostpolicy)
    add_subdirectory(apphost)
    add_subdirectory(dotnet)
    add_subdirectory(test)
endif()

# If there's a dynamic ASAN runtime, then install it in the directories where we put our executables.
if (NOT "${ASAN_RUNTIME}" STREQUAL "")
    install(FILES ${ASAN_RUNTIME} DESTINATION corehost)
    install(FILES ${ASAN_RUNTIME} DESTINATION corehost_test)
endif()

if (NOT RUNTIME_FLAVOR STREQUAL Mono)
    if(CLR_CMAKE_TARGET_WIN32)
        add_subdirectory(comhost)
        add_subdirectory(ijwhost)
    endif()
endif()
