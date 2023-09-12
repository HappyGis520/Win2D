using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace App2222
{
    public enum LogLevel
    {
        Error,
        Warning,
        Info
    }
    public class Logger : IDisposable
    {
        private object lockObject = new object();
        private string logFilePath;
        private StreamWriter writer;
        private bool isWriting = false;
        private Queue<string> logQueue = new Queue<string>();
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private Task logProcessingTask;

        public Logger(string filePath)
        {
            logFilePath = filePath;
            // 创建写入器
            writer = new StreamWriter(logFilePath, true);
            // 启动后台线程处理日志队列
            logProcessingTask = Task.Factory.StartNew(ProcessLogQueue, cancellationTokenSource.Token,
                TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
        public void Log(LogLevel level, string message)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string logEntry = $"{DateTime.Now} [{level}] - {message}";

            lock (lockObject)
            {
                logQueue.Enqueue(logEntry);
                Monitor.Pulse(lockObject); // 唤醒后台线程处理日志队列
            }
        }

        private void ProcessLogQueue()
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                string logEntry = null;

                lock (lockObject)
                {
                    while (logQueue.Count == 0)
                    {
                        Monitor.Wait(lockObject); // 等待日志队列有内容
                    }

                    logEntry = logQueue.Dequeue();
                }

                WriteLogToFile(logEntry);
            }
        }

        private void WriteLogToFile(string logEntry)
        {
            while (isWriting)
            {
                Thread.Sleep(10); // 等待上一次写入完成
            }

            isWriting = true;

            try
            {
                writer.WriteLine(logEntry);
                writer.Flush(); // 立即写入文件
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
            finally
            {
                isWriting = false;
            }
        }
        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            //logProcessingTask.Wait(); // 等待后台线程完成

            writer.Close();
            writer.Dispose();
        }
    }


}
