using System.Linq;
using Game.Enums;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "WheelOfFortuneConfigContainer", menuName = "Game/Containers/WheelOfFortuneConfigContainer")]
    public class WheelOfFortuneConfigContainerSO : ScriptableObject
    {
        [field: SerializeField] public WheelConfigSO[] WheelConfigs { get; private set; }
        
        public WheelConfigSO GetWheelConfig(WheelType wheelType)
        {
            var wheelSlotConfig = WheelConfigs.FirstOrDefault(config => config.WheelType == wheelType);

            return wheelSlotConfig;
        }

        public bool TryGetWheelConfig(WheelType wheelType, out WheelConfigSO wheelConfig)
        {
            var wheelSlotConfig = WheelConfigs.FirstOrDefault(config => config.WheelType == wheelType);
            
            wheelConfig = wheelSlotConfig;
            
            return wheelConfig;
        }
    }
}