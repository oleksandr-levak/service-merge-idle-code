using System;
using DG.Tweening;
using MergeIdle.Scripts.Managers.Tutorial.Enum;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.Tutorial.Views
{
    public class FingerView: MonoBehaviour
    {
        public event Action<EFinger> OnClick;
        
        [SerializeField] private EFinger _type;
        [SerializeField] private Transform _transform;
        [SerializeField] private Button _button;
        
        [Header("Position")]
        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private Vector3 _endPosition;

        private Sequence _sequence;
        public EFinger Type => _type;
        public void Show()
        {
            gameObject.SetActive(true);
            _button?.onClick.AddListener(() => OnClick?.Invoke(_type));
            
            _sequence = DOTween.Sequence()
                .SetRecyclable(true).SetAutoKill(true)
                .Append(_transform.DOLocalMove(_endPosition, 0.7f))
                .Append(_transform.DOLocalMove(_startPosition, 0.7f))
                .SetLoops(-1)
                .SetEase(Ease.Linear);
        }
        
        public void Hide()
        {
            _button?.onClick.RemoveAllListeners();
            _sequence.Kill();
            gameObject.SetActive(false);
        }
    }
}