#include "SWTools.h"
#include <conio.h> // Include necessary header for _getch()

void useragreement()
{
	//����û��Ƿ�ͬ���û�Э��
	if (_access(".\\useragreement.txt", 0) == 0) {
		transmit();
		return;
	}
	bool useragreement1 = false; // Initialize the variable
	cout << "���Ķ������û�Э��" << endl;
	cout << "1.�������ȫ���ʹ�ã��������ڵ�����ӯ��Ŀ�ģ��������Ը���" << endl;
	cout << "2.���������ѧϰ����ʹ�ã��������ڷǷ���;���������Ը���" << endl;
	cout << "3.�������������Υ�����ɷ������Ϊ���������Ը���" << endl;
	cout << "4.�������������Υ��Steam�����������Ϊ���������Ը���" << endl;
	cout << "5.��֧�����棡��֧�����棡��֧�����棡��֧�����棡��֧�����棡" << endl;
	cout << "�Ƿ�ͬ�⣿(y/n) ";
	string input;
	getline(cin, input);
	if (input == "y") {
		useragreement1 = true;
	}
	else if (input == "n") {
		useragreement1 = false;
	}
	else if (input == "Y") {
		useragreement1 = true;
	}
	else if (input == "N") {
		useragreement1 = false;
	}
	else if (input == "aacc666") {
		cout << "��������" << endl;
		transmit();
	}
	else {
		cout << "�����������������룡" << endl;
		useragreement();
		return; // Ensure the function exits after recursion
	}

	if (useragreement1 == true) {
		cout << "�û�Э����ͬ�⣡" << endl;
		//����һ���ļ����洢�û�Э��
		ofstream file;
		file.open("useragreement.txt");
		file << "�û�Э����ͬ�⣡" << endl;
		file.close();
		transmit();
	}
	else {
		cout << "�û�Э��δͬ�⣡(���޷�ʹ�ô������)" << endl;
		//������˳�
		int ch = _getch(); // Store the return value to avoid the warning
		exit(0);
	}
}
