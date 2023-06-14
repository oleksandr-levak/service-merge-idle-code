using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.UI.Screen.Views
{
    public class BottomHUDButtonView: MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _bag;
        
        public Button Button => _button;

        public void SetBag(bool state) => _bag.SetActive(state);
    }
}