using System;
using Cysharp.Threading.Tasks;
using Game.Data;
using Game.Handlers;
using Game.Models;
using Game.Views;
using Random = UnityEngine.Random;

namespace Game.Presenters
{
    public sealed class WheelOfFortunePresenter : IDisposable
    {
        private readonly IWheelZoneModel _wheelZoneModel;
        private readonly IWheelOfFortuneView _wheelOfFortuneView;
        private readonly IWheelSlotViewHandler _wheelSlotViewHandler;
        private readonly IWheelRewardCalculator _wheelRewardCalculator;
        private readonly IWheelRewardDatabase _wheelRewardDatabase;
        
        public WheelOfFortunePresenter(IWheelZoneModel wheelZoneModel, IWheelOfFortuneView wheelOfFortuneView, IWheelSlotViewHandler wheelSlotViewHandler, 
            IWheelRewardCalculator rewardCalculator, IWheelRewardDatabase rewardDatabase)
        {
            _wheelZoneModel = wheelZoneModel;
            _wheelOfFortuneView = wheelOfFortuneView;
            _wheelSlotViewHandler = wheelSlotViewHandler;
            _wheelRewardCalculator = rewardCalculator;
            _wheelRewardDatabase = rewardDatabase;
            
            _wheelOfFortuneView.OnInitialize += OnViewInitialized;
            _wheelZoneModel.OnZoneUpdate += OnZoneUpdated;
            _wheelOfFortuneView.SpinButton.onClick.AddListener(OnClickSpinButton);
        }

        private void OnClickSpinButton()
        {
            PlaySpinAnimation().Forget();
        }

        private async UniTask PlaySpinAnimation()
        {
            var rndIndex = Random.Range(0, 7);
            _wheelOfFortuneView.SetSpinButtonInteractable(false);
            await _wheelOfFortuneView.SpinAnimationModule.SpinTo(rndIndex);
            await UniTask.WaitForSeconds(2f);
            _wheelOfFortuneView.SpinAnimationModule.PlayIdle();
            _wheelOfFortuneView.SetSpinButtonInteractable(true);
            _wheelZoneModel.MoveNextZone();
        }

        private void OnViewInitialized()
        {
            _wheelOfFortuneView.SpinAnimationModule.PlayIdle();
            
            UpdateWheelRewards();
        }

        private void OnZoneUpdated()
        {
            UpdateWheelRewards();
        }

        private void UpdateWheelRewards()
        {
            var wheelType = _wheelZoneModel.GetWheelType();
            
            var wheelVisuals = _wheelSlotViewHandler.GetWheelVisuals(wheelType);
            
            _wheelOfFortuneView.UpdateWheelVisuals(wheelVisuals);
            
            _wheelSlotViewHandler.PopulateSlotViews(wheelType, out var views);

            foreach (var wheelSlotView in views)
            {
                var calculatedValue = _wheelRewardCalculator.Calculate(wheelSlotView.SlotIndex, wheelType, _wheelZoneModel.ZoneCounter);

                var formatValue = _wheelRewardCalculator.FormatValue(wheelSlotView.SlotIndex, wheelType, calculatedValue);
                
                wheelSlotView.SetValue(formatValue);
                
                SettleRewardView(wheelSlotView);
            }
        }

        private void SettleRewardView(WheelSlotView slotView)
        {
            foreach (var rewardHolder in _wheelOfFortuneView.RewardHolders)
            {
                if(rewardHolder.SlotIndex != slotView.SlotIndex) continue;
                
                slotView.SetParent(rewardHolder.transform);
            }
        }

        public void Dispose()
        {
            _wheelOfFortuneView.OnInitialize -= OnViewInitialized;
            _wheelZoneModel.OnZoneUpdate -= OnZoneUpdated;
            _wheelOfFortuneView.SpinButton.onClick.RemoveListener(OnClickSpinButton);
        }
    }
}