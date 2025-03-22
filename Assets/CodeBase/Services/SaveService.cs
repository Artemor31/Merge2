using System;
using Services.DataServices;
using Services.Infrastructure;
using Services.ProgressData;
using UnityEngine;
using YG;

namespace Services
{
    public class SaveService : IService
    {
        public void Save<TData>(string path, TData data) where TData : SaveData
        {
#if UNITY_EDITOR
            SaveEditor(path, data);
#else
            SaveYandex(path, data);
#endif
        }

        public TData Restore<TData>(string path) where TData : SaveData, new()
        {
#if UNITY_EDITOR
            return RestoreEditor<TData>(path);
#else
            return RestoreYandex<TData>(path);
#endif
        }

        private void SaveEditor<TData>(string path, TData data) where TData : SaveData
        {
            data.Serialize();
            PlayerPrefs.SetString(path, JsonUtility.ToJson(data));
        }

        private TData RestoreEditor<TData>(string path) where TData : SaveData, new()
        {
            if (!PlayerPrefs.HasKey(path))
            {
                return new TData();
            }
            
            string json = PlayerPrefs.GetString(path);
            TData data = JsonUtility.FromJson<TData>(json);
            data.Deserialize();
            return data;
        }

        private static TData RestoreYandex<TData>(string path) where TData : SaveData, new()
        {
            Type type = typeof(TData);
            string json = null;
            if (type == typeof(TutorData)) json = YG2.saves.TutorData;
            else if (type == typeof(GridProgress)) json = YG2.saves.GridProgress;
            else if (type == typeof(PersistantProgress)) json = YG2.saves.PersistantProgress;
            else if (type == typeof(UpgradeProgress)) json = YG2.saves.UpgradeProgress;
            else if (type == typeof(WaveRewardData)) json = YG2.saves.WaveRewardData;
            else if (type == typeof(GameplayProgress) && path == GameplayDataService.StoryData) json = YG2.saves.StoryGameplayProgress;

            if (string.IsNullOrEmpty(json))
            {
                return new TData();
            }

            TData data = JsonUtility.FromJson<TData>(json);
            data.Deserialize();
            return data;
        }

        private void SaveYandex<TData>(string path, TData data) where TData : SaveData
        {
            data.Serialize();
            Type type = data.GetType();

            if (type == typeof(TutorData)) YG2.saves.TutorData = JsonUtility.ToJson(data);
            else if (type == typeof(GridProgress)) YG2.saves.GridProgress = JsonUtility.ToJson(data);
            else if (type == typeof(PersistantProgress)) YG2.saves.PersistantProgress = JsonUtility.ToJson(data);
            else if (type == typeof(UpgradeProgress)) YG2.saves.UpgradeProgress = JsonUtility.ToJson(data);
            else if (type == typeof(WaveRewardData)) YG2.saves.WaveRewardData = JsonUtility.ToJson(data);
            else if (type == typeof(GameplayProgress) && path == GameplayDataService.StoryData) YG2.saves.StoryGameplayProgress = JsonUtility.ToJson(data);
            
            YG2.SaveProgress();
        }
    }
}