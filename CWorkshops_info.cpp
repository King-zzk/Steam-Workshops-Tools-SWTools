// CWorkshops_info.cpp: 实现文件
//

#include "pch.h"
#include "SWTools.h"
#include "afxdialogex.h"
#include "CWorkshops_info.h"
#include <corecrt_io.h>
#include <direct.h>
#include <nlohmann/json.hpp>
#include <fstream>

using json = nlohmann::json;
// CWorkshops_info 对话框

IMPLEMENT_DYNAMIC(CWorkshops_info, CDialogEx)

BOOL CWorkshops_info::OnInitDialog()
{
	CDialogEx::OnInitDialog();
	// 检查Temp文件夹
	if (_access("./workshops", 0) == -1)
	{
		if (_mkdir("./workshops") == -1) {
			MessageBox(_T("无法创建临时文件夹，请检查权限"), _T("错误"), MB_ICONERROR | MB_OK);
			return FALSE; // 退出对话框
		}
	}
    system(command_curl.GetString());
	if (_access(file_path, 0) == -1) {
		MessageBox(_T("无法下载物品信息，请检查网络连接"), _T("错误"), MB_ICONERROR | MB_OK);
		return FALSE; // 退出对话框
	}
	std::ifstream file(file_path);
	if (!file.is_open()) {
		MessageBox(_T("无法打开物品信息文件"), _T("错误"), MB_ICONERROR | MB_OK);
		return FALSE; // 退出对话框
	}
	// 读取JSON文件
	try
	{
		json j;
		file >> j;
		std::string nameStr = j["name"].get<std::string>();  // 获取物品名称
        CString name(nameStr.c_str());
		std::string file_description = j["title"].get<std::string>();// 获取物品描述
		CString description(file_description.c_str());
		std::string app_name = j["app_name"].get<std::string>(); // 所属软件&游戏
		CString appName(app_name.c_str());
		std::string views = j["views"].get<std::string>(); // 浏览量
		CString viewsStr(views.c_str());
        CString info;
        info.Format(_T("物品名称: %s\r\n物品描述: %s\r\n所属软件&游戏: %s\r\n浏览量: %s\r\n"),  
                    name.GetString(), description.GetString(), appName.GetString(), viewsStr.GetString());  
        m_workshops_info.SetWindowTextW(info.GetString());
		m_workshops_info.SetWindowTextW(info.GetString());
	}
	// 错误处理
	catch (const json::parse_error& e)
	{
		MessageBox(_T("JSON解析错误: ") + CString(e.what()), _T("错误"), MB_ICONERROR | MB_OK);
		return FALSE;
	}
	catch (const std::exception& e)
	{
		MessageBox(_T("发生异常: ") + CString(e.what()), _T("错误"), MB_ICONERROR | MB_OK);
		return FALSE;
	}

}

CWorkshops_info::CWorkshops_info(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_Workshops_info, pParent)
{

}

CWorkshops_info::~CWorkshops_info()
{
}

void CWorkshops_info::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT1, m_workshops_info);
}


BEGIN_MESSAGE_MAP(CWorkshops_info, CDialogEx)
	ON_EN_CHANGE(IDC_EDIT1, &CWorkshops_info::OnEnChangeEdit1)
END_MESSAGE_MAP()


// CWorkshops_info 消息处理程序

void CWorkshops_info::OnEnChangeEdit1()
{
	// TODO:  如果该控件是 RICHEDIT 控件，它将不
	// 发送此通知，除非重写 CDialogEx::OnInitDialog()
	// 函数并调用 CRichEditCtrl().SetEventMask()，
	// 同时将 ENM_CHANGE 标志“或”运算到掩码中。

	// TODO:  在此添加控件通知处理程序代码
}
