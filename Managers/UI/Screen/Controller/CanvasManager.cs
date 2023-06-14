using UnityEngine;

namespace MergeIdle.Scripts.Managers.UI.Screen.Controller
{
    public class CanvasManager: MonoBehaviour
    {
        [SerializeField] private RectTransform _safeArea;
        [SerializeField] private RectTransform _canvasRectTransform;
        
        public float MainCanvasHeight => _canvasRectTransform.rect.height;

        public void SetSafeArea()
        {
            SetSafeArea(_safeArea);
        }
        
        private void SetSafeArea(RectTransform safeAreaRectTransform) 
        {
            var safeArea = UnityEngine.Screen.safeArea;
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = anchorMin + safeArea.size;

            anchorMin.x /= UnityEngine.Screen.width;
            anchorMin.y /= UnityEngine.Screen.height;
            anchorMax.x /= UnityEngine.Screen.width;
            anchorMax.y /= UnityEngine.Screen.height;

            //safeAreaRectTransform.anchorMin = anchorMin;
            safeAreaRectTransform.anchorMax = anchorMax;
        }
    }
}