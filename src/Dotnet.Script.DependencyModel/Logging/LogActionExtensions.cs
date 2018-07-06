﻿using System;
using System.Collections.Generic;

namespace Dotnet.Script.DependencyModel.Logging
{
    public delegate Logger LogFactory(Type type);

    public delegate void Logger(LogLevel level, string message, Exception ex = null);
    
    public enum LogLevel
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error,
        Critical
    }

    public static class LogExtensions
    {
        public static Logger CreateLogger<T>(this LogFactory logFactory) => logFactory(typeof(T));

        public static void Trace(this Logger logger, string message) => logger(LogLevel.Trace, message);
        public static void Debug(this Logger logger, string message) => logger(LogLevel.Debug, message);
        public static void Info(this Logger logger, string message) => logger(LogLevel.Info, message);
        public static void Warning(this Logger logger, string message) => logger(LogLevel.Warning, message);
        public static void Error(this Logger logger, string message, Exception exception = null) => logger(LogLevel.Error, message, exception);
        public static void Critical(this Logger logger, string message, Exception exception = null) => logger(LogLevel.Critical, message, exception);
    }

    public static class LevelMapper
    {
        private static Dictionary<string, LogLevel> _levelMap = CreateMap();

        private static Dictionary<string, LogLevel> CreateMap()
        {
            var map = new Dictionary<string, LogLevel>(StringComparer.InvariantCultureIgnoreCase);
            map.Add("t", LogLevel.Trace);
            map.Add("trace", LogLevel.Trace);
            map.Add("d", LogLevel.Debug);
            map.Add("debug", LogLevel.Debug);
            map.Add("i", LogLevel.Info);
            map.Add("info", LogLevel.Info);
            map.Add("w", LogLevel.Warning);
            map.Add("warning", LogLevel.Warning);
            map.Add("e", LogLevel.Error);
            map.Add("error", LogLevel.Error);
            map.Add("c", LogLevel.Critical);
            map.Add("critical", LogLevel.Critical);
            return map;
        }

        public static LogLevel FromString(string levelName)
        {
            if (string.IsNullOrWhiteSpace(levelName))
            {
                return LogLevel.Warning;
            }

            if (!_levelMap.TryGetValue(levelName, out var level))
            {
                throw new InvalidOperationException($"Unknown level name : {levelName}");
            }

            return level;
        }
    }
}
