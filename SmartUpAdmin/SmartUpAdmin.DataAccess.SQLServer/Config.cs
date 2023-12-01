namespace SmartUp.DataAccess.SQLServer
{
    public class Config
    {
        private static Config? instance;
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        private Config()
        {
            Dictionary<string, string> loadedConfig = LoadConfigurationFromFile(GetFilePath());
            Server = loadedConfig["Server"];
            Database = loadedConfig["Database"];
            User = loadedConfig["User"];
            Password = loadedConfig["Password"];
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
           string configFilePath = FindSolutionPath() + "/SmartUp.DataAccess.SQLServer/";
           configFilePath = configFilePath.Replace("\\SmartUp.sln", "");
           return Path.Combine(configFilePath, "databaseConfig.yaml");
        }

        private static string FindSolutionPath()
        {

            // Start from the current directory and move upward to find the solution file
            string currentDirectory = Directory.GetCurrentDirectory();

            while (!string.IsNullOrEmpty(currentDirectory))
            {
                string[] solutionFiles = Directory.GetFiles(currentDirectory, "*.sln");

                if (solutionFiles.Length > 0)
                {
                    // Found a solution file, return its path
                    return solutionFiles[0];
                }

                // Move up one directory
                currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
            }

            // Solution file not found
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
