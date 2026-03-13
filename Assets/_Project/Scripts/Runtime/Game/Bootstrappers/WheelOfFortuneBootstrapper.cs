using Game.Configs;
using Game.Data;
using Game.Factories;
using Game.Handlers;
using Game.Presenters;
using Game.Views;
using UnityEngine;

namespace Game.Installers
{
    public class WheelOfFortuneBootstrapper : MonoBehaviour 
    {
        [field: SerializeField] private WheelPanelView WheelPanelView { get; set; }
        
        public void Initialize(ISlotViewFactory slotViewFactory, WheelOfFortuneConfigContainerSO wheelConfigContainer, RewardVisualConfigContainerSO rewardVisualContainer)
        {
            var wheelSlotViewHandler = new WheelSlotViewHandler(slotViewFactory, wheelConfigContainer, rewardVisualContainer);
            var wheelOfFortunePresenter = new WheelPanelPresenter(WheelPanelView, wheelSlotViewHandler);
            WheelPanelView.Initialize();
        }
    }
}