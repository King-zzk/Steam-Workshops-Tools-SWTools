#ifndef SWTOOLS_H
#define SWTOOLS_H

//��Ҫ��ͷ�ļ�
#include <string>
#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include <windows.h>
#include <cstdio>
#include <io.h>
#include <conio.h>
#include <urlmon.h>
#include <fstream>
#include <cerrno>
#include <cstring>
#pragma comment(lib, "urlmon.lib")
using namespace std;

//��������
void command(const string& input);
void help();
void transmit();
void goto_transmit();
void about();
void wallpaper();
void hoi4();
void gmod();
void useragreement();
void v3();
void update();
#endif // SWTOOLS_H