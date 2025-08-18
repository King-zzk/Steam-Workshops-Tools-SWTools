#pragma once
/**
 * @file		Process.hpp
 * @brief		包装一个进程，可以对其进行 Std I/O 操作
 *
 * @author		masterLazy
 * @copyright	Copyright (c) 2025 masterLazy
 */

#include <Windows.h>
#undef min
#undef max
#include <string>
#include <stdexcept>
#include <thread>
#include <mutex>
#include <deque>

namespace mlib {
	/**
	 * @brief	包装一个进程，可以对其进行 Std I/O 操作
	 */
	class Process {
		HANDLE to_hRead = NULL, to_hWrite = NULL;
		HANDLE from_hRead = NULL, from_hWrite = NULL;
		PROCESS_INFORMATION process_info;
		// 读取线程
		std::thread* readThread;
		int signal;
		const int SG_NULL = 0;
		const int SG_EXIT = 1;
		std::mutex mtx;
		std::deque<char> readBuffer;
		void readThreadHandler() {
			DWORD bytes_read;
			char buf;
			while (signal != SG_EXIT) {
				if (ReadFile(from_hRead, &buf, 1, &bytes_read, NULL) && bytes_read) {
					std::unique_lock<std::mutex> lk(mtx);
					readBuffer.push_back(buf);
				}
			}
		}
	public:
		/**
		* @brief				实例化进程
		* @param command_line	要执行的命令行
		* @param show_window	是否要显示窗口
		* @exception std::runtime_error 此函数会抛出异常！
		*/
		Process(const std::string& command_line, bool show_window = false) {
			// 创建管道
			SECURITY_ATTRIBUTES se = { 0 };
			se.lpSecurityDescriptor = NULL;
			se.bInheritHandle = TRUE;
			se.nLength = sizeof(se);
			if (CreatePipe(&to_hRead, &to_hWrite, &se, 0) == 0) {
				throw std::runtime_error("Failed to create pipe");
			}
			if (CreatePipe(&from_hRead, &from_hWrite, &se, 0) == 0) {
				throw std::runtime_error("Failed to create pipe");
			}
			// 创建进程
			STARTUPINFOA sInfo = { 0 };
			sInfo.dwFlags = STARTF_USESHOWWINDOW | STARTF_USESTDHANDLES;
			sInfo.wShowWindow = show_window ? SW_SHOW : SW_HIDE;
			sInfo.hStdInput = to_hRead;
			sInfo.hStdOutput = from_hWrite;
			sInfo.hStdError = from_hWrite;
			if (CreateProcessA(NULL, (char*)command_line.c_str(), NULL, NULL, TRUE, NULL, NULL, NULL, &sInfo, &process_info) == 0) {
				throw std::runtime_error("Failed to create process");
			}
			// 创建读取线程
			readThread = new std::thread(&Process::readThreadHandler, this);
			signal = NULL;
			readThread->detach();
		}
		~Process() {
			// 通知读取线程退出
			clock_t timer = clock();
			signal = SG_EXIT;
			while (signal == SG_EXIT && clock() - timer < 100);
			if (signal != SG_NULL) {
				TerminateThread(readThread->native_handle(), 0);
			}
			// 关闭句柄
			if (to_hWrite) {
				CloseHandle(to_hWrite);
			}
			if (from_hRead) {
				CloseHandle(from_hRead);
			}
			CloseHandle(process_info.hProcess);
		}
		/**
		* @brief	向进程的 stdin 写入数据
		* @return	实际写入的字节数
		* @exception std::runtime_error 写入失败时抛出异常
		*/
		DWORD write(const std::string& data) {
			DWORD bytes_written;
			if (WriteFile(to_hWrite, data.c_str(), data.size(), &bytes_written, NULL) == 0) {
				throw std::runtime_error("Failed to write to pipe");
			}
			return bytes_written;
		}
		/**
		* @brief	返回 stdout 中的字节数(不删除)
		* @return	stdout 中的字节数
		* @exception std::runtime_error peek 失败时抛出异常
		*/
		DWORD peek() {
			return readBuffer.size();
		}
		/**
		* @brief				从进程的 stdout 读取数据
		* @param bytes_to_read	要读取的最大字节数
		* @return				实际读取的字节数
		*/
		std::string read(DWORD bytes_to_read = 512) {
			std::string res;
			while (!readBuffer.empty() && res.size() < bytes_to_read) {
				std::unique_lock<std::mutex> lk(mtx);
				res += readBuffer.front(), readBuffer.pop_front();
			}
			return res;
		}
		/**
		* @brief				从进程的 stdout 读取数据直到行尾
		* @param max_bytes		要读取的最大字节数
		* @return				实际读取的字节数
		*/
		std::string readline(DWORD max_bytes = 1024) {
			std::string res;
			// 过滤换行符
			while (!readBuffer.empty() && (readBuffer.front() == '\r' || readBuffer.front() == '\n')) {
				std::unique_lock<std::mutex> lk(mtx);
				readBuffer.pop_front();
			}
			while (!readBuffer.empty() && res.size() < max_bytes) {
				std::unique_lock<std::mutex> lk(mtx);
				if (readBuffer.front() == '\r' || readBuffer.front() == '\n') break;
				res += readBuffer.front(), readBuffer.pop_front();
			}
			return res;
		}
		/**
		* @brief				获取进程的返回值
		* @retval STILL_ACTIVE	进程未退出
		*/
		DWORD getExitCode() {
			DWORD code;
			GetExitCodeProcess(process_info.hProcess, &code);
			return code;
		}
		/**
		* @brief	强制终止进程
		*/
		void kill(UINT exit_code = 0) {
			signal = SG_EXIT;
			TerminateProcess(process_info.hProcess, exit_code);
		}
	};
} // namespace  mlib