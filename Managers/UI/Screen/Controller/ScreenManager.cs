using MergeIdle.Scripts.Managers.UI.Screen.Data;
using MergeIdle.Scripts.Managers.UI.Screen.Views;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.UI.Screen.Controller
{
    public class ScreenManager : MonoBehaviour
    {
        [Header("HUD")] 
        [SerializeField] private TopHUDManager _topHUDManager;
        [SerializeField] private BottomHUDManager _bottomHUDManager;
        
        [Header("Screens")] 
        [SerializeField] private GameView _gameView;
        [SerializeField] private LoadingView _loadingView;
        
        [Header("Canvas")] 
        [SerializeField] private CanvasManager _canvasManager;

        public CanvasManager CanvasManager => _canvasManager;
        public GameView GameView => _gameView;
        public TopHUDManager TopHUDManager => _topHUDManager;
        public BottomHUDManager BottomHUDManager => _bottomHUDManager;

        public void ShowLoading()
        {
            _loadingView.Show();
        }
        
        public void HideLoading()
        {
            var height = _canvasManager.MainCanvasHeight;
            _loadingView.Hide(height);
        }

        public void InitUIData(UIData data)
        {
            _topHUDManager.SetEnergy(data.energy);
            _topHUDManager.SetMoney(data.money);
            _topHUDManager.SetDiamonds(data.diamonds);
        }

    }
}