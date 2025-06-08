#pragma once
/*
* eula.hpp
* 最终用户许可协议
*/

// 检查用户是否同意了用户协议
bool CheckEula() {
	wfstream file("eula.txt", wios::in);
	wchar_t buf[1024];
	wstring wstr;
	if (file.is_open()) {
		file.getline(buf, 1024);
		wstr = buf;
		// 文件中已经记录同意了
		if (wstr.find(text::eula_accepted_eng) != string::npos) {
			file.close();
			return true;
		}
	}
	file.close();
	return false;
}

// 记录同意信息
bool WriteEula() {
	wfstream file("eula.txt", wios::out);
	if (!file.is_open()) {
		return false;
	}
	file << text::eula_accepted_eng.c_str() << endl;
	file.close();
	return true;
}
