//这是执行用户输入命令的代码。
#include "SwTools.h"

void command(const string& input) {
    // 这里是command函数的实现
    if (input == "help") {
        help();
        transmit();
    }
    else if (input == "exit") {
        exit(0);
    }
    else if (input == "clear") {
        system("cls");
        transmit();
    }
    else if (input == "about") {
        about();
    }
    else if (input == "wallpaper") {
        wallpaper();
    }
    else if (input == "Gmod") {
        gmod();
    }
    else if (input == "hoi4") {
        hoi4();
    }
    else {
        cout << "Invalid command. Type 'help' for a list of commands." << endl;
        return transmit();
    }
}