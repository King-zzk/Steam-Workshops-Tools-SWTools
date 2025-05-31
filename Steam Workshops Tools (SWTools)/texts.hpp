#pragma once
/*
* text.hpp
* 程序中要用到的文本常量
*/

#include <string>
using namespace std;

namespace text {
	// TODO: 版本更新时修改这里的版本号
	const string version = "0.2.1";
	const string headline = "Commkom(king-zzk) Steam Workshops Tools (SWTools)[" + version +
		"]\nCopyright (c) Commkom(king-zzk), masterLazy(mLazy) 2025 All rights reserved.";
	const string website = "https://github.com/King-zzk/Steam-Workshops-Tools-SWTools";

	// TODO: EULA更新时修改这里的版本号
	const string eula_version = "2025.05.17";
	const string eula = R"(
最终用户许可协议(EULA)
最后修改日期：)" + eula_version + R"(
1. 著作权声明
本软件的所有著作权归属于Commkom(king-zzk)（以下简称“开发者”）。贡献代码版权归属于原所有者。您仅可根据本协议获得有限使用许可。
2. 许可范围
开发者授予您一项个人、非独占、不可转让、不可转授的免费使用权，仅限于个人学习、技术交流及合法目的使用。
3. 禁止行为
您不得：  
- 利用本软件直接或间接获利（包括但不限于转售、捆绑销售、提供付费支持服务）；  
- 实施任何违反Steam平台《服务条款》的行为；
- 将本软件用于开发/传播恶意程序、窃取数据、干扰网络服务等非法活动。
4. 正版支持与第三方合规
- 开发者鼓励用户通过Steam官方渠道获取正版资源。本软件不提供任何规避正版验证的功能，您应自行确保使用行为符合第三方平台规则。
5. 免责声明  
- 本软件按“现状”提供，开发者不承诺其无缺陷、持续可用或兼容特定环境。因违反本协议而导致的法律纠纷等后果，开发者概不承担责任。
6. 其他
- 未明示授权的权利均由开发者保留。
- 开发者有权在新的软件版本中更新本协议。
)";
	const string eula_accepted = "已同意用户协议(版本为" + eula_version + ")";

	const string usage = R"(
使用说明
hoi4      下载 钢铁雄心4 的创意工坊物品
gmod      下载 Garry's Mod 的创意工坊物品
wallpaper 下载 壁纸引擎 的创意工坊物品
v3        下载 维多利亚3 的创意工坊物品
ck3       下载 Crusader Kings III 的创意工坊物品
eu4       下载 欧陆风云4 的创意工坊物品

exit	  退出程序
clear	  清屏
help	  查看使用说明
about     查看关于本软件的说明
eula      查看最终用户许可协议(EULA)
有关更多信息，请访问Github主页 )" + website + R"(
)";
	const string about = headline + R"(
这是一个完全免费的 Steam Workshops 下载工具，请勿倒卖！
作者：king-Zzk(Commkom), masterLazy(mLazy)
Github：)" + website;
}