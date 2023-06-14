using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Salon.Data;
using UnityEngine;

namespace MergeIdle.Scripts.Configs.Salon.Scriptables
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Database/SalonItemsDatabase",
        fileName = "SalonItemsDatabase")]
    public class SalonItemsDatabase : ScriptableObject 
    {
#pragma warning disable 649
        [SerializeField] private List<SalonItem> _items;
#pragma warning restore 649

        public List<SalonItem> Items => _items;
    }
}
