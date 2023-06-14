using System;
using System.Collections.Generic;
using MergeIdle.Scripts.Managers.UI.Popup.Views.Development.Data;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.UI.Popup.Views.Development
{
    public class DevelopmentPopupView: BasePopupView
    {
        public event Action<List<InputRowData>> OnClickSetup;
        public event Action OnClickClear;
        public event Action OnClickExit;
        
        [SerializeField] private List<InputRowView> _inputRowViews;

        [SerializeField] private Button _setup;
        [SerializeField] private Button _clear;
        [SerializeField] private Button _exit;
        
        private List<InputRowData> InputRowData => _inputRowViews.ConvertAll(x => x.Data());

        protected override void OnEnable()
        {
            base.OnEnable();
            _setup.onClick.AddListener(() => OnClickSetup?.Invoke(InputRowData));
            _clear.onClick.AddListener(() => OnClickClear?.Invoke());
            _exit.onClick.AddListener(() => OnClickExit?.Invoke());
            OnClickClear += Hide;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnClickSetup = null;
            OnClickClear = null;
            OnClickExit = null;
            _setup.onClick.RemoveAllListeners();
            _clear.onClick.RemoveAllListeners();
            _exit.onClick.RemoveAllListeners();
        }
    }
}