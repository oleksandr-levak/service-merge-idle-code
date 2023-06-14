using MergeIdle.Scripts.Configs.Order.Data;
using MergeIdle.Scripts.Managers.Order.Views;

namespace MergeIdle.Scripts.Storage.Mapper
{
    public static class OrderClientMapper
    {
        public static OrderClient MapToItem(this OrderView orderView)
        {
            OrderClient orderClient = orderView.OrderClient;
            return orderClient;
        }
    }
}