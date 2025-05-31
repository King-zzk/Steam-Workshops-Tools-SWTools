#pragma once
/*
* text.hpp
* ������Ҫ�õ����ı�����
*/

#include <string>
using namespace std;

namespace text {
	// TODO: �汾����ʱ�޸�����İ汾��
	const string version = "0.2.1";
	const string headline = "Commkom(king-zzk) Steam Workshops Tools (SWTools)[" + version +
		"]\nCopyright (c) Commkom(king-zzk), masterLazy(mLazy) 2025 All rights reserved.";
	const string website = "https://github.com/King-zzk/Steam-Workshops-Tools-SWTools";

	// TODO: EULA����ʱ�޸�����İ汾��
	const string eula_version = "2025.05.17";
	const string eula = R"(
�����û����Э��(EULA)
����޸����ڣ�)" + eula_version + R"(
1. ����Ȩ����
���������������Ȩ������Commkom(king-zzk)�����¼�ơ������ߡ��������״����Ȩ������ԭ�����ߡ������ɸ��ݱ�Э��������ʹ����ɡ�
2. ��ɷ�Χ
������������һ����ˡ��Ƕ�ռ������ת�á�����ת�ڵ����ʹ��Ȩ�������ڸ���ѧϰ�������������Ϸ�Ŀ��ʹ�á�
3. ��ֹ��Ϊ
�����ã�  
- ���ñ����ֱ�ӻ��ӻ�����������������ת�ۡ��������ۡ��ṩ����֧�ַ��񣩣�  
- ʵʩ�κ�Υ��Steamƽ̨�������������Ϊ��
- ����������ڿ���/�������������ȡ���ݡ������������ȷǷ����
4. ����֧����������Ϲ�
- �����߹����û�ͨ��Steam�ٷ�������ȡ������Դ����������ṩ�κι��������֤�Ĺ��ܣ���Ӧ����ȷ��ʹ����Ϊ���ϵ�����ƽ̨����
5. ��������  
- �����������״���ṩ�������߲���ŵ����ȱ�ݡ��������û�����ض���������Υ����Э������µķ��ɾ��׵Ⱥ���������߸Ų��е����Ρ�
6. ����
- δ��ʾ��Ȩ��Ȩ�����ɿ����߱�����
- ��������Ȩ���µ�����汾�и��±�Э�顣
)";
	const string eula_accepted = "��ͬ���û�Э��(�汾Ϊ" + eula_version + ")";

	const string usage = R"(
ʹ��˵��
hoi4      ���� ��������4 �Ĵ��⹤����Ʒ
gmod      ���� Garry's Mod �Ĵ��⹤����Ʒ
wallpaper ���� ��ֽ���� �Ĵ��⹤����Ʒ
v3        ���� ά������3 �Ĵ��⹤����Ʒ
ck3       ���� Crusader Kings III �Ĵ��⹤����Ʒ
eu4       ���� ŷ½����4 �Ĵ��⹤����Ʒ

exit	  �˳�����
clear	  ����
help	  �鿴ʹ��˵��
about     �鿴���ڱ������˵��
eula      �鿴�����û����Э��(EULA)
�йظ�����Ϣ�������Github��ҳ )" + website + R"(
)";
	const string about = headline + R"(
����һ����ȫ��ѵ� Steam Workshops ���ع��ߣ���������
���ߣ�king-Zzk(Commkom), masterLazy(mLazy)
Github��)" + website;
}