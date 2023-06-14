using System;
using MergeIdle.Scripts.Managers.Order.Enum;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.Order.Data
{
    [Serializable]
    public class OrderButtonData
    {
        public EButton type;
        public Sprite sprite;
        public string text;
    }
}