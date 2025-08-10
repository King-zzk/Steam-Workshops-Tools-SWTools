#pragma once
/**
 * @file		Fuction.hpp
 * @brief		实用函数
 *
 * @author		masterLazy
 * @copyright	Copyright (c) 2025 masterLazy
 */

#include <iostream>
#include <sstream>		// ostringstream
#include <iomanip>		// setprecision()
#include <sys/stat.h>	// stat
#include <chrono>

namespace mlib {
	namespace Function {
		/**
		 * @brief				字符数转字符串
		 * @param bytes			字节数
		 * @param precision		保留几位小数
		 * @param i_style		是否使用如"KiB"的样式
		 */
		inline std::string ByteToString(float bytes, int precision = 2, bool i_style = true) {
			std::ostringstream oss;
			oss.setf(std::ios::fixed, std::ios::floatfield);
			if (bytes <= 999) {
				oss << std::setprecision(precision) << bytes << "B";
				return oss.str();
			}
			bytes /= 1024;
			if (bytes <= 999) {
				oss << std::setprecision(precision) << bytes << (i_style ? "KiB" : "KB");
				return oss.str();
			}
			bytes /= 1024;
			if (bytes <= 999) {
				oss << std::setprecision(precision) << bytes << (i_style ? "MiB" : "MB");
				return oss.str();
			}
			bytes /= 1024;
			if (bytes <= 999) {
				oss << std::setprecision(precision) << bytes << (i_style ? "GiB" : "GB");
				return oss.str();
			}
			oss << std::setprecision(precision) << bytes / 1024 << (i_style ? "TiB" : "TB");
			return oss.str();
		}

		/**
		 * @brief			获取文件大小
		 * @param file		要获取大小的文件指针
		 */
		inline size_t GetFileSize(FILE* file) {
			size_t off = _ftelli64(file);
			_fseeki64(file, -1, 0);
			size_t size = _ftelli64(file);
			_fseeki64(file, 0, off);
			return size;
		}

		/**
		* @brief		获取日期-时间字符串 (yyyy-mm-dd_hh:mm:ss)
		* @param date	是否包括 yyyy-mm-dd
		* @param datetime	是否包括 hh:mm:ss
		*/
		inline std::string GetFormatDateTime(bool date = true, bool datetime = true) {
			using namespace std::chrono;
			auto now = system_clock::now();
			time_t now_time = system_clock::to_time_t(now);
			tm timeinfo;
			localtime_s(&timeinfo, &now_time);
			std::ostringstream ss;
			if (date && datetime) {
				ss << std::put_time(&timeinfo, "%Y-%m-%d_%H:%M:%S");
			} else if (date) {
				ss << std::put_time(&timeinfo, "%Y-%m-%d");
			} else if (datetime) {
				ss << std::put_time(&timeinfo, "%H:%M:%S");
			}
			return ss.str();
		}
		inline std::wstring GetFormatDateTimeW(bool date = true, bool datetime = true) {
			using namespace std::chrono;
			auto now = system_clock::now();
			time_t now_time = system_clock::to_time_t(now);
			tm timeinfo;
			localtime_s(&timeinfo, &now_time);
			std::wostringstream ss;
			if (date && datetime) {
				ss << std::put_time(&timeinfo, L"%Y-%m-%d_%H:%M:%S");
			} else if (date) {
				ss << std::put_time(&timeinfo, L"%Y-%m-%d");
			} else if (datetime) {
				ss << std::put_time(&timeinfo, L"%H:%M:%S");
			}
			return ss.str();
		}
	} // namespace Function
} // namespace mlib