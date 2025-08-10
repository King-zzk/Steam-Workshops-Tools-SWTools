#pragma once
/*
* Downloader.h
* 下载器
*/

class Downloader {
	mlib::Logger *pLogger;	// Backend 接口的日志器

public:
	Downloader(mlib::Logger* pLogger);
};

