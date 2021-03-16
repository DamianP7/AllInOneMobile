using UnityEditor;
using UnityEngine;

namespace AllInOneMobile.Utils
{
	public static class AddPrefabToScene
	{
		[MenuItem("AllInOne Mobile/Create/Store Manager")]
		static void InstantiateStore()
		{
			GameObject go = new GameObject("StoreManager");
			go.AddComponent<InAppStore>();
			Selection.activeGameObject = go;
		}

		[MenuItem("AllInOne Mobile/Create/Ads Manager")]
		static void InstantiateAds()
		{
			GameObject go = new GameObject("AdsManager");
			go.AddComponent<Ads>();
			Selection.activeGameObject = go;
		}
	}
}