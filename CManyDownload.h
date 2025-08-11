#pragma once
#include "afxdialogex.h"


// CManyDownload 对话框

class CManyDownload : public CDialogEx
{
	DECLARE_DYNAMIC(CManyDownload)

public:
	CManyDownload(CWnd* pParent = nullptr);   // 标准构造函数
	virtual ~CManyDownload();

// 对话框数据
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_ManyDownload };
#endif

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
public:
	CStatic Loading;
	afx_msg void OnBnClickedManydwnlaunch();
	CComboBox COMBO_Choose;
	afx_msg void OnCbnSelchangeCombo1();
};
