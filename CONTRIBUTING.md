# 贡献指南

欢迎为本项目贡献一份力量！

下面介绍项目的实现方式，你可以选择一个擅长或者感兴趣的部分开发。

## 程序逻辑

```mermaid
sequenceDiagram
	actor 用户
	box SWTools
		participant GUI as GUI线程(前端)
		participant Task as 任务线程(后端)
	end
	participant Steamcmd
    autonumber
    用户->>+GUI: 请求下载
    GUI->>+Task: 提供下载信息<br/>通知开始下载
    Note over Task: 检查 Steamcmd是否存在
    Task-->>Steamcmd: (下载 Steamcmd)
    Note over Task: 检索用于下载目标 App 的<br/>用户名-密码
    Task->>+Steamcmd: 调用
    par 下载中
    	Steamcmd-->>Task: 实时运行结果
    	Task-->>GUI: 解析并显示
    end
   	    Note over Task, Steamcmd: 下载结束后立即终止Steamcmd进程
    Steamcmd->>-Task: 程序退出
    Task->>-GUI: 显示下载状态<br/>(打开目标文件夹)
   	Note right of Steamcmd: 通信方式
	Note over Task, Steamcmd: 匿名管道
	Note over GUI, Task: 互斥锁、共享内存
```

- GUI 采用的技术是 **MFC**（Visual C/C++）。
- 后端相关代码全部放在 `backend/` 下（声明和实现不分离）；下载一律用的是 `curl`。下面是部分文件的介绍：
  - `backend.hpp`：这是总头文件；
  - `app_info.hpp`：这里存储了用于登录不同 Steam App 的 *用户名-密码* 对信息；
  - `downloader.hpp`：这里是主下载逻辑；
  - `process.hpp`：这是一个包装器，用于调用 Steamcmd；
