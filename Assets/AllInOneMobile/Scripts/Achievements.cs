using UnityEngine;

namespace AllInOneMobile
{
	public static class Achievements
	{
#if GPG
		/// <summary>
		/// Use to reveal hidden achievement.
		/// </summary>
		/// <param name="id">Use class GPGIds to choose correct ID</param>
		public static bool Reveal(string id)
		{
			if (!AllInOneMobileSettings.Instance.useAchievements)
			{
				Debug.LogError("Game Services are disabled.");
				return false;
			}
			bool value = false;
			GooglePlayServices.SignIn();
			Social.ReportProgress(id, 0, succes => { value = succes; });
			return value;
		}

		/// <summary>
		/// Unlock achievement.
		/// </summary>
		/// <param name="id">Use class GPGIds to choose correct ID</param>
		public static bool Unlock(string id)
		{
			if (!AllInOneMobileSettings.Instance.useAchievements)
			{
				Debug.LogError("Game Services are disabled.");
				return false;
			}
			bool value = false;
			GooglePlayServices.SignIn();
			Social.ReportProgress(id, 100, succes => { value = succes; });
			return value;
		}

		/// <summary>
		/// Increase increment achievement.
		/// </summary>
		/// <param name="id">Use class GPGIds to choose correct ID</param>
		/// <param name="stepsToIncrement">Amount to increment</param>
		public static bool Increment(string id, int stepsToIncrement = 1)
		{
			if (!AllInOneMobileSettings.Instance.useAchievements)
			{
				Debug.LogError("Game Services are disabled.");
				return false;
			}
			bool value = false;
			GooglePlayServices.SignIn();
			GooglePlayGames.PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement,
				succes => { value = succes; });
			return value;
		}

		/// <summary>
		/// Show the build-in UI for achievements.
		/// </summary>
		public static void ShowUI()
		{
			if (!AllInOneMobileSettings.Instance.useAchievements)
			{
				Debug.LogError("Game Services are disabled.");
				return;
			}
			GooglePlayServices.SignIn();
			Social.ShowAchievementsUI();
		}
#endif
	}
}