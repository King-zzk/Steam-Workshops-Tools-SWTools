#include "SWTools.h"

void hoi4() {
	//����Ƿ���steam�ļ���
	if (_access(".\\Steamcmd", 0) == -1) {
		system("mkdir Steamcmd");
	}
	//����
	system("cls");
	cout << "���⹤����ţ�ʾ������https://steamcommunity.com/sharedfiles/filedetails/?id=XXXXXXX&searchtext=" << endl;
	cout << "��id���������Ϊ���⹤�����" << endl;
	cout << endl;
	cout << "��������ĸ�������4�Ĵ��⹤�����:";
	string input;
	getline(cin, input);
	//��������Ƿ�Ϊ����
	if (input.find_first_not_of("0123456789") != string::npos) {
		cout << "�����������������룡" << endl;
		return hoi4();
	}
	else {
		//�����steam�ļ����Ƿ���steamcmd.exe
		if (_access(".\\Steamcmd\\steamcmd.exe", 0) == -1) {
			cout << "Steamcmd.exeδ�ҵ�����������..." << endl;
			cout << "ע���벻Ҫʹ�ü�������" << endl;
			//����steamcmd.exe
			int downloadResult = system(R"(cd .\Steamcmd\ && curl -o "steamcmd.zip" "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip" && tar -xzvf steamcmd.zip && del steamcmd.zip)");
			if (downloadResult == 0) {
				cout << "������ɣ�" << endl;
			}
			else {
				cout << "����ʧ�ܣ�(������������)" << endl;
				return transmit();
			}
		}
	}
	//���ش��⹤���ļ�
	cout << "�������ش��⹤���ļ���..." << endl;
	string command = ".\\Steamcmd\\steamcmd.exe +login thb112259 steamok7416 +workshop_download_item 394360 " + input + " +quit";
	system(command.c_str()); // ���д����ȴ�steamcmd.exeִ�����
	//�򿪴��⹤���ļ���
	string path = ".\\Steamcmd\\steamapps\\workshop\\content\\394360\\" + input + "\\";
	// ʹ����ȷ�������ʽ�����ļ���
	if (_access(path.c_str(), 0) == -1) {
		cout << "����ʧ�ܣ�(�������Timeout������)" << endl;
		return transmit();
	}
	string explorerCommand = "explorer.exe " + path;
	system(explorerCommand.c_str()); // ���д����ȴ�explorer.exeִ�����
	cout << "������ɣ�(�������timeout�������ԣ�)" << endl;
	return transmit();
}