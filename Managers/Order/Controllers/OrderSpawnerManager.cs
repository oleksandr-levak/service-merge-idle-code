using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Order.Data;
using MergeIdle.Scripts.Databases.MergeItems.Enum;
using MergeIdle.Scripts.Managers.Order.Views;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.Order.Controllers
{
    public class OrderSpawnerManager : MonoBehaviour
    {
        private const int SMALL_ORDER_SPACING = 30;
        
        [SerializeField] private OrderItemView _orderItemView;

        public OrderView SpawnItem(Transform parent, OrderClient orderClient, OrderView orderView)
        {
            var spawnedOrderView = Instantiate(orderView, parent, false);
            var managerImage = Utils.GetSalonManagerImage(orderClient.salonData);

            bool isMoreThanTwoItems = orderClient.orderItems.Count > 2;

            spawnedOrderView.SetClient(orderClient);
            spawnedOrderView.SetManagerImage(managerImage);
            spawnedOrderView.SetOrderFieldSpacing(isMoreThanTwoItems ? 0 : SMALL_ORDER_SPACING);

            spawnedOrderView.OrderItemViews = SetOrders(orderClient.orderItems, spawnedOrderView.OrderField);
            return spawnedOrderView;
        }

        private List<OrderItemView> SetOrders(List<OrderItem> orderItems, Transform parent)
        {
            List<OrderItemView> orderItemViews = new List<OrderItemView>();
            foreach (var item in orderItems)
            {
                for (int i = 0; i < item.amount; i++)
                {
                    var orderItemViewImage =
                        Utils.GetItemVisualById(EMergeCategory.NO_GENERATOR, item.mergeType, item.level - 1);
                    var orderItemView = Instantiate(_orderItemView, parent, false);
                    orderItemView.SetImage(orderItemViewImage);
                    orderItemViews.Add(orderItemView);
                }
            }

            return orderItemViews;
        }
    }
}