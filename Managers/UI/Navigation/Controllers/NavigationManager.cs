using MergeIdle.Scripts.Managers.UI.Navigation.Views;
using MergeIdle.Scripts.Managers.UI.Screen.Enums;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.UI.Navigation.Controllers
{
    public class NavigationManager : MonoBehaviour
    {
        [SerializeField] private NavigationView _core;
        [SerializeField] private NavigationView _coreGameObject;
        
        [SerializeField] private NavigationView _idle;

        private NavigationView _currentView;
        public NavigationView CurrentView => _currentView;
        
        public void ShowView(EView viewType)
        {
            NavigationView view = GetView(viewType);
            _currentView = view;
            view.Open();
        }
    
        public void HideView(EView viewType)
        {
            NavigationView view = GetView(viewType);
            view.Close();
        }

        private NavigationView GetView(EView view)
        {
            switch (view)
            {
                case EView.CORE: return _core;
                case EView.CORE_GAME_OBJECT: return _coreGameObject;
                default: return _idle;
            }
        }
    }
}