#pragma once
/*
* app_info.hpp
* 负责各 app 具体下载
*/

struct AppInfo {
	string user, password, app_id;
};

// TODO: 扩充此列表来支持更多 app 的下载
map <wstring, AppInfo> app_infos = {
	{
		L"Hearts of Iron IV | 钢铁雄心4", // 这个字符串会显示在下拉菜单 (会自动按字母顺序排序)
		{"thb112259","steamok7416","394360"}
	},
	{
		L"Garry's Mod | 盖瑞模组",
		{"anonymous","","4000"}
	},
	{
		L"Wallpaper Engine | 壁纸引擎",
		{"kzeon410","wnq69815I","431960"}
	},
	{
		L"Crusader Kings III | 十字军之王3 ",
		{"wbtq1086059","steamok32548S","1158310"}
	},
	{
		L"Cities: Skylines | 天际线",
		{"thb112181","steamok123123","255710"}
	},
		{
		L"Victoria 3 | 维多利亚3",
		{"yejonfils","T39S5C3XYS97","529340"}
	},

};
