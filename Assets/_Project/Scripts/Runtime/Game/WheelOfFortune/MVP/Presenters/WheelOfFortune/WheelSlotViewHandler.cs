using Game.Configs;
using Game.Enums;
using Game.Factories;
using Game.Views;

namespace Game.Handlers
{
    public sealed class WheelSlotViewHandler : IWheelSlotViewHandler
    {
        private readonly ISlotViewFactory _slotViewFactory;
        private readonly WheelOfFortuneConfigContainerSO _configContainer;
        public WheelSlotView[] WheelSlotViews { get; private set; }

        public WheelSlotViewHandler(ISlotViewFactory slotViewFactory, WheelOfFortuneConfigContainerSO configContainer)
        {
            _slotViewFactory = slotViewFactory;
            _configContainer = configContainer;
        }

        public WheelVisualData GetWheelVisuals(WheelType wheelType) => _configContainer.GetWheelConfig(wheelType).WheelVisuals;
        
        public void PopulateSlotViews(WheelType wheelType, out WheelSlotView[] wheelSlotViews)
        {
            var wheelConfig = _configContainer.GetWheelConfig(wheelType);
            
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

                slotView.ApplyData(slotData.SlotIndex, slotData.RewardDefinition);
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

                slotView.ApplyData(slotData.SlotIndex, slotData.RewardDefinition);
                
                wheelSlotViews[i] = slotView;
            }
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