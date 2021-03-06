using UnityEngine;
using GooglePlayGames;

namespace AllInOneMobile
{
    public static class Achievements
    {
        /// <summary>
        /// Use to reveal hidden achievement.
        /// </summary>
        /// <param name="id">Use class GPGIds to choose correct ID</param>
        public static bool Reveal(string id)
        {
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
            bool value = false;
            GooglePlayServices.SignIn();
            PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, succes => { value = succes; });
            return value;
        }
        
        /// <summary>
        /// Show the build-in UI for achievements.
        /// </summary>
        public static void ShowUI()
        {
            GooglePlayServices.SignIn();
            Social.ShowAchievementsUI();
        }
    }
}