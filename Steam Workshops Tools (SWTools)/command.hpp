#pragma once
/*
* command.hpp
* 解析指令
*/

// 返回 false 表示要退出程序
bool command() {
	// 提示用户输入
	cout << "> ";
	string opt;
	getline(cin, opt);
	// 命令分支
	if (opt == "exit") {
		return false;
	} else if (opt == "clear") {
		system("cls");
	} else if (opt == "help") {
		cout << "\033[36m" << text::usage << "\033[0m" << endl;
	} else if (opt == "about") {
		cout << "\033[36m" << text::about << "\033[0m" << endl;
	} else if (opt == "eula") {
		cout << "\033[36m" << text::eula << "\033[0m" << endl;
		cout << "您已同意用户协议" << endl;
	} else if (app_cmd.find(opt) != app_cmd.end()) {
		download(app_cmd[opt]);
	} else {
		cout << "无效指令，使用 help 指令获取帮助" << endl;
	}
	return true;
}