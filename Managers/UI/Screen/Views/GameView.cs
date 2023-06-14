using System;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.UI.Screen.Views
{
    public class GameView: MonoBehaviour
    {
        public event Action OnClickUpdateSalonLabel;

        [Header("Core Panels")]
        [SerializeField] private Button _updateSalonLabel;
        [SerializeField] private CorePanelView _smallOrderPanel;
        
        [Header("Idle")]
        [SerializeField] private Transform _idleMapContent;

        public Transform Purchases => _smallOrderPanel.PurchasesContent;
        public Transform Orders => _smallOrderPanel.OrdersContent;
        public Transform IdleMap => _idleMapContent;
        public Button UpdateSalonLabel => _updateSalonLabel;
        
        private void Awake()
        {
            _updateSalonLabel.onClick.AddListener(() => OnClickUpdateSalonLabel?.Invoke());
        }

        private void OnDestroy()
        {
            OnClickUpdateSalonLabel = null;
            _updateSalonLabel.onClick.RemoveAllListeners();
        }
    }
}