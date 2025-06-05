#pragma once
/*
* downloader.hpp
* ���ش��⹤����Ʒ
*/

class Downloader {
private:
	CSWToolsDlg* pDlg;
	string last_path;

	// ����������

	void EnableBtns(BOOL bEnable) {
		pDlg->m_cboAppName.EnableWindow(bEnable);
		pDlg->m_edtID.EnableWindow(bEnable);
		pDlg->m_btnLaunch.EnableWindow(bEnable);
	}
	bool CheckSteamcmd() {
		pDlg->m_state.SetWindowText(_T("��� Steamcmd"));
		if (_access("Steamcmd/steamcmd.exe", 0) == -1) {
			pDlg->appendToDashboard(L"δ�ҵ� Steamcmd����������...\r\n");
			pDlg->appendToDashboard(L"��ע�ⲻҪʹ�ü�������\r\n");
			pDlg->m_state.SetWindowText(_T("���� Steamcmd..."));
			// ����steamcmd.exe
			CreateDirectoryA("steamcmd", NULL);
			int res = system(R"(cd .\steamcmd\ && curl -s -o "steamcmd.zip" "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip" && tar -xzvf steamcmd.zip && del steamcmd.zip)");
			if (res == 0) {
				pDlg->appendToDashboard(L"Steamcmd ������ɡ�\r\n");
			} else {
				pDlg->appendToDashboard(L"Steamcmd����ʧ�ܣ�������������\r\n");
				pDlg->m_state.SetWindowText(_T("Steamcmd ����ʧ�ܡ�"));
				return false;
			}
		}
		return true;
	}
	void DownloadFromId() {
		pDlg->m_state.SetWindowText(_T("�ȴ� Steamcmd ����"));
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
					pDlg->m_state.SetWindowText(_T("��������..."));
				} else {
					pDlg->m_state.SetWindowText(_T("�ȴ� Steamcmd ��ɸ���..."));
				}
				pDlg->appendToDashboard(ToWstr(msg).c_str());
				// TODO: ����ʵ��һ����̫���׵Ĳ���...Ҳ��...
				if (log.find("Unloading Steam API") != string::npos) {
					cmd.terminate();
					pDlg->appendToDashboard(L"����ֹ���� (��ֹ Steamcmd ����)��\r\n");
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
		// ����
		string path = "steamcmd\\steamapps\\workshop\\content\\" +
			downloadInfo.app_id + "\\" + downloadId + "\\";
		if (_access(path.c_str(), 0) == -1) {
			pDlg->m_state.SetWindowText(_T("����ʧ�ܣ�������־"));
			pDlg->appendToDashboard(_T("����ʧ�ܣ�"));
			if (log.find("File Not Found") != string::npos) {
				pDlg->appendToDashboard(_T("Ҫ���ص���Ʒδ�ҵ���\r\n"));
				pDlg->appendToDashboard(_T("��ʾ��������Ʒ ID �Ƿ���ȷ��\r\n"));
			} else if (log.find("Timeout") != string::npos) {
				pDlg->appendToDashboard(_T("���س�ʱ��\r\n"));
				pDlg->appendToDashboard(_T("��ʾ���볢���������ء�\r\n"));
			} else if (log.find("No Connection") != string::npos) {
				pDlg->appendToDashboard(_T("���������ӡ�\r\n"));
			} else if (log.find("Account Disabled") != string::npos) {
				pDlg->appendToDashboard(_T("�˺Ų����á��볢���򿪷��߷�ӳ�����⡣\r\n"));
			} else if (log.find("Invalid Password") != string::npos) {
				pDlg->appendToDashboard(_T("��������볢���򿪷��߷�ӳ�����⡣\r\n"));
			} else if (log.find("No match") != string::npos) {
				pDlg->appendToDashboard(_T("Steam App ����Ʒ ID ��ƥ�䡣\r\n"));
				pDlg->appendToDashboard(_T("��ʾ���������Ʒ�� Steam App����Ϸ���Ƿ��Ӧ��\r\n"));
			} else {
				pDlg->appendToDashboard(_T("δ֪����\r\n"));
			}
			return;
		}
		ExecuteCmd(L"explorer.exe", ToWstr(path), true);
		wchar_t fullpath[1024] = { 0 };
		_wfullpath(fullpath, ToWstr(path).c_str(), sizeof(fullpath) / sizeof(*fullpath));
		pDlg->m_state.SetWindowText(_T("���سɹ�"));
		pDlg->appendToDashboard((wstring(L"���⹤����Ʒ������ɣ������Ŀ��Ŀ¼��\r\n") + fullpath + L"\r\n").c_str());
		last_path = path;
	}

	// �����̵߳�ѭ������
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
		th.detach(); // �ſ�ط����߳�
	}
	~Downloader() {
		signal = SIGNAL_END;
		while (signal != SIGNAL_NULL);
	}

	void Download(wstring appname, wstring id) {
		EnableBtns(0);
		downloadInfo = app_infos[appname];
		downloadId = ToStr(id);
		signal = SIGNAL_DOWNLOAD; // �ſ�ؿ�ʼ����
	}
	void OpenLastFolder() {
		if (last_path == "") {
			MessageBox(NULL, L"��û�гɹ�������", L"��ʾ", MB_ICONINFORMATION | MB_OK);
		} else {
			ExecuteCmd(L"explorer.exe", ToWstr(last_path), true);
		}
	}
};