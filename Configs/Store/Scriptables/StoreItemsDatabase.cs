using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Store.Data;
using UnityEngine;

namespace MergeIdle.Scripts.Configs.Store.Scriptables
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Database/StoreItemsDatabase",
        fileName = "StoreItemsDatabase")]
    public class StoreItemsDatabase : ScriptableObject 
    {
#pragma warning disable 649
        [SerializeField] private List<StoreItem> _items;
#pragma warning restore 649

        public List<StoreItem> Items => _items;
    }
}
