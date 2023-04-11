project(mono)

# Include external platform files (eg. console implementation)
if(HOST_PS4)
  include_directories("${CMAKE_CURRENT_SOURCE_DIR}/../../ps4")
  add_definitions(-include fwd_decls.h)
elseif(HOST_PS5)
  include_directories("${CMAKE_CURRENT_SOURCE_DIR}/../../ps5")
  add_definitions(-include fwd_decls.h)
elseif(HOST_SWITCH)
  include_directories("${CMAKE_CURRENT_SOURCE_DIR}/../../switch")
  add_definitions(-include fwd_decls.h)
endif()

set(subdirs mini profiler)

foreach(dir ${subdirs})
  add_subdirectory(${dir})
endforeach()
