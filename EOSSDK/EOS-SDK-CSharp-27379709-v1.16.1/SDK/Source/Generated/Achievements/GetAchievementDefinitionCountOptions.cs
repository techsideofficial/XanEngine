// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices.Achievements
{
	/// <summary>
	/// Input parameters for the <see cref="AchievementsInterface.GetAchievementDefinitionCount" /> function.
	/// </summary>
	public struct GetAchievementDefinitionCountOptions
	{
	}

	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	internal struct GetAchievementDefinitionCountOptionsInternal : ISettable<GetAchievementDefinitionCountOptions>, System.IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref GetAchievementDefinitionCountOptions other)
		{
			m_ApiVersion = AchievementsInterface.GetachievementdefinitioncountApiLatest;
		}

		public void Set(ref GetAchievementDefinitionCountOptions? other)
		{
			if (other.HasValue)
			{
				m_ApiVersion = AchievementsInterface.GetachievementdefinitioncountApiLatest;
			}
		}

		public void Dispose()
		{
		}
	}
}