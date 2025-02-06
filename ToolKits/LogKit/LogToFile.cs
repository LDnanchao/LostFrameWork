using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Text.RegularExpressions;

public class LogToFile : MonoBehaviour
{
    public int logRetentionHours = 24; // 设置日志保留时间（小时），默认24小时
    private string logFilePath;
    private int logIntervalInSeconds = 5; // 设置日志输出间隔（秒）
    private int cleanupIntervalInSeconds = 3600; // 设置清理间隔（秒），例如1小时

    void OnEnable()
    {
        // 获取当前启动时间
        string startTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        // 设置不同平台的日志文件路径
        logFilePath = GetPlatformSpecificLogPath($"log_{startTime}.txt");
        // 创建文件夹
        Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
        // 开始监听日志
        Application.logMessageReceived += HandleLog;

        // 启动日志清理协程
        StartCoroutine(CleanupOldLogFiles());
    }

    void OnDisable()
    {
        // 停止监听日志
        Application.logMessageReceived -= HandleLog;
    }

    private string GetPlatformSpecificLogPath(string fileName)
    {
        string logDirectory = "";

        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                logDirectory = Path.Combine(Application.persistentDataPath, "Logs");
                break;
            case RuntimePlatform.IPhonePlayer:
                logDirectory = Path.Combine(Application.persistentDataPath, "Logs");
                break;
            case RuntimePlatform.WindowsPlayer:
                logDirectory = Path.Combine(Application.persistentDataPath, "Logs");
                break;
            case RuntimePlatform.OSXPlayer:
                logDirectory = Path.Combine(Application.persistentDataPath, "Logs");
                break;
            case RuntimePlatform.WebGLPlayer:
                logDirectory = Path.Combine(Application.persistentDataPath, "Logs");
                break;
            default:
                logDirectory = Path.Combine(Application.dataPath, "../Logs");
                break;
        }

        return Path.Combine(logDirectory, fileName);
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // 格式化日志信息
        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{type}] {logString}\n";
        if (!string.IsNullOrEmpty(stackTrace))
        {
            logMessage += $"StackTrace: {stackTrace}\n";
        }

        // 写入文件
        File.AppendAllText(logFilePath, logMessage);
    }

    private IEnumerator CleanupOldLogFiles()
    {
        while (true)
        {
            CleanupLogs();
            yield return new WaitForSeconds(cleanupIntervalInSeconds);
        }
    }

    private void CleanupLogs()
    {
        string logDirectory = Path.GetDirectoryName(logFilePath);
        DateTime retentionTime = DateTime.Now.AddHours(-logRetentionHours);

        if (Directory.Exists(logDirectory))
        {
            foreach (string file in Directory.GetFiles(logDirectory, "*.txt"))
            {
                if (IsLogFileNameValid(file) && File.GetCreationTime(file) < retentionTime)
                {
                    File.Delete(file);
                }
            }
        }
    }

    private bool IsLogFileNameValid(string filePath)
    {
        string fileName = Path.GetFileName(filePath);
        // 使用正则表达式检查文件名格式是否为 log_YYYYMMDD_HHMMSS.txt
        string pattern = @"^log_\d{8}_\d{6}\.txt$";
        return Regex.IsMatch(fileName, pattern);
    }

    
}
