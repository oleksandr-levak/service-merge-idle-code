using MergeIdle.Scripts.Managers.Order.Views;
using MergeIdle.Scripts.Managers.Purchase.Views;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.UI.Screen.Views
{
    public class CorePanelView: MonoBehaviour
    {
        [SerializeField] private OrdersView _orders;
        [SerializeField] private PurchasesView _purchases;

        public Transform PurchasesContent => _purchases.Content;
        public Transform OrdersContent => _orders.Content;
    }
}