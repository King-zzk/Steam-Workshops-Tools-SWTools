# Steam Workshops Tools 创意工坊下载工具

这是一款下载steam创意工坊下载模组的工具

没有什么技术含量，只是SteamCMD加一个正版账号登陆就没了(＠_＠;)

# 编译方法
使用**Visual Studio**打开sln编译即可

# 支持游戏

维多利亚3，钢铁雄心4，盖瑞模组，小红车。

# 声明

记得补票，支持正版。禁止用任何商业用途！

# 求助！！！

```

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
```
这里的有什么问题吗？
我尝试用github静态托管，去当更新服务器，但是每次更新时显示的是上一回的版本号，不知道为什么。
是GitHub静态网站的问题吗？还是我写的代码有问题？
有没有大佬能帮我看看！！！