#pragma once
/*
* userpool.cpp
* �˻���
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
	// ע��: ������������˻� (anonymous)

	// ��������4
	addUser(394360, "thb112259", "steamok7416");
	// ��ֽ����
	addUser(431960, "kzeon410", "wnq69815I");
	// ά������3
	addUser(529340, "steamok1090250", "steamok45678919");
	// ʮ�־�֮��3
	addUser(1158310, "wbtq1086059", "steamok32548S");
	// ���У������
	addUser(255710, "thb112181", "steamok123123");

	// �ܱ������û���
	appendPrivateUserPool();
}