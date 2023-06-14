using TMPro;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.UI.Popup.Views.Debug
{
    public class DebugPopupView: BasePopupView
    {
        [SerializeField] private TMP_Text _text;

        public void AddText(string str)
        {
            _text.text += $"{str}\n";
        }
    }
}