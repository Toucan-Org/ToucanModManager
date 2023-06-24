using System;
using System.Diagnostics;
using System.IO;

namespace ToucanUI.Services
{
    public class LoggerService
    {
        private readonly CustomTraceListener _traceListener;

        public LoggerService()
        {
            var logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            var logPath = Path.Combine(logsDirectory, "log.txt");
            CleanLogFile(logPath);

            _traceListener = new CustomTraceListener(logPath);
            Trace.Listeners.Add(_traceListener);
        }

        private void CleanLogFile(string logPath)
        {
            if (File.Exists(logPath))
            {
                var fileInfo = new FileInfo(logPath);
                var fileAge = DateTime.Now - fileInfo.LastWriteTime;

                if (fileAge.TotalDays >= 3) // Clean the file every 3 days
                {
                    File.Delete(logPath);
                }
            }
        }


        public void Close()
        {
            _traceListener.Close();
        }

        // CustomTraceListener
        private class CustomTraceListener : TextWriterTraceListener
        {
            public CustomTraceListener(string logPath) : base(logPath) { }

            public override void WriteLine(string message)
            {
                if (!IsBindingError(message))
                {
                    base.WriteLine(message);
                }
            }

            private bool IsBindingError(string message)
            {
                return message.StartsWith("[Binding]");
            }
        }
    }


}
