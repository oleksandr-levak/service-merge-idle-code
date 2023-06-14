using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.UI.Popup.Views.Settings
{
    public class SettingsPopupView: BasePopupView
    {
        public event Action OnClickClose;
        public event Action<bool> OnClickMusic;
        public event Action<bool> OnClickSound;

        [SerializeField] private Button _close;
        [SerializeField] private TMP_Text _version;
        [SerializeField] private SwitchButton _musicSwitchButton;
        [SerializeField] private SwitchButton _soundSwitchButton;

        public SwitchButton MusicSwitch => _musicSwitchButton;
        public SwitchButton SoundSwitch => _soundSwitchButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            _close.onClick.AddListener(() => OnClickClose?.Invoke());
            OnClickClose += Hide;
             
            _musicSwitchButton.OnClick += OnClickMusic;
            _soundSwitchButton.OnClick += OnClickSound;

            _version.text = $"v.{Application.version}";
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _close.onClick.RemoveAllListeners();
            OnClickClose = null;
            OnClickMusic = null;
            OnClickSound = null;
            
            _musicSwitchButton.OnClick -= OnClickMusic;
            _soundSwitchButton.OnClick -= OnClickSound;
        }
    }
}