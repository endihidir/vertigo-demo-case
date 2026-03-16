using System;
using Core.Modules;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Views
{
    public class WheelSpinResultView : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public Button TryAgainButton { get; private set; }
        [field: SerializeField, ReadOnly] public Button NextButton { get; private set; }
        [field: SerializeField, ReadOnly] public SizeAnimationModule SizeAnimationModule { get; private set; }
        
        [field: SerializeField] private GameObject BombPanel { get; set; }
        [field: SerializeField] private GameObject RewardPanel { get; set; }
        [field: SerializeField] private Image RewardImage { get; set; }
        [field: SerializeField] private TextMeshProUGUI RewardAmountValueTxt { get; set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            SizeAnimationModule = GetComponentInChildren<SizeAnimationModule>();
            
            var allButtons = GetComponentsInChildren<Button>(true);
            
            foreach (var button in allButtons)
            {
                if(button.name.Contains("try", StringComparison.OrdinalIgnoreCase)) TryAgainButton = button;
                if(button.name.Contains("next", StringComparison.OrdinalIgnoreCase)) NextButton = button;
            }
            
            EditorUtility.SetDirty(this);
        }
#endif

        public async UniTask SetActiveAsync(bool value)
        {
            gameObject.SetActive(value);
            await SizeAnimationModule.SetScale(value ? Vector3.one : Vector3.zero, value ? .25f : 0f, ease: Ease.OutBack);
        }

        public void InitBombPanel()
        {
            RewardPanel?.SetActive(false);
            
            TryAgainButton.interactable = true;
            BombPanel?.SetActive(true);
        }
        
        public void InitRewardPanel(Sprite rewardIcon, string rewardTxt)
        {
            BombPanel?.SetActive(false);
            
            NextButton.interactable = true;
            RewardImage.sprite = rewardIcon;
            RewardAmountValueTxt?.SetText(rewardTxt);
            RewardPanel?.SetActive(true);
        }
    }
}