#pragma once
/*
* 任务执行器
*/

/*
* 参考
* https://developer.valvesoftware.com/wiki/Command_line_options#SteamCMD
* https://github.com/dgibbs64/SteamCMD-Commands-List
*/

#include "backend.h"

Tasker::Tasker(HWND hWnd) : hWnd(hWnd) {
	thread th(&Tasker::threadHandler, this);
	th.detach(); // 分离线程
}
Tasker::~Tasker() {
	setSignal(exit);
	time_t timer = time(NULL);
	while (signal != exited); // 等待线程结束
}

void Tasker::setSignal(Signal s) {
	mtx.lock();
	signal = s;
	mtx.unlock();
}

void Tasker::threadHandler() {
	// DEBUGing! ! !
	mlib::process::Process steamcmd("steamcmd.exe", "", "C:\\Users\\lenovo\\Downloads\\");
	while (signal != exit) {

	}
	setSignal(exited);
}

int Tasker::addTask(string item_id) {
	for (size_t i = 0; i < task.size(); i++) {
		if (task[i].item.item_id == item_id) return -1;
	}
	Task t;
	t.beginTime = time(NULL);
	t.item.item_id = item_id;
	mtx.lock();
	task.push_back(t);
	mtx.unlock();
	return 0;
}
int Tasker::removeTask(string item_id) {
	for (size_t i = 0; i < task.size(); i++) {
		if (task[i].item.item_id == item_id) {
			if (task[i].status == Task::downloading) return 1;
			task.erase(task.begin() + i);
			return 0;
		}
	}
	return -1;
}