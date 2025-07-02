#pragma once
/**
 * @file		process.hpp
 * @brief		进程包装器
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

namespace mlib {
    namespace process {
        /**
         * @brief   进程包装器
         */
        class Process {
            HANDLE to_hRead = NULL, to_hWrite = NULL;
            HANDLE from_hRead = NULL, from_hWrite = NULL;
            PROCESS_INFORMATION process_info;

            DWORD buf_size = 16 * 1024 * 1024; // 缓冲区最大大小
            std::thread read_thread;
            std::string buf;
            std::mutex mtx;
            void readThread() {
                char ch;
                DWORD bytes_read;
                while (true) {
                    while (buf.size() >= buf_size);
                    ReadFile(from_hRead, &ch, 1, &bytes_read, NULL);
                    if (bytes_read) {
                        mtx.lock();
                        buf += ch;
                        mtx.unlock();
                    }
                }
            }
        public:
            /**
            * @brief				实例化进程
            * @param file_name		要执行的文件 (可以为空)
            * @param command_line	要执行的命令行
            * @param show_window	是否显示窗口
            * @exception std::runtime_error
            */
            Process(const std::string& file_name,
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
                    NULL,
                    (char*)(file_name + " " + command_line).c_str(),
                    NULL, NULL, TRUE, NULL, NULL,
                    working_dir.empty() ? NULL : working_dir.c_str(),
                    &sInfo, &process_info) == 0) {
                    throw std::runtime_error("Failed to create process");
                }
                // 启动读取线程
                std::thread read_thread(&Process::readThread, this);
                read_thread.detach();
            }
            ~Process() {
                TerminateThread(read_thread.native_handle(), 0);
            }
            /**
             * @brief   设置读取缓冲区大小
             */
            void resizeBuf(size_t size) {
                buf_size = size;
            }
            /**
             * @brief   清空缓冲区
             */
            void clearBuf() {
                mtx.lock();
                buf.clear();
                mtx.unlock();
            }
            /**
             * @brief   从 stdout 读取(不删除)
             * @param bytes_to_read	要读取的最大字节数
             */
            std::string peek(size_t bytes_to_read = -1) {
                size_t size = std::min(bytes_to_read, buf.size());
                std::string res = buf.substr(0, size);
                return res;
            }
            /**
             * @brief   从 stdout 读取(并删除)
             * @param bytes_to_read	要读取的最大字节数
             */
            std::string read(size_t bytes_to_read = -1) {
                size_t size = std::min(bytes_to_read, buf.size());
                std::string res = buf.substr(0, size);
                mtx.lock();
                buf.erase(0, size);
                mtx.unlock();
                return res;
            }
            /**
            * @brief	向进程的 stdin 写入数据
            * @return	实际写入的字节数
            * @exception std::exception 写入失败时抛出异常
            */
            DWORD write(const std::string& data) {
                DWORD bytes_written;
                if (WriteFile(to_hWrite, data.c_str(), data.size(), &bytes_written, NULL) == 0) {
                    throw std::runtime_error("Failed to write to pipe");
                }
                return bytes_written;
            }
            /**
            * @brief				从进程的 stdout 读取数据直到行尾
            * @param max_bytes		要读取的最大字节数
            * @return				实际读取的字节数
            */
            std::string getLine(DWORD max_bytes = 1024) {
                DWORD bytes_read;
                std::string res;
                char buf;
                for (DWORD i = 0; i < max_bytes; i++) {
                    buf = read(1)[0];
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
                GetExitCodeProcess(process_info.hProcess, &code);
                return code;
            }
            /**
            * @brief	强制终止进程
            */
            void kill(UINT exit_code = 0) {
                TerminateProcess(process_info.hProcess, exit_code);
            }
        };
    } // namespace process
} // namespace mlib