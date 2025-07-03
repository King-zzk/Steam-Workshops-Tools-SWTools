#pragma once
/*
* 任务执行器
*/

class Tasker {
	// 任务队列
	struct Task {
		enum Status { 
			parsing,		// 解析物品信息
			waiting,		// 等待下载
			downloading,	// 正在下载
			done			// 任务已完成
		} status = parsing;

		time_t beginTime;
		ItemInfo item;
	};
	vector<Task> task;

	// 线程信号 (每次使用后要复位信号)
	// 必须用 setSignal() 赋值！
	enum Signal {
		null, stop, start, exit, exited
	} signal = null;
	void setSignal(Signal s);

	// 任务线程
	mutex mtx;
	void threadHandler();

	HWND hWnd;
	Logger logger;
public:
	Tasker(HWND hWnd);
	~Tasker();

	/*
	* 添加任务
	* 0: 成功
	* -1: 任务已存在
	*/
	int addTask(string item_id);
	/*
	* 删除任务
	* 0: 成功
	* -1: 任务不存在
	* 1: 任务正在进行
	*/
	int removeTask(string item_id);
	
};