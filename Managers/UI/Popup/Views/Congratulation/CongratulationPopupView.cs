using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.UI.Popup.Views.Congratulation
{
    public class CongratulationPopupView: BasePopupView
    {
        public event Action OnClickOk;

        [SerializeField] private TMP_Text _moneyValue;
        [SerializeField] private Button _ok;
        
        public string MoneyValue
        {
            get => _moneyValue.text;
            set => _moneyValue.text = value;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            _ok.onClick.AddListener(() => OnClickOk?.Invoke());
            OnClickOk += Hide;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnClickOk = null;
            _ok.onClick.RemoveAllListeners();
        }
    }
}