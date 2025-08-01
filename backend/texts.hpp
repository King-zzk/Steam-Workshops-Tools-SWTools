#pragma once
/*
* text.hpp
* 程序中要用到的文本常量
*/

namespace text {
	// TODO: 版本更新时修改这里的版本号
	const wstring version = L"1.1.0_TEST_VERSION_BETA_2.0";
	const wstring appname = L"Steam Workshops Tools (SWTools)";
	const wstring authors = L"Commkom(king-zzk), masterLazy(mLazy)";
	const wstring copyright = L"Copyright (c) " + authors + L" 2025\r\nAll rights reserved.";
	const wstring website = L"https://github.com/King-zzk/Steam-Workshops-Tools-SWTools";

	// TODO: EULA更新时修改这里的版本号
	const wstring eula_version = L"2025.07.06";
	const wstring eula = L"\
最终用户许可协议(EULA)\r\n\
最后修改日期：" + eula_version + L"\r\n\
\r\n1. 著作权声明\r\n\
本软件的所有著作权归属于 " + authors + L"（以下简称“开发者”）。贡献代码版权归属于原所有者。您仅可根据本协议获得有限使用许可。\r\n\
\r\n2. 许可范围\r\n\
开发者授予您一项个人、非独占、不可转让、不可转授的免费使用权，仅限于个人学习、技术交流及合法目的使用。\r\n\
\r\n3. 禁止行为\r\n\
您不得：  \r\n\
- 利用本软件直接或间接获利（包括但不限于转售、捆绑销售、提供付费支持服务）；  \r\n\
- 实施任何违反Steam平台《服务条款》的行为；\r\n\
- 将本软件用于开发/传播恶意程序、窃取数据、干扰网络服务等非法活动。\r\n\
\r\n4. 正版支持与第三方合规\r\n\
- 开发者鼓励用户通过Steam官方渠道获取正版资源。本软件不提供任何规避正版验证的功能，您应自行确保使用行为符合第三方平台规则。\r\n\
\r\n5. 免责声明  \r\n\
- 本软件按“现状”提供，开发者不承诺其无缺陷、持续可用或兼容特定环境。因违反本协议而导致的法律纠纷等后果，开发者概不承担责任。\r\n\
\r\n6. 其他\r\n\
- 未明示授权的权利均由开发者保留。\r\n\
- 开发者有权在新的软件版本中更新本协议。\r\n\
";
	const wstring eula_accepted = L"已同意用户协议(版本为" + eula_version + L")";
	const wstring eula_accepted_eng = L"EULA accepted with version " + eula_version; // 用来写入文件的，去掉中文避免编码问题

	const wstring about = L"这是一个完全免费的 Steam Workshops 下载工具，请勿倒卖！";
}