using System;
using Cysharp.Threading.Tasks;
using Game.Data;
using Game.Enums;
using Game.Handlers;
using Game.Models;
using Game.Utils;
using Game.Views;

namespace Game.Presenters
{
    public sealed class WheelOfFortunePresenter : IDisposable
    {
        private readonly IWheelZoneModel _wheelZoneModel;
        private readonly IWheelOfFortuneView _wheelOfFortuneView;
        private readonly IWheelSlotViewHandler _wheelSlotViewHandler;
        private readonly IWheelRewardProvider _wheelRewardProvider;
        private readonly IWheelRewardDatabase _wheelRewardDatabase;
        private readonly IWheelSpinResolver _wheelSpinResolver;
        private readonly IWheelCollectedRewardSlotHandler _collectedRewardSlotHandler;
        
        private int _lastTargetSlotIndex;
        
        public WheelOfFortunePresenter(IWheelZoneModel wheelZoneModel, IWheelOfFortuneView wheelOfFortuneView, IWheelSlotViewHandler wheelSlotViewHandler, 
            IWheelRewardProvider rewardProvider, IWheelRewardDatabase rewardDatabase, IWheelSpinResolver wheelSpinResolver, IWheelCollectedRewardSlotHandler collectedRewardSlotHandler)
        {
            _wheelZoneModel = wheelZoneModel;
            _wheelOfFortuneView = wheelOfFortuneView;
            _wheelSlotViewHandler = wheelSlotViewHandler;
            _wheelRewardProvider = rewardProvider;
            _wheelRewardDatabase = rewardDatabase;
            _wheelSpinResolver = wheelSpinResolver;
            _collectedRewardSlotHandler = collectedRewardSlotHandler;
            
            _wheelOfFortuneView.OnInitialize += OnViewInitialized;
            _wheelZoneModel.OnZoneUpdate += OnZoneUpdated;
            
            _wheelOfFortuneView.WheelSpinView.SpinButton.onClick.AddListener(OnClickSpinButton);
            _wheelOfFortuneView.WheelSpinResultView.NextButton.onClick.AddListener(OnClickNextButton);
            _wheelOfFortuneView.WheelSpinResultView.TryAgainButton.onClick.AddListener(OnClickTryAgainButton);
            _wheelOfFortuneView.WheelRewardCollectView.CollectButton.onClick.AddListener(OnClickCollectButton);
            _wheelOfFortuneView.WheelRewardCollectView.ContinueButton.onClick.AddListener(OnClickContinueButton);
        }
        
        private void OnViewInitialized()
        {
            ResetWheelOfFortuneView();
            UpdateWheelRewards();
        }
        
        private void OnZoneUpdated() => UpdateWheelRewards();

        private void OnClickSpinButton() => PlaySpinAnimation().Forget();

        private async UniTask PlaySpinAnimation()
        {
            _lastTargetSlotIndex = _wheelSpinResolver.ResolveSlotIndex(_wheelZoneModel.ZoneCounter);
            _wheelOfFortuneView.WheelSpinView.SetSpinButtonInteractable(false);
            await _wheelOfFortuneView.WheelSpinView.SpinAnimationModule.SpinTo(_lastTargetSlotIndex);
            _wheelRewardProvider.PrepareSpinResultView(_lastTargetSlotIndex, _wheelZoneModel.ZoneCounter, _wheelOfFortuneView.WheelSpinResultView);
        }
        
        private void OnClickNextButton()
        {
            var itemId = _wheelRewardProvider.GetSlotData(_lastTargetSlotIndex, _wheelZoneModel.ZoneCounter).RewardDefinition.Id;
            var calculatedValue = _wheelRewardProvider.CalculateValue(_lastTargetSlotIndex, _wheelZoneModel.ZoneCounter);
            _wheelRewardDatabase.AddAmount(itemId, calculatedValue);
            ResetWheelOfFortuneView();
            _wheelZoneModel.MoveNextZone();
        }

        private void OnClickContinueButton() => ResetWheelOfFortuneView();

        private void OnClickTryAgainButton()
        {
            _wheelZoneModel.ResetZone();
            _wheelRewardDatabase.Reset();
            ResetWheelOfFortuneView();
        }
        
        private void OnClickCollectButton()
        {
            _wheelZoneModel.ResetZone();
            ResetWheelOfFortuneView();
        }

        private void UpdateWheelRewards()
        {
            var wheelType = WheelOfFortuneUtils.GetWheelType(_wheelZoneModel.ZoneCounter);

            if (wheelType is WheelType.Bronze or WheelType.Gold)
            {
                _collectedRewardSlotHandler.PopulateSlotViews(out var slotViews);
                
                foreach (var slotView in slotViews)
                {
                    _wheelOfFortuneView.WheelRewardCollectView.SettleReward(slotView);
                }
                
                _wheelOfFortuneView.WheelRewardCollectView.SetActive(true).Forget();
            }
            
            var wheelVisuals = _wheelSlotViewHandler.GetWheelVisuals(wheelType);
            
            _wheelOfFortuneView.WheelSpinView.UpdateWheelVisuals(wheelVisuals);
            _wheelSlotViewHandler.PopulateSlotViews(wheelType, out var views);

            foreach (var wheelSlotView in views)
            {
                var calculatedValue = _wheelRewardProvider.CalculateValue(wheelSlotView.SlotIndex, _wheelZoneModel.ZoneCounter);
                var formatValue = _wheelRewardProvider.FormatValue(wheelSlotView.SlotIndex, calculatedValue, _wheelZoneModel.ZoneCounter);
                wheelSlotView.SetValue(formatValue);
                SettleRewardView(wheelSlotView);
            }
        }

        private void SettleRewardView(WheelSlotView slotView)
        {
            foreach (var rewardHolder in _wheelOfFortuneView.WheelSpinView.RewardHolders)
            {
                if(rewardHolder.SlotIndex != slotView.SlotIndex) continue;
                
                slotView.SetParent(rewardHolder.transform);
            }
        }
        
        private void ResetWheelOfFortuneView()
        {
            _wheelOfFortuneView.WheelRewardCollectView.SetActive(false).Forget();
            _wheelOfFortuneView.WheelSpinResultView.SetActive(false).Forget();
            _wheelOfFortuneView.WheelSpinView.SpinAnimationModule.PlayIdle();
            _wheelOfFortuneView.WheelSpinView.SetSpinButtonInteractable(true);
        }
        
        public void Dispose()
        {
            _wheelOfFortuneView.OnInitialize -= OnViewInitialized;
            _wheelZoneModel.OnZoneUpdate -= OnZoneUpdated;
            _wheelOfFortuneView.WheelSpinView.SpinButton.onClick.RemoveListener(OnClickSpinButton);
            _wheelOfFortuneView.WheelSpinResultView.NextButton.onClick.RemoveListener(OnClickNextButton);
            _wheelOfFortuneView.WheelSpinResultView.TryAgainButton.onClick.RemoveListener(OnClickTryAgainButton);
            _wheelOfFortuneView.WheelRewardCollectView.CollectButton.onClick.RemoveListener(OnClickCollectButton);
            _wheelOfFortuneView.WheelRewardCollectView.ContinueButton.onClick.RemoveListener(OnClickContinueButton);
        }
    }
}