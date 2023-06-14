using UnityEngine;
using System.Collections.Generic;
using MergeIdle.Scripts.Databases.MapItems.Data;

[CreateAssetMenu(menuName = "ScriptableObjects/Database/MapItemsDatabase",
    fileName = "MapItemsDatabase")]
public class MapItemsDatabase : ScriptableObject 
{
#pragma warning disable 649
    [SerializeField] private List<MapItem> _items;
#pragma warning restore 649

    public List<MapItem> Items => _items;
}
