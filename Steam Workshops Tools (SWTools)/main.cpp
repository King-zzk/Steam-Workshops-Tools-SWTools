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
	cout << "按回车退出程序";
	cin.get();
}

#include "texts.hpp"
#include "update.hpp"
#include "eula.hpp"
#include "app_info.hpp"
#include "downloader.hpp"
#include "command.hpp"

int main() {
	// 打印版本和版权声明
	cout << text::headline << endl;

	// 检查更新
	try {
		check_update();
	} catch (exception e) {
		cout << "错误: " << e.what() << endl;
		cout << "警告: 正在跳过更新检查" << endl;
	}
	cout << endl;

	// 必须同意用户协议
	if (not elua()) {
		pause();
		return 0;
	}

	// 命令循环
	cout << text::usage  << endl;
	while (command());
	return 0;
}