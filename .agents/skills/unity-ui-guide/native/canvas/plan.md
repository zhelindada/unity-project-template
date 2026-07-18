# Canvas — 规划

## 决策清单

1. **渲染模式**：Overlay 占绝大多数场景。Camera 模式仅在需要 UI 跟随摄像机效果时使用。
2. **参考分辨率**：从 config 读取 `canvas.scaler.referenceResolution`，默认 1920×1080
3. **缩放方向**：竖屏=0，横屏=1，通用=0.5。节奏游戏（横屏）→ 1
4. **EventSystem**：交互式 UI 必须有。场景中存在一个即可。

## 前置条件

- 确认目标平台和屏幕比例
- 确认 config 已读取（`canvas.scaler.matchWidthOrHeight` 值）

## 创建顺序

1. `create_canvas` → 创建 Canvas + CanvasScaler + GraphicRaycaster
2. 检查或创建 `EventSystem`（`find_game_objects hasComponent=EventSystem`）
3. 开始创建 UI 子元素
