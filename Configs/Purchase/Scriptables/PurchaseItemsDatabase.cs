using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Purchase.Data;
using UnityEngine;

namespace MergeIdle.Scripts.Configs.Purchase.Scriptables
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Database/PurchaseItemsDatabase",
        fileName = "PurchaseItemsDatabase")]
    public class PurchaseItemsDatabase : ScriptableObject 
    {
#pragma warning disable 649
        [SerializeField] private List<PurchaseItem> _items;
#pragma warning restore 649

        public List<PurchaseItem> Items => _items;
    }
}
