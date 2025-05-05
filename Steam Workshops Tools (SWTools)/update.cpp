#include "SWTools.h"

void update() {
    cout << "��������..." << endl;

    // ��鲢ɾ���ɵ� version.txt �ļ�
    if (_access(".\\version.txt", 0) == 0) {
        if (remove("version.txt") != 0) {
            cerr << "ɾ���ļ�ʧ�ܣ��������: " << errno << endl;
            cout << "��ʼ��ʧ�ܣ������ֶ�ɾ����Ŀ¼�µ� version.txt �ļ���" << endl;
            int ch = _getch(); // �ȴ��û�����
            exit(0);
        }
    }

    // �����µ� version.txt �ļ�
    const wchar_t* url = L"https://king-zzk.github.io/version.txt";
    const wchar_t* path = L".\\version.txt";
    HRESULT hr = URLDownloadToFileW(NULL, url, path, 0, NULL);

    if (SUCCEEDED(hr)) {
        ifstream input(".\\version.txt");
        if (input.is_open()) {
            string line;
            if (getline(input, line)) { // ֻ��ȡ��һ��
                if (line == "0.1.9") {
                    cout << "��ǰ�汾Ϊ���°汾" << endl;
                } else {
                    cout << "��ǰ�������°汾�����°汾Ϊ��" << line << endl;
                    cout << "���������°汾��" << endl;
                }
            } else {
                cerr << "�ļ����ݶ�ȡʧ�ܣ�" << endl;
            }
            input.close(); // ȷ���ļ������ر�
        } else {
            cerr << "�޷��� version.txt �ļ���" << endl;
        }

        // ɾ���ļ�
        if (remove("version.txt") != 0) {
            cerr << "ɾ���ļ�ʧ�ܣ��������: " << errno << endl;
        }
    } else {
        cerr << "���ʧ�ܣ��������: 0x" << hex << hr << endl;
        cout << "�������������ԣ�" << endl;
    }

    useragreement(); // �����û�Э�麯��
}