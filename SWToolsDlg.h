
// SWToolsDlg.h: 头文件
//

#pragma once


// CSWToolsDlg 对话框
class CSWToolsDlg : public CDialogEx
{
// 构造
public:
	CSWToolsDlg(CWnd* pParent = nullptr);	// 标准构造函数
	~CSWToolsDlg();

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
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	CComboBox m_cboAppName;
	CStatic m_txtAppSlt;
	CButton m_btnExit;
	afx_msg void OnBnClickedAboutbtn();
	afx_msg void OnEnChangeEdit2();
	CButton m_btnLaunch;
	CEdit m_edtID;
	CEdit m_dashboard;
	afx_msg void OnBnClickedExitbtn();
	CEdit m_state;
	afx_msg void OnBnClickedUpdatebtn();
	void appendToDashboard(CString str); // 往仪表盘追加信息

	void* downloader; // 后端接口~
	afx_msg void OnBnClickedLaunchbtn();
	afx_msg void OnBnClickedFolderbtn();
	CStatic m_version;
	afx_msg void OnBnClickedManydwnbtn();
};
