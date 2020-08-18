using NLog;

namespace DNS_changer.Helper
{
    public class Logging
    {
        /// <summary>
        /// Configs the NLog
        /// </summary>
        public void SetConfig()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logFile = new NLog.Targets.FileTarget("logfile") {FileName = "LogFile.txt" };

            config.AddRule(LogLevel.Info, LogLevel.Fatal, logFile);

            LogManager.Configuration = config;
        }
    }
}
