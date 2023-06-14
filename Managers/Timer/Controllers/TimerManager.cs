using System;
using MergeIdle.Scripts.Managers.Timer.Views;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.Timer.Controllers
{
    public class TimerManager: MonoBehaviour
    {
        public event Action<int> OnTimerEnd;

        [SerializeField] private TimerView _timer;
        
        private DateTime NextRewardTime(string key) => GetNextRewardTime(key);
        private TimeSpan TimeUntilReward(string key) => NextRewardTime(key).Subtract(DateTime.Now);
        
        private float _timeRemaining;
        private bool _timerIsRunning;
        private int _deltaTime = 0;

        private void Update()
        {
            if (_timerIsRunning)
            {
                if (_timeRemaining > 0)
                {
                    _timeRemaining -= Time.deltaTime;
                    if(_timer != null) _timer.DisplayTime(_timeRemaining);
                }
                else
                {
                    //Debug.Log("Time has run out!");
                    StopTime();
                    OnTimerEnd?.Invoke(1);
                }
            }
        }
        
        public void RestartTimer(string key, int seconds)
        {
            ClearTime(key);
            StartTimer(key, seconds);
        }

        public void StartTimer(string key, int seconds)
        {
            if (IsStoredRewardTime(key) && CanRewardNow(key))
            {
                var timeUntilReward = TimeUntilReward(key);
                var amountOfTimerEnds = (int)Math.Round(Math.Abs(timeUntilReward.TotalSeconds) / seconds);
                
                _deltaTime = (int)Math.Round(Math.Abs(timeUntilReward.TotalSeconds) % seconds);
                StopTime();
                OnTimerEnd?.Invoke(amountOfTimerEnds);
            }
            else
            {
                if (!IsStoredRewardTime(key))
                {
                    TimeSpan timeSpan = new TimeSpan(0, 0, seconds - _deltaTime);
                    ResetNextRewardTime(key, timeSpan);
                }
                
                var timeUntilReward = TimeUntilReward(key);
                _timeRemaining = (float) timeUntilReward.TotalSeconds;
                _deltaTime = 0;
                StartTime();  
            }
        }

        public void ClearTime(string key)
        {
            PlayerPrefs.SetString(key, string.Empty);
        }
        
        private void StartTime()
        {
            _timerIsRunning = true;
        }
        
        public void StopTime()
        {
            _timeRemaining = 0;
            _timerIsRunning = false;
        }
        
        private bool CanRewardNow(string key)
        {
            return TimeUntilReward(key) <= TimeSpan.Zero;
        }

        private void ResetNextRewardTime(string key, TimeSpan time)
        {
            DateTime next = DateTime.Now.Add(time);
            StoreNextRewardTime(key, next);
        }

        private void StoreNextRewardTime(string key, DateTime time)
        {
            PlayerPrefs.SetString(key, time.ToBinary().ToString());
            PlayerPrefs.Save();
        }

        private DateTime GetNextRewardTime(string key)
        {
            string storedTime = PlayerPrefs.GetString(key, string.Empty);

            if (!string.IsNullOrEmpty(storedTime))
                return DateTime.FromBinary(Convert.ToInt64(storedTime));
            return DateTime.Now;
        }

        private bool IsStoredRewardTime(string key)
        {
            string storedTime = PlayerPrefs.GetString(key, string.Empty);
            return !string.IsNullOrEmpty(storedTime);
        }
    }
}