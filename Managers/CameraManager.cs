using UnityEngine;

namespace MergeIdle.Scripts.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;

        public Camera MainCamera => _mainCamera;
    }
}