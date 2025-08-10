#pragma once
/*
* ItemEntry.h
* 创意工坊物品登记
*/

#include <vector>

class ItemEntry {
public:
	std::vector<Item> list;

	// 转换为 Json 数组
	jsonxx::Array toJsonArray();
	// 写入 Json 文件
	void writeJson(std::string filename);
	// 读取 Json 文件
	void readJson(std::string filename);

	// 是否有编号对应的物品
	bool has(const std::string itemId) const;
	// 返回编号对应的物品
	Item& get(std::string itemId);
	const Item& getConst(std::string itemId) const;
	// 返回物品索引
	size_t getIndex(std::string itemId);
	// 添加物品
	void add(const Item& item);
	// 移除物品
	void remove(const std::string itemId);
	// 清空登记
	void clear();
	// 用传入的物品登记更新当前登记
	void updateWith(const ItemEntry& another);
};