using UnityEngine;
using System.Collections.Generic;
using MergeIdle.Scripts.Configs.Order.Data;

[CreateAssetMenu(menuName = "ScriptableObjects/Database/OrderClientsDatabase",
    fileName = "OrderClientsDatabase")]
public class OrderClientsDatabase : ScriptableObject 
{
#pragma warning disable 649
    [SerializeField] private List<OrderClient> _items;
#pragma warning restore 649

    public List<OrderClient> Items => _items;
}
