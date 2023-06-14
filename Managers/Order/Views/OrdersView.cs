using UnityEngine;

namespace MergeIdle.Scripts.Managers.Order.Views
{
    public class OrdersView : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        public Transform Content => _content;
    }
}