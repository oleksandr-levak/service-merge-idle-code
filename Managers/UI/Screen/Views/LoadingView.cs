using DG.Tweening;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.UI.Screen.Views
{
    public class LoadingView: MonoBehaviour
    {
        private const float MOVE_SPEED = 0.3f;
        [SerializeField] private Transform _transform;
        public void Show() => _transform.DOLocalMoveY(0, MOVE_SPEED);

        public void Hide(float canvasHeight) => _transform.DOLocalMoveY(canvasHeight, MOVE_SPEED)
            .OnComplete(() => transform.gameObject.SetActive(false));
    }
}