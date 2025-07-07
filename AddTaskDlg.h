#pragma once
#include "afxdialogex.h"


// CAddTaskDlg 对话框

class CAddTaskDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CAddTaskDlg)

public:
	CAddTaskDlg(CWnd* pParent = nullptr);   // 标准构造函数
	virtual ~CAddTaskDlg();

// 对话框数据
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_ADDTASK_DIALOG };
#endif

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
};
