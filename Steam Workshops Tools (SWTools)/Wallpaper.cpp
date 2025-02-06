#include "SWTools.h"

void wallpaper() {
	//检查是否有steam文件夹
	if (_access(".\\Steam", 0) == -1) {
		system("mkdir Steam");
	}
	//清屏
	system("cls");
	cout << "创意工坊编号（示例）：https://steamcommunity.com/sharedfiles/filedetails/?id=XXXXXXX&searchtext=" << endl;
	cout << "在id后面的数字为创意工坊编号" << endl;
	cout << endl;
	cout << "请输入你的Wallpaper Engine的创意工坊编号:";
	string input;
	getline(cin, input);
	//检查输入是否为数字
	if (input.find_first_not_of("0123456789")!= string::npos) {
		cout << "输入有误，请重新输入！" << endl;
		wallpaper();
	} else {
			//检查在steam文件夹是否有steamcmd.exe
			if (_access(".\\Steam\\steamcmd.exe", 0) == -1) {
			cout << "Steamcmd.exe未找到，正在下载..." << endl;
			//下载steamcmd.exe
			int downloadResult = system(R"(cd .\Steam\ && curl -o "steamcmd.zip" "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip" && tar -xzvf steamcmd.zip && del steamcmd.zip)");
			if (downloadResult == 0) {
				cout << "下载完成！" << endl;
			} else {
				cout << "下载失败！(请检查网络连接)" << endl;
				transmit();
			}
		}
	}
	//下载创意工坊文件
	cout << "正在下载创意工坊文件中..." << endl;
	string command = ".\\Steam\\steamcmd.exe +login Steam_username Steam_userpasswd +workshop_download_item 431960 " + input + " +quit";
	system(command.c_str()); // 这行代码会等待steamcmd.exe执行完毕
	//打开创意工坊文件夹
	string path = ".\\Steam\\steamapps\\workshop\\content\\431960\\" + input + "\\";
	// 使用正确的命令格式来打开文件夹
	string explorerCommand = "explorer.exe " + path;
	system(explorerCommand.c_str()); // 这行代码会等待explorer.exe执行完毕
	cout << "下载完成！(如果出现timeout，请重试！)" << endl;
	transmit();
}

