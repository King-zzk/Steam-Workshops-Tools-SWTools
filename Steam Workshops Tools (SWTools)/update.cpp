#include "SWTools.h"

void update() {
	cout << "��������..." << endl;
	if (_access  (".\\version.txt" ,0) == 0) {
		remove ("version.txt");
	}
	string line;
	const wchar_t* url = L"https://king-zzk.github.io/version.txt";
	const wchar_t* path = L".\\version.txt";
	// �����ļ�
	HRESULT hr = URLDownloadToFileW(NULL, url, path, 0, NULL);
	if (SUCCEEDED(hr)) {
		ifstream input(".\\version.txt");
		if (input.is_open()) {
			while (getline(input, line)) {
				if (line == "0.1.9") {
					cout << "��ǰ�汾Ϊ���°汾" << endl;
					input.close();
					remove ("version.txt");
					useragreement();
				}
				else {
					cout << "��ǰ�������°汾�����°汾Ϊ��" << line << endl;
					cout << "���������°汾" << endl;
					input.close();
					remove ("version.txt");
					useragreement();
				}
			}
		}
	}
	else {
		cout << "���ʧ��(����Ƿ�򿪼�������û����򿪣�)" << endl;
		useragreement();
	}
}