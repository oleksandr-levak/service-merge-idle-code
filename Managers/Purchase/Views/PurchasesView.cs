using System;
using MergeIdle.Scripts.Configs.Purchase.Enum;
using MergeIdle.Scripts.Managers.Timer.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.Purchase.Views
{
    public class  PurchasesView : MonoBehaviour
    {
        public event Action OnClickPurchaseTimer;
        
        [SerializeField] private TimerManager _timerManager;
        [SerializeField] private int _secondsToChange;
        
        [SerializeField] private Transform _content;
        [SerializeField] private Button _changePurchasesButton;
        
        private EPurchaseType _purchaseType;
        public GameObject Timer => _changePurchasesButton.gameObject;
        public TimerManager TimerManager => _timerManager;
        public Transform Content => _content;
        public EPurchaseType PurchaseType { get; set; }
        public int SecondsToChange => _secondsToChange;

        private void OnEnable()
        {
            _changePurchasesButton.onClick.AddListener(() => OnClickPurchaseTimer?.Invoke());
        }
        
        private void OnDisable()
        {
            _changePurchasesButton.onClick.RemoveAllListeners();
        }
    }
}