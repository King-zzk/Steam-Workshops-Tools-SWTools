#pragma once
/*
* app_info.hpp
* ����� app ��������
*/

struct AppInfo {
	string user, password, app_id;
};

// TODO: ������б���֧�ָ��� app ������
map < wstring, AppInfo> app_infos = {
	{
		L"Hearts of Iron IV | ��������IV", // ����ַ�������ʾ�������˵� (���Զ�����ĸ˳������)
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
		L"Victoria 3 | ά������3",
		{"steamok1090250","steamok45678919","529340"}
	},
	{
		L"Crusader Kings III | ʮ�־�֮��3",
		{"wgfy3000","Hh573461px","1158310"}
	},
	{
		L"Europa Universalis IV | ŷ½����4",
		{"junjun888666525","Xiaohejiaoao489891","236850"}

	}

};
