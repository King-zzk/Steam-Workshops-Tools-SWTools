#pragma once
/*
* app_info.hpp
* ����� app ��������
*/

struct AppInfo {
	string user, password, app_id;
};

// TODO: ������б���֧�ָ��� app ������
map <wstring, AppInfo> app_infos = {
	{
		L"Hearts of Iron IV | ��������4", // ����ַ�������ʾ�������˵� (���Զ�����ĸ˳������)
		{"thb112259","steamok7416","394360"}
	},
	{
		L"Garry's Mod | ����ģ��",
		{"anonymous","","4000"}
	},
	{
		L"Wallpaper Engine | ��ֽ����",
		{"kzeon410","wnq69815I","431960"}
	},
	{
		L"Crusader Kings III | ʮ�־�֮��3 ",
		{"wbtq1086059","steamok32548S","1158310"}
	},
	{
		L"Cities: Skylines | �����",
		{"thb112181","steamok123123","255710"}
	},
		{
		L"Victoria 3 | ά������3",
		{"yejonfils","T39S5C3XYS97","529340"}
	},

};
