#include "SWTools.h"

void update() {
	cout << "检查更新中..." << endl;
	if (_access  (".\\version.txt" ,0) == 0) {
		remove ("version.txt");
	}
	string line;
	const wchar_t* url = L"https://king-zzk.github.io/version.txt";
	const wchar_t* path = L".\\version.txt";
	// 下载文件
	HRESULT hr = URLDownloadToFileW(NULL, url, path, 0, NULL);
	if (SUCCEEDED(hr)) {
		ifstream input(".\\version.txt");
		if (input.is_open()) {
			while (getline(input, line)) {
				if (line == "0.1.9") {
					cout << "当前版本为最新版本" << endl;
					input.close();
					remove ("version.txt");
					useragreement();
				}
				else {
					cout << "当前不是最新版本，最新版本为：" << line << endl;
					cout << "请下载最新版本" << endl;
					input.close();
					remove ("version.txt");
					useragreement();
				}
			}
		}
	}
	else {
		cout << "检查失败(检查是否打开加速器，没有请打开！)" << endl;
		useragreement();
	}
}