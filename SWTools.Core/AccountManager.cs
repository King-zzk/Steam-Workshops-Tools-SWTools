using System.Text.Json;

namespace SWTools.Core {
    /// <summary>
    /// Steam 账户管理器 (静态类)
    /// </summary>
    static public class AccountManager {
        private static List<Account> _accounts = [];

        // 公有账户池版本
        public static string PubVersion { get; private set; } = string.Empty;
        // 匿名账户
        public static readonly Account Anonymous = new() { Name= "anonymous", Password="" };

        // 启动
        public static void Setup() {
            LoadPub();
        }

        // 读取保存的公有账户
        public static void LoadPub() {
            if (!File.Exists(Constants.PubAccountsFile)) {
                LogManager.Log?.Warning("{Filename} not found, skipping loading",
                    Constants.PubAccountsFile);
                return;
            }
            try {
                string jsonString;
                using StreamReader sr = new(Constants.PubAccountsFile);
                jsonString = sr.ReadToEnd();
                var pub = JsonSerializer.Deserialize<API.PubAccounts.Response>(jsonString, Constants.JsonOptions);
                if (pub == null) throw new Exception("pub is null");
                if (pub.Accounts == null) throw new Exception("pub.Accounts is null");
                if (pub.Version == null) throw new Exception("pub.Version is null");
                foreach (var account in pub.Accounts) {
                    _accounts.Remove(account); // 保证覆盖
                    _accounts.Add(account);
                }
                PubVersion = pub.Version;
                LogManager.Log.Information("Loaded public accounts from {Filaname} (version {Version})",
                    Constants.PubAccountsFile, pub.Version);
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when loading {Filename}:\n{Exception}",
                    Constants.PubAccountsFile, ex);
            }
        }

        // 是否有适用于指定 App 的账户
        public static bool HasAccountFor(long appId) {
            foreach (Account account in _accounts) {
                if (account.AppIds.Contains(appId)) return true;
            }
            return false;
        }

        // 获取适用于指定 App 的账户
        public static List<Account> GetAccountFor(long appId) {
            List<Account> accounts = [];
            foreach (Account account in _accounts) {
                if (account.AppIds.Contains(appId))
                    accounts.Add(account);
            }
            return accounts;
        }
    }
}
