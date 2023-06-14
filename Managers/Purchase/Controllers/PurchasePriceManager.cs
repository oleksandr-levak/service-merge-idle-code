using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Формула = X*Y*(C1+C2+C3)
// X это время таймера обновляющего список генераторов в секундах.
// Y множитель
// С1/С2/С3 - Нынешний пассивный доход заказчиков(салонов) в секунду

namespace MergeIdle.Scripts.Managers.Purchase.Controllers
{
    public class PurchasePriceManager : MonoBehaviour
    {
        private const float Y = 1f;
        public float GetPurchasePrice(int purchaseTimer, int salonLevel, List<float> passiveIncomes)
        {
            var factor = Utils.GetLevelFactor(salonLevel);
            var passiveIncomeCount = passiveIncomes.Count;
            var passiveIncomeSum = passiveIncomes.Take(passiveIncomeCount).Sum();

            return purchaseTimer * factor * passiveIncomeSum;
        }
    }
}