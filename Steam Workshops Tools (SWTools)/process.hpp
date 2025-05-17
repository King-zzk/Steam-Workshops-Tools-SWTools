#pragma once
/**
 * @file		process.hpp
 * @brief		包装一个进程，可以对其进行 Std I/O 操作
 *
 * @author		masterLazy
 * @copyright	Copyright (c) 2025 masterLazy
 */

#include <Windows.h>
#undef min
#undef max
#include <string>
#include <exception>

namespace mlib {
	namespace process {
		/**
		 * @brief	包装一个进程，可以对其进行 Std I/O 操作
		 */
		class Process {
		private:
			HANDLE to_hRead = NULL, to_hWrite = NULL;
			HANDLE from_hRead = NULL, from_hWrite = NULL;
			PROCESS_INFORMATION process_info;
		public:
			/**
			* @brief				实例化进程
			* @param command_line	要执行的命令行
			* @param show_window	是否要显示窗口
			* @exception std::exception 此函数会抛出异常！
			*/
			Process(const std::string& command_line, bool show_window = false) {
				// 创建管道
				SECURITY_ATTRIBUTES se = { 0 };
				se.lpSecurityDescriptor = NULL;
				se.bInheritHandle = TRUE;
				se.nLength = sizeof(se);
				if (CreatePipe(&to_hRead, &to_hWrite, &se, 0) == 0) {
					throw std::exception("Failed to create pipe");
				}
				if (CreatePipe(&from_hRead, &from_hWrite, &se, 0) == 0) {
					throw std::exception("Failed to create pipe");
				}
				// 创建进程
				STARTUPINFOA sInfo = { 0 };
				sInfo.dwFlags = STARTF_USESHOWWINDOW | STARTF_USESTDHANDLES;
				sInfo.wShowWindow = show_window ? SW_SHOW : SW_HIDE;
				sInfo.hStdInput = to_hRead;
				sInfo.hStdOutput = from_hWrite;
				sInfo.hStdError = from_hWrite;
				if (CreateProcessA(NULL, (char*)command_line.c_str(), NULL, NULL, TRUE, NULL, NULL, NULL, &sInfo, &process_info) == 0) {
					throw std::exception("Failed to create process");
				}
			}
			~Process() {
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
			* @exception std::exception 写入失败时抛出异常
			*/
			DWORD write(const std::string& data) {
				DWORD bytes_written;
				if (WriteFile(to_hWrite, data.c_str(), data.size(), &bytes_written, NULL) == 0) {
					throw std::exception("Failed to write to pipe");
				}
				return bytes_written;
			}
			/**
			* @brief	返回 stdout 中的字节数(不删除)
			* @return	stdout 中的字节数
			* @exception std::exception peek 失败时抛出异常
			*/
			DWORD peek() {
				DWORD bytes;
				if (PeekNamedPipe(from_hRead, NULL, NULL, NULL, &bytes, NULL) == 0) {
					throw std::exception("Failed to peek pipe");
				}
				return bytes;
			}
			/**
			* @brief				从进程的 stdout 读取数据
			* @param bytes_to_read	要读取的最大字节数
			* @return				实际读取的字节数
			* @exception std::exception 读取失败时抛出异常
			*/
			std::string read(DWORD bytes_to_read = 512) {
				DWORD bytes_read;
				unsigned char* buf = new unsigned char[bytes_to_read];
				if (ReadFile(from_hRead, buf, bytes_to_read, &bytes_read, NULL) == 0) {
					delete[] buf;
					throw std::exception("Failed to read from pipe");
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
			* @brief				获取进程的返回值
			* @retval STILL_ACTIVE	进程未退出
			*/
			DWORD get_exit_code() {
				DWORD code;
				GetExitCodeProcess(process_info.hProcess, &code);
				return code;
			}
		};
	} // namespace process
} // namespace  mlib