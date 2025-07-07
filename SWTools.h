
// SWTools.h: PROJECT_NAME 应用程序的主头文件
//

#pragma once

#include "framework.h"
#include "resource.h"		// 主符号


// CSWToolsApp:
// 有关此类的实现，请参阅 SWTools.cpp
//

class CSWToolsApp : public CWinApp
{
public:
	CSWToolsApp();

// 重写
public:
	virtual BOOL InitInstance();

// 实现

	DECLARE_MESSAGE_MAP()
};

extern CSWToolsApp theApp;
