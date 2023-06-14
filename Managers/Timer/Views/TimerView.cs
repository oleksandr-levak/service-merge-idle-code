using TMPro;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.Timer.Views
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _time;

        public void DisplayTime(float timeToDisplay)
        {
            timeToDisplay += 1;
            float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            _time.text = $"{minutes:0}:{seconds:00}";
        }
    }
}