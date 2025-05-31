#pragma once
/*
* command.hpp
* 解析指令
*/

// 返回 false 表示要退出程序
bool command() {
	// 提示用户输入
	cout << "swtools> ";
	string opt;
	cin >> opt;
	// 命令分支
	if (opt == "exit") {
		return false;
	} else if (opt == "clear") {
		system("cls");
	} else if (opt == "help") {
		cout << text::usage << endl;
	} else if (opt == "about") {
		cout << text::about << endl;
	} else if (opt == "eula") {
		cout << text::eula << endl;
		cout << "您已同意用户协议" << endl;
	} else if (app_cmd.find(opt) != app_cmd.end()) {
		download(app_cmd[opt]);
	} else {
		cout << "无效指令，使用 help 指令获取帮助" << endl;
	}
	return true;
}