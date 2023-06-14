using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.UI.Popup.Views.Tools
{
    public class ToolView: MonoBehaviour
    {
        [SerializeField] private Sprite _closeItem;
        [SerializeField] private Image _image;
        [SerializeField] private Image _imagePlaceholder;
        [SerializeField] private TMP_Text _number;

        public void Setup(Sprite sprite, int number, bool isOpen)
        {
            if (isOpen)
            {
                _image.sprite = sprite;
                _image.SetNativeSize();
            }
            else
            {
                _imagePlaceholder.sprite = _closeItem;
                _image.gameObject.SetActive(false);
            }

            if (_number != null)
            {
                _number.text = number.ToString();   
            }
        }
    }
}