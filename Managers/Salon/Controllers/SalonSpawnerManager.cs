using System.Linq;
using MergeIdle.Scripts.Configs.Salon.Data;
using MergeIdle.Scripts.Managers.Salon.Views;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.Salon.Controllers
{
    public class SalonSpawnerManager : MonoBehaviour
    {
        public SalonView SpawnItem(Transform parent, SalonItem salonItem, SalonView salonView)
        {  
            var spawnedSalonView = Instantiate(salonView, parent, false);
            var salonLevelData = salonItem.salonLevels.FirstOrDefault(x => x.level == salonItem.level);
            
            if (salonLevelData == null)
            {
                var salonLevels = salonItem.salonLevels;
                var salonLevelsAmount = salonLevels.Count;
                var lastSalonLevelIndex = salonLevelsAmount - 1;
                salonLevelData = salonItem.salonLevels[lastSalonLevelIndex];
            }

            spawnedSalonView.SalonId = salonItem.id;
            spawnedSalonView.Level = salonItem.level;
            spawnedSalonView.CurrentLevel = salonItem.currentLevel;
            spawnedSalonView.EXP = salonItem.exp;
            spawnedSalonView.SalonLevels = salonItem.salonLevels;
            spawnedSalonView.IsOpen = salonItem.isOpen;

            var level = salonItem.level;
            var progress = salonItem.exp;
            spawnedSalonView.UpdateXPBar(level, progress);
            
            if (salonLevelData != null)
            {
                spawnedSalonView.Sprite = salonLevelData.levelSprite;
                spawnedSalonView.Currency = salonLevelData.currency;
                spawnedSalonView.MaxEXP = salonLevelData.exp;
                spawnedSalonView.AmountPerSecond = salonLevelData.amountPerSecond;
            }

            if (!salonItem.isOpen)
            {
                spawnedSalonView.Close();
            }
            return spawnedSalonView;
        }
    }
}