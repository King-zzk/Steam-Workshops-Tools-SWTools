#pragma once
/*
* backend.hpp
* 后端暴露给前端调用的接口
*/

#include <iostream>
#include <fstream>
#include <sstream>		// ostringstream
#include <iomanip>		// setprecision()
#include <string>
#include <thread>
#include <io.h> // _access()
#include <map>
#include <windows.h>
#include <locale>	// string <-> wstring
#include <codecvt> // 同上
using namespace std;

#include "process.hpp"

// 静默运行命令行 (阻塞)
void ExecuteCmd(wstring file, wstring parameter, bool show = false) {
	SHELLEXECUTEINFO ShExecInfo = { 0 };
	ShExecInfo.cbSize = sizeof(SHELLEXECUTEINFO);
	ShExecInfo.fMask = SEE_MASK_NOCLOSEPROCESS;
	ShExecInfo.lpFile = file.c_str();
	ShExecInfo.lpParameters = parameter.c_str();
	ShExecInfo.nShow = show ? SW_SHOW : SW_HIDE;
	ShellExecuteEx(&ShExecInfo);
	if (ShExecInfo.hProcess == 0) return;
	WaitForSingleObject(ShExecInfo.hProcess, INFINITE);
}

// string -> wstring
wstring ToWstr(const string& str) {
	static wstring_convert<std::codecvt_utf8<wchar_t>> converter;
	return converter.from_bytes(str);
}
// wstring -> string
string ToStr(const wstring& wstr) {
	static wstring_convert<std::codecvt_utf8<wchar_t>> converter;
	return converter.to_bytes(wstr);
}

#include "app_info.hpp"
#include "texts.hpp"
#include "eula.hpp"
#include "update.hpp"
#include "downloader.hpp"