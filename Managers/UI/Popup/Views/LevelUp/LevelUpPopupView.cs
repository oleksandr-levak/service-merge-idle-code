using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.UI.Popup.Views.LevelUp
{
    public class LevelUpPopupView: BasePopupView
    {
        public event Action OnClickLookNow;
        
        [SerializeField] private TMP_Text _level;
        [SerializeField] private Button _lookNow;

        public string Level
        {
            get => _level.text;
            set => _level.text = value;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _lookNow.onClick.AddListener(() => OnClickLookNow?.Invoke());
            OnClickLookNow += Hide;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnClickLookNow = null;
            _lookNow.onClick.RemoveAllListeners();
        }
    }
}