using System.Collections.Generic;
using System.Linq;
using MergeIdle.Scripts.Managers.Order.Data;
using MergeIdle.Scripts.Managers.Order.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.Order.Views
{
    public class OrderButtonView : MonoBehaviour
    {
        [SerializeField] private List<OrderButtonData> _orderButtons;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _buttonText;
        [SerializeField] private Button _button;
        
        public Button Button => _button;

        public void SetButton(EButton type)
        {
            OrderButtonData button = GetButton(type);
            SetupButton(button);
        }
        
        private void SetupButton(OrderButtonData buttonData)
        {
            _image.sprite = buttonData.sprite;
            _buttonText.text = buttonData.text;
        }

        private OrderButtonData GetButton(EButton button)
        {
            return _orderButtons.FirstOrDefault(x => x.type == button);
        }
    }
}