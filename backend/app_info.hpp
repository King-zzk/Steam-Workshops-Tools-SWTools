#pragma once
/*
* app_info.hpp
* 负责各 app 具体下载
*/

struct AppInfo {
	string user, password, app_id;
};

// TODO: 扩充此列表来支持更多 app 的下载
map < wstring, AppInfo> app_infos = {
	{
		L"Hearts of Iron IV | 钢铁雄心IV", // 这个字符串会显示在下拉菜单 (会自动按字母顺序排序)
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
		L"Victoria 3 | 维多利亚3",
		{"steamok1090250","steamok45678919","529340"}
	},

};
