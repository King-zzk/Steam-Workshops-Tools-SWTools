#pragma once
/*
* backend.hpp
* 后端暴露给前端调用的接口
*/

#include <iostream>
#include <fstream>
#include <sstream>		// ostringstream
#include <iomanip>		// setprecision()
#include <exception>
#include <windows.h>
#include <io.h> // _access()
#include <map>
using namespace std;

#include <windows.h>

#include "app_info.hpp"
#include "texts.hpp"
#include "eula.hpp"
#include "update.hpp"