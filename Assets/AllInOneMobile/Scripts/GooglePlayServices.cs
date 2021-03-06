using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

namespace AllInOneMobile
{
	public static class GooglePlayServices
	{
		static bool signedIn = false;

		public static void SignIn()
		{
			if (signedIn)
				return;
			PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
			PlayGamesPlatform.InitializeInstance(config);
			PlayGamesPlatform.Activate();
			Social.localUser.Authenticate(succes => { signedIn = succes; });
		}
	}
}