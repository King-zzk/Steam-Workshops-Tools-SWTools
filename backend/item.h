#pragma once
/*
* Item.h
* 创意工坊物品
*/

class Item {
public:
	std::string	itemId;		// 物品 Id
	std::string	itemTitle;	// 物品标题
	size_t		itemSize;	// 该物品的文件大小
	appid_t		appId;		// 物品属于的 App 的 Id
	std::string	appName;	// 物品属于的 App 的名字

	enum class ParseState { // 解析状态
		InQueue, Handling, Done, Failed
	} parseState;
	enum class DownloadState { // 下载状态
		InQueue, Handling, Done, Failed, Missing
	} downloadState;

	// 构造一个未解析的物品实例
	Item(std::string itemId);
	// 从 Json 对象创建
	Item(jsonxx::Object o);

	// 转换为 Json 对象
	jsonxx::Object toJson();
	// 解析自己
	void parse();
	// 下载自己
	void download();

	// 把状态二值化到 InQueue / Done
	void fixState();
	// 获取物品所在相对路径
	std::string getPath();

private:
	// 枚举值和字符串互转

	std::string toString(ParseState s);
	ParseState parseStateOf(std::string str);

	std::string toString(DownloadState s);
	DownloadState downloadStateOf(std::string str);
};