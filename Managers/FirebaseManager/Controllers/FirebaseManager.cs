using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.FirebaseManager.Controllers
{
    public class FirebaseManager : MonoBehaviour
    {
        public event Action OnFetchedRemoteConfigs;
        public void Init() => InitializeFirebase();
        public long GetRemoteConfigLongValue(string key) => FirebaseRemoteConfig.DefaultInstance.GetValue(key).LongValue;
        public string GetRemoteConfigStringValue(string key) => FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;
        public bool GetRemoteConfigBooleanValue(string key) => FirebaseRemoteConfig.DefaultInstance.GetValue(key).BooleanValue;

        public void SendEvent(string eventName)
        {
            FirebaseAnalytics.LogEvent(eventName);
        }

        private void InitializeFirebase()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Result == DependencyStatus.Available)
                {
                    FetchRemoteConfig();
                    Debug.Log("Firebase correctly Initialized");
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
                }
            });
        }
        
        private void FetchRemoteConfig()
        {
            FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero)
                .ContinueWithOnMainThread(FetchComplete);
        }
        
        private void FetchComplete(Task fetchTask) {
            if (fetchTask.IsCanceled) {
                Debug.LogError("Fetch canceled.");
            } else if (fetchTask.IsFaulted) {
                Debug.LogError("Fetch encountered an error.");
            } else if (fetchTask.IsCompleted) {
                Debug.Log("Fetch completed successfully!");
                
            }
            
            var info = FirebaseRemoteConfig.DefaultInstance.Info;
            switch (info.LastFetchStatus) {
                case LastFetchStatus.Success:
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                        .ContinueWithOnMainThread(task => {
                            Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                                info.FetchTime));
                            
                            OnFetchedRemoteConfigs?.Invoke();
                        });

                    break;
                case LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason) {
                        case FetchFailureReason.Error:
                            Debug.LogError("Fetch failed for unknown reason");
                            break;
                        case FetchFailureReason.Throttled:
                            Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                            break;
                    }
                    break;
                case LastFetchStatus.Pending:
                    Debug.Log("Latest Fetch call still pending.");
                    break;
            }
        }
    }
}