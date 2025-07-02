#pragma once
/*
* item.h
* 创意工坊物品信息
*/

struct ItemInfo {
	string item_id;		// 物品 ID

	string title;		// 标题
	string description;	// 描述
	string app_name;	// SteamApp 名称
	appid_t appid;		// SteamApp ID
	size_t file_size;	// 文件大小
};

class Item {
	ItemInfo info;
public:
	Item(string id);

	// 解析物品信息
	ItemInfo parseInfo();

	// 获取物品信息
	ItemInfo getInfo();
};