using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AllInOneMobile
{
	public class AllInOneMobileSettings : ScriptableObject
	{
#region Instance

		const string settingsDir = "Assets/AllInOneMobile";
		const string settingsResourcesDir = "Assets/AllInOneMobile/Resources";
		const string settingsFile = "Assets/AllInOneMobile/Resources/AllInOneMobileSettings.asset";
		const string googleAdsSettings = "Assets/GoogleMobileAds/Resources/GoogleMobileAdsSettings.asset";

		static AllInOneMobileSettings instance;

		public static AllInOneMobileSettings Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Resources.Load<AllInOneMobileSettings>("AllInOneMobileSettings");
				}

				return instance;
			}
		}

		public void Save()
		{
#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
#endif
		}

#endregion

#region Ads Settings

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

		public bool useAdMob = false;
		public bool useBaner = false;
		public bool useInterstitial = false;
		public bool useRewarded = false;

		public string androidBaner;
		public string androidInterstitial;
		public string androidRewarded;
#if ADS
		public GoogleMobileAds.Api.AdSize bannerSize;
		public GoogleMobileAds.Api.AdPosition adPosition;
#endif
		public int minSecondsBetweenAds;

#endregion

#region Achievements

		public bool useAchievements = false;

#endregion

#region InAppPurchases

		public bool useInAppPurchases = false;
		public List<InAppProduct> products;

#endregion
	}

	[Serializable]
	public class InAppProduct
	{
		public string name;
		public string id;
#if UNITY_PURCHASING
		public UnityEngine.Purchasing.ProductType productType;
#endif
	}
}