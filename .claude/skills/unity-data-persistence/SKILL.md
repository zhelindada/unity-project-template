---
name: unity-data-persistence
description: 数据持久化系统（Save/Load）——基于 Data Binding + Serializable DTO + 可替换存储后端（File/JSON 默认）。支持 GUID 实体标识，保存多个对象（玩家/背包/敌人）。Use when 需要实现存档/读档、需要保存场景中多个实体的独立状态、或需要可替换的存储后端（本地文件→云端）。
---

# Unity Data Persistence

**Tier:** POWERFUL
**Category:** Unity / Gameplay Systems
**Tags:** Unity, save, load, persistence, data binding, DTO, serialization, GUID

Implement a professional Save/Load system built around **data binding**: saveable objects hold a live reference to a DTO that is always up-to-date, making serialization near-instant and backend-agnostic.

Inspired by [adammyhre/Unity-Inventory-System](https://github.com/adammyhre/Unity-Inventory-System/tree/master/Assets/_Project/Scripts/Persistence).

## Core Architecture

```
SaveLoadSystem
    ├── IDataService  (FileDataService by default)
    │       └── ISerializer  (JsonSerializer by default)
    └── GameData  (your root DTO)
            ├── PlayerData   (ISaveable) ←─ Hero.Bind()
            └── InventoryData (ISaveable) ←─ Inventory.Bind()
```

## Core Features

1. **Data Binding Pattern**: MonoBehaviours hold a reference to their own DTO, so saving is just serializing GameData — no polling, no property walking.
2. **IBind\<T\> / ISaveable**: Two interfaces that wire scene entities to their save data at scene load time.
3. **SerializableGuid**: Stable, fast GUID identity (int-based comparison) for matching saved data to spawned entities.
4. **Swappable Backend**: `IDataService` + `ISerializer` let you change from local JSON to Cloud/Binary with a one-line change.
5. **Multi-Entity Binding**: List-based `Bind<T, TData>(List<TData>)` handles many-entity scenarios (enemies, spawners).

## Core Files (Max 3)

- `PersistenceCore.cs.txt` — `ISerializer`, `JsonSerializer`, `IDataService`, `ISaveable`, `IBind<T>`, `SerializableGuid`
- `FileDataService.cs.txt` — Local disk implementation: Save, Load, Delete, DeleteAll, ListSaves
- `SaveLoadSystem.cs.txt` — Persistent MonoBehaviour orchestrator with `Bind<T,TData>()` and public Save/Load/New API

## Usage

### Quick Start (Light Path — no binding needed)
```csharp
// 1. Extend GameData with your fields
[Serializable] public class GameData {
    public string Name;
    public int Score;
}

// 2. Save / Load
SaveLoadSystem.Instance.gameData.Score = 100;
SaveLoadSystem.Instance.SaveGame();
SaveLoadSystem.Instance.LoadGame("MySave");
```

### Full System (Pro Path — data binding)
```csharp
// 1. Create a DTO
[Serializable] public class PlayerData : ISaveable {
    [field: SerializeField] public SerializableGuid Id { get; set; }
    public Vector3 position;
}

// 2. Implement IBind on your MonoBehaviour
public class PlayerEntity : MonoBehaviour, IBind<PlayerData> {
    [SerializeField] SerializableGuid id;
    PlayerData data;
    public SerializableGuid Id { get => id; set => id = value; }
    public void Bind (PlayerData d) { data = d; data.Id = id; transform.position = d.position; }
    void Update () { data.position = transform.position; } // write-through
}

// 3. Register in SaveLoadSystem.OnSceneLoaded
Bind<PlayerEntity, PlayerData>(gameData.playerData);
```

See `references/guide.md` for complete examples including multi-entity binding and backend swapping.

## Key Principle

Design your data model first. The DTO structure IS your save format. Start simple (`GameData` with flat fields) — add binding only when entities need independent, restorable state.
