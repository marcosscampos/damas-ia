using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

namespace Checkers.Common
{
    public class Logger
    {
        private static readonly object Lock = new object();
        private static Logger instance;
        private readonly string datetimeFormat;
        public string Filename { get; set; }

        public static Logger GetSimpleLogger()
        {
            if (instance == null)
            {
                lock (Lock)
                {
                    if (instance == null)
                    {
                        instance = new Logger(true);
                    }
                }
            }

            return instance;
        }

        public void Debug(string text) => WriteFormattedLog(LogLevel.DEBUG, text);
        public void Error(string text) => WriteFormattedLog(LogLevel.ERROR, text);
        public void Fatal(string text) => WriteFormattedLog(LogLevel.FATAL, text);
        public void Info(string text) => WriteFormattedLog(LogLevel.INFO, text);
        public void Trace(string text) => WriteFormattedLog(LogLevel.TRACE, text);
        public void Warning(string text) => WriteFormattedLog(LogLevel.WARNING, text);

        private Logger(bool append = false)
        {
            datetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            Filename = FileNameHelper.GetExecutingDirectory() + Assembly.GetExecutingAssembly().GetName().Name + "_" + GetCurrentDateString() + ".log";

            // Log file header line
            string logHeader = Filename + " is created.";
            if (!File.Exists(Filename))
            {
                WriteLine(DateTime.Now.ToString(datetimeFormat) + " " + logHeader, false);
            }
            else
            {
                if (append == false)
                {
                    WriteLine(DateTime.Now.ToString(datetimeFormat) + " " + logHeader, false);
                }
            }
        }

        private string GetCurrentDateString() => DateTime.Now.ToShortDateString().Replace("/", "_");

        private void WriteFormattedLog(LogLevel level, string text)
        {
            if ((int)level < ConstantsSettings.MinimumLogLevel)
            {
                return;
            }

            string pretext;
            switch (level)
            {
                case LogLevel.TRACE:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [TRACE]   ";
                    break;
                case LogLevel.INFO:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [INFO]    ";
                    break;
                case LogLevel.DEBUG:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [DEBUG]   ";
                    break;
                case LogLevel.WARNING:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [WARNING] ";
                    break;
                case LogLevel.ERROR:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [ERROR]   ";
                    break;
                case LogLevel.FATAL:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [FATAL]   ";
                    break;
                default:
                    pretext = string.Empty;
                    break;
            }

            WriteLine(pretext + text);
        }

        private void WriteLine(string text, bool append = true)
        {
            lock (Lock)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(Filename, append, Encoding.UTF8))
                    {
                        if (text != string.Empty)
                        {
                            writer.WriteLine(text);
                            Console.WriteLine(text);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error saving the log file: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Supported log level
        /// </summary>
        [Serializable]
        private enum LogLevel
        {
            TRACE = 0,
            DEBUG = 1,
            INFO = 2,
            WARNING = 3,
            ERROR = 4,
            FATAL = 5
        }
    }
}
