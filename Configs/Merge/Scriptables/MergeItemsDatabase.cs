using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Merge.Data;
using UnityEngine;

namespace MergeIdle.Scripts.Configs.Merge.Scriptables
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Database/MergeItemsDatabase",
        fileName = "MergeItemsDatabase")]
    public class MergeItemsDatabase : ScriptableObject 
    {
#pragma warning disable 649
        [SerializeField] private List<MergeCategory> _items;
#pragma warning restore 649

        public List<MergeCategory> Items => _items;
    }
}
