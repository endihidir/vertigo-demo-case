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
            IWheelRewardProvider rewardProvider, IWheelRewardDatabase rewardDatabase, IWheelSpinResolver spinResolver,
            IWheelCollectedRewardSlotHandler collectedRewardSlotHandler)
        {
            _zoneModel = zoneModel;
            _view = view;
            _wheelSlotViewHandler = wheelSlotViewHandler;
            _wheelRewardProvider = rewardProvider;
            _wheelRewardDatabase = rewardDatabase;
            _spinResolver = spinResolver;
            _collectedRewardSlotHandler = collectedRewardSlotHandler;

            Subscribe();
        }

        private void Subscribe()
        {
            _zoneModel.OnZoneUpdate += OnZoneUpdated;

            _view.PlayButton.onClick.AddListener(OnClickPlayButton);

            _view.WheelSpinView.ExitButton.onClick.AddListener(OnClickExitButton);
            _view.WheelSpinView.SpinButton.onClick.AddListener(OnClickSpinButton);

            _view.WheelSpinResultView.NextButton.onClick.AddListener(OnClickNextButton);
            _view.WheelSpinResultView.TryAgainButton.onClick.AddListener(OnClickTryAgainButton);

            _view.WheelRewardCollectView.CollectButton.onClick.AddListener(OnClickCollectButton);
            _view.WheelRewardCollectView.ContinueButton.onClick.AddListener(OnClickContinueButton);
        }

        private void OnClickPlayButton()
        {
            _view.PlayButton.gameObject.SetActive(false);
            UpdateWheelRewards();
            ResetToSpinView();
            _view.WheelSpinView.SetActiveAsync(true).Forget();
        }

        private void OnZoneUpdated() => UpdateWheelRewards();
        private void OnClickSpinButton() => PlaySpinAnimation().Forget();

        private async UniTask PlaySpinAnimation()
        {
            _lastTargetSlotIndex = _spinResolver.ResolveSlotIndex(_zoneModel.ZoneCounter);
            _view.WheelSpinView.SetSpinButtonInteractable(false);
            _view.WheelSpinView.SetExitButtonInteractable(false);
            await _view.WheelSpinView.SpinAnimationModule.SpinTo(_lastTargetSlotIndex);
            ShowSpinResult(_wheelRewardProvider.GetSpinResultData(_lastTargetSlotIndex, _zoneModel.ZoneCounter));
        }

        private void ShowSpinResult(SpinResultData result)
        {
            if (result.IsBomb) _view.WheelSpinResultView.InitBombPanel();
            else _view.WheelSpinResultView.InitRewardPanel(result.RewardIcon, result.RewardText);
            _view.WheelSpinResultView.SetActiveAsync(true).Forget();
        }

        private void OnClickNextButton()
        {
            var slotData = _wheelRewardProvider.GetRewardSlotData(_lastTargetSlotIndex, _zoneModel.ZoneCounter);
            var calculatedValue = _wheelRewardProvider.CalculateValue(_lastTargetSlotIndex, _zoneModel.ZoneCounter);
            _wheelRewardDatabase.AddAmount(slotData.RewardDefinition.Id, calculatedValue);
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

        private void OnClickExitButton()
        {
            _view.WheelSpinView.SpinAnimationModule.KillAnimation();
            _view.WheelSpinView.SetActiveAsync(false).Forget();
            _view.WheelRewardCollectView.SetActiveAsync(false).Forget();
            _view.WheelSpinResultView.SetActiveAsync(false).Forget();
            _view.PlayButton.gameObject.SetActive(true);
        }

        private void UpdateWheelRewards()
        {
            var wheelType = WheelOfFortuneUtils.GetWheelType(_zoneModel.ZoneCounter);
            var wheelVisuals = _wheelSlotViewHandler.GetWheelVisuals(wheelType);

            if (wheelType is WheelType.Bronze or WheelType.Gold)
                ShowCollectView();

            UpdateWheelTitle(wheelType, wheelVisuals);
            UpdateWheelSlots(wheelType, wheelVisuals);
        }

        private void ShowCollectView()
        {
            _collectedRewardSlotHandler.PopulateSlotViews(out var collectedRewardSlotViews);
            
            foreach (var collectedRewardSlotView in collectedRewardSlotViews)
                _view.WheelRewardCollectView.PlaceSlot(collectedRewardSlotView);
            
            _view.WheelRewardCollectView.SetActiveAsync(true).Forget();
        }

        private void UpdateWheelSlots(WheelType wheelType, WheelVisualData wheelVisuals)
        {
            _view.WheelSpinView.UpdateWheelVisuals(wheelVisuals);
            _wheelSlotViewHandler.PopulateSlotViews(wheelType, out var slotViews);

            foreach (var slotView in slotViews)
            {
                var calculatedValue = _wheelRewardProvider.CalculateValue(slotView.SlotIndex, _zoneModel.ZoneCounter);
                slotView.SetValue(_wheelRewardProvider.FormatValue(slotView.SlotIndex, calculatedValue, _zoneModel.ZoneCounter));
                _view.WheelSpinView.PlaceSlot(slotView);
            }
        }

        private void UpdateWheelTitle(WheelType wheelType, WheelVisualData wheelVisuals)
        {
            var titleTxt = WheelOfFortuneUtils.GetTitle(wheelType, _zoneModel.ZoneCounter);
            _view.WheelSpinView.SetZoneTitle(titleTxt, wheelVisuals.TitleColor);
        }

        private void ResetToSpinView()
        {
            _view.WheelRewardCollectView.SetActiveAsync(false).Forget();
            _view.WheelSpinResultView.SetActiveAsync(false).Forget();
            _view.WheelSpinView.SpinAnimationModule.PlayIdle();
            _view.WheelSpinView.SetExitButtonInteractable(true);
            _view.WheelSpinView.SetSpinButtonInteractable(true);
        }

        public void Dispose() => Unsubscribe();

        private void Unsubscribe()
        {
            _zoneModel.OnZoneUpdate -= OnZoneUpdated;

            _view.PlayButton.onClick.RemoveListener(OnClickPlayButton);

            _view.WheelSpinView.ExitButton.onClick.RemoveListener(OnClickExitButton);
            _view.WheelSpinView.SpinButton.onClick.RemoveListener(OnClickSpinButton);

            _view.WheelSpinResultView.NextButton.onClick.RemoveListener(OnClickNextButton);
            _view.WheelSpinResultView.TryAgainButton.onClick.RemoveListener(OnClickTryAgainButton);

            _view.WheelRewardCollectView.CollectButton.onClick.RemoveListener(OnClickCollectButton);
            _view.WheelRewardCollectView.ContinueButton.onClick.RemoveListener(OnClickContinueButton);
        }
    }
}