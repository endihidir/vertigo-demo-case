using System;
using Game.Configs;
using Game.Data;
using Game.Enums;
using Game.Factories;
using Game.Views;
using UnityEngine;

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
                
                PrepareSlotView(slotData, slotView, wheelConfig.BombIcon);
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

                PrepareSlotView(slotData, slotView, wheelConfig.BombIcon);

                wheelSlotViews[i] = slotView;
            }

            WheelSlotViews = wheelSlotViews;
        }

        private void PrepareSlotView(WheelSlotData slotData, WheelSlotView slotView, Sprite bombIcon)
        {
            var rewardDefinition = slotData.RewardDefinition;

            var rewardVisualData = _rewardVisualContainer.GetVisualData(rewardDefinition.Id);
            
            var isBomb = slotData.IsBomb;
            var icon = isBomb ? bombIcon : rewardVisualData.Icon;
            slotView.SetImage(icon);
            slotView.SetSlotIndex(slotData.SlotIndex);
            
            var isUniqueItem = rewardDefinition.IsUniqueItem;
            slotView.SetActiveValueTxt(!isBomb && !isUniqueItem);
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