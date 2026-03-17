using Core.SaveSystem;
using Game.Configs;
using Game.Data;
using Game.Factories;
using Game.Handlers;
using Game.Models;
using Game.Presenters;
using Game.Views;
using UnityEngine;

namespace Game.Installers
{
    public class WheelOfFortuneBootstrapper : MonoBehaviour 
    {
        [field: SerializeField] private WheelOfFortuneView WheelOfFortuneView { get; set; }
        
        private WheelOfFortunePresenter _wheelOfFortunePresenter;
        
        public void Initialize(ISlotViewFactory slotViewFactory, WheelOfFortuneConfigContainerSO wheelConfigContainer, RewardVisualConfigContainerSO rewardVisualContainer)
        {
            IJsonSaveService jsonSaveService = new JsonSaveService();
            IWheelRewardDatabase rewardDatabase = new WheelRewardDatabase(jsonSaveService);
            IWheelZoneModel wheelZoneModel = new WheelZoneModel(wheelConfigContainer);
            
            IWheelRewardProvider rewardProvider = new WheelRewardProvider(wheelConfigContainer, rewardVisualContainer);
            IWheelSpinResolver wheelSpinResolver = new WheelSpinResolver(wheelConfigContainer);
            
            IWheelSlotViewHandler wheelSlotViewHandler = new WheelSlotViewHandler(slotViewFactory, wheelConfigContainer, rewardVisualContainer);
            IWheelCollectedRewardSlotHandler collectedRewardSlotHandler = new WheelCollectedRewardSlotHandler(slotViewFactory, rewardVisualContainer, wheelConfigContainer, rewardDatabase);
            
            _wheelOfFortunePresenter = new WheelOfFortunePresenter(wheelZoneModel, WheelOfFortuneView, wheelSlotViewHandler, rewardProvider, rewardDatabase, wheelSpinResolver, collectedRewardSlotHandler);
        }
        
        private void OnDestroy()
        {
            _wheelOfFortunePresenter?.Dispose();
        }
    }
}