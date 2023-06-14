using System;
using System.Collections.Generic;
using System.Linq;
using MergeIdle.Scripts.Managers.UI.Popup.Enums;
using MergeIdle.Scripts.Managers.UI.Popup.Interfaces;
using MergeIdle.Scripts.Managers.UI.Popup.Views;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.UI.Popup.Controllers
{
    public class PopupManager : MonoBehaviour
    {
        public event Action<EPopup> OnShowPopup;
        public event Action<EPopup> OnHidePopup;
        
        [SerializeField] private PopupsView _popupsView;

        private void OnEnable()
        {
            _popupsView.SetupPopupViews();
            SubscribesPopups(_popupsView.PopupViews);
        }

        public IBasePopupView ShowPopup(EPopup popup)
        {
            IBasePopupView view = GetPopupView(popup);
            view.Show();

            return view;
        }

        public void HidePopup(EPopup popup)
        {
            IBasePopupView view = GetPopupView(popup);
            view.Hide();
        }

        public IBasePopupView GetPopupView(EPopup popup)
        {
            return _popupsView.PopupViews.FirstOrDefault(x => x.PopupType == popup);
        }
        
        private void SubscribesPopups(List<IBasePopupView> popupViews)
        {
            for (int i = 0; i < popupViews.Count; i++)
            {
                popupViews[i].OnShow += (e) =>
                {
                    OnShowPopup?.Invoke(e);
                };
                popupViews[i].OnHide += (e) =>
                {
                    OnHidePopup?.Invoke(e);
                };;
            }
        }
    }
}