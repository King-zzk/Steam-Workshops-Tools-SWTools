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
		int res = system(R"(cd .\steamcmd\ && curl -o "steamcmd.zip" "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip" && tar -xzvf steamcmd.zip && del steamcmd.zip)");
		if (res == 0) {
			cout << "�������" << endl;
		} else {
			cout << "\033[31m����steamcmd����ʧ�ܣ�������������\033[0m" << endl;
			return;
		}
	}

	cout << "���⹤����ƷURLʾ��: https://steamcommunity.com/sharedfiles/filedetails/?id=\033[33mXXXXXXX\033[0m" << endl;
	cout << "����\033[33mXXXXXXX\033[0mΪ���⹤����Ʒ���" << endl << endl;
	string id;
begin:
	cout << "����Ҫ���صĴ��⹤����Ʒ���:" << endl << "> ";
	getline(cin, id);
	if (id.find_first_not_of("0123456789") != string::npos) {
		cout << "��Ч�� id, ������" << endl;
		goto begin;
	}
	cout << "��ʼ���ش��⹤����Ʒ" << endl;
	string command = "steamcmd\\steamcmd.exe +login " + info.user + " " + info.password + 
		" +workshop_download_item " + info.app_id + " " + id + " +quit";
	system(command.c_str()); // ���д����ȴ�steamcmd.exeִ�����
	// �򿪴��⹤���ļ���
	string path = "steamcmd\\steamapps\\workshop\\content\\" + info.app_id + "\\" + id + "\\";
	if (_access(path.c_str(), 0) == -1) {
		cout << "\033[31m���󣺴��⹤����Ʒ����ʧ��(�������Timeout������)\033[0m" << endl;
		return;
	}
	string explorerCommand = "explorer.exe " + path;
	system(explorerCommand.c_str()); // ���д����ȴ�explorer.exeִ�����
	cout << "������ɣ������Ŀ��Ŀ¼" << endl;
}