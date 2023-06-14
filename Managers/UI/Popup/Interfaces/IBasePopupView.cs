using System;
using MergeIdle.Scripts.Managers.UI.Popup.Enums;

namespace MergeIdle.Scripts.Managers.UI.Popup.Interfaces
{
    public interface IBasePopupView
    {
        event Action<EPopup> OnShow;
        event Action<EPopup> OnHide;
        EPopup PopupType { get; }
        void Show();
        void Hide();
    }
}