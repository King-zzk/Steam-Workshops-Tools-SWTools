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

namespace mlib {
	/**
	 * @brief	包装一个进程，可以对其进行 Std I/O 操作
	 */
	class Process {
		HANDLE to_hRead = NULL, to_hWrite = NULL;
		HANDLE from_hRead = NULL, from_hWrite = NULL;
		PROCESS_INFORMATION processInfo;
	public:
		/**
		* @brief				实例化进程
		* @param file_name		要执行的文件 (可以为空)
		* @param command_line	要执行的命令行
		* @param show_window	是否显示窗口
		* @exception std::runtime_error 此函数会抛出异常！
		*/
		Process(
			const std::string& file_name,
			const std::string& command_line,
			const std::string& working_dir = "",
			bool show_window = false) {
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
			if (CreateProcessA(
				file_name.empty() ? NULL : file_name.c_str(),
				command_line.empty() ? NULL : (char*)command_line.c_str(),
				NULL, NULL, TRUE, NULL, NULL,
				working_dir.empty() ? NULL : working_dir.c_str(),
				&sInfo, &processInfo) == 0) {
				throw std::runtime_error("Failed to create process");
			}
		}
		~Process() {
			if (getExitCode() == STILL_ACTIVE) {
				kill();
			}
			if (to_hWrite) {
				CloseHandle(to_hWrite);
			}
			if (from_hRead) {
				CloseHandle(from_hRead);
			}
			CloseHandle(processInfo.hProcess);
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
			DWORD bytes;
			if (PeekNamedPipe(from_hRead, NULL, NULL, NULL, &bytes, NULL) == 0) {
				throw std::runtime_error("Failed to peek pipe");
			}
			return bytes;
		}
		/**
		* @brief				从进程的 stdout 读取数据
		* @param bytes_to_read	要读取的最大字节数
		* @return				实际读取的字节数
		* @exception std::runtime_error 读取失败时抛出异常
		*/
		std::string read(DWORD bytes_to_read = 512) {
			DWORD bytes_read;
			unsigned char* buf = new unsigned char[bytes_to_read];
			if (ReadFile(from_hRead, buf, bytes_to_read, &bytes_read, NULL) == 0) {
				delete[] buf;
				throw std::runtime_error("Failed to read from pipe");
			}
			std::string res;
			res.resize(bytes_read);
			for (DWORD i = 0; i < bytes_read; i++) {
				res[i] = buf[i];
			}
			delete[] buf;
			return res;
		}
		/**
		* @brief				从进程的 stdout 读取数据直到行尾
		* @param max_bytes		要读取的最大字节数
		* @return				实际读取的字节数
		* @exception std::runtime_error 读取失败时抛出异常
		*/
		std::string getline(DWORD max_bytes = 1024) {
			DWORD bytes_read;
			std::string res;
			char buf;
			for (DWORD i = 0; i < max_bytes; i++) {
				if (ReadFile(from_hRead, &buf, 1, &bytes_read, NULL) == 0) {
					throw std::runtime_error("Failed to read from pipe");
				}
				if (buf == '\r' || buf == '\n') {
					if (res.empty()) {
						continue;
					} else return res;
				}
				res += buf;
			}
			return res;
		}
		/**
		* @brief				获取进程的返回值
		* @retval STILL_ACTIVE	进程未退出
		*/
		DWORD getExitCode() {
			DWORD code;
			GetExitCodeProcess(processInfo.hProcess, &code);
			return code;
		}
		/**
		* @brief	强制终止进程
		*/
		void kill(UINT exit_code = 0) {
			TerminateProcess(processInfo.hProcess, exit_code);
		}
	};
} // namespace  mlib