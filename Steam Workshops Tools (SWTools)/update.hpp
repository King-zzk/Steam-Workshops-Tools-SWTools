#pragma once
/*
* upgrade.hpp
* 检查更新
*/

void check_update() {
	cout << "检查更新中..." << endl;
	fstream file("version.txt", ios::in);

	// 检查并删除旧的 headline.txt 文件
	if (file.is_open()) {
		file.close();
		if (remove("version.txt") != 0) {
			throw exception(("文件删除失败(代码" + to_string(errno) + ")，请尝试手动删除此目录下的 version.txt").c_str());
		}
	}

/*
	const char url[] = "https://gh-proxy.net/https://raw.githubusercontent.com/King-zzk/king-zzk.github.io/refs/heads/main/version.txt";
	const char path[] = "version.txt";

	if (URLDownloadToFileA(NULL, url, path, 0, NULL) == 0) {
		file.open("version.txt", ios::in);
		if (file.is_open()) {
			string line;
			if (getline(file, line)) { // 只读取第一行
				if (line == text::version) {
					cout << "当前版本为最新版本" << endl;
				} else {
					cout << "检测到新版本：\033[36m" << line << "\033[0m 当前版本：" << text::version << endl;
					cout << "请前往 " + text::website + " 下载最新版本" << endl;
				}
				file.close();
			} else {
				file.close();
				throw exception("version.txt中的内容无效");
			}
		} else {
			throw exception("无法读取version.txt");
		}

		// 删除文件
		if (remove("version.txt") != 0) {
			cout << "\033[33m警告: 文件删除失败(代码" << errno << ")，请尝试手动删除此目录下的version.txt\033[0m" << endl;
		}
	} else {
		throw exception("无法获取最新版本信息");
	}
}
*/

	//源代码注释

	system (R"(curl -o "version.txt" "https://gh-proxy.net/https://raw.githubusercontent.com/King-zzk/king-zzk.github.io/refs/heads/main/version.txt" -s)");
	//被逼无奈火绒会报毒，所以只能用curl了(┬┬﹏┬┬)
	file.open("version.txt", ios::in);
	if (file.is_open()) {
		string line;
		if (getline(file, line)) { // 只读取第一行
			if (line == text::version) {
				cout << "当前版本为最新版本" << endl;
			} else if(line > text::version) {
				cout << "检测到新版本：" << line << " 当前版本：" << text::version << endl;
				cout << "请前往 " + text::website + " 下载最新版本" << endl;
			} else {
				cout << "哇哦！当前版本居然比“最新版本”还要新！" << endl;
			}
			file.close();
		}
		else {
			file.close();
			throw exception("version.txt 中的内容无效");
		}
	} else {
		throw exception("version.txt 打开失败");
	}

	if (_access("version.txt", 0) == 0) {
		if (remove("version.txt") != 0) {
			cout << "警告: 文件删除失败(代码" << errno << ")，请尝试手动删除此目录下的 version.txt" << endl;
		}
	}
}