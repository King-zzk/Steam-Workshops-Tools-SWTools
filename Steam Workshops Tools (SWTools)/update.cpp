#include "SWTools.h"

void update() {
    cout << "检查更新中..." << endl;

    // 检查并删除旧的 version.txt 文件
    if (_access(".\\version.txt", 0) == 0) {
        if (remove("version.txt") != 0) {
            cerr << "删除文件失败，错误代码: " << errno << endl;
            cout << "初始化失败，尝试手动删除此目录下的 version.txt 文件！" << endl;
            int ch = _getch(); // 等待用户按键
            exit(0);
        }
    }

    // 下载新的 version.txt 文件
    const wchar_t* url = L"https://king-zzk.github.io/version.txt";
    const wchar_t* path = L".\\version.txt";
    HRESULT hr = URLDownloadToFileW(NULL, url, path, 0, NULL);

    if (SUCCEEDED(hr)) {
        ifstream input(".\\version.txt");
        if (input.is_open()) {
            string line;
            if (getline(input, line)) { // 只读取第一行
                if (line == "0.1.9") {
                    cout << "当前版本为最新版本" << endl;
                } else {
                    cout << "当前不是最新版本，最新版本为：" << line << endl;
                    cout << "请下载最新版本！" << endl;
                }
            } else {
                cerr << "文件内容读取失败！" << endl;
            }
            input.close(); // 确保文件流被关闭
        } else {
            cerr << "无法打开 version.txt 文件！" << endl;
        }

        // 删除文件
        if (remove("version.txt") != 0) {
            cerr << "删除文件失败，错误代码: " << errno << endl;
        }
    } else {
        cerr << "检查失败，错误代码: 0x" << hex << hr << endl;
        cout << "开启加速器试试？" << endl;
    }

    useragreement(); // 调用用户协议函数
}