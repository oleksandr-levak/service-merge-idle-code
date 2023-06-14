using MergeIdle.Scripts.Managers.UI.Popup.Views.Development.Data;
using MergeIdle.Scripts.Managers.UI.Popup.Views.Development.Enum;
using TMPro;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.UI.Popup.Views.Development
{
    public class InputRowView: MonoBehaviour
    {
        [SerializeField] private EInputRow _inputRowType;
        [SerializeField] private TMP_InputField _inputField;

        public string InputValue =>_inputField.text;

        public InputRowData Data()
        {
            InputRowData inputRowData = new InputRowData(_inputRowType, InputValue);
            return inputRowData;
        }
    }
}