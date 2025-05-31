#pragma once
/*
* app_info.hpp
* 负责各 app 具体下载
*/

struct AppInfo {
	string user, password, app_id;
};

// TODO: 扩充此列表来支持更多 app 的下载
map < string, AppInfo> app_cmd = {
	{
		"hoi4",
		{"thb112259","steamok7416","394360"}
	},
	{
		"gmod",
		{"anonymous","","4000"}
	},
	{
		"wallpaper",
		{"kzeon410","wnq69815I","431960"}
	},
	{
		"v3",
		{"steamok1090250","steamok45678919","529340"}
	},
	{
		"ck3",
		{"wgfy3000","Hh573461px","1158310"}
	},
	{
		"eu4",
		{"agt8729","Apk66433","236850"}

	}

};
