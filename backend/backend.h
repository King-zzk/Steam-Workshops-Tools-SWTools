#pragma once
/*
* Backend.h
* 后端总接口 (单实例，仿静态类)
*/

#define not !
#include "mlib/Process.hpp"
#include "mlib/Logger.hpp"
#include "mlib/ThreadPool.hpp"
#undef not
#include "jsonxx/jsonxx.h" // 必须先包含这个

#include <Windows.h>
#ifdef max
#undef max
#endif
#ifdef min
#undef min
#endif
#include <string>

// SteamApp ID 存储类型
typedef unsigned appid_t;

// 后端组件
#include "Item.h"
#include "ItemEntry.h"
#include "UserPool.h"
#include "Downloader.h"
#include "Parser.h"

// 前后端交互
#define WM_MY_CUSTOM_MESSAGE  (WM_USER)

extern class Backend {
	//HWND hWnd;

	/* 工具方法 */
public:
	// 静默运行命令行 (阻塞)
	void executeCmd(std::wstring file, std::wstring parameter, 
		std::wstring dir = L"", bool show = false);
	// 用 curl 请求 (直接返回结果)
	std::string requestWithCurl(std::string command_line);
	// string -> wstring
	std::wstring toWstr(const std::string& str);
	// wstring -> string
	std::string toStr(const std::wstring& wstr);

	/* 其他成员 */
public:
	mlib::Logger logger;	// 后端共享日志器
	// 组件
	Downloader downloader;
	Parser parser;

	Backend();
	~Backend();
} backend;