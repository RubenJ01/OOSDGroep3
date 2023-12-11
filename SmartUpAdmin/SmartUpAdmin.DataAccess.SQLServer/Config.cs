namespace SmartUp.DataAccess.SQLServer
{
    public class Config
    {
        private static Config? instance;
        public bool UseServer { get; set; }

        public string SshServer { get; set; }
        public bool TrustServerCertificate { get; set; }
        public string SshHost { get; set; }
        public string SshUsername { get; set; }
        public string SshPassword { get; set; }
        public string MssqlHost { get; set; }
        public string MssqlUsername { get; set; }
        public string MssqlPassword { get; set; }
        public int MssqlPort { get; set; }

        public string LocalServer { get; set; }
        public string LocalDatabase { get; set; }
        public string LocalUser { get; set; }
        public string LocalPassword { get; set; }

        private Config()
        {
            Dictionary<string, string> loadedConfig = LoadConfigurationFromFile(GetFilePath());
            UseServer = bool.Parse(loadedConfig["UseServer"]);
            SshServer = loadedConfig["SshServer"];
            TrustServerCertificate = bool.Parse(loadedConfig["TrustServerCertificate"]);
            SshHost = loadedConfig["SshHost"];
            SshUsername = loadedConfig["SshUsername"];
            SshPassword = loadedConfig["SshPassword"];
            MssqlHost = loadedConfig["MssqlHost"];
            MssqlUsername = loadedConfig["MssqlUsername"];
            MssqlPassword = loadedConfig["MssqlPassword"];
            MssqlPort = Int32.Parse(loadedConfig["MssqlPort"]);
            LocalServer = loadedConfig["LocalServer"];
            LocalDatabase = loadedConfig["LocalDatabase"];

        }


        public static Config GetInstance()
        {
            if (instance == null)
            {
                instance = new Config();
            }
            return instance;
        }

        private static string GetFilePath()
        {
            string configFilePath = FindSolutionPath() + "/SmartUpAdmin.DataAccess.SQLServer/";
            configFilePath = configFilePath.Replace("\\SmartUpAdmin.sln", "");
            return Path.Combine(configFilePath, "databaseConfig.yaml");
        }

        private static string FindSolutionPath()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            while (!string.IsNullOrEmpty(currentDirectory))
            {
                string[] solutionFiles = Directory.GetFiles(currentDirectory, "*.sln");

                if (solutionFiles.Length > 0)
                {
                    return solutionFiles[0];
                }

                currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
            }

            return null;
        }

        private static Dictionary<string, string> LoadConfigurationFromFile(string filePath)
        {
            Dictionary<string, string> config = new();
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();
                    config[key] = value;
                }
            }

            return config;
        }
    }
}
