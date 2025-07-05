#pragma once
#include "afxdialogex.h"


// CWorkshops_info 对话框

class CWorkshops_info : public CDialogEx
{
	DECLARE_DYNAMIC(CWorkshops_info)

public:
	CWorkshops_info(CWnd* pParent = nullptr);   // 标准构造函数
	virtual ~CWorkshops_info();
	CStringA workshops_id; // 物品 ID
	CStringA command_curl; // curl 命令
	CStringA file_path; // 文件路径
	virtual BOOL OnInitDialog();
// 对话框数据
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_Workshops_info };
#endif

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
public:
	CEdit m_workshops_info;
	afx_msg void OnEnChangeEdit1();
};
