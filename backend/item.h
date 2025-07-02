#pragma once
/*
* item.h
* ���⹤����Ʒ��Ϣ
*/

struct ItemInfo {
	string item_id;		// ��Ʒ ID

	string title;		// ����
	string description;	// ����
	string app_name;	// SteamApp ����
	appid_t appid;		// SteamApp ID
	size_t file_size;	// �ļ���С
};

class Item {
	ItemInfo info;
public:
	Item(string id);

	// ������Ʒ��Ϣ
	ItemInfo parseInfo();

	// ��ȡ��Ʒ��Ϣ
	ItemInfo getInfo();
};