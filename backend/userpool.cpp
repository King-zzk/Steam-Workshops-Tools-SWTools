#pragma once
/*
* UserPool.cpp
* 用户池 (静态类)
*/

#include "Backend.h"

UserPool userpool;
void UserPool::addUser(appid_t appid, std::string username, 
	std::string password) {
	if (pool.find(appid) == pool.end()) {
		pool[appid] = { {username, password} };
	}
	else {
		pool[appid].push_back({ username, password });
	}
}
bool UserPool::hasUserOf(appid_t appid) {
	return pool.find(appid) != pool.end();
}
std::vector<UserPool::User> UserPool::getUsersOf(appid_t appid) {
	return pool[appid];
}
UserPool::UserPool() {
	// 注意: 无需添加匿名账户 (anonymous)

	// 钢铁雄心4
	addUser(394360, "thb112259", "steamok7416");
	// 壁纸引擎
	addUser(431960, "kzeon410", "wnq69815I");
	// 维多利亚3
	addUser(529340, "steamok1090250", "steamok45678919");
	// 十字军之王3
	addUser(1158310, "wbtq1086059", "steamok32548S");
	// 城市：天际线
	addUser(255710, "thb112181", "steamok123123");

	// 受保护的用户池
	appendPrivateUserPool();
}