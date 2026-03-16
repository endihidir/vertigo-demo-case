# 🎡 Wheel of Fortune — Unity Feature Module

A self-contained **Wheel of Fortune** feature module built for a mobile game demo case study.  
Designed as a production-ready, drop-in feature with clean architecture, no external DI framework dependency, and full ScriptableObject-driven configuration.

---

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Systems](#systems)
  - [Core Layer](#core-layer)
  - [Wheel of Fortune Module](#wheel-of-fortune-module)
- [Wheel Zone Logic](#wheel-zone-logic)
- [Reward System](#reward-system)
- [Spin Weight System](#spin-weight-system)
- [Data Configuration](#data-configuration)
- [Dependencies](#dependencies)
- [Setup](#setup)

---

## Overview

The project implements a fully functional Wheel of Fortune reward system modelled after the mechanics found in titles such as *Gardenscapes* and *Coin Master*. Key design goals:

- **MVP pattern** with clear separation between Model, View, and Presenter
- **Manual Dependency Injection** wired in a dedicated `Bootstrapper` — no DI framework required
- **ScriptableObject-driven configuration** — game designers can author all wheel content without touching code
- **Generic Object Pooling** for slot views with both lazy and pre-warmed pool support
- **Persistent reward accumulation** via a save-service abstraction backed by `PlayerPrefs` or JSON files

---

## Architecture

```
GameBootstrapper
└── WheelOfFortuneBootstrapper  (Manual DI wiring)
    ├── WheelOfFortunePresenter  (Orchestrator)
    │   ├── IWheelZoneModel       (Zone state & events)
    │   ├── IWheelRewardProvider  (Value calculation)
    │   ├── IWheelSpinResolver    (Weighted random spin)
    │   ├── IWheelRewardDatabase  (Persistent accumulation)
    │   ├── IWheelSlotViewHandler (Slot view lifecycle)
    │   └── IWheelCollectedRewardSlotHandler
    └── IWheelOfFortuneView
        ├── WheelSpinView
        ├── WheelSpinResultView
        └── WheelRewardCollectView
```

All dependencies are injected through constructor parameters. `WheelOfFortuneBootstrapper` acts as the composition root and calls `Dispose()` on the presenter when destroyed, cleaning up all event subscriptions.

---

## Project Structure

```
Scripts/
├── Editor/
│   ├── ConstantDropdownDrawer.cs     # Custom property drawer for ItemId constants
│   └── Editor.asmdef
└── Runtime/
    ├── Core/                          # Reusable, game-agnostic systems
    │   ├── Attributes/
    │   │   └── ConstantDropdown.cs
    │   ├── Modules/
    │   │   └── Animation/
    │   │       └── SizeAnimationModule.cs
    │   ├── Extensions/
    │   │   ├── ColorExtensions.cs
    │   │   ├── EnumerableExtensions.cs
    │   │   ├── FloatExtensions.cs
    │   │   ├── GameObjectExtensions.cs
    │   │   └── StringExtensions.cs
    │   ├── Services/
    │   │   ├── Pool/
    │   │   │   ├── ObjectPool.cs
    │   │   │   ├── ObjectPoolService.cs
    │   │   │   ├── PooledObject.cs
    │   │   │   ├── Data/  (PooledAssetConfigSO, PoolServiceConfigSO)
    │   │   │   └── Interfaces/  (IObjectPoolService, IPooledObject)
    │   │   └── Save/
    │   │       ├── JsonSaveService.cs
    │   │       └── Interfaces/  (IJsonSaveService)
    │   └── Utils/
    │       ├── EditorLogger.cs
    │       └── PoolSearchUtils.cs
    └── Game/                          # Game-specific code
        ├── Bootstrappers/
        │   ├── GameBootstrapper.cs
        │   ├── WheelOfFortuneBootstrapper.cs
        │   └── Data/  (GameConfigContainerSO, WheelOfFortuneConfigContainerSO)
        ├── Factories/
        │   ├── BaseSlotView.cs
        │   ├── SlotViewFactory.cs
        │   └── Interfaces/  (ISlotViewFactory)
        ├── Reward/
        │   └── Data/
        │       ├── ItemId.cs
        │       ├── RewardDefinition.cs
        │       ├── RewardTier.cs
        │       ├── RewardValueType.cs
        │       ├── RewardVisualConfigContainerSO.cs
        │       └── RewardVisualDataSO.cs
        └── WheelOfFortune/
            ├── Modules/
            │   └── Animation/
            │       └── WheelSpinAnimationModule.cs
            └── MVP/
                ├── Models/
                │   └── WheelOfFortune/
                │       ├── IWheelZoneModel.cs
                │       └── WheelZoneModel.cs
                ├── Presenters/
                │   └── WheelOfFortune/
                │       ├── Data/      (SpinResultData, WheelConfigSO, WheelRewardData,
                │       │               WheelSlotData, WheelSlotType, WheelSpinAnimationConfigSO,
                │       │               WheelType, WheelVisualData)
                │       ├── Interfaces/ (IWheelCollectedRewardSlotHandler, IWheelRewardDatabase,
                │       │               IWheelRewardProvider, IWheelSlotViewHandler, IWheelSpinResolver)
                │       ├── Utils/
                │       │   └── WheelOfFortuneUtils.cs
                │       ├── WheelCollectedRewardSlotHandler.cs
                │       ├── WheelOfFortunePresenter.cs
                │       ├── WheelRewardDatabase.cs
                │       ├── WheelRewardProvider.cs
                │       ├── WheelSlotViewHandler.cs
                │       └── WheelSpinResolver.cs
                └── Views/
                    └── WheelofFortune/
                        ├── Interfaces/  (IWheelOfFortuneView)
                        ├── Pooled/      (WheelCollectedRewardSlotView, WheelSlotView)
                        ├── WheelOfFortuneView.cs
                        ├── WheelRewardCollectView.cs
                        ├── WheelRewardHolderView.cs
                        ├── WheelSpinResultView.cs
                        └── WheelSpinView.cs

ScriptableObjects/
├── Core/
│   ├── GameConfigContainer.asset
│   └── PoolServiceConfig.asset
└── Game/
    ├── RewardVisualData/
    │   ├── RewardVisualConfigContaner.asset
    │   └── RewardVisuals/           # One RewardVisualDataSO per reward item
    └── WheelOfFortune/
        ├── PooledAssetConfigs/      # Pool configs for WheelSlotView & CollectedRewardSlotView
        ├── WheelConfigs/
        │   ├── Animation/
        │   │   └── WheelSpinAnimationConfig.asset
        │   ├── BronzeWheelConfig.asset
        │   ├── GoldWheelConfig.asset
        │   └── SilverWheelConfig.asset
        └── WheelOfFortuneConfigContainer.asset
```

---

## Systems

### Core Layer

#### Object Pool (`ObjectPoolService`)

A generic pool that supports two lookup strategies:

| Strategy | Key | Use case |
|----------|-----|----------|
| Type-based | `typeof(T)` | Pre-warmed pools declared in `PoolServiceConfigSO` |
| Prefab-based | `prefab.GetInstanceID()` | Dynamic, runtime-created pools |

Pools are configured per-asset in `PooledAssetConfigSO`:

```
PooledAssetConfigSO
├── IsLazy        — false = pre-warm on Initialize()
├── PoolSize      — initial capacity
└── PoolObject    — prefab (must contain a PooledObject-derived component)
```

`PooledObject` is the base MonoBehaviour that implements `IPooledObject`. Override `OnActivate()` / `OnDeactivate()` for custom behaviour on pool get/return.

#### Save Service (`JsonSaveService`)

Dual-backend save system abstracted behind `IJsonSaveService`:

```csharp
// PlayerPrefs backend (default for runtime data)
saveService.SaveToPrefs<T>(key, data);
T data = saveService.LoadFromPrefs<T>(key, defaultData);

// File backend (for larger data or editor inspection)
saveService.SaveToTextFile<T>(key, data);
T data = saveService.LoadFromTextFile<T>(key, defaultData);
```

In Editor builds, JSON files are written to `Assets/JsonGameData[EditorOnly]/`.  
In Player builds, they go to `Application.persistentDataPath/JsonGameData/`.

Editor menu shortcuts: **Edit → Clear All Json Data**, **Edit → Clear All**

#### `ConstantDropdown` Attribute

A custom `PropertyAttribute` + `PropertyDrawer` pair that renders a dropdown populated from the `public const string` fields of a given type.

```csharp
[ConstantDropdown(typeof(ItemId))]
public string Id;
```

Missing values (deleted constants) are flagged in the Inspector with a warning box, keeping ScriptableObject assets safe during refactors.

#### Extension Methods

| Class | Extensions |
|-------|-----------|
| `ColorExtensions` | `SetAlpha`, `SetRed`, `SetOpaque`, `SetTransparent` |
| `EnumerableExtensions` | `Shuffle<T>` (Fisher-Yates) |
| `FloatExtensions` | `Remap` |
| `GameObjectExtensions` | `GetOrAddComponent`, `OrNull`, `HideInHierarchy`, `Path`, `PathFull` |
| `StringExtensions` | `Simplify`, `SplitCamelCase`, `ExtractInt`, `ConvertToRanking`, `HideBigNumber`, `ComputeFnv1AHash` |

`EditorLogger` wraps `Debug.Log/Warning/Error` behind `[Conditional("UNITY_EDITOR")]` so all logging strips out of release builds with zero runtime overhead.

#### `SizeAnimationModule` (Core)

A reusable DOTween-based animation component for scale and size transitions, living in the Core layer so any feature module can use it:

```csharp
// Pop-in with OutBack ease (used by all WoF panels)
await sizeAnimationModule.SetScale(Vector3.one, duration: 0.25f, ease: Ease.OutBack);

// Pop-out (instant, duration=0)
await sizeAnimationModule.SetScale(Vector3.zero, duration: 0f);

// RectTransform size delta
sizeAnimationModule.SetRectSize(targetSize, duration, delay, ease);
```

`Transform` is serializable — if left empty, the component falls back to its own transform. Kills the active tween before starting a new one and cleans up in `OnDestroy`.

---

### Wheel of Fortune Module

#### `WheelOfFortunePresenter`

The central orchestrator. Subscribes to all button events in the constructor and unsubscribes via `Dispose()`. It drives the state machine:

```
[Idle: PlayButton visible]
       ↓  PlayButton clicked
       →  UpdateWheelRewards() + ResetToSpinView() + WheelSpinView.SetActiveAsync(true)
[SpinView: pop-in animation, idle rotation, SpinButton enabled]
       ↓  SpinButton clicked
[Spinning: buttons locked, DOTween animation plays]
       ↓  Animation complete
[SpinResultView: pops in — shows reward panel or bomb panel]
       ↓  NextButton (reward) / TryAgainButton (bomb) / ContinueButton (collect)
[Back to SpinView] or [ZoneReset]
```

Bronze and Gold zone transitions additionally show the **WheelRewardCollectView** before returning to spin.

#### `WheelZoneModel`

Minimal model holding a single integer `ZoneCounter` and two events:

```csharp
event Action OnZoneUpdate;
void MoveNextZone();   // counter++ then fire event
void ResetZone();      // counter = 1 then fire event
```

The presenter reacts to `OnZoneUpdate` and rebuilds the wheel UI to reflect the new zone's wheel type, visuals, and slot configurations.

#### `WheelSpinResolver`

Performs weighted random slot selection:

1. Fetches `WheelConfigSO` for the current zone's `WheelType`
2. Calls `slot.GetWeight(zoneCount)` on each slot to get the zone-aware weight
3. Rolls `Random.Range(0f, totalWeight)` and walks the cumulative distribution

#### `WheelRewardProvider`

All reward value computation is isolated here:

```csharp
int CalculateValue(int slotIndex, int zoneCounter);
// Numeric   → BaseValue × WheelMultiplier × ZoneCount
// Stackable → BaseValue × WheelMultiplier
// Unique    → 0 (weapons, special items)

string FormatValue(int slotIndex, int calculatedValue, int zoneCounter);
// Numeric   → "1.5k", "2M"  (HideBigNumber formatting)
// Stackable → "X3"
// Unique    → ""
```

#### `WheelRewardDatabase`

Persists accumulated rewards across sessions using `IJsonSaveService`. Exposes:

```csharp
event Action<string> OnRewardAmountChanged;  // fires per item ID
event Action OnRewardsReset;
void AddAmount(string itemId, int amount);
void Reset();
void SaveRewards();
```

Save key: `"wheel_reward_data"` → stored in PlayerPrefs as JSON.

#### `WheelSlotViewHandler`

Manages the lifecycle of `WheelSlotView` pooled objects. On a zone change, it compares the new slot count against the existing view count:

- **Same count** → reuse existing views, update icon/value in place
- **Different count** → return all to pool, allocate fresh set

#### `WheelCollectedRewardSlotHandler`

Populates `WheelCollectedRewardSlotView` items from `IWheelRewardDatabase.RewardEntries`. Each entry maps to one pooled slot view with its icon and accumulated value label. On each call, previous views are returned to pool before re-populating.

#### `WheelSpinAnimationModule`

DOTween-based animation component attached to the wheel's `Transform`:

```
Idle:   DOLocalRotate(-360°, duration=360/idleSpeed, LoopType.Incremental, Ease.Linear)
SpinTo: DOLocalRotate(targetAngle, spinDuration, RotateMode.FastBeyond360, customCurve)
```

Target angle calculation:
```
targetAngle = -slotIndex × 45°   (8 slots = 45° per slot)
delta       = targetAngle - currentAngle (unwound to always rotate backwards)
delta      -= fullRotationCount × 360°
endAngle    = currentAngle + delta
```

Animation config is fully designer-editable via `WheelSpinAnimationConfigSO`:
- `SpinDuration` (default: 6s)
- `IdleSpeed` in degrees/sec (default: 30)
- `FullRotationCount` (default: 2)
- `SpinCurve` — custom `AnimationCurve` for ease profile
- `UseUnscaledTime` — survives pause screens

---

## Wheel Zone Logic

Zone-to-wheel-type mapping in `WheelOfFortuneUtils`:

```csharp
if (zoneCount % 30 == 0) → WheelType.Gold    // Every 30th zone
if (zoneCount % 5  == 0) → WheelType.Bronze  // Every 5th zone (except Gold)
else                     → WheelType.Silver  // All other zones
```

Zone title display:
- **Silver** → `"ZONE {N}"`
- **Bronze** → `"BRONZE SPIN"`
- **Gold** → `"GOLDEN SPIN"`

Only the **Silver wheel** contains bomb slots. Hitting a bomb triggers `TryAgainButton`, resets the zone counter to 1, and clears the reward database.

---

## Reward System

### Reward Value Types

| Enum | Behaviour | Display |
|------|-----------|---------|
| `Numeric` | Scales with `ZoneCount × WheelMultiplier` | `"1.5k"`, `"2M"` |
| `Stackable` | Fixed multiplier only, accumulates as count | `"X3"` |
| `Unique` | No numeric value (e.g., weapons) | icon only |

### Configured Item IDs (`ItemId`)

| Category | IDs |
|----------|-----|
| Currency | `cash_01`, `gold_01` |
| Chests (tier) | `chest_silver_01`, `chest_bronze_01`, `chest_gold_01` |
| Chests (size) | `chest_small_01`, `chest_standart_01`, `chest_big_01`, `chest_super_01` |
| Weapons | `shotgun_tier1`, `mle_tier2`, `rifle_tier2`, `shotgun_tier3`, `smg_tier3`, `sniper_tier3` |
| Grenades | `m26_tier1`, `m67_tier1` |

---

## Spin Weight System

Each `WheelSlotData` carries a list of `WeightThreshold` entries:

```
WeightThreshold
├── ZoneCount  — minimum zone to activate this weight
└── Weight     — probability weight (relative, not percentage)
```

Active weight resolution:
```
Walk thresholds in order.
Apply the last threshold where threshold.ZoneCount <= currentZone.
Fall back to weight=1 if no threshold matches.
```

This allows reward probabilities to shift as the player progresses through zones without any code changes — purely data-driven.

---

## Data Configuration

### `GameConfigContainerSO`

Top-level config container assigned to `GameBootstrapper`:

| Field | Type | Purpose |
|-------|------|---------|
| `TargetFrameRate` | int | `Application.targetFrameRate` |
| `IsMultitouchEnabled` | bool | `Input.multiTouchEnabled` |
| `PoolServiceConfig` | `PoolServiceConfigSO` | Pool asset registry |
| `RewardVisualConfigContainer` | `RewardVisualConfigContainerSO` | Icon registry |
| `WheelOfFortuneConfigContainer` | `WheelOfFortuneConfigContainerSO` | Wheel configs |

### `WheelConfigSO`

One asset per wheel type (Bronze, Silver, Gold):

| Field | Notes |
|-------|-------|
| `WheelType` | Enum identifier |
| `ValueMultiplier` | Multiplies all `BaseValue` calculations (Bronze = 1.25) |
| `WheelVisuals` | Title color, indicator sprite, spinner sprite |
| `BombIcon` | Shown only on Silver wheel bomb slots |
| `WheelSlotData` | List of 8 `WheelSlotData` entries |

### Adding a New Reward

1. Add a `public const string` to `ItemId.cs`
2. Create a `RewardVisualDataSO` asset and assign its icon sprite
3. Register it in `RewardVisualConfigContaner` as a new `RewardTier` entry
4. Add a `WheelSlotData` entry referencing the new ID to any `WheelConfigSO`

---

## Dependencies

| Package | Usage |
|---------|-------|
| **DOTween** | Spin animation (`WheelSpinAnimationModule`), panel pop transitions (`SizeAnimationModule`) |
| **UniTask** | `async/await` for animation awaiting and view transitions |
| **TextMeshPro** | All in-game text on slot and result views |
| **NaughtyAttributes** | Inspector attributes (`[ReadOnly]`, `[ShowIf]`, `[Required]`, `[ValidateInput]`) |

Assembly layout:
- `Core.asmdef` — no game-layer dependencies
- `Game.asmdef` — references Core + all third-party packages
- `Editor.asmdef` — references Core, Editor-only

---

## Setup

1. Clone / import the `Scripts/` and `ScriptableObjects/` folders into your Unity project
2. Install required packages (DOTween, UniTask, TextMeshPro, NaughtyAttributes)
3. Create a scene with a `GameBootstrapper` MonoBehaviour
4. Assign `GameConfigContainer.asset` to the bootstrapper's `GameConfigContainer` field
5. Add a child `WheelOfFortuneBootstrapper` in the hierarchy (auto-detected via `OnValidate`)
6. Assign your `WheelOfFortuneView` prefab to the bootstrapper
7. Press Play — the bootstrapper wires all dependencies at `Awake`