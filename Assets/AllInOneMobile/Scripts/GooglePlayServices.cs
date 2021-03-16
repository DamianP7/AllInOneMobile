using UnityEngine;

namespace AllInOneMobile
{
	public static class GooglePlayServices
	{
		static bool signedIn = false;

		public static void SignIn()
		{
			if (signedIn)
				return;
			GooglePlayGames.BasicApi.PlayGamesClientConfiguration config =
				new GooglePlayGames.BasicApi.PlayGamesClientConfiguration.Builder().Build();
			GooglePlayGames.PlayGamesPlatform.InitializeInstance(config);
			GooglePlayGames.PlayGamesPlatform.Activate();
			Social.localUser.Authenticate(succes => { signedIn = succes; });
		}
	}
}