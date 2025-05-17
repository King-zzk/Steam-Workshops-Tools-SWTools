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
		cout << "����: " << e.what() << endl;
		cout << "����: �����������¼��" << endl;
	}
	cout << endl;

	// ����ͬ���û�Э��
	if (not elua()) {
		pause();
		return 0;
	}

	// ����ѭ��
	cout << text::usage  << endl;
	while (command());
	return 0;
}