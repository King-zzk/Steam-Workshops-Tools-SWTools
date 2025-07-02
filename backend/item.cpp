/*
* item.cpp
* 创意工坊物品信息
*/

#include "backend.h"

Item::Item(string id) {
	info.item_id = id;
}
ItemInfo Item::parseInfo() {
	// 用 curl 发送 POST 请求
	Process curl("curl",
		"https://steamworkshopdownloader.io/api/details/file -d [" +
		info.item_id + "]");
	string data;
	while (curl.getExitCode() == STILL_ACTIVE);
	Sleep(10); // 再等等
	data = curl.read();
	if (data.empty() or data.find("[") == data.npos)return info;
	data = data.substr(data.find("[") + 1);
	data.erase(data.find_last_of("]"));
	// 解析 json
	using namespace jsonxx;
	Object o;
	o.parse(data);
	if (o.has<String>("title")) {
		info.title = o.get<String>("title");
	}
	if (o.has<String>("file_description")) {
		info.description = o.get<String>("file_description");
	}
	if (o.has<String>("app_name")) {
		info.app_name = o.get<String>("app_name");
	}
	if (o.has<Number>("appid")) {
		info.appid = o.get<Number>("appid");
	}
	if (o.has<String>("file_size")) {
		string file_size = o.get<String>("file_size");
		info.file_size = strtoull(file_size.c_str(), nullptr, 10);
	}
	return info;
}
ItemInfo Item::getInfo() {
	return info;
}