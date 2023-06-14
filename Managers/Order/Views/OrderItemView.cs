using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.Order.Views
{
    public class OrderItemView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _greenTick;
        public void SetGreenTickState(bool state) => _greenTick.gameObject.SetActive(state);
        public void SetImage(Sprite sprite)
        {
            _image.sprite = sprite;
            _image.SetNativeSize();
            _image.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
    }
}