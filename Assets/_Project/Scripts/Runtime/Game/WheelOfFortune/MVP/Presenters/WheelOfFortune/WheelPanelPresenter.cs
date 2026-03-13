using System;
using Game.Handlers;
using Game.Views;

namespace Game.Presenters
{
    public sealed class WheelPanelPresenter : IDisposable
    {
        private readonly IWheelPanelView _wheelPanelView;
        private readonly IWheelSlotViewHandler _wheelSlotViewHandler;
        
        public WheelPanelPresenter(IWheelPanelView view, IWheelSlotViewHandler wheelSlotViewHandler)
        {
            _wheelPanelView = view;
            _wheelSlotViewHandler = wheelSlotViewHandler;
            _wheelPanelView.OnInitialize += OnViewInitialized;
        }

        private void OnViewInitialized()
        {
            
        }

        public void SettleRewardView(WheelSlotView slotView)
        {
            foreach (var rewardHolder in _wheelPanelView.RewardHolders)
            {
                if(rewardHolder.SlotIndex != slotView.SlotIndex) continue;
                
                rewardHolder.SetRewardSlotView(slotView);
            }
        }

        public void Dispose()
        {
            _wheelPanelView.OnInitialize -= OnViewInitialized;
        }
    }
}