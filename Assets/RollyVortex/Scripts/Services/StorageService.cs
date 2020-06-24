using System;
using UnityEngine;

namespace RollyVortex.Scripts.Services
{
    public enum PlayerPrefStrings
    {
        AvailableSkins,
    }

    public enum PlayerPrefInts
    {
        Gems,
        HighScore,
        SelectedCharacterSkin,
        SelectedTunnelSkin,
        
        TotalObstaclesDestroyed,
        TotalTimesPlayed,
        TotalGemsCollected,
        
        // Used for missions - reset to 0 when the mission is completed
        TimesPlayed,
        ObstaclesDestroyed,
        GemsCollected,
        GemsSpent
    }

    public static class StorageService
    {
        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }
        
        public static bool HasKey(Enum key)
        {
            return PlayerPrefs.HasKey(key.ToString());
        }
        
        public static string GetString(PlayerPrefStrings key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key.ToString(), defaultValue);
        }

        public static void SetString(PlayerPrefStrings key, string value)
        {
            PlayerPrefs.SetString(key.ToString(), value);
            PlayerPrefs.Save();
        }

        public static int GetInt(PlayerPrefInts key, int defaultValue = -1)
        {
            return PlayerPrefs.GetInt(key.ToString(), defaultValue);
        }

        public static void SetInt(PlayerPrefInts key, int value)
        {
            PlayerPrefs.SetInt(key.ToString(), value);
            PlayerPrefs.Save();
        }
    }
}
