using MergeIdle.Scripts.Configs.Purchase.Enum;
using MergeIdle.Scripts.Managers.UI.Popup.Views.Development.Enum;

namespace MergeIdle.Scripts.Managers.UI.Popup.Views.Development.Data
{
    public class InputRowData
    {
        public EInputRow InputRowType;
        public string Value;
        
        public InputRowData(EInputRow InputRowType, string Value)
        {
            this.InputRowType = InputRowType;
            this.Value = Value;
        }

        public InputRowData()
        {
        }
    }
}