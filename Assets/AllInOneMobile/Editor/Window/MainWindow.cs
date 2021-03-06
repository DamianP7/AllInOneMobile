using UnityEditor;
using UnityEngine;

namespace AllInOneMobile
{
	/// <summary>
	/// Plugin main window editor
	/// </summary>
	public class MainWindow : EditorWindow
	{
		bool adsInstalled = false;
		const string adsPluginPath = "/Plugins/Android/googlemobileads-unity.aar";
		bool achievementsInstalled = false;
		const string achievementsPluginPath = "/GooglePlayGames/Plugins/Android/GooglePlayGamesManifest.plugin/project.properties";
		bool inAppPurchasesInstalled = false;
		const string inAppPurchasesPath = "/Plugins/UnityPurchasing/Bin/Stores.dll";
		bool analyticsInstalled = false;
		const string analyticsPath = "/Firebase/Plugins/Firebase.Analytics.dll";
		
		AllInOneMobileSettings settings;

		AdsTab adsTab;
		AchievementsTab achievementsTab;
		InAppStoreTab inAppStoreTab;
		AnalyticsTab analyticsTab;

		#region Open/Close
		[MenuItem("AllInOne Mobile/Main Settings")]
		public static void ShowWindow()
		{
			GetWindow(typeof(MainWindow), false, "AllInOne Mobile Settings");
		}

		void OnFocus()
		{
			adsInstalled = System.IO.File.Exists(Application.dataPath + adsPluginPath);
			adsTab.adsInstalled = adsInstalled;

			achievementsInstalled = System.IO.File.Exists(Application.dataPath + achievementsPluginPath);
			achievementsTab.achievementsInstalled = achievementsInstalled;

			inAppPurchasesInstalled = System.IO.File.Exists(Application.dataPath + inAppPurchasesPath);
			inAppStoreTab.inappsInstalled = inAppPurchasesInstalled;

			analyticsInstalled = System.IO.File.Exists(Application.dataPath + analyticsPath);
			analyticsTab.analyticsInstalled = analyticsInstalled;

		}

		protected void OnEnable()
		{
			settings = AllInOneMobileSettings.Instance;

			adsTab = new AdsTab(settings);
			achievementsTab = new AchievementsTab(settings);
			inAppStoreTab = new InAppStoreTab(settings);
			analyticsTab = new AnalyticsTab(settings);
		}
		#endregion

		void OnGUI()
		{
			EditorUtility.SetDirty(this);

			//groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
			//Time.timeScale = EditorGUILayout.Slider("Time Scale", Time.timeScale, 0, 2);
			//EditorGUILayout.EndToggleGroup();

			adsTab.ShowTab();

			achievementsTab.ShowTab();
			
			inAppStoreTab.ShowTab();

			if (GUI.changed)
			{
				adsTab.Save();
				settings.Save();
			}
		}
	}
}