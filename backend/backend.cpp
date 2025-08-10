/*
* Backend.cpp
* 后端接口
*/

#include "Backend.h"

Backend backend;
/* 工具方法 */
void Backend::executeCmd(std::wstring file, std::wstring parameter,
	std::wstring dir, bool show) {
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
std::string Backend::requestWithCurl(std::string parameter) {
	try {
		logger.debug("requestWithCurl() called with parameter \"" + parameter + "\"");
		mlib::Process curl("", "curl " + parameter);
		while (curl.getExitCode() == STILL_ACTIVE);
		Sleep(10); // 再等等
		return curl.read(curl.peek());
	} catch (std::runtime_error e) {
		logger.error(std::string("Exception occurred when requestWithCurl(): ") + e.what());
	}
	return "";
}
std::wstring Backend::toWstr(const std::string& str) {
	int len = MultiByteToWideChar(CP_ACP, 0, str.c_str(), str.size(), NULL, 0);
	wchar_t* buf = new wchar_t[len];
	MultiByteToWideChar(CP_ACP, 0, str.c_str(), str.size(), buf, len);
	std::wstring res(buf, len);
	delete[] buf;
	return res;
}
std::string Backend::toStr(const std::wstring& wstr) {
	int len = WideCharToMultiByte(CP_ACP, 0, wstr.c_str(), wstr.size(), NULL, 0, NULL, NULL);
	char* buf = new char[len];
	WideCharToMultiByte(CP_ACP, 0, wstr.c_str(), wstr.size(), buf, len, NULL, NULL);
	std::string res(buf, len);
	delete[] buf;
	return res;
}
/* 其他成员 */
Backend::Backend()
	: downloader(&logger), parser(&logger),
#ifdef _DEBUG
	logger(mlib::Logger::Level::debug, "latest.log")
#else
	logger(mlib::Logger::Level::info, "latest.log")
#endif
{
	logger.debug("Backend setup completed");
}
Backend::~Backend() {

}