// Copyright Epic Games, Inc. All Rights Reserved.

namespace Epic.OnlineServices.Samples
{
	public class LogLine
	{
		public string Message { get; private set; }
		public LogStyle Style { get; private set; }

		public LogLine(string message, LogStyle style)
		{
			Message = message;
			Style = style;
		}
	}
}
