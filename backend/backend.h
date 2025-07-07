#pragma once
/*
* backend.h
* 后端接口
*/

#define not !
#include "mlib/process.hpp"
#include "mlib/logger.hpp"
#undef not
#include "jsonxx/jsonxx.h"

#include <windows.h>
#include <iostream>
#include <fstream>
#include <string>
#include <thread>
#include <vector>
#include <map>
#include <mutex>	// 线程锁
#include <io.h>		// _access()

using namespace std;
//using namespace mlib::process;
//using namespace mlib::logger;

/* 类型定义 */
typedef unsigned appid_t;	// 用于 SteamApp ID

/* 实用函数 */
// 静默运行命令行 (阻塞)
void ExecuteCmd(wstring file, wstring parameter, wstring dir = L"", bool show = false);
// string -> wstring
wstring ToWstr(const string& str);
// wstring -> string
string ToStr(const wstring& wstr);

/* 组件 */
//#include "app_name.h"
//#include "item.h"
//#include "userpool.h"
//#include "tasker.h"