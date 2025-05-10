#pragma once
/*
* elua.hpp
* 最终用户许可协议
*/

// 返回值: 用户是否同意了用户协议
bool elua() {
	fstream file("eula.txt", ios::in);
	string buf;
	if (file.is_open()) {
		file >> buf;
		// 文件中已经记录同意了
		if (buf.find(text::eula_accepted) != string::npos) {
			file.close();
			cout << "您已同意用户协议" << endl;
			return true;
		}
		file.close();
	}

	cout << "要使用本软件，请阅读并接受以下最终用户许可协议(EULA): " << endl;
	cout <<"\033[36m"<< text::eula << "\033[0m" << endl;
	cout << "是否接受以上协议? (y/n)" << endl << "> ";

	getline(cin, buf);
	if (buf == "Y" or buf == "y") {
		cout << "您已同意用户协议" << endl;
		file.open("eula.txt", ios::out);
		if (not file.is_open()) {
			throw exception("无法写入到eula.txt");
		}
		file << text::eula_accepted << endl;
		file.close();
		return true;
	} else if (buf == "N" or buf == "n") {
		cout << "您未同意用户协议，不能使用本软件" << endl;
		return false;
	}

	cout << "无效输入";
	return false;
}
