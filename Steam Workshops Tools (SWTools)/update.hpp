#pragma once
/*
* upgrade.hpp
* ������
*/

void check_update() {
	cout << "��������..." << endl;
	fstream file("version.txt", ios::in);

	// ��鲢ɾ���ɵ� headline.txt �ļ�
	if (file.is_open()) {
		file.close();
		if (remove("version.txt") != 0) {
			throw exception(("�ļ�ɾ��ʧ��(����" + to_string(errno) + ")���볢���ֶ�ɾ����Ŀ¼�µ�version.txt").c_str());
		}
	}

	// �����µ� headline.txt �ļ� (�ô�����վ)
	const char url[] = "https://gh-proxy.net/https://raw.githubusercontent.com/King-zzk/king-zzk.github.io/refs/heads/main/version.txt";
	const char path[] = "version.txt";

	if (URLDownloadToFileA(NULL, url, path, 0, NULL) == 0) {
		file.open("version.txt", ios::in);
		if (file.is_open()) {
			string line;
			if (getline(file, line)) { // ֻ��ȡ��һ��
				if (line == text::version) {
					cout << "��ǰ�汾Ϊ���°汾" << endl;
				} else {
					cout << "��⵽�°汾��\033[36m" << line << "\033[0m ��ǰ�汾��" << text::version << endl;
					cout << "��ǰ�� " + text::website + " �������°汾" << endl;
				}
				file.close();
			} else {
				file.close();
				throw exception("version.txt�е�������Ч");
			}
		} else {
			throw exception("�޷���ȡversion.txt");
		}

		// ɾ���ļ�
		if (remove("version.txt") != 0) {
			cout << "\033[33m����: �ļ�ɾ��ʧ��(����" << errno << ")���볢���ֶ�ɾ����Ŀ¼�µ�version.txt\033[0m" << endl;
		}
	} else {
		throw exception("�޷���ȡ���°汾��Ϣ");
	}
}