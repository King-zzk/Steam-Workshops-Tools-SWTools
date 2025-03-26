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
    else if (input == "wallpaper") {
        wallpaper();
    }
    else if (input == "gmod") {
        gmod();
    }
    else if (input == "hoi4") {
        hoi4();
    }
	else if (input == "v3") {
		v3();
	}
    else {
        cout << "Invalid command. Type 'help' for a list of commands." << endl;
        return transmit();
    }
}