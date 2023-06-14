using System;
using System.Collections.Generic;
using System.Linq;
using MergeIdle.Scripts.Configs.Merge.Data;
using MergeIdle.Scripts.Configs.Merge.Enum;
using MergeIdle.Scripts.Databases.MergeItems.Enum;
using MergeIdle.Scripts.Managers.Merge.Data;
using MergeIdle.Scripts.Storage.Controller;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MergeIdle.Scripts.Managers.Merge.Controllers
{
    public class GeneratorManager : MonoBehaviour
    {
        private const string DROP_LIST_1 = "AABBABABBBBABBBABBAABBABABABAAABABAAABBB";
        private const string DROP_LIST_2 = "CDCCDCDDCCCDCDCCDCDDDCDCCDDCDCCCCDCDDDCDCCDCDDDCCD";
        private const string DROP_LIST_3 = "FEEFEEFEEFEEFEFEEFEFEEFEFEEFFEFFEEEFEEFEEFEEFFEEEFEE";

        private const string RandomChance_1 = "1-100";
        private const string RandomChance_2 = "1-60/2-30/3-10";
        private const string RandomChance_3 = "4-50/5-30/6-15/7-5";
        
        [Header("Storage")]
        [SerializeField] private GameStorageController _gameStorage;

        private Dictionary<string, EMergeType> _generatorsItems = new()
        {
            { "A", EMergeType.A_VIDEOCARD },
            { "B", EMergeType.B_PROCESSOR },
            
            { "C", EMergeType.C_RAM },
            { "D", EMergeType.D_CASE },
            
            { "F", EMergeType.F_BATTERY },
            { "E", EMergeType.E_CASE },
        };

        public SpawnItemData GetGeneratorSpawnItem(int id, EMergeType mergeType, int slotStartId, int slotEndId)
        {
            SpawnItemData spawnItemData =
                new SpawnItemData(EItemState.OPEN, EMergeCategory.NO_GENERATOR, mergeType, true, 0, id, slotStartId, slotEndId);
            return spawnItemData;
        }

        public MergeItemToGenerate GetMergeItem(EMergeType mergeType)
        {
            MergeItemToGenerate itemToGenerate;
            if (IsDropListEnd(mergeType))
            {
                itemToGenerate = Utils.GetItemTypeByGenerator(mergeType);
                itemToGenerate.level = GetMergeItemLevel(mergeType);
            }
            else
            {
                itemToGenerate = GetMergeItemByDropList(mergeType);
                
                var dropList = GetDropList(mergeType);
                IncDropListPointer(dropList);
            }
            return itemToGenerate;
        }

        private bool IsDropListEnd(EMergeType mergeType)
        {
            var dropList = GetDropList(mergeType);
            var dropListIndex = GetDropListPointer(dropList);
            var isDropListEnd = dropList.Length <= dropListIndex;
            return isDropListEnd;
        }

        private MergeItemToGenerate GetMergeItemByDropList(EMergeType mergeType)
        {
            MergeItemToGenerate mergeItemToGenerate = new MergeItemToGenerate();
            
            var dropList = GetDropList(mergeType);
            var dropListIndex = GetDropListPointer(dropList);
            string dropListLetter = dropList[dropListIndex].ToString();

            var mergeItemTypeToSpawn = GetTypeByLetter(dropListLetter);
            var mergeLevelToSpawn = GetMergeItemLevel(mergeType);

            mergeItemToGenerate.mergeType = mergeItemTypeToSpawn;
            mergeItemToGenerate.level = mergeLevelToSpawn;

            return mergeItemToGenerate;
        }

        private string GetDropList(EMergeType mergeType)
        {
            switch (mergeType)
            {
               case EMergeType.PUR_A_1:
               case EMergeType.PUR_A_2:
               case EMergeType.PUR_A_3:
                   return DROP_LIST_1;
               case EMergeType.PUR_B_1:
               case EMergeType.PUR_B_2:
               case EMergeType.PUR_B_3:
                   return DROP_LIST_2;
               default:
                   return DROP_LIST_3;
            }
        }
        
        private string GetLevelsProbabilitiesString(EMergeType mergeType)
        {
            switch (mergeType)
            {
                case EMergeType.PUR_A_1:
                case EMergeType.PUR_B_1:
                case EMergeType.PUR_C_1:
                    return RandomChance_1;
                case EMergeType.PUR_A_2:
                case EMergeType.PUR_B_2:
                case EMergeType.PUR_C_2:
                    return RandomChance_2;
                default:
                    return RandomChance_3;
            }
        }

        private int GetMergeItemLevel(EMergeType mergeType)
        {
            var levelsProbabilitiesString = GetLevelsProbabilitiesString(mergeType);
            var levelsProbabilitiesDict = GetLevelsProbabilities(levelsProbabilitiesString);

            var level = GetLevelByProbabilities(levelsProbabilitiesDict);
            return level;
        }

        private int GetLevelByProbabilities(Dictionary<int, int> levelsProbabilities)
        {
            int i = Random.Range(1, 101);
            
            var bottomLimit = 0;
            foreach (var levelProbability in levelsProbabilities)
            {
                var key = levelProbability.Key;
                var value = levelProbability.Value;
                
                if (i >= bottomLimit && i <= value + bottomLimit)
                {
                    return key;
                }
                bottomLimit += value;
            }

            return levelsProbabilities.Keys.ToList()[0];
        }

        private Dictionary<int, int> GetLevelsProbabilities(string data)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();
            List<string> list = data.Split('/').ToList();
            foreach(var l in list)
            {
                var values = l.Split('-');
                var key = Int32.Parse(values[0]);
                var value = Int32.Parse(values[1]);
                result.Add(key, value);
            }

            return result;
        }

        private EMergeType GetTypeByLetter(string letter)
        {
            return _generatorsItems[letter];
        }

        private int GetDropListPointer(string key)
        {
            return _gameStorage.GetIntValue(key);
        }

        private void IncDropListPointer(string key)
        {
            int prevVal = GetDropListPointer(key);
            int newValue = prevVal + 1;
            _gameStorage.SetIntValue(key, newValue);
        }
    }
}