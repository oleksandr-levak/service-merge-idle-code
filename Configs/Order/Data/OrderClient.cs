using System;
using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Order.Enum;
using MergeIdle.Scripts.Managers.Salon.Data;

namespace MergeIdle.Scripts.Configs.Order.Data
{
    [Serializable]
    public class OrderClient
    {
        public SalonData salonData;
        public EOrderId orderId;
        public List<OrderItem> orderItems;
    }
    
}