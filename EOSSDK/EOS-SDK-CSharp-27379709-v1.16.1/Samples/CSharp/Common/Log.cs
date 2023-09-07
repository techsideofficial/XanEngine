// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Logging;
using System;
using System.Diagnostics;

namespace Epic.OnlineServices.Samples
{
	public static class Log
	{
		public static event LogLineReceived LineReceived;

		public static void WriteLine(string message, LogStyle style = LogStyle.Info)
		{
			Console.ResetColor();

			if (style == LogStyle.Info)
			{
			}
			else if (style == LogStyle.SuperInfo)
			{
				Console.BackgroundColor = ConsoleColor.White;
				Console.ForegroundColor = ConsoleColor.Black;
			}
			else if (style == LogStyle.Muted)
			{
				Console.ForegroundColor = ConsoleColor.DarkGray;
			}
			else if (style == LogStyle.SuperMuted)
			{
				Console.BackgroundColor = ConsoleColor.DarkGray;
				Console.ForegroundColor = ConsoleColor.Black;
			}
			else if (style == LogStyle.Warning)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
			}
			else if (style == LogStyle.SuperWarning)
			{
				Console.BackgroundColor = ConsoleColor.Yellow;
				Console.ForegroundColor = ConsoleColor.Black;
			}
			else if (style == LogStyle.Bad)
			{
				Console.ForegroundColor = ConsoleColor.Red;
			}
			else if (style == LogStyle.SuperBad)
			{
				Console.BackgroundColor = ConsoleColor.Red;
				Console.ForegroundColor = ConsoleColor.Black;
			}
			else if (style == LogStyle.Good)
			{
				Console.ForegroundColor = ConsoleColor.Green;
			}
			else if (style == LogStyle.SuperGood)
			{
				Console.BackgroundColor = ConsoleColor.Green;
				Console.ForegroundColor = ConsoleColor.Black;
			}

			var output = $"[{DateTimeOffset.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] {message}";
			Console.WriteLine(output);
			Trace.WriteLine(output);

			Console.ResetColor();

			LineReceived?.Invoke(new LogLine(output, style));
		}

		public static void WriteException(Exception exception)
		{
			WriteLine($"{exception.GetType().Name}: {exception.Message}\r\n{exception.StackTrace}", LogStyle.Bad);
		}

		public static void WriteEpic(LogMessage message)
		{
			var level = LogStyle.Muted;
			if (message.Level == LogLevel.Info)
			{
				level = LogStyle.Info;
			}
			else if (message.Level == LogLevel.Warning)
			{
				level = LogStyle.Warning;
			}
			else if (message.Level == LogLevel.Error)
			{
				level = LogStyle.Bad;
			}
			else if (message.Level == LogLevel.Fatal)
			{
				level = LogStyle.SuperBad;
			}

			WriteLine($"[Native] {message.Category} {message.Level}: {message.Message}", level);
		}

		public static void WriteResult(string message, Result result)
		{
			var level = LogStyle.Warning;
			if (result == Result.Success)
			{
				level = LogStyle.Good;
			}
			else if (Common.IsOperationComplete(result))
			{
				level = LogStyle.Bad;
			}

			WriteLine($"{message}: {result}", level);
		}
	}
}