#pragma once
/*
* UserPool.h
* 用户池 (单实例，静态类)
*/

#include <map>
#include <vector>

extern class UserPool {
public:
	struct User { // 用户信息
		std::string name, pwd;
	};

	// 初始化用户池
	UserPool();
	// 受保护的用户池
	void appendPrivateUserPool();

	// 添加账户
	void addUser(appid_t appid, std::string username, std::string password);
	// 是否有该 SteamApp 的用户组
	bool hasUserOf(appid_t appid);
	// 获取账户组
	std::vector<User> getUsersOf(appid_t appid);
private:
	// 用户池本体
	std::map<appid_t, std::vector<User>> pool;
} userpool;