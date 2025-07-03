#pragma once
/**
 * @file		logger.hpp
 * @brief		简单的日志类
 *
 * @author		masterLazy
 * @copyright	Copyright (c) 2025 masterLazy
 */

#include <iostream>
#include <fstream>
#include <vector>

#include "function.hpp"

namespace mlib {
	namespace logger {
		enum LogLevel {
			debug = 0,
			info = 1,
			warn = 2,
			error = 3,
			fatal = 4
		};
		/**
		* @brief	简单的日志类
		*/
		class Logger {
			LogLevel log_level;
			bool date_time;
			std::ostream& os;
			std::vector<std::string> filenames;
			void log(LogLevel log_level, const std::string& msg) const {
				if (log_level < this->log_level) return;
				if (date_time) {
					dispatch(function::GetFormattedDateTime(false));dispatch(" ");
				}
				switch (log_level) {
				case logger::debug:
					dispatch("[DEBUG] ");
					break;
				case logger::info:
					dispatch("[INFO] ");
					break;
				case logger::warn:
					dispatch("[WARN] ");
					break;
				case logger::error:
					dispatch("[ERROR]");
					break;
				case logger::fatal:
					dispatch("[FATAL]");
					break;
				}
				dispatch(" ");dispatch(msg);dispatch("\n");
			}
			void dispatch(std::string msg) const {
				os << msg;
				std::ofstream of;
				for (auto filename : filenames) {
					of.open(filename);
					if (not of.is_open())continue; // 忽略这个输出目标
					of << msg;
					of.close();
				}
			}
		public:
			/**
			* @param log_level	输出日志级别
			* @param date_time	是否输出时间
			* @param os			要使用的输出流
			*/
			Logger(LogLevel log_level = logger::info, bool date_time = true, std::ostream& os = std::clog) :
				log_level(log_level), date_time(date_time), os(os) {
			}

			/**
			 * @brief 			添加文件输出
			 * @param filename	要写入的文件名
			 */
			void addFileSink(std::string filename) {
				filenames.push_back(filename);
			}

			/** @brief 调试信息 */
			void debug(const std::string& msg) const {
				log(logger::debug, msg);
			}
			/** @brief 一般信息 */
			void info(const std::string& msg) const {
				log(logger::info, msg);
			}
			/** @brief 警告 */
			void warn(const std::string& msg) const {
				log(logger::warn, msg);
			}
			/** @brief 错误 */
			void error(const std::string& msg) const {
				log(logger::error, msg);
			}
			/** @brief 致命错误 */
			void fatal(const std::string& msg) const {
				log(logger::fatal, msg);
			}
		};
	} // namespace logger
} // namespace mlib