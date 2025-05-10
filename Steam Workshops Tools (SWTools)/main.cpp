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
		cout << "\033[31m错误: " << e.what() << "\033[0m" << endl;
		cout << "\033[33m警告: 正在跳过更新检查\033[0m" << endl;
	}
	cout << endl;

	// 必须同意用户协议
	if (not elua()) {
		pause();
		return 0;
	}

	// 命令循环
	cout << "\033[36m" << text::usage << "\033[0m" << endl;
	while (command());
	return 0;
}