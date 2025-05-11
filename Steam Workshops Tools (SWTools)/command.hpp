#pragma once
/*
* command.hpp
* ����ָ��
*/

// ���� false ��ʾҪ�˳�����
bool command() {
	// ��ʾ�û�����
	cout << "> ";
	string opt;
	getline(cin, opt);
	// �����֧
	if (opt == "exit") {
		return false;
	} else if (opt == "clear") {
		system("cls");
	} else if (opt == "help") {
		cout << "\033[36m" << text::usage << "\033[0m" << endl;
	} else if (opt == "about") {
		cout << "\033[36m" << text::about << "\033[0m" << endl;
	} else if (opt == "eula") {
		cout << "\033[36m" << text::eula << "\033[0m" << endl;
		cout << "����ͬ���û�Э��" << endl;
	} else if (app_cmd.find(opt) != app_cmd.end()) {
		download(app_cmd[opt]);
	} else {
		cout << "��Чָ�ʹ�� help ָ���ȡ����" << endl;
	}
	return true;
}