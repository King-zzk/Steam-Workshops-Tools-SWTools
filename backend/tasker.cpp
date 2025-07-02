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

Tasker::Tasker() {
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
	mlib::process::Process steamcmd("steamcmd.exe", "", "C:\\Users\\lenovo\\Downloads\\");
	while (signal != exit) {

	}
	setSignal(exited);
}

int Tasker::addTask(string objId) {
	for (size_t i = 0; i < task.size(); i++) {
		if (task[i].objId == objId) return -1;
	}
	Task t;
	t.beginTime = time(NULL);
	t.objId = objId;
	mtx.lock();
	task.push_back(t);
	mtx.unlock();
	return 0;
}
int Tasker::removeTask(string objId) {
	for (size_t i = 0; i < task.size(); i++) {
		if (task[i].objId == objId) {
			if (task[i].status == Task::handling) return 1;
			task.erase(task.begin() + i);
			return 0;
		}
	}
	return -1;
}