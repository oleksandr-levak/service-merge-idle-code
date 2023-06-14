using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Purchase.Data;
using UnityEngine;

namespace MergeIdle.Scripts.Configs.Purchase.Scriptables
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Database/PurchaseFactorsDatabase",
        fileName = "PurchaseFactorsDatabase")]
    public class PurchaseFactorsDatabase : ScriptableObject 
    {
#pragma warning disable 649
        [SerializeField] private List<PurchaseFactor> _items;
#pragma warning restore 649

        public List<PurchaseFactor> Items => _items;
    }
}
