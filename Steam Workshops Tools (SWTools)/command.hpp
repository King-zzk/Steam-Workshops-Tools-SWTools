#pragma once
/*
* command.hpp
* ����ָ��
*/

// ���� false ��ʾҪ�˳�����
bool command() {
	// ��ʾ�û�����
	cout << "swtools> ";
	string opt;
	cin >> opt;
	// �����֧
	if (opt == "exit") {
		return false;
	} else if (opt == "clear") {
		system("cls");
	} else if (opt == "help") {
		cout << text::usage << endl;
	} else if (opt == "about") {
		cout << text::about << endl;
	} else if (opt == "eula") {
		cout << text::eula << endl;
		cout << "����ͬ���û�Э��" << endl;
	} else if (app_cmd.find(opt) != app_cmd.end()) {
		download(app_cmd[opt]);
	} else {
		cout << "��Чָ�ʹ�� help ָ���ȡ����" << endl;
	}
	return true;
}