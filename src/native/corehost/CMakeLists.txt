cmake_minimum_required(VERSION 3.6.2)

# Custom platforms (external)
if(TARGET_SYSTEM_NAME STREQUAL "PS4")
  message("Build corehost/CmakeList.txt for PS4")
  include(../../ps4/PS4Toolchain.txt)
endif()

if (CMAKE_VERSION VERSION_GREATER 3.15 OR CMAKE_VERSION VERSION_EQUAL 3.15)
    cmake_policy(SET CMP0091 NEW)
endif()

project(corehost)

include(../../../eng/native/configurepaths.cmake)
include(${CLR_ENG_NATIVE_DIR}/configurecompiler.cmake)

# Include external platform files (eg. console implementation)
if(TARGET_SYSTEM_NAME STREQUAL "PS4")
  include_directories("${CMAKE_CURRENT_SOURCE_DIR}/../../ps4")
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

if (NOT RUNTIME_FLAVOR STREQUAL Mono)
    if(CLR_CMAKE_TARGET_WIN32)
        add_subdirectory(comhost)
        add_subdirectory(ijwhost)
    endif()
endif()
