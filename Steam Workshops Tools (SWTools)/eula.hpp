#pragma once
/*
* elua.hpp
* �����û����Э��
*/

// ����ֵ: �û��Ƿ�ͬ�����û�Э��
bool elua() {
	fstream file("eula.txt", ios::in);
	string buf;
	if (file.is_open()) {
		file >> buf;
		// �ļ����Ѿ���¼ͬ����
		if (buf.find(text::eula_accepted) != string::npos) {
			file.close();
			cout << "����ͬ���û�Э��" << endl;
			return true;
		}
		file.close();
	}

	cout << "Ҫʹ�ñ���������Ķ����������������û����Э��(EULA): " << endl;
	cout <<"\033[36m"<< text::eula << "\033[0m" << endl;
	cout << "�Ƿ��������Э��? (y/n)" << endl << "> ";

	getline(cin, buf);
	if (buf == "Y" or buf == "y") {
		cout << "����ͬ���û�Э��" << endl;
		file.open("eula.txt", ios::out);
		if (not file.is_open()) {
			throw exception("�޷�д�뵽eula.txt");
		}
		file << text::eula_accepted << endl;
		file.close();
		return true;
	} else if (buf == "N" or buf == "n") {
		cout << "��δͬ���û�Э�飬����ʹ�ñ����" << endl;
		return false;
	}

	cout << "��Ч����";
	return false;
}
