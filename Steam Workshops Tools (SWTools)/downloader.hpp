#pragma once
/*
* downloader.hpp
* ���ش��⹤����Ʒ
*/

clock_t last_print = 0;

// app_cmd: steam app ���
void download(AppInfo info) {
	// �����steam�ļ����Ƿ���steamcmd.exe
	if (_access("Steamcmd/steamcmd.exe", 0) == -1) {
		cout << "steamcmdδ�ҵ�����������..." << endl;
		cout << "ע���벻Ҫʹ�ü�������" << endl;
		// ����steamcmd.exe
		CreateDirectoryA("steamcmd", NULL);
		int res = system(R"(cd .\steamcmd\ && curl -s -o "steamcmd.zip" "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip" && tar -xzvf steamcmd.zip && del steamcmd.zip)");
		if (res == 0) {
			cout << "�������" << endl;
		} else {
			cout << "����steamcmd����ʧ�ܣ�������������" << endl;
			return;
		}
	}

	cout << "���⹤����ƷURLʾ��: https://steamcommunity.com/sharedfiles/filedetails/?id=XXXXXXX" << endl;
	cout << "                                                      ���⹤����Ʒ���~~~~~~^" << endl << endl;
	string id;
begin:
	cout << "�ڴ�����Ҫ���ص���Ʒ���> ";
	getline(cin, id);
	if (id.find_first_not_of("0123456789") != string::npos) {
		cout << "��Ч�� id, ������" << endl;
		goto begin;
	}
	cout << "��ʼ���ش��⹤����Ʒ" << endl;

	cout << "--------����������� steamcmd.exe--------" << endl;
	string command = "steamcmd\\steamcmd.exe +login " + info.user + " " + info.password +
		" +workshop_download_item " + info.app_id + " " + id + " +quit";
	mlib::process::Process cmd(command);
	std::string msg, log;
	while (cmd.get_exit_code() == STILL_ACTIVE) {
		if (cmd.peek() > 0) {
			msg = cmd.read(cmd.peek());
			log += msg;
			cout << msg;
			Sleep(1);
		}
	}
	cout << "--------------------------------" << endl;

	string path = "steamcmd\\steamapps\\workshop\\content\\" + info.app_id + "\\" + id + "\\";
	if (_access(path.c_str(), 0) == -1) {
		cout << "��������ʧ�ܣ�";
		if (log.find("File Not Found") != string::npos) {
			cout << "Ҫ���ص���Ʒδ�ҵ�" << endl;
			cout << "��ʾ��������Ƿ���ȷ������Ʒ����Ϸ���Ƿ��Ӧ" << endl;
		} else if (log.find("Timeout") != string::npos) { // TODO���������߼��Ƿ���ȷ����û�������� Timeout�������
			cout << "���س�ʱ(������)" << endl;			// >û������
			cout << "��ʾ��������" << endl;
		} else if (log.find("No Connection") != string::npos) {
			cout << "����������" << endl;
		} else {
			cout << "δ֪����" << endl;
		}
		return;
	}
	string explorerCommand = "explorer.exe " + path;
	system(explorerCommand.c_str()); // ���д����ȴ�explorer.exeִ�����
	cout << "������ɣ������Ŀ��Ŀ¼ " << path << endl;
}