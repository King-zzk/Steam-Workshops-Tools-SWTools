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
}