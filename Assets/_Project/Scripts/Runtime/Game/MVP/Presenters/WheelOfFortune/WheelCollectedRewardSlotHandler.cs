using System;
using Game.Configs;
using Game.Data;
using Game.Factories;
using Game.Views;

namespace Game.Handlers
{
    public sealed class WheelCollectedRewardSlotHandler : IWheelCollectedRewardSlotHandler
    {
        private readonly ISlotViewFactory _slotViewFactory;
        private readonly RewardVisualConfigContainerSO _rewardVisualContainer;
        private readonly WheelOfFortuneConfigContainerSO  _wheelOfFortuneConfigContainer;
        private readonly IWheelRewardDatabase _rewardDatabase;
        public WheelCollectedRewardSlotView[] CollectedRewardSlotViews { get; private set; } = Array.Empty<WheelCollectedRewardSlotView>();

        public WheelCollectedRewardSlotHandler(ISlotViewFactory slotViewFactory, RewardVisualConfigContainerSO rewardVisualContainer, 
            WheelOfFortuneConfigContainerSO wheelOfFortuneConfigContainer, IWheelRewardDatabase wheelRewardDatabase)
        {
            _slotViewFactory = slotViewFactory;
            _rewardVisualContainer = rewardVisualContainer;
            _wheelOfFortuneConfigContainer = wheelOfFortuneConfigContainer;
            _rewardDatabase = wheelRewardDatabase;
        }
        
        public void PopulateSlotViews(out WheelCollectedRewardSlotView[] wheelSlotViews)
        {
            ResetSlotViews();

            wheelSlotViews = new WheelCollectedRewardSlotView[_rewardDatabase.RewardEntries.Count];

            for (var i = 0; i < _rewardDatabase.RewardEntries.Count; i++)
            {
                var rewardEntry = _rewardDatabase.RewardEntries[i];
                
                var slotView = _slotViewFactory.GetSlot<WheelCollectedRewardSlotView>();

                var rewardVisualData = _rewardVisualContainer.GetVisualData(rewardEntry.id);

                var wheelSlotData = _wheelOfFortuneConfigContainer.GetSlotDataById(rewardEntry.id);
                
                slotView.SetImage(rewardVisualData.Icon);
                
                slotView.SetValue(wheelSlotData.GetValueFormat(rewardEntry.amount));
                
                slotView.SetActiveValueTxt(!wheelSlotData.RewardDefinition.IsUniqueItem);
                
                wheelSlotViews[i] = slotView;
            }
            
            CollectedRewardSlotViews = wheelSlotViews;
        }

        private void ResetSlotViews()
        {
            foreach (var view in CollectedRewardSlotViews)
            {
                _slotViewFactory.ReleaseSlot(view);
            }
        }
    }
}