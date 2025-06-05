#pragma once
/*
* downloader.hpp
* 下载创意工坊物品
*/

class Downloader {
private:
	CSWToolsDlg* pDlg;
	string last_path;

	// 下载子任务

	void EnableBtns(BOOL bEnable) {
		pDlg->m_cboAppName.EnableWindow(bEnable);
		pDlg->m_edtID.EnableWindow(bEnable);
		pDlg->m_btnLaunch.EnableWindow(bEnable);
	}
	bool CheckSteamcmd() {
		pDlg->m_state.SetWindowText(_T("检查 Steamcmd"));
		if (_access("Steamcmd/steamcmd.exe", 0) == -1) {
			pDlg->appendToDashboard(L"未找到 Steamcmd，正在下载...\r\n");
			pDlg->appendToDashboard(L"请注意不要使用加速器！\r\n");
			pDlg->m_state.SetWindowText(_T("下载 Steamcmd..."));
			// 下载steamcmd.exe
			CreateDirectoryA("steamcmd", NULL);
			int res = system(R"(cd .\steamcmd\ && curl -s -o "steamcmd.zip" "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip" && tar -xzvf steamcmd.zip && del steamcmd.zip)");
			if (res == 0) {
				pDlg->appendToDashboard(L"Steamcmd 下载完成。\r\n");
			} else {
				pDlg->appendToDashboard(L"Steamcmd下载失败，请检查网络连接\r\n");
				pDlg->m_state.SetWindowText(_T("Steamcmd 下载失败。"));
				return false;
			}
		}
		return true;
	}
	void DownloadFromId() {
		pDlg->m_state.SetWindowText(_T("等待 Steamcmd 启动"));
		string command = "steamcmd\\steamcmd.exe +login " + downloadInfo.user + " " + downloadInfo.password +
			" +workshop_download_item " + downloadInfo.app_id + " " + downloadId + " +quit";
		mlib::process::Process cmd(command);
		string msg, log;
		DWORD peek_size;
		while (true) {
			peek_size = cmd.peek();
			while (peek_size > 0) {
				msg = cmd.readline();
				msg += "\r\n";
				log += msg;
				if (msg.find("Downloading update") == string::npos) {
					pDlg->m_state.SetWindowText(_T("正在下载..."));
				} else {
					pDlg->m_state.SetWindowText(_T("等待 Steamcmd 完成更新..."));
				}
				pDlg->appendToDashboard(ToWstr(msg).c_str());
				// TODO: 这其实是一个不太靠谱的策略...也许...
				if (log.find("Unloading Steam API") != string::npos) {
					cmd.terminate();
					pDlg->appendToDashboard(L"已终止进程 (阻止 Steamcmd 更新)。\r\n");
					break;
				}
				peek_size = cmd.peek();
				Sleep(1);
			}
			if (cmd.get_exit_code() != STILL_ACTIVE) {
				Sleep(10);
				if (cmd.get_exit_code() != STILL_ACTIVE) break;
			}
		}
		// 后处理
		string path = "steamcmd\\steamapps\\workshop\\content\\" +
			downloadInfo.app_id + "\\" + downloadId + "\\";
		if (_access(path.c_str(), 0) == -1) {
			pDlg->m_state.SetWindowText(_T("下载失败，请检查日志"));
			pDlg->appendToDashboard(_T("下载失败，"));
			if (log.find("File Not Found") != string::npos) {
				pDlg->appendToDashboard(_T("要下载的物品未找到。\r\n"));
				pDlg->appendToDashboard(_T("提示：请检查物品 ID 是否正确。\r\n"));
			} else if (log.find("Timeout") != string::npos) {
				pDlg->appendToDashboard(_T("下载超时。\r\n"));
				pDlg->appendToDashboard(_T("提示：请尝试重新下载。\r\n"));
			} else if (log.find("No Connection") != string::npos) {
				pDlg->appendToDashboard(_T("无网络连接。\r\n"));
			} else if (log.find("Account Disabled") != string::npos) {
				pDlg->appendToDashboard(_T("账号不可用。请尝试向开发者反映此问题。\r\n"));
			} else if (log.find("Invalid Password") != string::npos) {
				pDlg->appendToDashboard(_T("密码错误。请尝试向开发者反映此问题。\r\n"));
			} else if (log.find("No match") != string::npos) {
				pDlg->appendToDashboard(_T("Steam App 与物品 ID 不匹配。\r\n"));
				pDlg->appendToDashboard(_T("提示：请检查该物品与 Steam App（游戏）是否对应。\r\n"));
			} else {
				pDlg->appendToDashboard(_T("未知错误。\r\n"));
			}
			return;
		}
		ExecuteCmd(L"explorer.exe", ToWstr(path), true);
		wchar_t fullpath[1024] = { 0 };
		_wfullpath(fullpath, ToWstr(path).c_str(), sizeof(fullpath) / sizeof(*fullpath));
		pDlg->m_state.SetWindowText(_T("下载成功"));
		pDlg->appendToDashboard((wstring(L"创意工坊物品下载完成，将会打开目标目录：\r\n") + fullpath + L"\r\n").c_str());
		last_path = path;
	}

	// 任务线程的循环函数
	const int SIGNAL_NULL = 0;
	const int SIGNAL_DOWNLOAD = 1;
	const int SIGNAL_END = -1;
	int signal = SIGNAL_NULL;
	AppInfo downloadInfo;
	string downloadId;
	void TaskThreadCallback() {
		while (signal != SIGNAL_END) {
			if (signal == SIGNAL_DOWNLOAD) {
				if (CheckSteamcmd()) {
					DownloadFromId();
				}
				EnableBtns(1);
				signal = SIGNAL_NULL;
			}
		}
		signal = SIGNAL_NULL;
	}
public:
	Downloader(CSWToolsDlg* pDlg) :pDlg(pDlg) {
		thread th(&Downloader::TaskThreadCallback, this);
		th.detach(); // 炫酷地分离线程
	}
	~Downloader() {
		signal = SIGNAL_END;
		while (signal != SIGNAL_NULL);
	}

	void Download(wstring appname, wstring id) {
		EnableBtns(0);
		downloadInfo = app_infos[appname];
		downloadId = ToStr(id);
		signal = SIGNAL_DOWNLOAD; // 炫酷地开始下载
	}
	void OpenLastFolder() {
		if (last_path == "") {
			MessageBox(NULL, L"还没有成功的下载", L"提示", MB_ICONINFORMATION | MB_OK);
		} else {
			ExecuteCmd(L"explorer.exe", ToWstr(last_path), true);
		}
	}
};