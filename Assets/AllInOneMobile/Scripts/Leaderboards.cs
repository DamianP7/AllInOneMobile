using GooglePlayGames;
using UnityEngine;

namespace AllInOneMobile
{
	public static class Leaderboards
	{
		/// <summary>
		/// Save score on leaderboard.
		/// </summary>
		/// <param name="id">Use class GPGIds to choose correct ID</param>
		/// <param name="score">Score to post</param>
		public static bool ReportScore(string id, long score)
		{
			bool value = false;
			GooglePlayServices.SignIn();
			Social.ReportScore(score, id, succes => { value = succes; });
			return value;
		}
		
		/// <summary>
		/// Show the build-in UI for leaderboards.
		/// </summary>
		public static void ShowUI()
		{
			GooglePlayServices.SignIn();
			Social.ShowLeaderboardUI();
		}
		
		/// <summary>
		/// Show the build-in UI fora  particular leaderboard.
		/// </summary>
		/// <param name="id">Use class GPGIds to choose correct ID</param>
		public static void ShowUI(string id)
		{
			GooglePlayServices.SignIn();
			PlayGamesPlatform.Instance.ShowLeaderboardUI(id);
		}
	}
}