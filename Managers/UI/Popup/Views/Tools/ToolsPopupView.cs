using System;
using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Merge.Enum;
using MergeIdle.Scripts.Configs.Purchase.Enum;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.UI.Popup.Views.Tools
{
    public class ToolsPopupView: BasePopupView
    {
        public event Action OnClickClose;
        [SerializeField] private Button _close;
        
        [Header("Merge Items")]
        [SerializeField] private List<ToolView> _items;
        [Header("Generators")]
        [SerializeField] private List<ToolView> _generators;

        public void Setup(EMergeType mergeType)
        {
            SetupItemsChain(mergeType);
            SetupGenerators(mergeType);
        }

        private void SetupItemsChain(EMergeType mergeType)
        {
            var sprites = Utils.GetMergeItemsSprites(mergeType);
            SetupItems(_items, sprites);
        }
        
        private void SetupGenerators(EMergeType mergeType)
        {
            var purchaseType = GetPurchaseType(mergeType);
            var sprites = Utils.GetPurchasesSprites(purchaseType);
            SetupItems(_generators, sprites);
        }

        private void SetupItems(List<ToolView> toolViews, List<Sprite> sprites)
        {
            for (int i = 0; i < toolViews.Count; i++)
            {
                int num = i + 1;

                Sprite sprite = sprites[0];
                bool isOpen = num <= sprites.Count;
                
                if (isOpen)
                {
                    sprite = sprites[i];
                }

                var item = toolViews[i];
                item.Setup(sprite, num, isOpen);
            }
        }

        private EPurchaseType GetPurchaseType(EMergeType mergeType)
        {
            switch (mergeType)
            {
                case EMergeType.A_VIDEOCARD:
                case EMergeType.B_PROCESSOR:
                    return EPurchaseType.A;
                case EMergeType.C_RAM:
                case EMergeType.D_CASE:
                    return EPurchaseType.B;
                default:
                    return EPurchaseType.C;
            }
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _close.onClick.AddListener(() => OnClickClose?.Invoke());
            OnClickClose += Hide;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _close.onClick.RemoveAllListeners();
            OnClickClose = null;
        }
    }
}