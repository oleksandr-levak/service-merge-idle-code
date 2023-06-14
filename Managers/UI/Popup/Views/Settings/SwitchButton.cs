using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.UI.Popup.Views.Settings
{
    public class SwitchButton: MonoBehaviour
    {
        public event Action<bool> OnClick;

        [SerializeField] private Button _switchButton;
        [SerializeField] private Transform _barTransform;
        [SerializeField] private Image _barImage;

        [Header("Params")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Vector3 _offPosition;
        [SerializeField] private Vector3 _onPosition;
        
        [SerializeField] private Sprite _offSprite;
        [SerializeField] private Sprite _onSprite;

        private bool _state;

        public bool State => _state;

        private void Start()
        {
            _switchButton.onClick.AddListener(() => OnClick?.Invoke(!_state));
            OnClick += OnClickSwitch;
        }

        private void OnClickSwitch(bool state)
        {
            if (state) On();
            else Off();
        }
        
        public void On()
        {
            _barTransform.DOLocalMove(_onPosition, _moveSpeed)
                .OnComplete(() =>
                {
                    _barImage.sprite = _onSprite;
                    _state = true;
                });
        }

        public void Off()
        {
            _barTransform.DOLocalMove(_offPosition, _moveSpeed)
                .OnComplete(() =>
                {
                    _barImage.sprite = _offSprite;
                    _state = false;
                });
        }
        
        private void OnDestroy()
        {
            OnClick = null;
            _switchButton.onClick.RemoveAllListeners();
        }
    }
}