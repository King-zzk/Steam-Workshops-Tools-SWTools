#include "SWTools.h"

void useragreement()
{
	//检查用户是否同意用户协议
	if (_access(".\\useragreement.txt", 0) == 0) {
		transmit();
		return;
	}
	bool useragreement1 = false; // Initialize the variable
	cout << "请阅读以下用户协议" << endl;
	cout << "1.本软件完全免费使用，请勿用于倒卖或盈利目的，否则后果自负！" << endl;
	cout << "2.本软件仅供学习交流使用，不得用于非法用途，否则后果自负！" << endl;
	cout << "3.本软件不得用于违反法律法规的行为，否则后果自负！" << endl;
	cout << "4.本软件不得用于违反Steam服务条款的行为，否则后果自负！" << endl;
	cout << "5.请支持正版！请支持正版！请支持正版！请支持正版！请支持正版！" << endl;
	cout << "是否同意？(y/n) ";
	string input;
	getline(cin, input);
	if (input == "y") {
		useragreement1 = true;
	}
	else if (input == "n") {
		useragreement1 = false;
	}
	else {
		cout << "输入有误，请重新输入！" << endl;
		useragreement();
		return; // Ensure the function exits after recursion
	}

	if (useragreement1 == true) {
		cout << "用户协议已同意！" << endl;
		//生成一个文件来存储用户协议
		ofstream file;
		file.open("useragreement.txt");
		file << "用户协议已同意！" << endl;
		file.close();
		transmit();
	}
	else {
		cout << "用户协议未同意！(你无法使用此软件！)" << endl;
		//任意键退出
		_getch();
		exit(0);
	}
}
