using System;
using System.Collections.Generic;
using System.Linq;
using MergeIdle.Scripts.Configs.Purchase.Enum;
using MergeIdle.Scripts.Configs.Salon.Data;
using MergeIdle.Scripts.Configs.Salon.Enum;
using MergeIdle.Scripts.Managers.Timer.Controllers;
using MergeIdle.Scripts.Managers.Tutorial.Views;
using UnityEngine;
using UnityEngine.UI;

namespace MergeIdle.Scripts.Managers.Salon.Views
{
    public class SalonView : MonoBehaviour
    {
        public event Action<ESalonId, int> OnClickUpdate;
        
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private HorizontalLayoutGroup _horizontalLayoutGroup;
        [SerializeField] private Image _image;
        [SerializeField] private Button _item;
        [SerializeField] private RectTransform _salon;
        
        [SerializeField] private ECurrency _currency;
        [SerializeField] private XPBarView _xpBarView;
        [SerializeField] private Button _updateButton;

        [Header("Timer")] 
        [SerializeField] private TimerManager _timerManager;
        
        //TEMP
        [Header("Finger")] 
        [SerializeField] private FingerView _fingerView;
        
        private ESalonId _salonId;
        private int _level;
        private int _exp = 0;
        private int _maxExp;
        private float _amountPerSecond;
        private List<SalonLevel> _salonLevels;
        private List<Sprite> _levelSprites;
        private bool _isOpen;
        private int _currentLevel;

        public TimerManager TimerManager => _timerManager;
        public Button Item => _item;
        public int EXP { get => _exp; set => _exp = value; }
        public int MaxEXP { set => _maxExp = value; }
        public List<SalonLevel> SalonLevels { get => _salonLevels; set => _salonLevels = value; }
        public float AmountPerSecond { get => _amountPerSecond; set => _amountPerSecond = value; }
        public ESalonId SalonId { get => _salonId; set => _salonId = value; }
        public int Level { get => _level; set => _level = value; } // View level
        public ECurrency Currency { get => _currency; set => _currency = value; }
        public int CurrentLevel { get => _currentLevel;  set => _currentLevel = value;}
        public Sprite Sprite { set => _image.sprite = value; }

        public bool IsOpen { get => _isOpen; set => _isOpen = value; }

        private void OnEnable()
        {
            _updateButton.onClick.AddListener(() => OnClickUpdate?.Invoke(_salonId, _currentLevel));
            OnClickUpdate += OnUpdate;
        }

        public void UpdateExp(int exp)
        {
            TryUpdateButton(exp);
            AddEXP(exp);
            _level = _currentLevel;
            TryUpdateXPBar(_level, _exp, !IsNewLevel);
        }
        
        public bool IsNewLevel =>  _updateButton.isActiveAndEnabled;
        
        public void UpdateXPBar(int level, float progress)
        {
            _xpBarView.SetData(level, progress);
        }

        private void OnUpdate(ESalonId salonId, int level)
        {
            UpdateLevel();
            UpdateSalon(salonId, level);
            UpdateXPBar(level, _exp);
            ChangeUpdateButton();
        }
        
        private void UpdateLevel()
        {
            TryUpdateButton(_exp);
            _level = _currentLevel;
            TryUpdateXPBar(_level, _exp, !IsNewLevel);
        }

        private void ChangeUpdateButton()
        {
            _updateButton.gameObject.SetActive(false);
            _fingerView.Hide();
        }
        
        private void UpdateSalon(ESalonId salonId, int level)
        {
            var salon = Utils.GetSalonById(salonId);
            var salonLevelData = salon.salonLevels.FirstOrDefault(x => x.level == level);

            if (salonLevelData != null)
            {
                _image.sprite = salonLevelData.levelSprite;

                _maxExp = salonLevelData.exp;   
                _amountPerSecond = salonLevelData.amountPerSecond;
            }
        }

        private void TryUpdateButton(int exp)
        {
            if (CanUpdate(_exp, exp, _maxExp))
            {
                _updateButton.gameObject.SetActive(true);
                TryUpdateFinger(_salonId, _level);
            }
        }

        private void AddEXP(int value)
        {
            if (CanUpdate(_exp, value, _maxExp))
            {
                _currentLevel++;
                _exp = _exp + value - _maxExp;
            }
            else
            {
                _exp += value;
            }
        }

        private bool CanUpdate(int curExp, int newExp, int maxExp) => curExp + newExp - maxExp >= 0;

        private void TryUpdateXPBar(int level, float exp, bool isNewLevel)
        {
            if (isNewLevel)
            {
                UpdateXPBar(level, exp);
            }
            else
            {
                _xpBarView.SetMaxProgress();
            }
        }

        private void TryUpdateFinger(ESalonId salonId, int level)
        {
            if (salonId == ESalonId.S_1 && level == 1) 
            {
                _fingerView.Show();
            }
        }

        #region transform

        public void SetHorizontalPosition(TextAnchor anchor)
        {
            _horizontalLayoutGroup.childAlignment = anchor;
        }
        
        public void SetHorizontalScale(int value)
        {
            _image.transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);;
        }
        
        public void SetHorizontalPadding(int padding, bool isLeft)
        {
            if (isLeft)
            {
                _horizontalLayoutGroup.padding.left = padding;
            }
            else
            {
                _horizontalLayoutGroup.padding.right = padding;
            }
        }

        public void Close()
        {
            _isOpen = false;
            _canvasGroup.alpha = 0.4f;
            _canvasGroup.interactable = false;
            _xpBarView.gameObject.SetActive(false);
        }
        
        public void Open()
        {
            _isOpen = true;
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _xpBarView.gameObject.SetActive(true);
        }

        #endregion

        private void OnDisable()
        {
            _updateButton.onClick.RemoveAllListeners();
            OnClickUpdate -= OnUpdate;
        }
    }
}