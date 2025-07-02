#pragma once
/*
* userpool.h
* 账户池
*/

struct User {
	string name, pwd;
};

class UserPool {
	static map<appid_t, vector<User>> pool;
public:
	// 初始化用户池
	UserPool();
	// 受保护的用户池
	static void appendPrivateUserPool();

	// 添加账户
	static void addUser(appid_t appid, string username, string password);
	// 是否有该 SteamApp 的用户组
	static bool hasUserOf(appid_t appid);
	// 获取账户组
	static vector<User> getUsersOf(appid_t appid);
};