
// SWToolsDlg.cpp: 实现文件
//

#include "pch.h"
#include "framework.h"
#include "SWTools.h"
#include "SWToolsDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// 用于应用程序“关于”菜单项的 CAboutDlg 对话框

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// 对话框数据
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_ABOUTBOX };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

// 实现
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(IDD_ABOUTBOX)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// CSWToolsDlg 对话框



CSWToolsDlg::CSWToolsDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_SWTOOLS_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CSWToolsDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_COMBO1, m_cboAppName);
	DDX_Control(pDX, IDC_APPSLT, m_txtAppSlt);
	DDX_Control(pDX, IDC_EXITBTN, m_btnExit);
	DDX_Control(pDX, IDC_LAUNCHBTN, m_btnLaunch);
	DDX_Control(pDX, IDC_EDIT2, m_edtID);
	DDX_Control(pDX, IDC_DASHBOARD, m_dashboard);
}

BEGIN_MESSAGE_MAP(CSWToolsDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_ABOUTBTN, &CSWToolsDlg::OnBnClickedAboutbtn)
	ON_EN_CHANGE(IDC_EDIT2, &CSWToolsDlg::OnEnChangeEdit2)
END_MESSAGE_MAP()


// CSWToolsDlg 消息处理程序

BOOL CSWToolsDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 将“关于...”菜单项添加到系统菜单中。

	// IDM_ABOUTBOX 必须在系统命令范围内。
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != nullptr)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// 设置此对话框的图标。  当应用程序主窗口不是对话框时，框架将自动执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// TODO: 添加新的 app 时修改这里
	// 初始化下拉菜单
	// 格式: [英文名] | [中文名]
	m_cboAppName.AddString(_T("Victoria 3 | 维多利亚3"));
	m_cboAppName.AddString(_T("Hearts of Iron IV | 钢铁雄心IV"));
	m_cboAppName.AddString(_T("Garry's Mod | 盖瑞模组"));
	m_cboAppName.AddString(_T("Wallpaper Engine | 壁纸引擎"));
	m_cboAppName.AddString(_T("Crusader Kings III | 十字军之王3"));
	m_cboAppName.SetCurSel(0); // 选中第一项

	m_dashboard.SetWindowText(_T("提示：先选择 App 再填写物品 ID，然后点击“开始下载”"));

	// TODO: 在此添加额外的初始化代码

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

void CSWToolsDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。  对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void CSWToolsDlg::OnPaint()
{
	if (IsIconic())
	{
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
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

//当用户拖动最小化窗口时系统调用此函数取得光标
//显示。
HCURSOR CSWToolsDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

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
