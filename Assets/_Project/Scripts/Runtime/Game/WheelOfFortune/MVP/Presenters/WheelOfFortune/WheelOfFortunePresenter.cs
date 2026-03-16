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
        private readonly IWheelZoneModel _zoneModel;
        private readonly IWheelOfFortuneView _view;
        private readonly IWheelSlotViewHandler _wheelSlotViewHandler;
        private readonly IWheelRewardProvider _wheelRewardProvider;
        private readonly IWheelRewardDatabase _wheelRewardDatabase;
        private readonly IWheelSpinResolver _spinResolver;
        private readonly IWheelCollectedRewardSlotHandler _collectedRewardSlotHandler;
        
        private int _lastTargetSlotIndex;
        
        public WheelOfFortunePresenter(IWheelZoneModel zoneModel, IWheelOfFortuneView view, IWheelSlotViewHandler wheelSlotViewHandler, 
            IWheelRewardProvider rewardProvider, IWheelRewardDatabase rewardDatabase, IWheelSpinResolver spinResolver, IWheelCollectedRewardSlotHandler collectedRewardSlotHandler)
        {
            _zoneModel = zoneModel;
            _view = view;
            _wheelSlotViewHandler = wheelSlotViewHandler;
            _wheelRewardProvider = rewardProvider;
            _wheelRewardDatabase = rewardDatabase;
            _spinResolver = spinResolver;
            _collectedRewardSlotHandler = collectedRewardSlotHandler;
            
            _view.PlayButton.onClick.AddListener(OnClickPlayButton);
            _view.OnInitialize += OnViewInitialized;
            _zoneModel.OnZoneUpdate += OnZoneUpdated;
            
            _view.WheelSpinView.ExitButton.onClick.AddListener(OnClickExitButton);
            _view.WheelSpinView.SpinButton.onClick.AddListener(OnClickSpinButton);
            
            _view.WheelSpinResultView.NextButton.onClick.AddListener(OnClickNextButton);
            _view.WheelSpinResultView.TryAgainButton.onClick.AddListener(OnClickTryAgainButton);
            
            _view.WheelRewardCollectView.CollectButton.onClick.AddListener(OnClickCollectButton);
            _view.WheelRewardCollectView.ContinueButton.onClick.AddListener(OnClickContinueButton);
        }

        private void OnClickExitButton()
        {
            _view.WheelSpinView.SpinAnimationModule.KillAnimation();
            _view.WheelSpinView.SetActiveAsync(false).Forget();
            _view.WheelRewardCollectView.SetActiveAsync(false).Forget();
            _view.WheelSpinResultView.SetActiveAsync(false).Forget();
            _view.PlayButton.gameObject.SetActive(true);
        }

        private void OnClickPlayButton()
        {
            _view.PlayButton.gameObject.SetActive(false);
            _view.Initialize();
        }

        private void OnViewInitialized()
        {
            ResetToSpinView();
            UpdateWheelRewards();
        }
        
        private void OnZoneUpdated() => UpdateWheelRewards();
        private void OnClickSpinButton() => PlaySpinAnimation().Forget();

        private async UniTask PlaySpinAnimation()
        {
            _lastTargetSlotIndex = _spinResolver.ResolveSlotIndex(_zoneModel.ZoneCounter);
            _view.WheelSpinView.SetSpinButtonInteractable(false);
            _view.WheelSpinView.SetExitButtonInteractable(false);
            await _view.WheelSpinView.SpinAnimationModule.SpinTo(_lastTargetSlotIndex);
            _view.WheelSpinView.SetExitButtonInteractable(true);
            _wheelRewardProvider.PrepareSpinResultView(_lastTargetSlotIndex, _zoneModel.ZoneCounter, _view.WheelSpinResultView);
        }
        
        private void OnClickNextButton()
        {
            var itemId = _wheelRewardProvider.GetRewardSlotData(_lastTargetSlotIndex, _zoneModel.ZoneCounter).RewardDefinition.Id;
            var calculatedValue = _wheelRewardProvider.CalculateValue(_lastTargetSlotIndex, _zoneModel.ZoneCounter);
            _wheelRewardDatabase.AddAmount(itemId, calculatedValue);
            ResetToSpinView();
            _zoneModel.MoveNextZone();
        }

        private void OnClickContinueButton() => ResetToSpinView();

        private void OnClickTryAgainButton()
        {
            _zoneModel.ResetZone();
            _wheelRewardDatabase.Reset();
            ResetToSpinView();
        }
        
        private void OnClickCollectButton()
        {
            _zoneModel.ResetZone();
            ResetToSpinView();
        }

        private void UpdateWheelRewards()
        {
            var wheelType = WheelOfFortuneUtils.GetWheelType(_zoneModel.ZoneCounter);

            if (wheelType is WheelType.Bronze or WheelType.Gold)
            {
                _collectedRewardSlotHandler.PopulateSlotViews(out var slotViews);
                
                foreach (var slotView in slotViews)
                {
                    _view.WheelRewardCollectView.SettleReward(slotView);
                }
                
                _view.WheelRewardCollectView.SetActiveAsync(true).Forget();
            }
            
            var wheelVisuals = _wheelSlotViewHandler.GetWheelVisuals(wheelType);
            
            _view.WheelSpinView.UpdateWheelVisuals(wheelVisuals);
            _wheelSlotViewHandler.PopulateSlotViews(wheelType, out var views);

            foreach (var wheelSlotView in views)
            {
                var calculatedValue = _wheelRewardProvider.CalculateValue(wheelSlotView.SlotIndex, _zoneModel.ZoneCounter);
                var formatValue = _wheelRewardProvider.FormatValue(wheelSlotView.SlotIndex, calculatedValue, _zoneModel.ZoneCounter);
                wheelSlotView.SetValue(formatValue);
                SettleRewardView(wheelSlotView);
            }

            var titleTxt = WheelOfFortuneUtils.GetTitle(wheelType);
            var title = string.IsNullOrEmpty(titleTxt) ? $"ZONE {_zoneModel.ZoneCounter}" : titleTxt;
            _view.WheelSpinView.SetZoneTitle(title, wheelVisuals.TitleColor);
        }

        private void SettleRewardView(WheelSlotView slotView)
        {
            foreach (var rewardHolder in _view.WheelSpinView.RewardHolders)
            {
                if(rewardHolder.SlotIndex != slotView.SlotIndex) continue;
                
                slotView.SetParent(rewardHolder.transform);
            }
        }
        
        private void ResetToSpinView()
        {
            _view.WheelRewardCollectView.SetActiveAsync(false).Forget();
            _view.WheelSpinResultView.SetActiveAsync(false).Forget();
            _view.WheelSpinView.SpinAnimationModule.PlayIdle();
            _view.WheelSpinView.SetSpinButtonInteractable(true);
        }
        
        public void Dispose()
        {
            _view.PlayButton.onClick.RemoveListener(OnClickPlayButton);
            
            _view.OnInitialize -= OnViewInitialized;
            _zoneModel.OnZoneUpdate -= OnZoneUpdated;
            
            _view.WheelSpinView.ExitButton.onClick.RemoveListener(OnClickExitButton);
            _view.WheelSpinView.SpinButton.onClick.RemoveListener(OnClickSpinButton);
            
            _view.WheelSpinResultView.NextButton.onClick.RemoveListener(OnClickNextButton);
            _view.WheelSpinResultView.TryAgainButton.onClick.RemoveListener(OnClickTryAgainButton);
            
            _view.WheelRewardCollectView.CollectButton.onClick.RemoveListener(OnClickCollectButton);
            _view.WheelRewardCollectView.ContinueButton.onClick.RemoveListener(OnClickContinueButton);
        }
    }
}