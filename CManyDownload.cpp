// CManyDownload.cpp: 实现文件
//

#include "pch.h"
#include "SWTools.h"
#include "afxdialogex.h"
#include <io.h>
#include "CManyDownload.h"


// CManyDownload 对话框

IMPLEMENT_DYNAMIC(CManyDownload, CDialogEx)

CManyDownload::CManyDownload(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_ManyDownload, pParent)
{

}

CManyDownload::~CManyDownload()
{
}

void CManyDownload::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_TEXT, Loading);
}


BEGIN_MESSAGE_MAP(CManyDownload, CDialogEx)
	ON_BN_CLICKED(IDC_BUTTON1, &CManyDownload::OnBnClickedButton1)
	ON_BN_CLICKED(IDC_BUTTON2, &CManyDownload::OnBnClickedButton2)
	ON_STN_CLICKED(IDC_TEXT, &CManyDownload::OnStnClickedText)
END_MESSAGE_MAP()


// CManyDownload 消息处理程序

void CManyDownload::OnBnClickedButton1()
{
	if (_access("./Wallpaper.txt", 0) == -1)
	{
		MessageBox(TEXT("Wallpaperid.txt文件不存在！"), TEXT("提示"), MB_OK);
	} 
	else
	{
		MessageBox(TEXT("Wallpaperid.txt文件存在！"), TEXT("提示"), MB_OK);
	}
	// TODO: 在此添加控件通知处理程序代码
}

void CManyDownload::OnBnClickedButton2()
{
	// 检查文件是否存在
	if (_access("./WallpaperID.txt", 0) == -1)
	{
		MessageBox(TEXT("WallpaperID.txt不存在！"), TEXT("提示"), MB_OK | MB_ICONWARNING);
		return;
	}

	CString lines[10];
	int lineCount = 0;
	CStdioFile file;

	// 打开文件并读取前10个ID
	if (file.Open(TEXT("WallpaperID.txt"), CFile::modeRead))
	{
		CString line;
		while (lineCount < 10 && file.ReadString(line))
		{
			// 跳过空行
			if (!line.IsEmpty())
				lines[lineCount++] = line;
		}
		file.Close();
	}
	else
	{
		MessageBox(TEXT("无法打开文件！"), TEXT("错误"), MB_OK | MB_ICONERROR);
		return;
	}

	if (lineCount == 0)
	{
		MessageBox(TEXT("文件中没有有效的ID！"), TEXT("提示"), MB_OK);
		return;
	}

	// 确认下载
	CString msg;
	msg.Format(TEXT("准备下载 %d 个创意工坊内容，是否继续？"), lineCount);
	if (MessageBox(msg, TEXT("确认下载"), MB_YESNO | MB_ICONQUESTION) != IDYES)
		return;

	// 获取当前程序路径，构建steamcmd绝对路径
	TCHAR szPath[MAX_PATH];
	GetModuleFileName(NULL, szPath, MAX_PATH);
	PathRemoveFileSpec(szPath);
	CString steamcmdPath = CString(szPath) + TEXT("\\steamcmd\\");

	// 执行批量下载
	for (int i = 0; i < lineCount; i++)
	{
		// 更新进度显示
		msg.Format(TEXT("正在下载第 %d/%d 个: %s"), i + 1, lineCount, lines[i]);
		SetDlgItemText(IDC_TEXT, msg);
		UpdateWindow();

		// 构建命令行
		CString command;
		command.Format(
			TEXT("cd /d \"%s\" && steamcmd.exe +login kzeon410 wnq69815I +workshop_download_item %s +quit"),
			steamcmdPath, lines[i]);

		// 执行命令
		int result = _wsystem(command);

		// 检查结果
		if (result != 0)
		{
			msg.Format(TEXT("下载失败: %s\n错误码: %d"), lines[i], result);
			MessageBox(msg,TEXT("下载错误"), MB_OK | MB_ICONERROR);
		}
	}

	// 完成提示
	MessageBox(TEXT("所有内容下载完成！"), TEXT("完成"), MB_OK | MB_ICONINFORMATION);
	SetDlgItemText(IDC_TEXT, TEXT("下载已完成"));
	Sleep(3000);
	SetDlgItemText(IDC_TEXT, TEXT("进度：未知（上一次下载已完成！）"));
}

void CManyDownload::OnStnClickedText()
{
	// TODO: 在此添加控件通知处理程序代码
}
