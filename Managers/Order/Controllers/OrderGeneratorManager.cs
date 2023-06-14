using System;
using System.Collections.Generic;
using System.Linq;
using MergeIdle.Scripts.Configs.Merge.Enum;
using MergeIdle.Scripts.Configs.Order.Data;
using MergeIdle.Scripts.Configs.Order.Enum;
using MergeIdle.Scripts.Configs.Salon.Enum;
using MergeIdle.Scripts.Managers.Order.Data;
using MergeIdle.Scripts.Managers.Salon.Data;
using MergeIdle.Scripts.Storage.Controller;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MergeIdle.Scripts.Managers.Order.Controllers
{
    public class OrderGeneratorManager : MonoBehaviour
    {
        private const string Customer_1 = "A2,B2;A2,B3;B3,B2;A2,B2,A2;A2,A4,B4;B5,B2,A5;B5,A3,A4;B4,B3,A5;B6,A5,A5;B3,B3,A3,A5;";
        private const string Customer_2 = "C2,D3;D2,C4;D3,D3;C2,D2,C3;D2,D4,C4;D3,C2,C5;C4,C4,D5;C2,C5,D5,D3;C4,D3,D6;C5,C6,C6;";
        private const string Customer_3 = "F3,E4;E4,F3,F2;F3,F3,E5,F2;F2,F7;E4,E3,F5;F2,F4,E4,F5;F4,F6;E2,F6,E4;F5,E6;F2,F6,E6;";
        
        private Dictionary<string, EMergeType> _generatorsItems = new()
        {
            { "A", EMergeType.A_VIDEOCARD },
            { "B", EMergeType.B_PROCESSOR },
            
            { "C", EMergeType.C_RAM },
            { "D", EMergeType.D_CASE },
            
            { "F", EMergeType.F_BATTERY },
            { "E", EMergeType.E_CASE },
        };

        private Dictionary<ESalonId, string> _customers = new()
        {
            {ESalonId.S_1, Customer_1},
            {ESalonId.S_2, Customer_2},
            {ESalonId.S_3, Customer_3},
        };

        [Header("Storage")]
        [SerializeField] private GameStorageController _gameStorage;

        public OrderClient GetNextOrderClient(SalonData salon)
        {
            var salonOrdersString = _customers[salon.id];
            IncOrderListPointer(salonOrdersString);
            
            OrderClient orderClient = GetOrderClient(salon);
            return orderClient;
        }

        public List<OrderClient> GetOrderClients(List<SalonData> salonData)
        {
            List<OrderClient> orderClients = new List<OrderClient>();
            foreach (var salon in salonData)
            {
                OrderClient orderClient = GetOrderClient(salon);
                orderClients.Add(orderClient);
            }

            return orderClients;
        }
        
        private bool IsOrderListEnd(ESalonId salonId)
        {
            var salonOrdersString = _customers[salonId];
            var orderListIndex = GetOrderListPointer(salonOrdersString);
            var isOrderListEnd = salonOrdersString.Length <= orderListIndex;
            return isOrderListEnd;
        }

        private int GetRandomClientOrderIndex(ESalonId salonId)
        {
            var salonOrdersString = _customers[salonId];
            var ordersList = GetOrdersList(salonOrdersString);
            int index = Random.Range(1, ordersList.Count);
            return index;
        }

        public OrderClient GetOrderClient(SalonData salonData)
        {
            var salonId = salonData.id;
            OrderClient orderClient = new OrderClient();
            var salonOrdersString = _customers[salonId];
            var ordersList = GetOrdersList(salonOrdersString);
            
            int orderIndex;
            if (IsOrderListEnd(salonId))
            {

                orderIndex = GetRandomClientOrderIndex(salonId);
            }
            else
            {
                orderIndex = GetOrderListPointer(salonOrdersString);
            }

            orderClient.salonData = salonData;
            orderClient.orderId = (EOrderId)salonId;
            orderClient.orderItems = ordersList[orderIndex].orderItems;
            return orderClient;
        }

        private List<OrderData> GetOrdersList(string data)
        {
            List<OrderData> result = new List<OrderData>();
            List<string> list = data.Split(';').ToList().FindAll(x => x.Length > 0);
            foreach(var l in list)
            {
                OrderData orderData = new OrderData();
                orderData.orderItems = new List<OrderItem>();
                var values = l.Split(',');
                foreach(var value in values)
                {
                    var type = value[0].ToString();
                    var level = Int32.Parse(value[1].ToString());
               
                    OrderItem orderItem = new OrderItem();

                    orderItem.level = level;
                    orderItem.amount = 1;
                    orderItem.mergeType = GetTypeByLetter(type);

                    orderData.orderItems.Add(orderItem);
                }
                result.Add(orderData);
            }

            return result;
        }
        
        private void IncOrderListPointer(string key)
        {
            int prevVal = GetOrderListPointer(key);
            int newValue = prevVal + 1;
            _gameStorage.SetIntValue(key, newValue);
        }
        
        private EMergeType GetTypeByLetter(string letter)
        {
            return _generatorsItems[letter];
        }

        private int GetOrderListPointer(string key)
        {
            return _gameStorage.GetIntValue(key);
        }
    }
}