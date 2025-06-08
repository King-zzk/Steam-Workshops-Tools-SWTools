#pragma once
/*
* eula.hpp
* �����û����Э��
*/

// ����û��Ƿ�ͬ�����û�Э��
bool CheckEula() {
	wfstream file("eula.txt", wios::in);
	wchar_t buf[1024];
	wstring wstr;
	if (file.is_open()) {
		file.getline(buf, 1024);
		wstr = buf;
		// �ļ����Ѿ���¼ͬ����
		if (wstr.find(text::eula_accepted_eng) != string::npos) {
			file.close();
			return true;
		}
	}
	file.close();
	return false;
}

// ��¼ͬ����Ϣ
bool WriteEula() {
	wfstream file("eula.txt", wios::out);
	if (!file.is_open()) {
		return false;
	}
	file << text::eula_accepted_eng.c_str() << endl;
	file.close();
	return true;
}
