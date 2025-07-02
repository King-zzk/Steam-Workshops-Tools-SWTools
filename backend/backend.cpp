/*
* backend.h
* 后端接口
*/

#include "backend.h"

void ExecuteCmd(wstring file, wstring parameter, wstring dir, bool show) {
	SHELLEXECUTEINFO ShExecInfo = { 0 };
	ShExecInfo.cbSize = sizeof(SHELLEXECUTEINFO);
	ShExecInfo.fMask = SEE_MASK_NOCLOSEPROCESS;
	ShExecInfo.lpFile = file.c_str();
	ShExecInfo.lpParameters = parameter.c_str();
	ShExecInfo.nShow = show ? SW_SHOW : SW_HIDE;
	ShExecInfo.lpDirectory = dir.c_str();
	ShellExecuteEx(&ShExecInfo);
	if (ShExecInfo.hProcess == 0) return;
	WaitForSingleObject(ShExecInfo.hProcess, INFINITE);
}

wstring ToWstr(const string& str) {
	static wstring_convert<std::codecvt_utf8<wchar_t>> converter;
	return converter.from_bytes(str);
}
string ToStr(const wstring& wstr) {
	static wstring_convert<std::codecvt_utf8<wchar_t>> converter;
	return converter.to_bytes(wstr);
}