using NLog;

namespace DNS_changer.Helper
{
    public static class Logging
    {
        /// <summary>
        /// Configs the NLog
        /// </summary>
        public static void SetConfig()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logFile = new NLog.Targets.FileTarget("logfile") { FileName = "LogFile.txt" };

            config.AddRule(LogLevel.Info, LogLevel.Fatal, logFile);

            LogManager.Configuration = config;
        }
    }
}
