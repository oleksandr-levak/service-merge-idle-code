using System;
using MergeIdle.Scripts.Managers.UI.Screen.Views;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.UI.Screen.Controller
{
    public class BottomHUDManager: MonoBehaviour
    {
        public event Action OnClickIdleButton;
        public event Action OnClickShopButton;
        public event Action OnClickLipsButton;
        
        [SerializeField] private BottomHUDView _bottomHUDView;
        
        public BottomHUDButtonView IdleButton => _bottomHUDView.IdleButton;

        private void Awake()
        {
            _bottomHUDView.IdleButton.Button.onClick.AddListener(() => OnClickIdleButton?.Invoke());
            _bottomHUDView.ShopButton.onClick.AddListener(() => OnClickShopButton?.Invoke());
            _bottomHUDView.LipsButton.onClick.AddListener(() => OnClickLipsButton?.Invoke());
        }

        private void OnDestroy()
        {
            OnClickIdleButton = null;
            OnClickShopButton = null;
            _bottomHUDView.IdleButton.Button.onClick.RemoveAllListeners();
            _bottomHUDView.ShopButton.onClick.RemoveAllListeners();
            _bottomHUDView.LipsButton.onClick.RemoveAllListeners();
        }
    }
}