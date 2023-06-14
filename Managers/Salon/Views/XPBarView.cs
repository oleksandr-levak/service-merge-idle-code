using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.Salon.Views
{
    public class XPBarView : MonoBehaviour
    {
        private const string LEVEL = "LVL. ";
        
        [SerializeField] private Text _level;
        [SerializeField] private Image _progressImage;

        public void SetData(int level, float progress)
        {
            SetProgress(progress);
            SetLevel(level);
        }

        public void SetProgress(float value)
        {
            _progressImage.fillAmount = value * 0.01F;
        }

        public void SetLevel(int value)
        {
            _level.text = LEVEL+value;
        }

        public void SetMaxProgress()
        {
            _progressImage.fillAmount = 1;
        }
        
    }
}