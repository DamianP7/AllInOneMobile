using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/*
 * Define ADS_TEST to test ads.
 * This is really important, because testing with real ads is against AdMob policy
 * 
 * 
 * 
 * 
 * 
 */

namespace AllInOneMobile
{

	public class AllInOneMobileSettings : ScriptableObject
	{
		#region Instance
		private const string settingsDir = "Assets/AllInOneMobile";
		private const string settingsResourcesDir = "Assets/AllInOneMobile/Resources";
		private const string settingsFile = "Assets/AllInOneMobile/Resources/AllInOneMobileSettings.asset";

		private const string googleAdsSettings = "Assets/GoogleMobileAds/Resources/GoogleMobileAdsSettings.asset";

		private static AllInOneMobileSettings instance;
		public static AllInOneMobileSettings Instance
		{
			get
			{
				if (instance == null)
				{
#if UNITY_EDITOR
					if (!AssetDatabase.IsValidFolder(settingsResourcesDir))
					{
						AssetDatabase.CreateFolder(settingsDir, "Resources");
					}

					instance = (AllInOneMobileSettings)AssetDatabase.LoadAssetAtPath(
						settingsFile, typeof(AllInOneMobileSettings));

					if (instance == null)
					{
						instance = ScriptableObject.CreateInstance<AllInOneMobileSettings>();
						AssetDatabase.CreateAsset(instance, settingsFile);
					}
#endif
				}
				return instance;
			}
		}
		#endregion

		#region Ads Settings
		// TODO: ADS_DEBUG - show errors and warnings (e.x. not setted ids)
		// TODO: add error to OnApplicationBuild (??) - can't build with some errors
		public string AndroidBaner
		{
#if ADS_TEST
			get => "ca-app-pub-3940256099942544/6300978111";    // Sample ad unit ID
#else
			get => androidBaner;
#endif
			set => androidBaner = value;
		}
		public string AndroidInterstitial
		{
#if ADS_TEST
			get => "ca-app-pub-3940256099942544/1033173712";    // Sample ad unit ID
#else
			get => androidInterstitial;
#endif
			set => androidInterstitial = value;
		}
		public string AndroidRewarded
		{
#if ADS_TEST
			get => "ca-app-pub-3940256099942544/5224354917";    // Sample ad unit ID
#else
			get => androidRewarded;
#endif
			set => androidRewarded = value;
		}

		public void WriteSettingsToFile()
		{
#if UNITY_EDITOR
			AssetDatabase.SaveAssets();
#endif
		}

		public bool useAdMob = false;
		public bool useBaner = false;
		public bool useInterstitial = false;
		public bool useRewarded = false;

		public string androidID = "";
		public string androidBaner;
		public string androidInterstitial;
		public string androidRewarded;

		public AdSize bannerSize;
		public AdPosition adPosition;
		public int minSecondsBetweenAds;


		#endregion

		#region Achievements


		public bool useAchievements = false;




		#endregion
	}
}
