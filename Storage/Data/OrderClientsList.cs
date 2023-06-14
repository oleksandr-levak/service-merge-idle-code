using System;
using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Order.Data;

namespace MergeIdle.Scripts.Storage.Data
{
    [Serializable]
    public class OrderClientsList
    {
        public List<OrderClient> orderClients;
        public OrderClientsList(List<OrderClient> orderClients)
        {
            this.orderClients = orderClients;
        }
    }
}