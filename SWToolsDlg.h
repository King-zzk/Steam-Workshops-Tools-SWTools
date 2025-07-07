
// SWToolsDlg.h: 头文件
//

#pragma once


// CSWToolsDlg 对话框
class CSWToolsDlg : public CDialogEx
{
// 构造
public:
	CSWToolsDlg(CWnd* pParent = nullptr);	// 标准构造函数

// 对话框数据
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_SWTOOLS_DIALOG };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持


// 实现
protected:
	HICON m_hIcon;

	// 生成的消息映射函数
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	CButton m_btn_start;
	CButton m_btn_pause;
	CButton m_btn_addTask;
	CButton m_btn_delete;
	CButton m_btn_openFolder;
	CEdit m_edit_status;
	afx_msg void OnBnClickedBtnAddtask();
};
