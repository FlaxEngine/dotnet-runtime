project(mono)

# Include external platform files (eg. console implementation)
if(HOST_PS4)
  include_directories("${CMAKE_CURRENT_SOURCE_DIR}/../../ps4")
  add_definitions(-include fwd_decls.h)
endif()

set(subdirs mini profiler)

foreach(dir ${subdirs})
  add_subdirectory(${dir})
endforeach()
