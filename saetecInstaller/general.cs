using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saetecInstaller
{
    public static class general
    {

        // Paths
        public static string wampExecutable = @"C:\wamp\wampmanager.exe";
        public static string wamp64Executable = @"C:\wamp64\wampmanager.exe";
        public static string installPath = @"C:\wamp\www";
        public static string tccPath = "";
        public static string finalPath = "";

        // Options Set
        public static bool MySQLPasswordChanged = false;
        public static int installType = 0;
        public static bool sqlMount = true;
        public static bool startWamp = true;
        public static bool formatRootDirectory = false;
        public static bool installSubDirectory = false;
        public static string subdirectoryName = "";
        public static bool wamp64 = false;
        public static bool subdirectoryClose = false;
        public static bool downloadAwait = true;

        // Download Options
        public static string gitAddress = "https://github.com/trusted-ws/saetec/archive/master.zip";
        public static string gitToken = "";
        public static string gitRepName = "saetec";

        // Database Connection
        public static string dbUsername = "root";
        public static string dbPassword = "";
        public static string dbHost = "127.0.0.1";
        public static string dbPort = "3306";
        public static string dbName = "saetec";

    }
}
