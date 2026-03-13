using System;
using Game.Enums;
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
            var wheelVisuals = _wheelSlotViewHandler.GetWheelVisuals(WheelType.Silver);
            
            _wheelPanelView.UpdateWheelVisuals(wheelVisuals);
            
            _wheelSlotViewHandler.PopulateSlotViews(WheelType.Silver, out var views);

            foreach (var wheelSlotView in views)
            {
                SettleRewardView(wheelSlotView);
            }
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