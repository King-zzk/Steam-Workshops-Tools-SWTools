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
	int len = MultiByteToWideChar(CP_ACP, 0, str.c_str(), str.size(), NULL, 0);
	wchar_t* buf = new wchar_t[len];
	MultiByteToWideChar(CP_ACP, 0, str.c_str(), str.size(), buf, len);
	wstring res(buf, len);
	delete[] buf;
	return res;
}
string ToStr(const wstring& wstr) {
	int len = WideCharToMultiByte(CP_ACP, 0, wstr.c_str(), wstr.size(), NULL, 0, NULL, NULL);
	char* buf = new char[len];
	WideCharToMultiByte(CP_ACP, 0, wstr.c_str(), wstr.size(), buf, len, NULL, NULL);
	string res(buf, len);
	delete[] buf;
	return res;
}