using Game.Configs;
using Game.Factories;
using Game.Handlers;
using Game.Presenters;
using Game.Views;
using UnityEngine;

namespace Game.Installers
{
    public class WheelOfFortuneBootstrapper : MonoBehaviour 
    {
        [field: SerializeField] private WheelPanelView WheelPanelView { get; set; }
        
        public void Initialize(WheelOfFortuneConfigContainerSO configContainer, ISlotViewFactory slotViewFactory)
        {
            var wheelSlotViewHandler = new WheelSlotViewHandler(slotViewFactory, configContainer);
            var wheelOfFortunePresenter = new WheelPanelPresenter(WheelPanelView, wheelSlotViewHandler);
            WheelPanelView.Initialize();
        }
    }
}