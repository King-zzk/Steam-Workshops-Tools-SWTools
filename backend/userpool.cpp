#pragma once
/*
* userpool.cpp
* 账户池
*/

#include "backend.h"

map<appid_t, vector<User>> UserPool::pool;
void UserPool::addUser(appid_t appid, string username, string password) {
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
vector<User> UserPool::getUsersOf(appid_t appid) {
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