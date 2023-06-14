using DG.Tweening;
using MergeIdle.Scripts.Managers.UI.Screen.Enums;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.UI.Navigation.Views
{
    public class NavigationView: MonoBehaviour
    {
        private const float MOVE_SPEED = 0.3f;
        
        [SerializeField] private EView _viewType;
        [SerializeField] private Vector3 _closePosition;
        [SerializeField] private Vector3 _openPosition;

        public EView ViewType => _viewType;
            
        public void Open()
        {
            transform.DOLocalMove(_openPosition, MOVE_SPEED);
        }

        public void Close()
        {
            transform.DOLocalMove(_closePosition, MOVE_SPEED);
        }
    }
}