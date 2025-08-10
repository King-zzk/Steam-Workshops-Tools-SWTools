/*
* Item.cpp
* 创意工坊物品
*/
#include "Backend.h"
#include <stdexcept>
#include <io.h> // _access()

Item::Item(std::string itemId) : itemId(itemId) {
	itemTitle = "", itemSize = 0;
	appId = 0, appName = "";
	parseState = ParseState::InQueue;
	downloadState = DownloadState::InQueue;
}
Item::Item(jsonxx::Object o) : Item("") {
	using namespace jsonxx;
	// 解析出现异常，抛出给上游方法处理
	if (o.has<String>("itemId")) {
		itemId = o.get<String>("itemId");
	} else {
		throw std::runtime_error("Missing key \"itemId\"");
	}
	// 剩下的不是特别重要，不向外抛出异常
	try {
		if (o.has<String>("itemTitle")) {
			itemTitle = o.get<String>("itemTitle");
		} else throw std::exception();
		if (o.has<Number>("itemSize")) {
			itemSize = o.get<Number>("itemSize");
		} else throw std::exception();
		if (o.has<Number>("appId")) {
			appId = o.get<Number>("appId");
		} else throw std::exception();
		if (o.has<String>("appName")) {
			appName = o.get<String>("appName");
		} else throw std::exception();
		if (o.has<String>("parseState")) {
			parseState = parseStateOf(o.get<String>("parseState"));
		} else throw std::exception();
	} catch (std::exception e) { // 信息不完整，重新解析
		parseState = ParseState::InQueue;
		backend.logger.warn("One or more keys are missing when paring Item from jsonxx::Object, setting parseState to InQueue");
	}
	// 才考虑 downloadState
	if (o.has<String>("downloadState")) {
		downloadState = downloadStateOf(o.get<String>("downloadState"));
	} else {
		downloadState = DownloadState::InQueue;
		backend.logger.warn("Missing key \"downloadState\" when paring Item from jsonxx::Object");
	}
	// 检查一下文件是否存在
	if (downloadState == DownloadState::Done && !_access(getPath().c_str(), 0)) {
		downloadState = DownloadState::Missing;
		backend.logger.info(getPath() + " doesn't exist, item " + itemId + " is missing");
	}
}

jsonxx::Object Item::toJson() {
	using namespace jsonxx;
	Object o;
	o << "itemId" << itemId;
	o << "itemTitle" << itemTitle;
	o << "itemSize" << itemSize;
	o << "appId" << appId;
	o << "appName" << appName;
	o << "parseState" << toString(parseState);
	o << "downloadState" << toString(downloadState);
	return o;
}

void Item::parse() {
	parseState = ParseState::Handling;
	// 用 curl 发送 POST 请求
	std::string data = backend.requestWithCurl("https://steamworkshopdownloader.io/api/details/file -d [" + itemId + "]");
	// 回复无效
	if (data.empty()) {
		backend.logger.error("Failed to parse item " + itemId + ": empty response");
		parseState = ParseState::Failed;
		return;
	}
	if (data.find("[") == data.npos || data.find("]") == data.npos) {
		backend.logger.error("Failed to parse item " + itemId + ": can't find \"[]\" pairs");
		parseState = ParseState::Failed;
		return;
	}
	data = data.substr(data.find("[") + 1);
	data.erase(data.find_last_of("]"));
	// 解析 Json
	using namespace jsonxx;
	Object o;
	if (!o.parse(data)) {
		backend.logger.error("Failed to parse item " + itemId + ": invalid JSON style");
		parseState = ParseState::Failed;
		return;
	}
	if (o.has<Number>("consumer_appid")) {
		appId = o.get<Number>("consumer_appid");
	} else {  // 不包含 appId，视为解析失败
		backend.logger.error("Failed to parse item " + itemId + ": can't find key \"consumer_appid\"");
		parseState = ParseState::Failed;
		return;
	}
	if (o.has<String>("title")) {
		itemTitle = o.get<String>("title");
	} else {
		backend.logger.warn("Can't parse itemTitle of item " + itemId);
	}
	if (o.has<String>("app_name")) {
		appName = o.get<String>("app_name");
	} else {
		backend.logger.warn("Can't parse appName of item " + itemId);
	}
	if (o.has<String>("file_size")) {
		std::string file_size = o.get<String>("file_size");
		itemSize = strtoull(file_size.c_str(), nullptr, 10);
	} else {
		backend.logger.warn("Can't parse itemSize of item " + itemId);
	}
	// 完成
	parseState = ParseState::Done;
	backend.logger.info("Successfully parsed item " + itemId);
}
void Item::download() {

}

void Item::fixState() {
	parseState = parseState == ParseState::Done ? ParseState::Done : ParseState::InQueue;
	downloadState = downloadState == DownloadState::Done ? DownloadState::Done : DownloadState::InQueue;
}
std::string Item::getPath() {
	return "steamcmd\\steamapps\\workshop\\content\\" + std::to_string(appId) + "\\" + itemId + "\\";
}

std::string Item::toString(ParseState s) {
	switch (s) {
	case Item::ParseState::InQueue:
		return "InQueue";
		break;
	case Item::ParseState::Handling:
		return "Handling";
		break;
	case Item::ParseState::Done:
		return "Done";
		break;
	case Item::ParseState::Failed:
		return "Failed";
		break;
	}
	return "";
}
Item::ParseState Item::parseStateOf(std::string str) {
	if (str == "InQueue") return ParseState::InQueue;
	else if (str == "Handling") return ParseState::Handling;
	else if (str == "Done") return ParseState::Done;
	else if (str == "Failed") return ParseState::Failed;
	else {
		backend.logger.error("Item::parseStateOf(): Unknown enum value \"" + str + "\"");
		return ParseState();
	}
}
std::string Item::toString(DownloadState s) {
	switch (s) {
	case Item::DownloadState::InQueue:
		return "InQueue";
		break;
	case Item::DownloadState::Handling:
		return "Handling";
		break;
	case Item::DownloadState::Done:
		return "Done";
		break;
	case Item::DownloadState::Failed:
		return "Failed";
		break;
	case Item::DownloadState::Missing:
		return "Missing";
		break;
	}
	return "";
}
Item::DownloadState Item::downloadStateOf(std::string str) {
	if (str == "InQueue") return DownloadState::InQueue;
	else if (str == "Handling") return DownloadState::Handling;
	else if (str == "Done") return DownloadState::Done;
	else if (str == "Failed") return DownloadState::Failed;
	else if (str == "Missing") return DownloadState::Missing;
	else {
		backend.logger.error("Item::downloadStateOf(): Unknown enum value \"" + str + "\"");
		return DownloadState();
	}
}