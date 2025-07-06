
// SWToolsDlg.cpp: 实现文件
//

#include "pch.h"
#include "CWorkshops_info.h"
#include "framework.h"
#include "SWTools.h"
#include "SWToolsDlg.h"
#include "afxdialogex.h"
#include "backend/backend.hpp"
#include "CManyDownload.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CEulaDlg 对话框

class CEulaDlg : public CDialogEx {
public:
	CEulaDlg();

	// 对话框数据
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_EULA_DIALOG };
#endif

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持
	virtual BOOL OnInitDialog();

	// 实现
protected:
	DECLARE_MESSAGE_MAP()
public:
	bool accepted = false;
	bool canChange = true;
	CEdit m_eula;
	CButton m_rbDecline;
	CButton m_rbAccept;
	CButton m_btnOk;
	afx_msg void OnBnClickedAccept();
	afx_msg void OnBnClickedDecline();
	afx_msg void OnBnClickedOk();
};

CEulaDlg::CEulaDlg() : CDialogEx(IDD_EULA_DIALOG) {
}

void CEulaDlg::DoDataExchange(CDataExchange* pDX) {
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT1, m_eula);
	DDX_Control(pDX, IDC_DECLINE, m_rbDecline);
	DDX_Control(pDX, IDC_ACCEPT, m_rbAccept);
	DDX_Control(pDX, IDOK, m_btnOk);
}

BOOL CEulaDlg::OnInitDialog() {
	CDialogEx::OnInitDialog();

	m_eula.SetWindowText(text::eula.c_str());
	if (accepted) {
		m_rbAccept.SetCheck(1);
	} else {
		m_rbDecline.SetCheck(1);
		m_btnOk.EnableWindow(0);
	}
	if (!canChange) {
		m_rbAccept.EnableWindow(0);
		m_rbDecline.EnableWindow(0);
	}

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

BEGIN_MESSAGE_MAP(CEulaDlg, CDialogEx)
	ON_BN_CLICKED(IDC_ACCEPT, &CEulaDlg::OnBnClickedAccept)
	ON_BN_CLICKED(IDC_DECLINE, &CEulaDlg::OnBnClickedDecline)
	ON_BN_CLICKED(IDOK, &CEulaDlg::OnBnClickedOk)
END_MESSAGE_MAP()




// 用于应用程序“关于”菜单项的 CAboutDlg 对话框

class CAboutDlg : public CDialogEx {
public:
	CAboutDlg();

	// 对话框数据
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_ABOUTBOX };
#endif

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持
	virtual BOOL OnInitDialog();

	// 实现
protected:
	DECLARE_MESSAGE_MAP()
public:
	CEdit m_about;
	afx_msg void OnBnClickedGithubbtn();
	afx_msg void OnBnClickedEulabtn();
	CEdit m_accepted;
};

CAboutDlg::CAboutDlg() : CDialogEx(IDD_ABOUTBOX) {
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX) {
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT1, m_about);
	DDX_Control(pDX, IDC_EDIT2, m_accepted);
}

BOOL CAboutDlg::OnInitDialog() {
	CDialogEx::OnInitDialog();

	wstring aboutInfo;
	aboutInfo += text::appname + L" 版本 " + text::version + L"\r\n";
	aboutInfo += L"作者：" + text::authors + L"\r\n";
	aboutInfo += text::about + L"\r\n\r\n";
	aboutInfo += text::copyright;
	m_about.SetWindowText(aboutInfo.c_str());
	m_accepted.SetWindowText(text::eula_accepted.c_str());

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
	ON_BN_CLICKED(IDC_GITHUBBTN, &CAboutDlg::OnBnClickedGithubbtn)
	ON_BN_CLICKED(IDC_EULABTN, &CAboutDlg::OnBnClickedEulabtn)
END_MESSAGE_MAP()




// CSWToolsDlg 对话框

CSWToolsDlg::CSWToolsDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_SWTOOLS_DIALOG, pParent) {
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	downloader = new Downloader(this);
}
CSWToolsDlg::~CSWToolsDlg() {
	delete (Downloader*)downloader;
}

void CSWToolsDlg::DoDataExchange(CDataExchange* pDX) {
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_COMBO1, m_cboAppName);
	DDX_Control(pDX, IDC_APPSLT, m_txtAppSlt);
	DDX_Control(pDX, IDC_EXITBTN, m_btnExit);
	DDX_Control(pDX, IDC_LAUNCHBTN, m_btnLaunch);
	DDX_Control(pDX, IDC_EDIT2, m_edtID);
	DDX_Control(pDX, IDC_DASHBOARD, m_dashboard);
	DDX_Control(pDX, IDC_EDIT1, m_state);
	DDX_Control(pDX, IDC_TEXT, m_version);
}

BEGIN_MESSAGE_MAP(CSWToolsDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_ABOUTBTN, &CSWToolsDlg::OnBnClickedAboutbtn)
	ON_EN_CHANGE(IDC_EDIT2, &CSWToolsDlg::OnEnChangeEdit2)
	ON_BN_CLICKED(IDC_EXITBTN, &CSWToolsDlg::OnBnClickedExitbtn)
	ON_BN_CLICKED(IDC_UPDATEBTN, &CSWToolsDlg::OnBnClickedUpdatebtn)
	ON_BN_CLICKED(IDC_LAUNCHBTN, &CSWToolsDlg::OnBnClickedLaunchbtn)
	ON_BN_CLICKED(IDC_FOLDERBTN, &CSWToolsDlg::OnBnClickedFolderbtn)
	ON_BN_CLICKED(IDC_MANYDWNBTN, &CSWToolsDlg::OnBnClickedManydwnbtn)
	ON_BN_CLICKED(IDC_MANYDWNBTN2, &CSWToolsDlg::OnBnClickedManydwnbtn2)
END_MESSAGE_MAP()


// CSWToolsDlg 消息处理程序

BOOL CSWToolsDlg::OnInitDialog() {
	CDialogEx::OnInitDialog();

	// 先让用户同意用户协议
	if (!CheckEula()) {
		CEulaDlg eulaDlg;
		if (eulaDlg.DoModal() != IDOK) {
			CDialog::OnOK(); // 退出程序
			return TRUE;
		}
		if (!WriteEula()) {
			MessageBox(_T("未能写入EULA同意信息"), _T("警告"), MB_ICONWARNING | MB_OK);
		}
	}

	// 将“关于...”菜单项添加到系统菜单中。

	// IDM_ABOUTBOX 必须在系统命令范围内。
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != nullptr) {
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty()) {
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// 设置此对话框的图标。  当应用程序主窗口不是对话框时，框架将自动执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// 初始化下拉菜单
	for (auto pair : app_infos) {
		m_cboAppName.AddString(pair.first.c_str());
	}
	m_cboAppName.SetCurSel(0); // 选择第一项

	m_dashboard.SetWindowText(_T("提示：先选择 App，然后填写物品 ID，最后点击“开始下载”。\r\n"));
	m_state.SetWindowTextW(_T("空闲中"));
	m_version.SetWindowTextW(text::version.c_str());

	// TODO: 在此添加额外的初始化代码

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

void CSWToolsDlg::OnSysCommand(UINT nID, LPARAM lParam) {
	if ((nID & 0xFFF0) == IDM_ABOUTBOX) {
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	} else {
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。  对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void CSWToolsDlg::OnPaint() {
	if (IsIconic()) {
		CPaintDC dc(this); // 用于绘制的设备上下文

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 使图标在工作区矩形中居中
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 绘制图标
		dc.DrawIcon(x, y, m_hIcon);
	} else {
		CDialogEx::OnPaint();
	}
}

//当用户拖动最小化窗口时系统调用此函数取得光标
//显示。
HCURSOR CSWToolsDlg::OnQueryDragIcon() {
	return static_cast<HCURSOR>(m_hIcon);
}

// “关于”对话框
void CSWToolsDlg::OnBnClickedAboutbtn() {
	CAboutDlg aboutDlg;
	aboutDlg.DoModal();
}


void CSWToolsDlg::OnEnChangeEdit2() {
	if (m_edtID.GetWindowTextLengthW()) {
		m_btnLaunch.EnableWindow(1);
	} else {
		m_btnLaunch.EnableWindow(0);
	}
}

void CSWToolsDlg::OnBnClickedExitbtn() {
	CDialog::OnOK(); // 退出程序
}


void CAboutDlg::OnBnClickedGithubbtn() {
	ShellExecute(NULL, L"open", text::website.c_str(), NULL, NULL, SW_SHOWNORMAL);
}


void CAboutDlg::OnBnClickedEulabtn() {
	CEulaDlg eulaDlg;
	eulaDlg.accepted = true;
	eulaDlg.canChange = false;
	eulaDlg.DoModal();
}


void CEulaDlg::OnBnClickedAccept() {
	m_btnOk.EnableWindow(m_rbAccept.GetCheck());
}


void CEulaDlg::OnBnClickedDecline() {
	m_btnOk.EnableWindow(m_rbAccept.GetCheck());
}


void CEulaDlg::OnBnClickedOk() {
	CDialogEx::OnOK();
}


void CSWToolsDlg::OnBnClickedUpdatebtn() {
	m_state.SetWindowText(_T("检查更新中"));
	CheckUpdate();
	m_state.SetWindowText(_T("空闲中"));
}

void CSWToolsDlg::appendToDashboard(CString str) {
	CString str0;
	m_dashboard.GetWindowText(str0);
	str0 += str;
	m_dashboard.SetWindowText(str0.GetBuffer());
	m_dashboard.PostMessage(WM_VSCROLL, SB_BOTTOM, 0); // 跟随滚动
}

// 下载按钮
void CSWToolsDlg::OnBnClickedLaunchbtn() {
	CString appname, id;
	m_cboAppName.GetWindowText(appname);
	m_edtID.GetWindowText(id);
	appendToDashboard(L"\r\n");
	((Downloader*)downloader)->Download(appname.GetBuffer(), id.GetBuffer());
}


void CSWToolsDlg::OnBnClickedFolderbtn() {
	((Downloader*)downloader)->OpenLastFolder();
}


void CSWToolsDlg::OnBnClickedManydwnbtn() {
	CManyDownload dlg;
	dlg.DoModal();
}


void CSWToolsDlg::OnBnClickedManydwnbtn2()
{
	m_state.SetWindowText(TEXT("查看创意工坊文件信息中"));
	// TODO: 在此添加控件通知处理程序代码
	CString id;
	m_edtID.GetWindowText(id);
	if (id.IsEmpty()) {
		MessageBox(TEXT("请输入id在继续！"), TEXT("Error"), MB_ICONERROR | MB_OK);
	}
	else {
		CString file_path;
		file_path.Format(TEXT("./workshops/%s.json"), id.GetString());
		CString command_curl;
		command_curl.Format(TEXT("echo 下载json文件中... && curl -s -o \"%s\" https://steamworkshopdownloader.io/api/details/file -d [%s]"), file_path.GetString(), id.GetString());
		CWorkshops_info dlg;
		dlg.workshops_id = id.GetString();
		dlg.command_curl = command_curl.GetString();
		dlg.file_path = file_path.GetString();
		dlg.DoModal();
		m_state.SetWindowText(TEXT("空闲中"));
	}
}
