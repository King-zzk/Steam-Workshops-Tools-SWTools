#include <iostream>
#include <fstream>
#include <sstream>		// ostringstream
#include <iomanip>		// setprecision()
#include <exception>
#include <windows.h>
#include <io.h> // _access()
#include <map>
#pragma comment(lib, "urlmon.lib") // URLDownloadToFile()
using namespace std;

inline void pause() {
	cout << "���س��˳�����";
	cin.get();
}

#include "texts.hpp"
#include "update.hpp"
#include "eula.hpp"
#include "app_info.hpp"
#include "downloader.hpp"
#include "command.hpp"

int main() {
	// ��ӡ�汾�Ͱ�Ȩ����
	cout << text::headline << endl;

	// ������
	try {
		check_update();
	} catch (exception e) {
		cout << "\033[31m����: " << e.what() << "\033[0m" << endl;
		cout << "\033[33m����: �����������¼��\033[0m" << endl;
	}
	cout << endl;

	// ����ͬ���û�Э��
	if (not elua()) {
		pause();
		return 0;
	}

	// ����ѭ��
	cout << "\033[36m" << text::usage << "\033[0m" << endl;
	while (command());
	return 0;
}