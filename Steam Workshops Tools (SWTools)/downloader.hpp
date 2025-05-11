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
		cout << "注意请不要使用加速器！" << endl;
		// 下载steamcmd.exe
		CreateDirectoryA("steamcmd", NULL);
		int res = system(R"(cd .\steamcmd\ && curl -o "steamcmd.zip" "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip" && tar -xzvf steamcmd.zip && del steamcmd.zip)");
		if (res == 0) {
			cout << "下载完成" << endl;
		} else {
			cout << "\033[31m错误：steamcmd下载失败，请检查网络连接\033[0m" << endl;
			return;
		}
	}

	cout << "创意工坊物品URL示例: https://steamcommunity.com/sharedfiles/filedetails/?id=\033[33mXXXXXXX\033[0m" << endl;
	cout << "其中\033[33mXXXXXXX\033[0m为创意工坊物品编号" << endl << endl;
	string id;
begin:
	cout << "输入要下载的创意工坊物品编号:" << endl << "> ";
	getline(cin, id);
	if (id.find_first_not_of("0123456789") != string::npos) {
		cout << "无效的 id, 请重试" << endl;
		goto begin;
	}
	cout << "开始下载创意工坊物品" << endl;
	string command = "steamcmd\\steamcmd.exe +login " + info.user + " " + info.password + 
		" +workshop_download_item " + info.app_id + " " + id + " +quit";
	system(command.c_str()); // 这行代码会等待steamcmd.exe执行完毕
	// 打开创意工坊文件夹
	string path = "steamcmd\\steamapps\\workshop\\content\\" + info.app_id + "\\" + id + "\\";
	if (_access(path.c_str(), 0) == -1) {
		cout << "\033[31m错误：创意工坊物品下载失败(如果出现Timeout请重试)\033[0m" << endl;
		return;
	}
	string explorerCommand = "explorer.exe " + path;
	system(explorerCommand.c_str()); // 这行代码会等待explorer.exe执行完毕
	cout << "下载完成，将会打开目标目录" << endl;
}