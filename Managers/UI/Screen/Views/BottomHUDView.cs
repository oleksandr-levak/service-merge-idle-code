using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.UI.Screen.Views
{
    public class BottomHUDView: MonoBehaviour
    {
        [SerializeField] private Button _shopButton;
        [SerializeField] private BottomHUDButtonView _idleButton;
        [SerializeField] private Button _lipsButton;
        
        public BottomHUDButtonView IdleButton => _idleButton;
        public Button ShopButton => _shopButton;
        public Button LipsButton => _lipsButton;
    }
}