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
            IWheelZoneModel wheelZoneModel = new WheelZoneModel();
            IWheelRewardCalculator rewardCalculator = new WheelRewardCalculator(wheelConfigContainer);
            IWheelRewardDatabase rewardDatabase = new WheelRewardDatabase();
            
            var wheelSlotViewHandler = new WheelSlotViewHandler(slotViewFactory, wheelConfigContainer, rewardVisualContainer);
            _wheelOfFortunePresenter = new WheelOfFortunePresenter(wheelZoneModel, WheelOfFortuneView, wheelSlotViewHandler, rewardCalculator, rewardDatabase);
            WheelOfFortuneView.Initialize();
        }


        private void OnDestroy()
        {
            _wheelOfFortunePresenter?.Dispose();
        }
    }
}