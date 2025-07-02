#pragma once
/*
* userpool.h
* �˻���
*/

struct User {
	string name, pwd;
};

class UserPool {
	static map<appid_t, vector<User>> pool;
public:
	// ��ʼ���û���
	UserPool();
	// �ܱ������û���
	static void appendPrivateUserPool();

	// ����˻�
	static void addUser(appid_t appid, string username, string password);
	// �Ƿ��и� SteamApp ���û���
	static bool hasUserOf(appid_t appid);
	// ��ȡ�˻���
	static vector<User> getUsersOf(appid_t appid);
};