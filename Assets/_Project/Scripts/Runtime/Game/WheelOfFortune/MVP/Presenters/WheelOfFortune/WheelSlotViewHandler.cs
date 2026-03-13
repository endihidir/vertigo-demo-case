using System;
using Game.Configs;
using Game.Data;
using Game.Enums;
using Game.Factories;
using Game.Views;

namespace Game.Handlers
{
    public sealed class WheelSlotViewHandler : IWheelSlotViewHandler
    {
        private readonly ISlotViewFactory _slotViewFactory;
        private readonly WheelOfFortuneConfigContainerSO _wheelConfigContainer;
        private readonly RewardVisualConfigContainerSO _rewardVisualContainer;
        public WheelSlotView[] WheelSlotViews { get; private set; } = Array.Empty<WheelSlotView>();

        public WheelSlotViewHandler(ISlotViewFactory slotViewFactory, WheelOfFortuneConfigContainerSO wheelConfigContainer, RewardVisualConfigContainerSO rewardVisualContainer)
        {
            _slotViewFactory = slotViewFactory;
            _wheelConfigContainer = wheelConfigContainer;
            _rewardVisualContainer = rewardVisualContainer;
        }

        public WheelVisualData GetWheelVisuals(WheelType wheelType) => _wheelConfigContainer.GetWheelConfig(wheelType).WheelVisuals;
        
        public void PopulateSlotViews(WheelType wheelType, out WheelSlotView[] wheelSlotViews)
        {
            var wheelConfig = _wheelConfigContainer.GetWheelConfig(wheelType);
            
            if (WheelSlotViews.Length == wheelConfig.WheelSlotData.Count)
            {
                UpdateSlotViews(wheelConfig, out wheelSlotViews);
            }
            else
            {
                CreateSlotViews(wheelConfig, out wheelSlotViews);
            }
        }

        private void UpdateSlotViews(WheelConfigSO wheelConfig, out WheelSlotView[] wheelSlotViews)
        {
            for (var i = 0; i < WheelSlotViews.Length; i++)
            {
                var slotData = wheelConfig.WheelSlotData[i];
                
                var slotView = WheelSlotViews[i];
                
                PrepareSlotView(slotData, slotView);
            }

            wheelSlotViews = WheelSlotViews;
        }

        private void CreateSlotViews(WheelConfigSO wheelConfig, out WheelSlotView[] wheelSlotViews)
        {
            ResetSlotViews();
            
            var lenght = wheelConfig.WheelSlotData.Count;
            
            wheelSlotViews = new WheelSlotView[lenght];

            for (var i = 0; i < lenght; i++)
            {
                var slotData = wheelConfig.WheelSlotData[i];

                var slotView = _slotViewFactory.GetSlot<WheelSlotView>();

                PrepareSlotView(slotData, slotView);

                wheelSlotViews[i] = slotView;
            }
        }

        private void PrepareSlotView(WheelSlotData slotData, WheelSlotView slotView)
        {
            var rewardId = slotData.RewardDefinition.Id;

            var rewardVisualData = _rewardVisualContainer.GetVisualData(rewardId);
                
            slotView.SetSlotIndex(slotData.SlotIndex);
            
            slotView.SetImage(rewardVisualData.Icon);

            var baseValue = slotData.RewardDefinition.BaseValue;
            
            slotView.SetValue($"X{baseValue}");
            
            var isUniqueItem = slotData.RewardDefinition.IsUniqueItem;
            
            slotView.SetActiveValueTxt(!isUniqueItem);
        }

        private void ResetSlotViews()
        {
            foreach (var view in WheelSlotViews)
            {
                _slotViewFactory.ReleaseSlot(view);
            }
        }
    }
}