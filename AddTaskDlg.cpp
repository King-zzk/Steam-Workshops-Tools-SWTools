// CAddTaskDlg.cpp: 实现文件
//

#include "SWTools.h"
#include "afxdialogex.h"
#include "AddTaskDlg.h"


// CAddTaskDlg 对话框

IMPLEMENT_DYNAMIC(CAddTaskDlg, CDialogEx)

CAddTaskDlg::CAddTaskDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_ADDTASK_DIALOG, pParent)
{

}

CAddTaskDlg::~CAddTaskDlg()
{
}

void CAddTaskDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CAddTaskDlg, CDialogEx)
END_MESSAGE_MAP()


// CAddTaskDlg 消息处理程序
