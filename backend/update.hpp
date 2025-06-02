#pragma once
/*
* update.hpp
* 检查更新
*/

void CheckUpdate() {
	cout << "检查更新中..." << endl;
	wfstream file("version.txt", ios::in);

	// 检查并删除旧的 headline.txt 文件
	if (file.is_open()) {
		file.close();
		if (remove("version.txt") != 0) {
			MessageBox(NULL, _T("文件删除失败，请尝试手动删除此目录下的 version.txt"), _T("错误"), MB_ICONERROR | MB_OK);
		}
	}

	wstring command = LR"(-o "version.txt" "https://gh-proxy.net/https://raw.githubusercontent.com/King-zzk/king-zzk.github.io/refs/heads/main/version.txt" -s)";
	SHELLEXECUTEINFO ShExecInfo = { 0 };
	ShExecInfo.cbSize = sizeof(SHELLEXECUTEINFO);
	ShExecInfo.fMask = SEE_MASK_NOCLOSEPROCESS;
	ShExecInfo.lpFile = L"curl";
	ShExecInfo.lpParameters = LR"(-o "version.txt" "https://gh-proxy.net/https://raw.githubusercontent.com/King-zzk/king-zzk.github.io/refs/heads/main/version.txt" -s)";
	ShExecInfo.nShow = SW_HIDE;
	ShellExecuteEx(&ShExecInfo);
	WaitForSingleObject(ShExecInfo.hProcess, INFINITE);
	// 被逼无奈火绒会报毒，所以只能用curl了(┬┬﹏┬┬)
	file.open("version.txt", ios::in);
	if (file.is_open()) {
		wstring line;
		if (getline(file, line)) { // 只读取第一行
			if (line == text::version) {
				MessageBox(NULL, _T("当前版本为最新版本"), _T("信息"), MB_ICONINFORMATION | MB_OK);
			} else if (line > text::version) {
				wstring temp = L"检测到新版本 " + line + L"\r\n是否要跳转到Github仓库页面下载新版本？";
				if (MessageBox(NULL, temp.c_str(), _T("检测到新版本"), MB_ICONINFORMATION | MB_YESNO) == IDYES) {
					ShellExecute(NULL, L"open", (text::website + L"/releases").c_str(), NULL, NULL, SW_SHOWNORMAL);
				}
			} else {
				MessageBox(NULL, _T("哇哦！当前版本居然比“最新版本”还要新！"), _T("信息"), MB_ICONINFORMATION | MB_OK);
			}
			file.close();
		} else {
			file.close();
			MessageBox(NULL, _T("更新检查失败，version.txt 中的内容无效"), _T("错误"), MB_ICONERROR | MB_OK);
		}
	} else {
		MessageBox(NULL, _T("更新检查失败，无法打开 version.txt"), _T("错误"), MB_ICONERROR | MB_OK);
	}

	if (_access("version.txt", 0) == 0) {
		if (remove("version.txt") != 0) {
			MessageBox(NULL, _T("文件删除失败，请尝试手动删除此目录下的 version.txt"), _T("警告"), MB_ICONWARNING | MB_OK);
		}
	}
}