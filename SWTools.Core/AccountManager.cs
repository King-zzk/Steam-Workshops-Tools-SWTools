using System;

namespace SWTools.Core {
    /// <summary>
    /// Steam 账户 (静态类)
    /// </summary>
    static public class AccountManager {
        // 公开的账户
        private static readonly Account[] _pubAccounts = [
            new() { // 钢铁雄心4
                Name = "thb112259", Password = "steamok7416",
                AppIds = [394360]
            },
            new() { // 壁纸引擎
                Name = "kzeon410", Password = "wnq69815I",
                AppIds = [431960]
            },
            new() { // 十字军之王3
                Name = "wbtq1086059", Password = "steamok32548S",
                AppIds = [1158310]
            },
            new() { // 天际线
                Name = "thb112181", Password = "steamok123123",
                AppIds = [255710]
            },
            new() { // 文明6
                Name = "wenming622", Password = "Sw608728",
                AppIds = [289070]
            }
            ];

        // 是否有适用于指定 App 的账户
        public static bool HasAccountFor(long appId) {
            foreach (Account account in _pubAccounts) {
                if (account.AppIds.Contains(appId)) return true;
            }
            return false;
        }

        // 获取适用于指定 App 的账户
        public static List<Account> GetAccountFor(long appId) {
            List<Account> accounts = [];
            foreach (Account account in _pubAccounts) {
                if (account.AppIds.Contains(appId))
                    accounts.Add(account);
            }
            return accounts;
        }
    }
}
