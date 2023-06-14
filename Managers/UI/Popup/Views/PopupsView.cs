using System.Collections.Generic;
using MergeIdle.Scripts.Managers.UI.Popup.Interfaces;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.UI.Popup.Views
{
    public class PopupsView: MonoBehaviour
    {
        [SerializeField] private BasePopupView _tools;
        [SerializeField] private BasePopupView _settings;
        [SerializeField] private BasePopupView _moreCurrency;
        [SerializeField] private BasePopupView _mistake;
        [SerializeField] private BasePopupView _levelUp;
        [SerializeField] private BasePopupView _congratulation;
        [SerializeField] private BasePopupView _storePopupView;
        [SerializeField] private BasePopupView _development;
        [SerializeField] private BasePopupView _debug;

        private List<IBasePopupView> _popupViews;
        
        public List<IBasePopupView> PopupViews => _popupViews;

        public void SetupPopupViews()
        {
            _popupViews = new List<IBasePopupView>()
            {
                _tools, _congratulation, _settings,
                _moreCurrency, _mistake, _levelUp,
                _storePopupView, _development,
                _debug
            };
        }
    }
}