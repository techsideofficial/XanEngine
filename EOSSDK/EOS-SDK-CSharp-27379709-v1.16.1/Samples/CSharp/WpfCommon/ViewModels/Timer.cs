// Copyright Epic Games, Inc. All Rights Reserved.

namespace Epic.OnlineServices.Samples.ViewModels
{
	public class Timer : Bindable
	{
		private float m_MaxTime;
		public float MaxTime
		{
			get { return m_MaxTime; }
			set { SetProperty(ref m_MaxTime, value); }
		}

		private float m_CurrentTime;
		public float CurrentTime
		{
			get { return m_CurrentTime; }
			set { SetProperty(ref m_CurrentTime, value); }
		}

		public TimerComplete Complete { get; set; }

		public void Update(float deltaTime)
		{
			CurrentTime += deltaTime;
			if (CurrentTime >= m_MaxTime)
			{
				Complete?.Invoke();
				CurrentTime = 0;
			}
		}
	}
}
