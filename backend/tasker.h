#pragma once
/*
* 任务执行器
*/

class Tasker {
	// 任务队列
	struct Task {
		time_t beginTime;
		enum Status { idle, handling, done } status = idle;

		string objId;
		wstring objName, objPath;
	};
	vector<Task> task;

	// 线程信号 (每次使用后要复位信号)
	enum Signal {
		null, stop, start, exit, exited
	} signal = null; // 必须用 setSignal() 赋值！
	void setSignal(Signal s);

	// 任务线程
	mutex mtx;
	void threadHandler();

public:
	Tasker();
	~Tasker();

	/*
	* 添加任务
	* 0: 成功
	* -1: 任务已存在
	*/
	int addTask(string objId);
	/*
	* 删除任务
	* 0: 成功
	* -1: 任务不存在
	* 1: 任务正在进行
	*/
	int removeTask(string objId);
};