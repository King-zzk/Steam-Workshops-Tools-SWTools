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
	ON_BN_CLICKED(IDC_MANYDWNLAUNCH, &CManyDownload::OnBnClickedManydwnlaunch)
END_MESSAGE_MAP()


// CManyDownload 消息处理程序

void CManyDownload::OnBnClickedManydwnlaunch() {
	// 检查文件是否存在  
	if (_access("./WallpaperID.txt", 0) == -1) {
		MessageBox(TEXT("WallpaperID.txt不存在！"), TEXT("提示"), MB_OK | MB_ICONWARNING);
		return;
	}

	// 检查steamcmd目录是否存在，不存在则创建并下载
	bool needSteamcmd = (_access("./steamcmd", 0) == -1);
	if (needSteamcmd) {
		CString Path = TEXT("steamcmd");
		bool flag = CreateDirectory(Path, NULL);
		if (!flag) {
			MessageBox(TEXT("无法创建steamcmd目录，请检查权限！"), TEXT("错误"), MB_OK | MB_ICONERROR);
			return;
		}

		// 下载steamcmd.zip
		CString downloadCmd = TEXT("curl -s https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip -o steamcmd.zip");
		int downloadResult = system(CT2A(downloadCmd));
		if (downloadResult != 0) {
			MessageBox(TEXT("下载steamcmd.zip失败，请检查网络连接！"), TEXT("错误"), MB_OK | MB_ICONERROR);
			return;
		}

		// 解压steamcmd.zip
		CString extractCmd = TEXT("tar -xf steamcmd.zip");
		int extractResult = system(CT2A(extractCmd));
		if (extractResult != 0) {
			MessageBox(TEXT("解压steamcmd.zip失败，请确保tar命令可用！"), TEXT("错误"), MB_OK | MB_ICONERROR);
			return;
		}
	}

	// 读取文件内容
	CString lines[128];  // 增大数组大小为128
	int lineCount = 0;
	CStdioFile file;

	// 打开文件并读取ID
	if (file.Open(TEXT("WallpaperID.txt"), CFile::modeRead)) {
		CString line;
		while (lineCount < 128 && file.ReadString(line)) {
			// 跳过空行  
			if (!line.IsEmpty())
				lines[lineCount++] = line;
		}
		file.Close();
	}
	else {
		MessageBox(TEXT("无法打开文件！"), TEXT("错误"), MB_OK | MB_ICONERROR);
		return;
	}

	if (lineCount == 0) {
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
	int successCount = 0;
	int failedCount = 0;
	CString failedIDs;

	for (int i = 0; i < lineCount; i++) {
		// 更新进度显示  
		msg.Format(TEXT("正在下载第 %d/%d 个: %s"), i + 1, lineCount, lines[i]);
		SetDlgItemText(IDC_TEXT, msg);
		UpdateWindow();

		// 构建命令行  
		CString command;
		command.Format(
			TEXT("cd /d \"%s\" && steamcmd.exe +login kzeon410 wnq69815I +workshop_download_item %s +quit"),
			steamcmdPath, lines[i]);

		// 执行命令并获取结果  
		int result = system(CT2A(command));
		if (result != 0) {
			failedCount++;
			failedIDs += lines[i] + TEXT("\n");
			msg.Format(TEXT("下载失败: %s"), lines[i]);
			MessageBox(msg, TEXT("下载错误"), MB_OK | MB_ICONERROR);
		}
		else {
			successCount++;
		}
	}

	// 完成提示  
	if (failedCount == 0) {
		MessageBox(TEXT("所有内容下载完成！"), TEXT("完成"), MB_OK | MB_ICONINFORMATION);
	}
	else {
		msg.Format(TEXT("下载完成！成功: %d，失败: %d\n\n失败的ID:\n%s"),
			successCount, failedCount, failedIDs);
		MessageBox(msg, TEXT("下载结果"), MB_OK | MB_ICONWARNING);
	}

	SetDlgItemText(IDC_TEXT, TEXT("下载已完成"));
}