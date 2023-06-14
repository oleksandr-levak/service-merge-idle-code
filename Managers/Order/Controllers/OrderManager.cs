using System;
using System.Collections.Generic;
using System.Linq;
using MergeIdle.Scripts.Configs.Order.Data;
using MergeIdle.Scripts.Configs.Order.Enum;
using MergeIdle.Scripts.Managers.Order.Views;
using MergeIdle.Scripts.Managers.Salon.Controllers;
using MergeIdle.Scripts.Managers.Salon.Data;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.Order.Controllers
{
    public class OrderManager : MonoBehaviour
    {
        private const int ORDERS_AMOUNT_TO_SHOW = 3;
        public event Action<OrderClient> OnClickItem;
        
        [SerializeField] private OrderView _smallOrderView;
        [Header("Managers")]
        [SerializeField] private SalonManager _salonManager;
        [SerializeField] private OrderGeneratorManager _orderGeneratorManager;
        [SerializeField] private OrderSpawnerManager _orderSpawnerManager;
        
        private Transform _parent;
        private Dictionary<EOrderId, OrderView> _orderViews;
        public Dictionary<EOrderId, OrderView> OrderViews => _orderViews;
        
        private void Start()
        {
            _orderViews = new Dictionary<EOrderId, OrderView>();
        }

        public void InitOrders(Transform parent)
        {
            var openSalons = _salonManager.GetOpenSalons();
            var openSalonsData = openSalons.Select(SalonData.MapFrom).ToList();
            var items = _orderGeneratorManager.GetOrderClients(openSalonsData);
            SetupOrders(parent, items);
        }
        
        public void SetupOrders(Transform parent, List<OrderClient> orderClients)
        {
            _parent = parent;
            var length = orderClients.Count > ORDERS_AMOUNT_TO_SHOW ? ORDERS_AMOUNT_TO_SHOW : orderClients.Count; 
            for (int i = 0; i < length; i++)
            {
                var item = orderClients[i];
                SetupOrder(parent, item, _smallOrderView);
            }
        }

        public int GetOrderItemsExp(List<OrderItem> orderItems)
        {
            var orderItemsExps = orderItems.ConvertAll(x => Math.Pow(2, x.level - 1) * 10);
            var orderItemsExpsCount = orderItemsExps.Count;
            return (int) orderItemsExps.Take(orderItemsExpsCount).Sum();
        }

        public void AddItem()
        {
            var nextSalon = _salonManager.GetNextSalon();
            var item = _orderGeneratorManager.GetOrderClient(nextSalon);
            if (item != null)
            {
                SetupOrder(_parent, item, _smallOrderView);
            }
        }

        public void SpawnNextOrder(SalonData salon)
        {
            var nextOrder = _orderGeneratorManager.GetNextOrderClient(salon);
            SetupOrder(_parent, nextOrder, _smallOrderView);
        }
        
        private void SetupOrder(Transform parent, OrderClient orderClient, OrderView orderViewPrefab)
        {
            var spawnedOrderView = _orderSpawnerManager.SpawnItem(parent, orderClient, orderViewPrefab);
            spawnedOrderView.Button.onClick.AddListener(() => OnClickItem?.Invoke(spawnedOrderView.OrderClient));
            UpdateOrderViews(orderClient.orderId, spawnedOrderView);
        }
        
        private void UpdateOrderViews(EOrderId orderId, OrderView orderView)
        {
            if (_orderViews.ContainsKey(orderId))
            {
                _orderViews.Remove(orderId);
            }
            _orderViews.Add(orderId, orderView);
        }

        private OrderView GetViewById(EOrderId id)
        {
            return _orderViews.ContainsKey(id) ? _orderViews[id] : null;
        }

        public void DeleteById(EOrderId key)
        {
            var item = GetViewById(key);
            if (item == null) return;
            item.Button.onClick.RemoveAllListeners();
            _orderViews.Remove(key);
            Destroy(item.gameObject);
        }

        private void OnDestroy()
        {
            OnClickItem = null;

            if (_parent != null)
            {
                foreach (Transform child in _parent)
                {
                    if (child.GetComponent<OrderView>() != null)
                    {
                        OrderView orderView = child.GetComponent<OrderView>();
                        orderView.Button.onClick.RemoveAllListeners();
                    }
                }
            }
        }
    }
}