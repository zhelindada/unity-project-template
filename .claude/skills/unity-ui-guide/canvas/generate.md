# Canvas — 生成

## 创建 Canvas

使用 `create_canvas` 工具，自动附带 Canvas + CanvasScaler + GraphicRaycaster。

```
create_canvas name="MyCanvas"
```

## 配置 CanvasScaler

```
set_component_property CanvasScaler uiScaleMode = "ScaleWithScreenSize"
set_component_property CanvasScaler referenceResolution = "1920,1080"
set_component_property CanvasScaler screenMatchMode = "MatchWidthOrHeight"
set_component_property CanvasScaler matchWidthOrHeight = "1"     ← 横屏游戏
```

## 确保 EventSystem 存在

```
find_game_objects nameContains="EventSystem"
```

若无 → `create_event_system`

## 验证

```
get_gameobject_info Canvas → active=true, components 含 Canvas/CanvasScaler/GraphicRaycaster
get_scene_hierarchy → 确认 EventSystem 存在
```
