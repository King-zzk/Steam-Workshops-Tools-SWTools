using System;
using System.Collections.Generic;
using System.Text;

namespace SWTools.Core {
    /// <summary>
    ///  App 名字
    ///  完整的列表太长，这里只列举程序支持的
    /// </summary>
    public static partial class Constants {
        public static readonly Dictionary<long, string> AppNames = new() {
            { 1158310, "Crusader Kings III" },
            { 255710, "Cities: Skylines" },
            { 394360, "Hearts of Iron IV" },
            { 289070, "Sid Meier's Civilization VI" },
            { 431960, "Wallpaper Engine" },
            { 529340, "Victoria 3" },
            //
            { 236850, "Europa Universalis IV" },
            { 1677280, "Company of Heroes 3" },
        };
    }
}