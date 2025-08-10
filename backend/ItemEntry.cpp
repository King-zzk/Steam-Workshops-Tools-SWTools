/*
* ItemEntry.cpp
* 创意工坊物品登记
*/
#include "Backend.h"
#include <fstream>

jsonxx::Array ItemEntry::toJsonArray() {
	jsonxx::Array a;
	for (auto& i : list) {
		a << i.toJson();
	}
	return a;
}
void ItemEntry::writeJson(std::string filename) {
	std::ofstream of(filename);
	if (!of.is_open()) {
		backend.logger.error("ItemEntry::writeJson() failed to open file " + filename);
		return;
	}
	of << toJsonArray().json();
	backend.logger.info("ItemEntry::writeJson() successfully write to " + filename);
}
void ItemEntry::readJson(std::string filename) {
	std::ifstream f(filename);
	if (!f.is_open()) {
		backend.logger.warn("ItemEntry::readJson() failed open file " + filename + ", skipping");
		return;
	}
	// 神奇的语法，没有一个括号是多余的
	std::string data((std::istreambuf_iterator<char>(f)), std::istreambuf_iterator<char>());
	if (data.empty()) {
		backend.logger.warn("ItemEntry::readJson() failed to parse " + filename + ": empty file");
		return;
	}
	// 解析 Json
	using namespace jsonxx;
	Array a;
	if (!a.parse(data)) {
		backend.logger.error("ItemEntry::readJson() failed to parse " + filename + ": invalid JSON style");
		return;
	}
	size_t i = 0;
	try {
		for (; i < a.size(); i++) {
			add(Item(a.get<Object>(i)));
		}
	} catch (std::runtime_error e) {
		backend.logger.error("ItemEntry::readJson() failed to parse " + filename + ": " + e.what());
	}
	backend.logger.info("ItemEntry::readJson() successfully read from " + filename);
}

bool ItemEntry::has(const std::string itemId) const {
	for (auto& i : list) {
		if (i.itemId == itemId) return true;
	}
	return false;
}
Item& ItemEntry::get(std::string itemId) {
	static Item empty_item(0);
	for (auto& i : list) {
		if (i.itemId == itemId) return i;
	}
	return empty_item;
}
const Item& ItemEntry::getConst(std::string itemId) const {
	static Item empty_item(0);
	for (auto& i : list) {
		if (i.itemId == itemId) return i;
	}
	return empty_item;
}
size_t ItemEntry::getIndex(std::string itemId) {
	size_t i;
	for (i = 0; i < list.size(); i++) {
		if (list[i].itemId == itemId) break;
	}
	return i;
}
void ItemEntry::add(const Item& item) {
	// 不允许重复添加
	if (has(item.itemId)) {
		backend.logger.warn("ItemEntry::add() is ignoring duplicated item " + item.itemId);
		return;
	}
	list.push_back(item);
}
void ItemEntry::remove(const std::string itemId) {
	static Item empty_item(0);
	if (!has(itemId)) {
		backend.logger.warn("ItemEntry::remove() is ignoring non-existent item " + itemId);
		return;
	}
	list.erase(list.begin() + getIndex(itemId));
}
void ItemEntry::clear() {
	list.clear();
}
void ItemEntry::updateWith(const ItemEntry& another) {
	// 理论上这个实现是 O(n^2) 的，不过实际也不用处理很多数据，就这样吧
	for (auto& i : list) {
		if (another.has(i.itemId)) {
			i = another.getConst(i.itemId);
		}
	}
}
