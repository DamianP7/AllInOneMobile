using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;


namespace AllInOneMobile.Utils
{
	public class BuildCheck : IPreprocessBuildWithReport
	{
		public int callbackOrder
		{
			get { return 0; }
		}

		public void OnPreprocessBuild(BuildReport report)
		{
			if (AllInOneMobileSettings.Instance.useAdMob)
			{
#if ADS_TEST
				if (EditorUserBuildSettings.development)
					Debug.Log("Manager will display test ads");
				else
					Debug.LogWarning("Manager will display test ads");
#else
				if (EditorUserBuildSettings.development)
					Debug.LogError("Manager will display real ads");
				else
					Debug.Log("Manager will display real ads");
#endif
			}
		}
	}
}