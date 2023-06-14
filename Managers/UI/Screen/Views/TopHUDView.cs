using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.UI.Screen.Views
{
    public class TopHUDView: MonoBehaviour
    {
        [SerializeField] private TMP_Text _moneyValue;
        [SerializeField] private TMP_Text _energyValue;
        [SerializeField] private TMP_Text _diamondsValue;
        [SerializeField] private Button _settingsButton;

        public int Money { set => _moneyValue.text = value.ToString(); }
        public int Diamonds { set => _diamondsValue.text = value.ToString(); }
        public int Energy { set => _energyValue.text = value.ToString(); }

        public Button SettingsButton => _settingsButton;
    }
}