using System;
using Game.Factories;
using Game.Views;

namespace Game.Presenters
{
    public sealed class WheelPanelPresenter : IDisposable
    {
        private readonly IWheelPanelView _wheelPanelView;
        private readonly ISlotViewFactory _slotViewFactory;
        
        public WheelPanelPresenter(IWheelPanelView view, ISlotViewFactory slotViewFactory)
        {
            _wheelPanelView = view;
            _slotViewFactory = slotViewFactory;
            _wheelPanelView.OnInitialize += OnViewInitialized;
        }

        private void OnViewInitialized()
        {
            
        }

        public void SettleRewardView(WheelRewardSlotView rewardSlotView)
        {
            foreach (var rewardHolder in _wheelPanelView.RewardHolders)
            {
                if(rewardHolder.SlotIndex != rewardSlotView.SlotIndex) continue;
                
                rewardHolder.SetRewardSlotView(rewardSlotView);
            }
        }

        public void Dispose()
        {
            _wheelPanelView.OnInitialize -= OnViewInitialized;
        }
    }
}
