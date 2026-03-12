using Core.Pool.Services;
using Core.SaveSystem;
using Game.Configs;
using Game.Factories;
using Game.Installers;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Bootstrappers
{
    public class GameBootstrapper : MonoBehaviour
    {
        [field: SerializeField] private GameConfigContainerSO GameConfigContainer { get; set; }
        [field: SerializeField] public WheelOfFortuneBootstrapper WheelOfFortuneBootstrapper { get; private set; }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (WheelOfFortuneBootstrapper) return;
            
            WheelOfFortuneBootstrapper = GetComponentInChildren<WheelOfFortuneBootstrapper>();
            
            if (WheelOfFortuneBootstrapper) EditorUtility.SetDirty(this);
        }
#endif
        
        private void Awake()
        {
            Application.targetFrameRate = GameConfigContainer.TargetFrameRate;
            Input.multiTouchEnabled = GameConfigContainer.IsMultitouchEnabled;
            
            IJsonSaveService saveService = new JsonSaveService();
            IObjectPoolService poolService = new ObjectPoolService(GameConfigContainer.PoolServiceConfig).Initialize();
            ISlotViewFactory slotViewFactory = new SlotViewFactory(poolService);
            
            WheelOfFortuneBootstrapper?.Initialize(saveService, GameConfigContainer.WheelOfFortuneConfigContainer, slotViewFactory);
        }
    }
}