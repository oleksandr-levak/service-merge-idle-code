using MergeIdle.Scripts.Managers.UI.Screen.Enums;
using UnityEngine;

namespace MergeIdle.Scripts.Helpers
{
    public class OrthographicSize: MonoBehaviour
    {
        private const float TOLERANCE = 0.1f;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Camera _camera;

        private float _targetX;
        private float _targetY;
        
        private float _screenRatio;
        private float _targetRatio;
        
        private float _differenceSize;
        private float _cameraOrthographicSize;

        private void Awake()
        {
            _targetX = _spriteRenderer.bounds.size.x;
            _targetY = _spriteRenderer.bounds.size.y;
            _screenRatio = (float)Screen.width / (float) Screen.height;
            _targetRatio = _targetX / _targetY;
            _differenceSize = _targetRatio / _screenRatio;
            _cameraOrthographicSize = _camera.orthographicSize;
        }

        public void SetOrthographicSize()
        {
            if (_screenRatio >= _targetRatio)
            {
                _camera.orthographicSize = _targetY / 2 * 1.05f;
            }
            else
            {
                _camera.orthographicSize = _targetY / 2 * _differenceSize * 1.05f;
            }
        }
        
        public void SetPosition(RectTransform rectTransform)
        {
            float diff = (_cameraOrthographicSize - _targetY / 2 * _differenceSize) * GetDelta();
            Vector3 distance = new Vector3(0, diff, 0); 
            rectTransform.Translate(distance);
        }

        public float GetDelta()
        {
            float screenRatio = (float)Screen.width / (float) Screen.height;
            if (screenRatio <= 0.462f) return 0.55f;
            if (screenRatio <= 0.5f) return 1.2f;
            return 0.83f;
        }
    }
}