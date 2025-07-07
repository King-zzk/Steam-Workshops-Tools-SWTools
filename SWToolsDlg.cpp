
// SWToolsDlg.cpp: 实现文件
//

#include "framework.h"
#include "SWTools.h"
#include "SWToolsDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CSWToolsDlg 对话框



CSWToolsDlg::CSWToolsDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_SWTOOLS_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CSWToolsDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_BTN_START, m_btn_start);
	DDX_Control(pDX, IDC_BTN_PAUSE, m_btn_pause);
	DDX_Control(pDX, IDC_BTN_ADDTASK, m_btn_addTask);
	DDX_Control(pDX, IDC_BTN_DELETE, m_btn_delete);
	DDX_Control(pDX, IDC_BTN_OPENFOLDER, m_btn_openFolder);
	DDX_Control(pDX, IDC_EDIT_STATUS, m_edit_status);
	DDX_Control(pDX, IDC_EDIT_STATUS, m_edit_status);
}

BEGIN_MESSAGE_MAP(CSWToolsDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BTN_ADDTASK, &CSWToolsDlg::OnBnClickedBtnAddtask)
END_MESSAGE_MAP()


// CSWToolsDlg 消息处理程序

BOOL CSWToolsDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 设置此对话框的图标。  当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	ShowWindow(SW_MINIMIZE);

	// TODO: 在此添加额外的初始化代码

	// 设置按钮图标
	// 图标大小我做了微调，以保证视觉上的大小一致性
	m_btn_start.SetIcon((HICON)LoadImage(AfxGetInstanceHandle(), MAKEINTRESOURCE(IDI_START), 
		IMAGE_ICON, 18, 18, LR_DEFAULTCOLOR | LR_CREATEDIBSECTION));
	m_btn_pause.SetIcon((HICON)LoadImage(AfxGetInstanceHandle(), MAKEINTRESOURCE(IDI_PAUSE), 
		IMAGE_ICON, 20, 20, LR_DEFAULTCOLOR | LR_CREATEDIBSECTION));
	m_btn_addTask.SetIcon((HICON)LoadImage(AfxGetInstanceHandle(), MAKEINTRESOURCE(IDI_ADDTASK),
		IMAGE_ICON, 27, 27, LR_DEFAULTCOLOR | LR_CREATEDIBSECTION));
	m_btn_delete.SetIcon((HICON)LoadImage(AfxGetInstanceHandle(), MAKEINTRESOURCE(IDI_DELETE),
		IMAGE_ICON, 22, 22, LR_DEFAULTCOLOR | LR_CREATEDIBSECTION));
	m_btn_openFolder.SetIcon((HICON)LoadImage(AfxGetInstanceHandle(), MAKEINTRESOURCE(IDI_OPENFOLDER),
		IMAGE_ICON, 22, 22, LR_DEFAULTCOLOR | LR_CREATEDIBSECTION));

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
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



void CSWToolsDlg::OnBnClickedBtnAddtask() {
	// TODO: 在此添加控件通知处理程序代码
}
