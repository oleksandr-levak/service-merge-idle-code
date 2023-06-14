using System;
using MergeIdle.Scripts.Configs.Purchase.Enum;
using MergeIdle.Scripts.Managers.Timer.Controllers;
using MergeIdle.Scripts.Managers.UI.Screen.Views;
using MergeIdle.Scripts.Storage.Const;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.UI.Screen.Controller
{
    public class TopHUDManager: MonoBehaviour
    {
        public event Action OnClickSettings;

        [SerializeField] private TopHUDView _topHUDView;
        [SerializeField] private TimerManager _timerManager;
        [SerializeField] private LevelManager _levelManager;
        
        public TimerManager TimerManager => _timerManager;
        public LevelManager LevelManager => _levelManager;

        private void Awake()
        {
            _topHUDView.SettingsButton.onClick.AddListener(() => OnClickSettings?.Invoke());
        }
        
        public void SetCurrencyValue(ECurrency currency, float value)
        {
            switch (currency)
            {
                case ECurrency.MONEY:
                    SetMoney(value);
                    break;
                default:
                    SetDiamonds(value);
                    break;
            }
        }

        public void SetMoney(float value)
        {
            _topHUDView.Money = (int) Math.Round(value, 2);
        }
        
        public void SetEnergy(int value)
        {
            _topHUDView.Energy = value;
        }
        
        public void SetDiamonds(float value)
        {
            _topHUDView.Diamonds = (int) Math.Round(value, 2);
        }

        public void StartEnergyTimer(int time)
        {
            _timerManager.StartTimer(StorageType.ENERGY_TIMER, time);
        }
        
        public void RestartEnergyTimer(int time)
        {
            _timerManager.RestartTimer(StorageType.ENERGY_TIMER, time);
        }

        private void OnDestroy()
        {
            OnClickSettings = null;
            _topHUDView.SettingsButton.onClick.RemoveListener(() => OnClickSettings?.Invoke());
        }
    }
}