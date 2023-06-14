using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Order.Data;
using MergeIdle.Scripts.Managers.Order.Enum;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.Order.Views
{
    public class OrderView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Transform _orderField;
        [SerializeField] private HorizontalLayoutGroup _orderFieldHorizontalLayoutGroup;
        [SerializeField] private OrderButtonView _buttonView;
        
        public Transform OrderField => _orderField;
        public Button Button => _buttonView.Button;
        public OrderClient OrderClient => _orderClient;
        public List<OrderItemView> OrderItemViews
        {
            get => _orderItemViews;
            set => _orderItemViews = value;
        }

        private OrderClient _orderClient;
        private List<OrderItemView> _orderItemViews;

        public void SetClient(OrderClient orderClient)
        {
            _orderClient = orderClient;
        }
        
        public void SetManagerImage(Sprite managerImage)
        {
            _image.sprite = managerImage;
        }
        
        public void SetOrderFieldSpacing(int value)
        {
            _orderFieldHorizontalLayoutGroup.spacing = value;
        }
        
        public void SetButton(EButton type)
        {
            _buttonView.SetButton(type);
        }
    }
}