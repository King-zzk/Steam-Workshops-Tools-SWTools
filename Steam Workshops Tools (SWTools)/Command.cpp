//����ִ���û���������Ĵ��롣
#include "SwTools.h"

void command(const string& input) {
    // ������command������ʵ��
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