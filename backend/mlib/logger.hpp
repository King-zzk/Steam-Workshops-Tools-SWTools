#pragma once
/**
 * @file		Logger.hpp
 * @brief		简单的日志器
 *
 * @author		masterLazy
 * @copyright	Copyright (c) 2025 masterLazy
 */

#include <iostream>
#include <fstream>
#include <vector>
#include <mutex>

#include "Function.hpp"

namespace mlib {
	/**
	* @brief	简单的日志器
	*/
	class Logger {
	public:
		enum class Level {
			debug = 0,
			info = 1,
			warn = 2,
			error = 3,
			fatal = 4
		};
	private:
		std::ofstream of; // 文件输出流
		Level logLevel;
		std::string filename;
		bool datetime;
		std::mutex mtx;

		void log(Level logLevel, const std::string& msg) {
			if (logLevel < this->logLevel) return;
			std::string full_msg;
			if (datetime) {
				full_msg += "[" + Function::GetFormatDateTime(true, true) + "] ";
			}
			full_msg += "[";
			switch (logLevel) {
			case Level::debug:
				full_msg += "DEBUG";
				break;
			case Level::info:
				full_msg += "INFO";
				break;
			case Level::warn:
				full_msg += "WARN";
				break;
			case Level::error:
				full_msg += "ERROR";
				break;
			case Level::fatal:
				full_msg += "FATAL";
				break;
			}
			full_msg += "]: " + msg + "\n";
			emit(full_msg);
		}
		void log(Level logLevel, const std::wstring& msg) {
			if (logLevel < this->logLevel) return;
			std::wstring full_msg;
			if (datetime) {
				full_msg += L"[" + Function::GetFormatDateTimeW(true, true) + L"] ";
			}
			full_msg += L"[";
			switch (logLevel) {
			case Level::debug:
				full_msg += L"DEBUG";
				break;
			case Level::info:
				full_msg += L"INFO";
				break;
			case Level::warn:
				full_msg += L"WARN";
				break;
			case Level::error:
				full_msg += L"ERROR";
				break;
			case Level::fatal:
				full_msg += L"FATAL";
				break;
			}
			full_msg += L"]: " + msg + L"\n";
			emit(full_msg);
		}
		void emit(std::string msg) {
			std::unique_lock<std::mutex> lock(mtx);
			std::clog << msg;
			if (not filename.empty()) {
				std::ofstream of(filename, std::ios::app);
				if (of) {
					of << msg;
					of.close();
				}
			}
		}
		void emit(std::wstring msg) {
			std::unique_lock<std::mutex> lock(mtx);
			std::wclog << msg;
			if (not filename.empty()) {
				std::wofstream of(filename, std::ios::app);
				if (of) {
					of << msg;
					of.close();
				}
			}
		}
	public:
		/**
		* @param logLevel	输出日志级别
		* @param dir		输出文件名 (留空表示不使用文件日志)
		* @param datetime	是否追加日期-时间信息
		*/
		Logger(Level logLevel = Level::info, std::string filename = "", bool datetime = true) :
			logLevel(logLevel), filename(filename), datetime(datetime) {
			if (not filename.empty()) {
				std::ofstream f(filename, std::ios::ate); // 清空文件
				f.close();
			}
		}

		/** @brief 调试信息 */
		void debug(const std::string& msg) { log(Level::debug, msg); }
		void debug(const std::wstring& msg) { log(Level::debug, msg); }
		/** @brief 一般信息 */
		void info(const std::string& msg) { log(Level::info, msg); }
		void info(const std::wstring& msg) { log(Level::info, msg); }
		/** @brief 警告 */
		void warn(const std::string& msg) { log(Level::warn, msg); }
		void warn(const std::wstring& msg) { log(Level::warn, msg); }
		/** @brief 错误 */
		void error(const std::string& msg) { log(Level::error, msg); }
		void error(const std::wstring& msg) { log(Level::error, msg); }
		/** @brief 致命错误 */
		void fatal(const std::string& msg) { log(Level::fatal, msg); }
		void fatal(const std::wstring& msg) { log(Level::fatal, msg); }
	};
} // namespace mlib