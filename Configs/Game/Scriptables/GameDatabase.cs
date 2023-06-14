using System.Collections.Generic;
using System.Linq;
using MergeIdle.Scripts.Configs.Purchase.Enum;
using MergeIdle.Scripts.Databases.Game.Data;
using UnityEngine;

namespace MergeIdle.Scripts.Configs.Game.Scriptables
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Database/GameDatabase",
        fileName = "GameDatabase")]
    public class GameDatabase : ScriptableObject 
    {
#pragma warning disable 649
        [SerializeField] private Sprite _closeSprite;
        [Header("Currency")]
        [SerializeField] private List<Currency> _currencies;
#pragma warning restore 649

        public Sprite CloseSprite => _closeSprite;

        public Sprite GetCurrencySpriteByType(ECurrency currency) => _currencies.FirstOrDefault(x => x.currency == currency)?.sprite;
    }
}
