#pragma once
/*
* downloader.hpp
* 下载创意工坊物品
*/

clock_t last_print = 0;

// app_cmd: steam app 编号
void download(AppInfo info) {
	// 检查在steam文件夹是否有steamcmd.exe
	if (_access("Steamcmd/steamcmd.exe", 0) == -1) {
		cout << "steamcmd未找到，正在下载..." << endl;
		cout << "请注意不要使用加速器！" << endl;
		// 下载steamcmd.exe
		CreateDirectoryA("steamcmd", NULL);
		int res = system(R"(cd .\steamcmd\ && curl -s -o "steamcmd.zip" "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip" && tar -xzvf steamcmd.zip && del steamcmd.zip)");
		if (res == 0) {
			cout << "下载完成" << endl;
		} else {
			cout << "错误：steamcmd下载失败，请检查网络连接" << endl;
			return;
		}
	}

	cout << "创意工坊物品URL示例: https://steamcommunity.com/sharedfiles/filedetails/?id=XXXXXXX" << endl;
	cout << "                                                      创意工坊物品编号~~~~~~^" << endl;
	string id;
begin:
	cout << "在此输入要下载的物品编号> ";
	cin >> id;
	if (id.find_first_not_of("0123456789") != string::npos) {
		cout << "无效的 id, 请重试" << endl;
		goto begin;
	}
	cout << "开始下载创意工坊物品" << endl;

	cout << "--------以下输出来自 steamcmd.exe--------" << endl;
	string command = "steamcmd\\steamcmd.exe +login " + info.user + " " + info.password +
		" +workshop_download_item " + info.app_id + " " + id + " +quit";
	mlib::process::Process cmd(command);
	std::string msg, log;
	bool last_is_update_msg = false;
	while (true) {
		if (cmd.get_exit_code() != STILL_ACTIVE) {
			Sleep(3000);
			if (cmd.get_exit_code() != STILL_ACTIVE) break;
		}
		if (cmd.peek() > 0) {
			msg = cmd.readline();
			log += msg;
			if (msg.find("Downloading update") == string::npos and last_is_update_msg) {
				if (last_is_update_msg) cout << endl;
			} else {
				cout << "\r";
			}
			cout << msg;
			if (msg.find("Downloading update") == string::npos) {
				last_is_update_msg = false;
				cout << endl;
			} else {
				last_is_update_msg = true;
			}
			Sleep(1);
		}
	}
	cout << "--------------------------------" << endl;

	string path = "steamcmd\\steamapps\\workshop\\content\\" + info.app_id + "\\" + id + "\\";
	if (_access(path.c_str(), 0) == -1) {
		cout << "错误：下载失败，";
		if (log.find("File Not Found") != string::npos) {
			cout << "要下载的物品未找到" << endl;
			cout << "提示：检查编号是否正确、该物品与游戏名是否对应" << endl;
		} else if (log.find("Timeout") != string::npos) {
			cout << "下载超时" << endl;
			cout << "提示：请尝试重新下载" << endl;
		} else if (log.find("No Connection") != string::npos) {
			cout << "无网络连接" << endl;
		} else {
			cout << "未知错误" << endl;
		}
		return;
	}
	string explorerCommand = "explorer.exe " + path;
	system(explorerCommand.c_str()); // 这行代码会等待explorer.exe执行完毕
	char fullpath[1024] = { 0 };
	_fullpath(fullpath, path.c_str(), sizeof(fullpath));
	cout << "下载完成，将会打开目标目录 " << fullpath << endl;
}