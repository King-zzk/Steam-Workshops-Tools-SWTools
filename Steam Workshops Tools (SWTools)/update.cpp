#include "SWTools.h"

void update() {
	cout << "��������..." << endl;
	string line;
	const wchar_t* url = L"https://king-zzk.github.io/version.txt";
	const wchar_t* path = L"C:\\Windows\\Temp\\version.txt";
	// �����ļ�
	HRESULT hr = URLDownloadToFileW(NULL, url, path, 0, NULL);
	if (SUCCEEDED(hr)) {
		ifstream input("C:\\Windows\\Temp\\version.txt");
		if (input.is_open()) {
			while (getline(input, line)) {
				if (line == "0.1.9") {
					cout << "��ǰ�汾Ϊ���°汾" << endl;
					useragreement();
				}
				else {
					cout << "���������°汾�����°汾Ϊ��" << line << endl;
					cout << "���������°汾" << endl;
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