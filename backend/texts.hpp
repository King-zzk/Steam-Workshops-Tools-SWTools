#pragma once
/*
* text.hpp
* ������Ҫ�õ����ı�����
*/

namespace text {
	// TODO: �汾����ʱ�޸�����İ汾��
	const wstring version = L"1.1.0_TEST_VERSION_BETA_2.0";
	const wstring appname = L"Steam Workshops Tools (SWTools)";
	const wstring authors = L"Commkom(king-zzk), masterLazy(mLazy)";
	const wstring copyright = L"Copyright (c) " + authors + L" 2025\r\nAll rights reserved.";
	const wstring website = L"https://github.com/King-zzk/Steam-Workshops-Tools-SWTools";

	// TODO: EULA����ʱ�޸�����İ汾��
	const wstring eula_version = L"2025.07.06";
	const wstring eula = L"\
�����û����Э��(EULA)\r\n\
����޸����ڣ�" + eula_version + L"\r\n\
\r\n1. ����Ȩ����\r\n\
���������������Ȩ������ " + authors + L"�����¼�ơ������ߡ��������״����Ȩ������ԭ�����ߡ������ɸ��ݱ�Э��������ʹ����ɡ�\r\n\
\r\n2. ��ɷ�Χ\r\n\
������������һ����ˡ��Ƕ�ռ������ת�á�����ת�ڵ����ʹ��Ȩ�������ڸ���ѧϰ�������������Ϸ�Ŀ��ʹ�á�\r\n\
\r\n3. ��ֹ��Ϊ\r\n\
�����ã�  \r\n\
- ���ñ����ֱ�ӻ��ӻ�����������������ת�ۡ��������ۡ��ṩ����֧�ַ��񣩣�  \r\n\
- ʵʩ�κ�Υ��Steamƽ̨�������������Ϊ��\r\n\
- ����������ڿ���/�������������ȡ���ݡ������������ȷǷ����\r\n\
\r\n4. ����֧����������Ϲ�\r\n\
- �����߹����û�ͨ��Steam�ٷ�������ȡ������Դ����������ṩ�κι��������֤�Ĺ��ܣ���Ӧ����ȷ��ʹ����Ϊ���ϵ�����ƽ̨����\r\n\
\r\n5. ��������  \r\n\
- �����������״���ṩ�������߲���ŵ����ȱ�ݡ��������û�����ض���������Υ����Э������µķ��ɾ��׵Ⱥ���������߸Ų��е����Ρ�\r\n\
\r\n6. ����\r\n\
- δ��ʾ��Ȩ��Ȩ�����ɿ����߱�����\r\n\
- ��������Ȩ���µ�����汾�и��±�Э�顣\r\n\
";
	const wstring eula_accepted = L"��ͬ���û�Э��(�汾Ϊ" + eula_version + L")";
	const wstring eula_accepted_eng = L"EULA accepted with version " + eula_version; // ����д���ļ��ģ�ȥ�����ı����������

	const wstring about = L"����һ����ȫ��ѵ� Steam Workshops ���ع��ߣ���������";
}