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
if(_GAMING_XBOX)
  add_definitions(-D_GAMING_XBOX)
  add_definitions(-DWIN32_LEAN_AND_MEAN)
  add_definitions(-D_ATL_NO_DEFAULT_LIBS)
  add_definitions(-D__WRL_NO_DEFAULT_LIB__)
endif()
if(_GAMING_XBOX_XBOXONE)
  add_definitions(-D_GAMING_XBOX_XBOXONE)
endif()
if(_GAMING_XBOX_SCARLETT)
  add_definitions(-D_GAMING_XBOX_SCARLETT)
endif()

set(subdirs
  eglib
  utils
  sgen
  metadata
  mini
  profiler)

foreach(dir ${subdirs})
  add_subdirectory(${dir})
endforeach()
