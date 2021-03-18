using UnityEditor;
using UnityEngine;

namespace AllInOneMobile.Editor
{
	/// <summary>
	/// Plugin main window editor
	/// </summary>
	public class MainWindow : EditorWindow
	{
		bool adsInstalled = false;
		const string adsPluginPath = "/Plugins/Android/googlemobileads-unity.aar";
		bool achievementsInstalled = false;

		const string achievementsPluginPath =
			"/GooglePlayGames/Plugins/Android/GooglePlayGamesManifest.plugin/project.properties";

		bool inAppPurchasesInstalled = false;
		const string inAppPurchasesPath = "/Plugins/UnityPurchasing/Bin/Stores.dll";

		AllInOneMobileSettings settings;

		AdsTab adsTab;
		GameServicesTab gameServicesTab;
		InAppStoreTab inAppStoreTab;
		
		int tab = 0;

#region Open/Close

		[MenuItem("AllInOne Mobile/Main Settings")]
		public static void ShowWindow()
		{
			GetWindow(typeof(MainWindow), false, "AllInOne Mobile Settings");
		}

		void OnFocus()
		{
			if(adsTab == null || gameServicesTab == null || inAppStoreTab == null)
				OnEnable();
				
			adsInstalled = System.IO.File.Exists(Application.dataPath + adsPluginPath);
			adsTab.adsInstalled = adsInstalled;

			achievementsInstalled = System.IO.File.Exists(Application.dataPath + achievementsPluginPath);
			gameServicesTab.achievementsInstalled = achievementsInstalled;

			inAppPurchasesInstalled = System.IO.File.Exists(Application.dataPath + inAppPurchasesPath);
			inAppStoreTab.inappsInstalled = inAppPurchasesInstalled;

			SetDefineSymbols();
		}

		protected void OnEnable()
		{
			settings = AllInOneMobileSettings.Instance;

			adsTab = new AdsTab(settings);
			gameServicesTab = new GameServicesTab(settings);
			inAppStoreTab = new InAppStoreTab(settings);
		}

#endregion


		void OnGUI()
		{
			EditorUtility.SetDirty(this);

			//groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
			//Time.timeScale = EditorGUILayout.Slider("Time Scale", Time.timeScale, 0, 2);
			//EditorGUILayout.EndToggleGroup();
			tab = GUILayout.Toolbar(tab, new string[] {"Ads", "Game Services", "Store"});
			switch (tab)
			{
				case 0:
					adsTab.ShowTab();
					break;
				case 1:
					gameServicesTab.ShowTab();
					break;
				case 2:
					inAppStoreTab.ShowTab();
					break;
			}


			if (GUI.changed)
			{
				adsTab.Save();
				settings.Save();
			}
		}

		void SetDefineSymbols()
		{
			string s = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
			if (adsInstalled && !s.Contains("ADS"))
				s += ";ADS";
			else if (!adsInstalled && s.Contains("ADS"))
				s.Remove(s.IndexOf("ADS"), 3);

			if (achievementsInstalled && !s.Contains("GPG"))
				s += ";GPG";
			else if (!achievementsInstalled && s.Contains("GPG"))
				s.Remove(s.IndexOf("GPG"), 3);

			PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, s);
		}
	}
}