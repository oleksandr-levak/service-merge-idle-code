using System;
using System.Collections.Generic;
using System.Linq;
using MergeIdle.Scripts.Configs.Salon.Data;
using MergeIdle.Scripts.Configs.Salon.Enum;
using MergeIdle.Scripts.Databases.MapItems.Enum;
using MergeIdle.Scripts.Managers.Salon.Data;
using MergeIdle.Scripts.Managers.Salon.Views;
using MergeIdle.Scripts.Storage.Mapper;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.Salon.Controllers
{
    public class SalonManager : MonoBehaviour
    {
        private const int SECONDS_PER_UPDATE = 1;
        private const int SALON_ROW_PADDING = -20;
        public event Action OnClickItem;
        public event Action<ESalonId, int> OnSalonUpdate;
        public event Action<ESalonId, int> OnSalonOpen;
        public event Action<SalonView, float> OnSalonTimerEnd;
        private Action<Dictionary<ESalonId, SalonView>> OnSpawnFinished;

        private Action<int> OnTimerEnd;
        
        [SerializeField] private SalonView _salonView;
        [SerializeField] private SalonSpawnerManager _salonSpawnerManager;

        private Transform _parent;
        private Dictionary<ESalonId, SalonView> _salonViews;
        
        public Dictionary<ESalonId, SalonView> SalonViews => _salonViews;

        private void Start()
        {
            _salonViews = new Dictionary<ESalonId, SalonView>();
            
            OnSpawnFinished += SetupSalonsTimers;
            OnSalonTimerEnd += SalonTimerRestart;
        }

        public void InitSalons(Transform parent, EMapId mapId)
        {
            var items = Utils.GetSalonsByMapId(mapId);
            SetupSalons(parent, items);
        }
        
        public void SetupSalons(Transform parent, List<SalonItem> salonItems)
        {
            _parent = parent;
            int counter = 1;
            foreach (var item in salonItems)
            {
                SalonView salonView = _salonSpawnerManager.SpawnItem(parent, item, _salonView);
                salonView.OnClickUpdate += (id, lvl) => OnSalonUpdate?.Invoke(id, lvl);
                
                bool isRight = counter % 2 == 0;
                SetupSalonView(salonView, isRight);
                counter++;
                
                _salonViews.Add(item.id, salonView);
            }
            OnSpawnFinished?.Invoke(_salonViews);
        }

        private void SetupSalonView(SalonView salonView, bool isRight)
        {
            salonView.SetHorizontalPosition(isRight ? TextAnchor.MiddleRight :  TextAnchor.MiddleLeft);
            salonView.SetHorizontalPadding(SALON_ROW_PADDING, !isRight);
            salonView.SetHorizontalScale(isRight ? -1 : 1);
            salonView.Item.onClick.AddListener(() => OnClickItem?.Invoke());
        }

        public SalonData GetNextSalon()
        {
            var lastOpenSalon = GetOpenSalons().Select(SalonData.MapFrom).ToList().Last();
            return lastOpenSalon;
        }

        public void UpdateSalon(ESalonId id, int exp)
        {
            var salonView = GetById(id);
            salonView.UpdateExp(exp);
        }

        public void OpenSalon(ESalonId id)
        {
            if (_salonViews.ContainsKey(id))
            {
                var salonView = GetById(id);
                if (salonView != null)
                {
                    UpdateSalon(id, 0);
                    SetupSalonTimer(salonView);
                    salonView.Open();
                    OnSalonOpen?.Invoke(salonView.SalonId, 1);
                }
            }
        }

        public List<SalonView> GetOpenSalons()
        {
            List<SalonView> salonViews = _salonViews.Values.ToList();
            List<SalonView> openSalonViews = salonViews.FindAll(x => x.IsOpen);
            return openSalonViews;
        }
        
        public float GetOpenSalonsPassiveIncomesSum()
        {
            return GetOpenSalonsPassiveIncomes().Sum();
        }

        public List<float> GetOpenSalonsPassiveIncomes()
        {
            List<SalonView> openSalonViews = GetOpenSalons();
            List<float> salonIncomes = openSalonViews.ConvertAll(x => x.AmountPerSecond);
            return salonIncomes;
        }

        public bool IsNewLevelSalon()
        {
            var newLevelSalons = _salonViews.Values.ToList().FindAll(x => x.IsNewLevel);
            bool isNewLevelSalon = newLevelSalons.Count > 0;
            return isNewLevelSalon;
        }
        
        public SalonData GetSalonByType(ESalonId type)
        {
            return GetById(type).MapToData();
        }
        
        private void SetupSalonsTimers(Dictionary<ESalonId, SalonView> salonViews)
        {
            List<SalonView> openSalonViews = salonViews.Values.ToList().FindAll(x => x.IsOpen);
            foreach (var salonView in openSalonViews) SetupSalonTimer(salonView);
        }
        
        private void SetupSalonTimer(SalonView salonView)
        {
            string key = $"{salonView.SalonId}";
            
            OnTimerEnd = amount => OnSalonTimerEnd?.Invoke(salonView, salonView.AmountPerSecond * amount);
            salonView.TimerManager.OnTimerEnd += OnTimerEnd;

            salonView.TimerManager.StartTimer(key, SECONDS_PER_UPDATE);
        }

        private void SalonTimerRestart(SalonView salonView, float amount)
        {
            string key = $"{salonView.SalonId}";
            salonView.TimerManager.RestartTimer(key, SECONDS_PER_UPDATE);
        }

        private SalonView GetById(ESalonId key)
        {
            return _salonViews[key];
        }

        private void OnDestroy()
        {
            OnClickItem = null;
            OnSalonTimerEnd = null;
            OnSpawnFinished = null;
            OnTimerEnd = null;
            
            if (_parent != null)
            {
                foreach (Transform child in _parent)
                {
                    if (child.GetComponent<SalonView>() != null)
                    {
                        SalonView salonView = child.GetComponent<SalonView>();
                        salonView.Item.onClick.RemoveAllListeners();
                        salonView.TimerManager.OnTimerEnd -= OnTimerEnd;
                    }
                }
            }
        }
    }
}