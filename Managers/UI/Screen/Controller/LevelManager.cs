using System;
using System.Collections.Generic;
using System.Linq;
using MergeIdle.Scripts.Managers.UI.Screen.Views;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.UI.Screen.Controller
{
    public class LevelManager : MonoBehaviour
    {
        public event Action<int> OnLevelUpdate;
        
        private const string LVL_FOR_EXP = "0,4,7,14,28,56,112,224,448,896";
        
        [SerializeField] private LevelView _levelView;
        
        public void UpdateCurrentLevel(int orders)
        {
            var levelForExpDict = GetLevelForExpDict(LVL_FOR_EXP);
            var curLevel = GetCurrentLevel(levelForExpDict, orders - 1);
            var newLevel = SetCurrentLevel(orders);

            if (curLevel < newLevel)
            {
                OnLevelUpdate?.Invoke(newLevel);
            }
        }

        public int SetCurrentLevel(int orders)
        {
            var levelForExpDict = GetLevelForExpDict(LVL_FOR_EXP);
            var lvl = GetCurrentLevel(levelForExpDict, orders);
            _levelView.Level = lvl;
            return lvl;
        }

        private float GetCurrentLevelExp(int orders)
        {
            var levelForExpDict = GetLevelForExpDict(LVL_FOR_EXP);
            var lvl = GetCurrentLevel(levelForExpDict, orders);
            var ordersCountBetweenLevels = GetOrdersCountBetweenLevels(lvl, lvl + 1);
            var startLevelOrders = GetLevelOrders(levelForExpDict, lvl);

            var startValue = orders - startLevelOrders;
            var progress = (100 / startValue) * ordersCountBetweenLevels;

            return progress;
        }

        private int GetOrdersCountBetweenLevels(int lvlStart, int lvlEnd)
        {
            var levelForExpDict = GetLevelForExpDict(LVL_FOR_EXP);
            var lvlStartOrders = GetLevelOrders(levelForExpDict, lvlStart);
            var lvlEndOrders = GetLevelOrders(levelForExpDict, lvlEnd);
            return lvlEndOrders - lvlStartOrders;
        }

        private int GetLevelOrders(Dictionary<int, int> levels, int level)
        {
            var levelOrders = levels.FirstOrDefault(x => x.Value == level).Key;
            return levelOrders;
        }

        private int GetCurrentLevel(Dictionary<int, int> levels, int orders)
        {
            if (levels.ContainsKey(orders))
            {
                var newLevel = levels[orders];
                return newLevel;
            }
            var keys = levels.Keys;
            int prevKey = 0;
            foreach (var key in keys)
            {
                if (orders < key)
                {
                    return levels[prevKey];
                }

                prevKey = key;
            }

            return 100;
        }

        private Dictionary<int, int> GetLevelForExpDict(string data)
        {
            Dictionary<int, int> levelForExpDict = new Dictionary<int, int>();
            List<int> list = data.Split(',').ToList().ConvertAll(x => Int32.Parse(x));

            for (int i = 0; i < list.Count; i++)
            {
                int lvl = i + 1;
                int orders = list[i];
                levelForExpDict.Add(orders, lvl);
            }

            return levelForExpDict;
        }
    }
}