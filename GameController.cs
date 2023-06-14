using System.Collections.Generic;
using System.Linq;
using MergeIdle.Scripts.Configs;
using MergeIdle.Scripts.Configs.Audio.Enums;
using MergeIdle.Scripts.Configs.Merge.Enum;
using MergeIdle.Scripts.Configs.Order.Data;
using MergeIdle.Scripts.Configs.Order.Enum;
using MergeIdle.Scripts.Configs.Purchase.Data;
using MergeIdle.Scripts.Configs.Purchase.Enum;
using MergeIdle.Scripts.Configs.Salon.Data;
using MergeIdle.Scripts.Configs.Salon.Enum;
using MergeIdle.Scripts.Configs.Store.Const;
using MergeIdle.Scripts.Databases.MapItems.Enum;
using MergeIdle.Scripts.Databases.MergeItems.Enum;
using MergeIdle.Scripts.Helpers;
using MergeIdle.Scripts.Managers;
using MergeIdle.Scripts.Managers.AppLovinMAX.Consts;
using MergeIdle.Scripts.Managers.AppLovinMAX.Controllers;
using MergeIdle.Scripts.Managers.Audio.Controllers;
using MergeIdle.Scripts.Managers.FirebaseManager.Const;
using MergeIdle.Scripts.Managers.FirebaseManager.Controllers;
using MergeIdle.Scripts.Managers.Merge.Controllers;
using MergeIdle.Scripts.Managers.Merge.Data;
using MergeIdle.Scripts.Managers.Merge.Views;
using MergeIdle.Scripts.Managers.Order.Controllers;
using MergeIdle.Scripts.Managers.Order.Enum;
using MergeIdle.Scripts.Managers.Order.Views;
using MergeIdle.Scripts.Managers.Purchase.Controllers;
using MergeIdle.Scripts.Managers.Purchase.Views;
using MergeIdle.Scripts.Managers.Salon.Controllers;
using MergeIdle.Scripts.Managers.Salon.Views;
using MergeIdle.Scripts.Managers.Tutorial.Controllers;
using MergeIdle.Scripts.Managers.Tutorial.Enum;
using MergeIdle.Scripts.Managers.UI.Navigation.Controllers;
using MergeIdle.Scripts.Managers.UI.Popup.Controllers;
using MergeIdle.Scripts.Managers.UI.Popup.Enums;
using MergeIdle.Scripts.Managers.UI.Popup.Views.Congratulation;
using MergeIdle.Scripts.Managers.UI.Popup.Views.Development;
using MergeIdle.Scripts.Managers.UI.Popup.Views.Development.Data;
using MergeIdle.Scripts.Managers.UI.Popup.Views.Development.Enum;
using MergeIdle.Scripts.Managers.UI.Popup.Views.LevelUp;
using MergeIdle.Scripts.Managers.UI.Popup.Views.Mistake;
using MergeIdle.Scripts.Managers.UI.Popup.Views.MoreCurrency;
using MergeIdle.Scripts.Managers.UI.Popup.Views.Settings;
using MergeIdle.Scripts.Managers.UI.Popup.Views.Store;
using MergeIdle.Scripts.Managers.UI.Popup.Views.Tools;
using MergeIdle.Scripts.Managers.UI.Screen.Controller;
using MergeIdle.Scripts.Managers.UI.Screen.Data;
using MergeIdle.Scripts.Managers.UI.Screen.Enums;
using MergeIdle.Scripts.Storage.Controller;
using MergeIdle.Scripts.Storage.Data;
using MergeIdle.Scripts.Storage.Enum;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace MergeIdle.Scripts
{
    public class GameController : MonoBehaviour
    {
        private int ROW_LENGHT;
        private int COL_LENGHT;
        
        private const int ENERGY_GENERATOIN_TIME = 30;
        private const int HINT_IMMEDIATE_TIME = 10; // SECONDS
        private const int GAME_IMMEDIATE_TIME = 1; // SECONDS
        private const int SUPER_OFFER_SECONDS_FIRST = 30; // SECONDS
        private const int SUPER_OFFER_SECONDS_NEXT = 240; // SECONDS
        private const int SUPER_OFFER_SHOW_TIMES = 5;
        
        [Header("Data")]
        [SerializeField] private List<int> _closeSlotIds;
        [SerializeField] private List<int> _immovableSlotIds;
        [Header("Storage")]
        [SerializeField] private GameStorageController _gameStorage;
        [Header("Configs")]
        [SerializeField] private ConfigsController _configsController; 
        [Header("Helpers")]
        [SerializeField] private OrthographicSize _orthographicSize;
        [Header("Managers")]
        [SerializeField] private MergeManager _mergeManager;
        [SerializeField] private SlotsManager _slotsManager;
        [SerializeField] private ScreenManager _screenManager;
        [SerializeField] private PopupManager _popupManager;
        [SerializeField] private CameraManager _cameraManager;
        [SerializeField] private PurchaseManager _purchaseManager;
        [SerializeField] private OrderManager _orderManager;
        [SerializeField] private NavigationManager _navigationManager;
        [SerializeField] private SalonManager _salonManager;
        [SerializeField] private CoroutineManager _coroutineManager;
        [SerializeField] private AppLovinMAXManager _appLovinMAXManager;
        [SerializeField] private TutorialManager _tutorialManager;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private FirebaseManager _firebaseManager;

        [Header("Development")]
        [SerializeField] private Button _openDevelopmentButton;
        [SerializeField] private Button _openDebugButton;

        private Camera _mainCamera;
        private RaycastHit2D _prevHit;
        private Vector3 _target;
        private ItemInfo _carryingItem;
        private EMergeType _currGen;
        private float _dist = 1;
        private PurchasesView _purchasesPanelView;

        // Ads
        private PurchaseView _purchaseView;
        private string _adType;
        
        // Timer
        private int _lastGameTimeMinute;
        
        // Tutorial
        private ETutorialStage _tutorialStage;
        private Coroutine _checkClientOrdersCoroutine;
        
        // Popups
        private StorePopupView _storePopupView;
        private MistakePopupView _mistakePopupView;
        private LevelUpPopupView _levelUpPopupView;
        private SettingsPopupView _settingsPopupView;
        private DevelopmentPopupView _developmentPopupView;
        private MoreCurrencyPopupView _moreCurrencyPopupView;
        
        // Development
        private int _counter;
        
        private void Awake() 
        {
            _configsController.InitConfigs();
            Debug.unityLogger.logEnabled = false;
        }

        private void Start()
        {
            _firebaseManager.OnFetchedRemoteConfigs += OnFetchedRemoteConfigs;
            _firebaseManager.Init();
        }

        private void OnFetchedRemoteConfigs()
        {
            SetupGameTimer(GAME_IMMEDIATE_TIME);
            SetupHintTimer(HINT_IMMEDIATE_TIME);
            
            _navigationManager.ShowView(EView.CORE);
            _navigationManager.ShowView(EView.CORE_GAME_OBJECT);
            
            _screenManager.CanvasManager.SetSafeArea();
            
            _orthographicSize.SetOrthographicSize();
            _orthographicSize.SetPosition(_slotsManager.SlotsRectTransform);
            _purchasesPanelView = _purchaseManager.PurchasesPanelView;
        
            _mainCamera = _cameraManager.MainCamera;
            _mergeManager.SetCamera(_mainCamera);
            _storePopupView = (StorePopupView) _popupManager.GetPopupView(EPopup.STORE);
            _storePopupView.SetupPurchases();
            
            _mistakePopupView = (MistakePopupView) _popupManager.GetPopupView(EPopup.MISTAKE);
            _levelUpPopupView = (LevelUpPopupView) _popupManager.GetPopupView(EPopup.LEVEL_UP);
            _settingsPopupView = (SettingsPopupView) _popupManager.GetPopupView(EPopup.SETTINGS);
            _developmentPopupView = (DevelopmentPopupView) _popupManager.GetPopupView(EPopup.DEVELOPMENT);
            _moreCurrencyPopupView = (MoreCurrencyPopupView) _popupManager.GetPopupView(EPopup.MORE_CURRENCY);
            
            AddEventListeners();
            Initialize();
            _appLovinMAXManager.Initialize();
            
            _screenManager.HideLoading();
        }

        private void Initialize()
        {
            _screenManager.TopHUDManager.StartEnergyTimer(ENERGY_GENERATOIN_TIME);

            var isFirstVisit = _gameStorage.IsFirstVisit;
            var orders = _screenManager.GameView.Orders;
            var purchases = _screenManager.GameView.Purchases;
            var idleMap = _screenManager.GameView.IdleMap;
            
            int energyConfig = _gameStorage.Energy;
            float moneyConfig = _gameStorage.Money;
            float diamondsConfig = _gameStorage.Diamonds;
            
            _slotsManager.InitSlots();

            UpdateTutorialStage(ETutorialStage.NONE);
            SetupAudio(isFirstVisit);
            
            if (isFirstVisit)
            {
                energyConfig = (int)_firebaseManager.GetRemoteConfigLongValue(FirebaseRemoteConfigs.ENERGY);
                moneyConfig =  _firebaseManager.GetRemoteConfigLongValue(FirebaseRemoteConfigs.MONEY);
                diamondsConfig = _firebaseManager.GetRemoteConfigLongValue(FirebaseRemoteConfigs.DIAMONDS);

                _slotsManager.InitItems(_closeSlotIds, EItemState.CLOSE);
                _slotsManager.InitItems(_immovableSlotIds, EItemState.IMMOVABLE);
                
                _slotsManager.InitItem(EItemState.OPEN, EMergeType.A_VIDEOCARD, 7);
                _slotsManager.InitItem(EItemState.IMMOVABLE, EMergeType.A_VIDEOCARD, 8);
                //_slotsManager.InitGenerators();

                _salonManager.InitSalons(idleMap, EMapId.M_1);
                _purchaseManager.InitPurchases(purchases);
                _orderManager.InitOrders(orders);
                _gameStorage.SetIsFirstVisit("No");

                SaveSlots();
                SaveOrders();
                SavePurchases();
                SaveSalons();
            }
            else
            {
                var completedOrdersAmount = _gameStorage.CompletedOrdersAmount;
                _screenManager.TopHUDManager.LevelManager.SetCurrentLevel(completedOrdersAmount);
                
                LoadSlots();
                LoadSalons(idleMap);
                LoadOrders(orders);
                LoadPurchases(purchases);
                
                CheckClientOrders();
            }
            
            _gameStorage.SetMoney(moneyConfig);
            _gameStorage.SetEnergy(energyConfig);
            _gameStorage.SetDiamonds(diamondsConfig);
            UIData uiData = new UIData(moneyConfig, diamondsConfig, energyConfig);
            _screenManager.InitUIData(uiData);
        }
        
        private void AddEventListeners()
        {
            _screenManager.GameView.OnClickUpdateSalonLabel += OnUpdateSalonLabel;
            _screenManager.TopHUDManager.TimerManager.OnTimerEnd += OnEnergyTimerEnd;
            _screenManager.TopHUDManager.LevelManager.OnLevelUpdate += OnLevelUpdate;
            
            _screenManager.BottomHUDManager.OnClickIdleButton += OnClickIdle;
            _screenManager.BottomHUDManager.OnClickShopButton += OnClickShop;
            _screenManager.BottomHUDManager.OnClickLipsButton += OnClickLips;
            _screenManager.TopHUDManager.OnClickSettings += OnClickSettings;

            _mergeManager.OnUpdateSlots += SaveSlots;
            _mergeManager.OnGenerateItem += OnGenerateItem;
            _mergeManager.OnClickGenerator += OnClickGenerator;
            _purchaseManager.OnClickItem += OnClickPurchase;
            _purchaseManager.OnPurchaseTimerEnd += OnPurchasesTimerEnd;
            _purchaseManager.OnSpawnFinished += OnSpawnFinished;
            _orderManager.OnClickItem += OnClickOrder;
            _salonManager.OnSalonTimerEnd += OnSalonTimerEnd;
            _salonManager.OnSalonUpdate += OnSalonUpdate;
            _salonManager.OnSalonOpen += OnSalonOpen;
            
            _storePopupView.OnClickBuyComplete += OnClickBuyComplete;
            _storePopupView.OnClickBuyFailed += OnClickBuyFailed;
            _levelUpPopupView.OnClickLookNow += OnClickLookNow;
            _settingsPopupView.OnClickMusic += OnClickMusic;
            _settingsPopupView.OnClickSound += OnClickSound;
            _developmentPopupView.OnClickClear += OnClickClearProgress;
            _developmentPopupView.OnClickSetup += OnClickSetupCurrency;
            _developmentPopupView.OnClickExit += OnClickExit;
            _mistakePopupView.OnClickBuyMore += OnClickBuyMore;

            _purchasesPanelView.OnClickPurchaseTimer += OnClickPurchaseTimer;

            _popupManager.OnShowPopup += OnShowPopup;
            _popupManager.OnHidePopup += OnHidePopup;
            
            _mergeManager.OnMergeItem += OnMergeItem;

            _appLovinMAXManager.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            _mergeManager.OnMergeItemSelect += OnMergeItemSelect;
            
            _openDevelopmentButton.onClick.AddListener(() =>
            {
                if (_counter == 10)
                {
                    _popupManager.ShowPopup(EPopup.DEVELOPMENT);
                    _counter = 0;
                }
                else
                {
                    _counter++;
                }

            });
            
            _openDebugButton.onClick.AddListener(() =>
            {
                if (_counter == 10)
                {
                    _popupManager.ShowPopup(EPopup.DEBUG);
                    _counter = 0;
                }
                else
                {
                    _counter++;
                }

            });
        }
                

        private void RemoveEventListeners()
        {
            _screenManager.GameView.OnClickUpdateSalonLabel -= OnUpdateSalonLabel;
            _screenManager.TopHUDManager.TimerManager.OnTimerEnd -= OnEnergyTimerEnd;
            _screenManager.TopHUDManager.LevelManager.OnLevelUpdate -= OnLevelUpdate;
            
            _screenManager.BottomHUDManager.OnClickIdleButton -= OnClickIdle;
            _screenManager.BottomHUDManager.OnClickShopButton -= OnClickShop;
            _screenManager.BottomHUDManager.OnClickLipsButton -= OnClickLips;
            _screenManager.TopHUDManager.OnClickSettings -= OnClickSettings;

            _mergeManager.OnUpdateSlots -= SaveSlots;
            _mergeManager.OnGenerateItem -= OnGenerateItem;
            _mergeManager.OnClickGenerator -= OnClickGenerator;
            _purchaseManager.OnClickItem -= OnClickPurchase;
            _purchaseManager.OnPurchaseTimerEnd -= OnPurchasesTimerEnd;
            _purchaseManager.OnSpawnFinished -= OnSpawnFinished;
            _orderManager.OnClickItem -= OnClickOrder;
            _salonManager.OnSalonTimerEnd -= OnSalonTimerEnd;
            _salonManager.OnSalonUpdate -= OnSalonUpdate;
            _salonManager.OnSalonOpen -= OnSalonOpen;
            
            _storePopupView.OnClickBuyComplete -= OnClickBuyComplete;
            _storePopupView.OnClickBuyFailed -= OnClickBuyFailed;
            _levelUpPopupView.OnClickLookNow -= OnClickLookNow;
            _settingsPopupView.OnClickMusic -= OnClickMusic;
            _settingsPopupView.OnClickSound -= OnClickSound;
            _developmentPopupView.OnClickClear -= OnClickClearProgress;
            _developmentPopupView.OnClickSetup -= OnClickSetupCurrency;
            _developmentPopupView.OnClickExit -= OnClickExit;
            _mistakePopupView.OnClickBuyMore -= OnClickBuyMore;

            _purchasesPanelView.OnClickPurchaseTimer -= OnClickPurchaseTimer;
            
            _popupManager.OnShowPopup -= OnShowPopup;
            _popupManager.OnHidePopup -= OnHidePopup;
            
            _mergeManager.OnMergeItem -= OnMergeItem;
            
            _appLovinMAXManager.OnRewardedAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent;
            _mergeManager.OnMergeItemSelect -= OnMergeItemSelect;

            _openDevelopmentButton.onClick.RemoveAllListeners();
            _openDebugButton.onClick.RemoveAllListeners();
        }

        private void OnSpawnFinished(PurchasesView purchasesView)
        {
            if (_tutorialStage == ETutorialStage.END || _tutorialStage == ETutorialStage.TIMER)
            {
                _purchaseManager.SetupPurchasesTimer();
            }
        }

        private void OnSalonOpen(ESalonId salonId, int level)
        {
            if (_tutorialStage != ETutorialStage.END)
            {
                _purchaseManager.SetupPurchasesTimer();
            }
            UpdateTutorialStage(ETutorialStage.TIMER);
        }

        private void OnClickBuyMore()
        {
            _storePopupView.Show();
        }

        private void SetupAudio(bool isFirstVisit)
        {
            if (isFirstVisit)
            {
                _gameStorage.Music = 1;
                _gameStorage.Sound = 1;
                
                _audioManager.PlayBackground(EAudio.MAIN_BACKGROUND);
                _audioManager.UnPauseSound();

                _settingsPopupView.MusicSwitch.On();
                _settingsPopupView.SoundSwitch.On();
            }
            else
            {
                var isPlayMusic = _gameStorage.Music == 1;
                var isPlaySound = _gameStorage.Sound == 1;

                if (isPlayMusic)
                {
                    _settingsPopupView.MusicSwitch.On();
                    _audioManager.PlayBackground(EAudio.MAIN_BACKGROUND); 
                }

                if (isPlaySound)
                {
                    _settingsPopupView.SoundSwitch.On();
                    _audioManager.UnPauseSound();
                }
            }

        }

        private void UpdateTutorialStage(ETutorialStage curStage)
        {
            ETutorialStage tutorialStage = (ETutorialStage) _gameStorage.TutorialStage;
            if (curStage == tutorialStage || curStage == ETutorialStage.NONE)
            {
                _tutorialStage = tutorialStage;

                var uncheckedStages = new List<ETutorialStage>() {ETutorialStage.END};
                if (!uncheckedStages.Contains(tutorialStage))
                {
                    ShowFingerByStage(tutorialStage);
                }
            }
        }

        private void ShowFingerByStage(ETutorialStage tutorialStage)
        {
            var fingerType = _tutorialManager.GetFingerByStage(tutorialStage);
            _tutorialManager.ShowFinger(fingerType);

            var freezedFingers = new List<EFinger>()
            {
                EFinger.SMALL_TIMER, EFinger.TIMER,
                EFinger.COMPLETED_ORDERS, EFinger.COMPLETED_SMALL_ORDERS,
                EFinger.SHOW_ORDERS, EFinger.SHOW_SMALL_ORDERS
            };
            if (freezedFingers.Contains(fingerType))
            {
                _mergeManager.IsFreezed = true;
            }
        }

        private void GoToNextTutorialStage(ETutorialStage curStage, ETutorialStage nextStage)
        {
            if (curStage != ETutorialStage.END && _tutorialStage == curStage)
            {
                var fingerType = _tutorialManager.GetFingerByStage(_tutorialStage);
                _tutorialManager.HideFinger(fingerType);
                _gameStorage.TutorialStage = (int) nextStage;
                _tutorialStage = nextStage;
                
                var freezedFingers = new List<EFinger>()
                {
                    EFinger.PURCHASE, EFinger.SMALL_PURCHASE,
                    EFinger.COMPLETED_ORDERS, EFinger.COMPLETED_SMALL_ORDERS,
                    EFinger.SHOW_ORDERS, EFinger.SHOW_SMALL_ORDERS,
                    EFinger.SMALL_TIMER, EFinger.TIMER
                };
                
                if (freezedFingers.Contains(fingerType) && _tutorialStage != ETutorialStage.TIMER)
                {
                    _mergeManager.IsFreezed = false;
                }
            }
        }

        private void OnClickBuyFailed(Product product, PurchaseFailureReason purchaseFailureReason)
        {
            
        }

        private void OnClickBuyComplete(Product product)
        {
            var definition = product.definition;

            if (_gameStorage.IsFirstPurchase)
            {
                _gameStorage.SetIsFirstPurchase("No");
            }

            switch (definition.id)
            {
                case InAppProductId.DOLLAR_100:
                {
                    AddCurrency(ECurrency.MONEY, 100);
                }
                    break;
                case InAppProductId.DOLLAR_1000:
                {
                    AddCurrency(ECurrency.MONEY, 1000);
                }
                    break;
                case InAppProductId.DOLLAR_1500:
                {
                    AddCurrency(ECurrency.MONEY, 1500);
                }
                    break;
                case InAppProductId.RUBY_5600:
                {
                    AddCurrency(ECurrency.DIAMONDS, 5600);
                }
                    break;
                case InAppProductId.RUBY_8200:
                {
                    AddCurrency(ECurrency.DIAMONDS, 8200);
                }
                    break;
                case InAppProductId.RUBY_10000:
                {
                    AddCurrency(ECurrency.DIAMONDS, 10000);
                }
                    break;
                case InAppProductId.PURCHASE_1:
                {
                    var generatorType = _purchaseManager.GetRandomPurchaseType();
                    var generator = new SpawnItemData(EItemState.OPEN, EMergeCategory.GENERATOR, generatorType, false, 1000, 0, -2);
                    _slotsManager.PlaceItem(generator);
                }
                    break;
            }
        }

        private void OnClickSetupCurrency(List<InputRowData> inputRowData)
        {
            foreach (var inputRow in inputRowData)
            {
                int value = int.Parse(inputRow.Value);
                switch (inputRow.InputRowType)
                {
                    case EInputRow.Energy:
                    {
                        _gameStorage.SetEnergy(value);
                        _screenManager.TopHUDManager.SetEnergy(value);
                    }
                        break;
                    case EInputRow.Money:
                    {
                        _gameStorage.SetCurrencyValue(ECurrency.MONEY, value);
                        _screenManager.TopHUDManager.SetMoney(value);
                    }
                        break;
                    case EInputRow.Diamond:
                    {
                        _gameStorage.SetCurrencyValue(ECurrency.DIAMONDS, value);
                        _screenManager.TopHUDManager.SetDiamonds(value);
                    }
                        break;
                }
            }
        }

        private void OnClickClearProgress()
        {
            _gameStorage.Clear();
        }
        
        private void OnClickExit()
        {
            _popupManager.HidePopup(EPopup.DEVELOPMENT);
            Application.Quit();
        }


        private void OnClickSound(bool state)
        {
            if (state)
            {
                _audioManager.UnPauseSound();
                _gameStorage.Sound = 1;
            }
            else
            {
                _audioManager.PauseSound();
                _gameStorage.Sound = 0;
            }
        }

        private void OnClickMusic(bool state)
        {
            if (state)
            {
                _audioManager.PlayBackground(EAudio.MAIN_BACKGROUND);
                _gameStorage.Music = 1;
            }
            else
            {
                _audioManager.PauseMusic();
                _gameStorage.Music = 0;
            }
        }

        private void OnMergeItemSelect()
        {
            GoToNextTutorialStage(ETutorialStage.MERGE, ETutorialStage.PURCHASE);
            UpdateTutorialStage(ETutorialStage.PURCHASE);
        }

        private void SetupGameTimer(float duration)
        {
            _lastGameTimeMinute = _gameStorage.GameTimeMinutes;
            
            _coroutineManager.SetImmediate(duration, true, () =>
            {
                _gameStorage.GameTimeSeconds++;

                if (_gameStorage.GameTimeSeconds % 60 == 0 && _gameStorage.GameTimeSeconds / 60 > _lastGameTimeMinute)
                {
                    int time = _gameStorage.GameTimeMinutes;
                    _lastGameTimeMinute = time;
                    Debug.Log($"Game time (min): { _gameStorage.GameTimeMinutes}");
                }
                //Debug.Log($"Game time (sec): { _gameStorage.GameTimeSeconds}");
            });
        }

        private void SetupHintTimer(float duration)
        {
            _coroutineManager.SetImmediate(duration, true, () =>
            {
                var slots = _slotsManager.Slots;
                var duplicateItems = _slotsManager.GetDuplicateItemsSlots(slots);

                if (duplicateItems.Count > 1)
                {
                    // TODO
                    _mergeManager.ShowMergeHint(duplicateItems[0], duplicateItems[1]);
                }  
            });
        }
        
        private void OnClickMoreCurrencyWatchNow()
        {
            _adType = AppLovinMAXConst.MORE_CURRENCY;
            _appLovinMAXManager.ShowRewardedAd();
        }

        private void OnEnergyTimerEnd(int amount)
        {
            AddEnergy(amount);
            _screenManager.TopHUDManager.RestartEnergyTimer(ENERGY_GENERATOIN_TIME);
        }

        private void OnUpdateSalonLabel()
        {
            ShowIdleScreen();
            GoToNextTutorialStage(ETutorialStage.UPDATE_SALON, ETutorialStage.TIMER);
        }
        
        private void OnClickLips()
        {
            _moreCurrencyPopupView.Show();
            _moreCurrencyPopupView.OnClickWatchNow += OnClickMoreCurrencyWatchNow;
        }
        
        private void OnRewardedAdReceivedRewardEvent(string arg1, MaxSdkBase.Reward arg2, MaxSdkBase.AdInfo arg3)
        {
            switch (_adType)
            {
                case AppLovinMAXConst.PURCHASE_TIMER:
                {
                    var type = _purchasesPanelView.PurchaseType;
                    _purchaseManager.ReplaceByNextItems(type);
                }
                break;
                case AppLovinMAXConst.REWARD_PURCHASE:
                {
                    if (_purchaseView != null)
                    {
                        SpawnPurchase(_purchaseView);
                        _purchaseView = null;
                    }
                }
                break;
                case AppLovinMAXConst.MORE_CURRENCY:
                {
                    var second = _firebaseManager.GetRemoteConfigLongValue(FirebaseRemoteConfigs.MORE_CURRENCY_IDLE_TIME);
                    var income = _salonManager.GetOpenSalonsPassiveIncomesSum();
                    AddCurrency(ECurrency.MONEY, second * income);
                }
                break;
            }

            int time = _gameStorage.GameTimeMinutes;
            _firebaseManager.SendEvent($"{_adType}_{time}");
            
            SavePurchases();
            _adType = "";
            _appLovinMAXManager.LoadRewardedAd();
        }
        
        private void OnClickGenerator(EMergeType obj)
        {
            int energyAmount = _gameStorage.Energy;
            if (energyAmount <= 0)
            {
                _popupManager.ShowPopup(EPopup.MISTAKE);
            }
        }
        
        private void OnMergeItem()
        {
            _audioManager.PlayEffect(EAudio.MERGE);
            CheckClientOrders();
            SaveSlots();
        }
        
        private void CheckClientOrders()
        {
            var orderClients = _orderManager.OrderViews;
            foreach (var orderView in orderClients.Values)
            {
                CheckClientOrderItems(orderView);
            }
        }

        private void CheckClientOrderItems(OrderView orderView)
        {
            var orderItems = orderView.OrderClient.orderItems;
            List<OrderItem> currentOrders = new List<OrderItem>();
            
            int counter = 0;
            for (int i = 0; i < orderItems.Count; i++)
            {
                var orderItem = orderItems[i];
                orderItem.amount = 1;

                int currentOrderAmount = 
                    currentOrders.FindAll(x => x.mergeType == orderItem.mergeType && x.level == orderItem.level).Count;

                orderItem.amount += currentOrderAmount;
                
                bool isOrderItemEnough = IsOrderItemEnough(orderItem);
                if(isOrderItemEnough) counter++;
                orderView.OrderItemViews[i].SetGreenTickState(isOrderItemEnough);
                
                currentOrders.Add(orderItem);
            }

            if (counter == orderItems.Count)
            {
                orderView.SetButton(EButton.SUBMIT);
                UpdateTutorialStage(ETutorialStage.COMPLETED_ORDER);
            }
        }

        private bool IsOrderItemEnough(OrderItem orderItem)
        {
            var orderLevel = orderItem.level;
            var orderType = orderItem.mergeType;
            var itemsOnField = _slotsManager.GetOpenSlots(orderType, orderLevel);
            bool isOrderItemEnough = orderItem.amount <= itemsOnField.Count;
            return isOrderItemEnough;
        }

        private void OnShowPopup(EPopup popup)
        {
            var tutorialStage = _gameStorage.TutorialStage;
            if (tutorialStage == (int)ETutorialStage.END)
            {
                _mergeManager.IsFreezed = true;
            }
          
            GoToNextTutorialStage(ETutorialStage.SHOW_ORDER, ETutorialStage.COMPLETED_ORDER);
        }

        private void OnHidePopup(EPopup popup)
        {
            var tutorialStage = _gameStorage.TutorialStage;
            if (tutorialStage == (int)ETutorialStage.END)
            {
                _mergeManager.IsFreezed = false;
            }
        }
        
        private void OnClickPurchaseTimer()
        {
            _adType = AppLovinMAXConst.PURCHASE_TIMER;
            
            if (_tutorialStage == ETutorialStage.TIMER)
            {
                GoToNextTutorialStage(ETutorialStage.TIMER, ETutorialStage.END);
                OnRewardedAdReceivedRewardEvent(null, new MaxSdkBase.Reward(), null);
            }
            else
            {
                _appLovinMAXManager.ShowRewardedAd();
            }
        }
        
        private void OnClickLookNow()
        {
            ShowIdleScreen();
        }

        private void OnSalonUpdate(ESalonId id, int level)
        {
            _audioManager.PlayEffect(EAudio.IDLE_UPGRADE);
            _purchaseManager.UpdatePurchasePrice(EPurchaseId.POS_1);
            SetUpdateSalonLabelsState(false);
            
            var salon = Utils.GetSalonById(id);
            var salonLevelData = salon.salonLevels.FirstOrDefault(x => x.level == level);
            
            int time = _gameStorage.GameTimeMinutes;
            _firebaseManager.SendEvent($"salon_{(int)id}_{time}");
            
            CongratulationPopupView view = (CongratulationPopupView) _popupManager.ShowPopup(EPopup.CONGRATULATION);
            view.MoneyValue = $"{salonLevelData?.amountPerSecond}";
        }

        private void OnLevelUpdate(int newLevel)
        {
            _salonManager.OpenSalon((ESalonId)newLevel);
            _orderManager.AddItem();

            int time = _gameStorage.GameTimeMinutes;
            _firebaseManager.SendEvent($"general_{time}");
            
            LevelUpPopupView view = (LevelUpPopupView)_popupManager.ShowPopup(EPopup.LEVEL_UP);
            view.Level = newLevel.ToString();
        }

        private void OnSalonTimerEnd(SalonView salonView, float amount)
        {
            AddCurrency(salonView.Currency, amount);
        }

        private void OnClickSettings()
        {
            _popupManager.ShowPopup(EPopup.SETTINGS);
        }

        private void OnClickShop()
        {
            _popupManager.ShowPopup(EPopup.STORE);
        }

        private void OnClickIdle()
        {
            var currentView = _navigationManager.CurrentView;
            if (currentView.ViewType == EView.CORE || currentView.ViewType == EView.CORE_GAME_OBJECT)
            {
                ShowIdleScreen();

                if (_tutorialStage == ETutorialStage.UPDATE_SALON)
                {
                    GoToNextTutorialStage(ETutorialStage.UPDATE_SALON, ETutorialStage.TIMER);
                }
            }
            else
            {
                ShowMergeScreen();
            }
        }

        private void ShowIdleScreen()
        {
            _navigationManager.HideView(EView.CORE);
            _navigationManager.HideView(EView.CORE_GAME_OBJECT);
            _navigationManager.ShowView(EView.IDLE);
        }
        
        private void ShowMergeScreen()
        {
            _navigationManager.ShowView(EView.CORE);
            _navigationManager.ShowView(EView.CORE_GAME_OBJECT);
            _navigationManager.HideView(EView.IDLE);
        }

        private void OnClickOrder(OrderClient orderClient)
        {
            bool canOrder = true;
            EMergeType notEnoughOrderMergeType = EMergeType.A_VIDEOCARD;

            foreach (var orderItem in orderClient.orderItems)
            {
                bool isOrderItemEnough = IsOrderItemEnough(orderItem);
                if (!isOrderItemEnough)
                {
                    canOrder = false;
                    notEnoughOrderMergeType = orderItem.mergeType;
                    break;
                }
            }

            if (canOrder)
            {
                _audioManager.PlayEffect(EAudio.ORDER_COMPLETED);
                GoToNextTutorialStage(ETutorialStage.COMPLETED_ORDER, ETutorialStage.UPDATE_SALON);

                foreach (var orderItem in orderClient.orderItems)
                {
                    var orderLevel = orderItem.level;
                    var orderType = orderItem.mergeType;
                    var orderAmount = orderItem.amount;
                    _slotsManager.DeleteSlotsItems(orderType, orderLevel, orderAmount);
                }
            
                _orderManager.DeleteById(orderClient.orderId);
                
                var salonData = _salonManager.GetSalonByType(orderClient.salonData.id);
                _orderManager.SpawnNextOrder(salonData);
                
                _gameStorage.IncCompletedOrdersAmount();
                
                var completedOrdersAmount = _gameStorage.CompletedOrdersAmount;
                _screenManager.TopHUDManager.LevelManager.UpdateCurrentLevel(completedOrdersAmount);

                var orderExp = _orderManager.GetOrderItemsExp(orderClient.orderItems);
                _salonManager.UpdateSalon(orderClient.salonData.id, orderExp);
                
                SaveSlots();
                SaveOrders();
                SaveSalons();
            }
            else
            {
                ToolsPopupView view = (ToolsPopupView)_popupManager.ShowPopup(EPopup.TOOLS);
                view.Setup(notEnoughOrderMergeType);
            }
            
            var isNewLevel = _salonManager.IsNewLevelSalon();
            if (isNewLevel)
            {
                SetUpdateSalonLabelsState(true);
                UpdateTutorialStage(ETutorialStage.UPDATE_SALON);
            }

            _checkClientOrdersCoroutine = _coroutineManager.SetTimeout(0.1f, () =>
            {
                CheckClientOrders();
                _coroutineManager.Stop(_checkClientOrdersCoroutine);
            });
        }
    
        private void OnPurchasesTimerEnd(EPurchaseType type)
        {
            _purchaseManager.ReplaceByNextItems(type);
            SavePurchases();
        }
    
        private void OnClickPurchase(PurchaseView purchaseView)
        {
            if(_tutorialStage == ETutorialStage.MERGE) return;
            
            GoToNextTutorialStage(ETutorialStage.PURCHASE, ETutorialStage.GENERATOR);
            UpdateTutorialStage(ETutorialStage.GENERATOR);
            
            if (purchaseView.PurchaseCategory == EPurchaseCategory.REWARD)
            {
                TrySpawnAdsPurchase(purchaseView);
            }
            else
            {
                TrySpawnPurchase(purchaseView);
            }

            SaveSlots();
            SavePurchases();
        }
        
        private void TrySpawnPurchase(PurchaseView purchaseView)
        {
            var currency = purchaseView.Currency;
            var price = purchaseView.Price;
            var currencyAmount = _gameStorage.GetCurrencyValue(currency);

            if (currencyAmount >= price)
            {
                SpawnPurchase(purchaseView);
                AddCurrency(currency, -price);
            }
        }
        
        private void TrySpawnAdsPurchase(PurchaseView purchaseView)
        {
            _purchaseView = purchaseView;
            
            _adType = AppLovinMAXConst.REWARD_PURCHASE;
            _appLovinMAXManager.ShowRewardedAd();
        }

        private void SpawnPurchase(PurchaseView purchaseView)
        {
            var level = purchaseView.Level;
            var mergeType = purchaseView.MergeType;
            
            var amountOfItems = purchaseView.AmountOfItems;
            var isMergeable = purchaseView.IsMergeable;

            var generator = new SpawnItemData(EItemState.OPEN, EMergeCategory.GENERATOR, mergeType, isMergeable, amountOfItems, level - 1, -2);
            _slotsManager.PlaceItem(generator);
        }

        private void AddCurrency(ECurrency currency, float price)
        {
            var newValue = _gameStorage.GetCurrencyValue(currency) + price;
            _gameStorage.SetCurrencyValue(currency, newValue);
            _screenManager.TopHUDManager.SetCurrencyValue(currency, newValue);
        }
    
        private void AddEnergy(int value)
        {
            var newEnergy = _gameStorage.Energy + value;
            _gameStorage.SetEnergy(newEnergy);
            _screenManager.TopHUDManager.SetEnergy(newEnergy);
        }

        private void OnGenerateItem(EMergeType mergeType)
        {
            GoToNextTutorialStage(ETutorialStage.GENERATOR, ETutorialStage.SHOW_ORDER);
            UpdateTutorialStage(ETutorialStage.SHOW_ORDER);
            AddEnergy(-1);
            CheckClientOrders();
        }

        private void SetUpdateSalonLabelsState(bool state)
        {
            _screenManager.GameView.UpdateSalonLabel.gameObject.SetActive(state);
            _screenManager.BottomHUDManager.IdleButton.SetBag(state);
        }

        private void SaveSalons()
        {
            Dictionary<ESalonId, SalonView> salonViews = _salonManager.SalonViews;
            Dictionary<ESalonId, SalonView> newSalonViews = new Dictionary<ESalonId, SalonView>(salonViews);
            _gameStorage.SetSalonsValue(newSalonViews);
        }
    
        private void LoadSalons(Transform salons)
        {
            List<SalonItem> salonItems = _gameStorage.GetSalonsValue();
            if (salonItems.Count > 0)
            {
                _salonManager.SetupSalons(salons, salonItems);
            }
        }
    
        private void SaveOrders()
        {
            Dictionary<EOrderId, OrderView> orderViews = _orderManager.OrderViews;
            Dictionary<EOrderId, OrderView> newOrderViews = new Dictionary<EOrderId, OrderView>(orderViews);
            _gameStorage.SetOrderClientsValue(newOrderViews);
        }
    
        private void LoadOrders(Transform salons)
        {
            List<OrderClient> orderClients = _gameStorage.GetOrderClientsValue();
            if (orderClients.Count > 0)
            {
                _orderManager.SetupOrders(salons, orderClients);
            }
        }
    
        private void SavePurchases()
        {
            Dictionary<EPurchaseId, PurchaseView> purchaseViews = _purchaseManager.PurchaseViews;
            Dictionary<EPurchaseId, PurchaseView> newPurchaseViews = new Dictionary<EPurchaseId, PurchaseView>(purchaseViews);
            _gameStorage.SetPurchasesValue(newPurchaseViews);
        }
    
        private void LoadPurchases(Transform purchases)
        {
            List<PurchaseItem> purchaseItems = _gameStorage.GetPurchasesValue();
            if (purchaseItems.Count > 0)
            {
                _purchaseManager.SetupPurchases(purchases, purchaseItems);
            }
        }

        private void SaveSlots()
        {
            List<Slot> slots = _slotsManager.Slots;
            List<Slot> newList = new List<Slot>(slots);
            _gameStorage.SetSlotsValue(newList);
        }
    
        private void LoadSlots()
        {
            List<SlotInStorage> slotInStorages = _gameStorage.GetSlotsValue();
            if (slotInStorages.Count > 0)
            {
                for (int i = 0; i < slotInStorages.Count; i++)
                {
                    SlotInStorage item = slotInStorages[i];
                    var slotInStorage = new SpawnItemData(item.state, item.category, item.type, item.isMergeable, item.amountOfItems, item.id, item.slotId);
                    _slotsManager.PlaceItem(slotInStorage);
                }
            }
        }

        public void OnDestroy()
        {
            RemoveEventListeners();
            _appLovinMAXManager.InitializeRewardedAds();
            _firebaseManager.OnFetchedRemoteConfigs -= OnFetchedRemoteConfigs;
        }
    }
}