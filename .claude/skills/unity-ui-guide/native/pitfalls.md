# 踩坑经验 —— 实际排查中遇到的高频深坑

> 此文件由真实排查案例积累而成，是 `check.md` 中四大常见错误的深度补充。
> 对应关系：[check.md](check.md) 的「快速排查表」→ 本文档的「深层原因 + 修复方案」。

---

## 1. TMP 字体相关深坑

### 1.1 切换字体后所有文字大小剧变

**现象**：把 TMP 的 Font Asset 从 A 字体换成 B 字体后，所有文字突然变大或变小，fontSize 参数完全没动过。

**根因**：不同 SDF 字体资产的 **Sampling Point Size**（采样字号）不同。

TMP 的渲染缩放公式为：
```
实际像素大小 = fontSize / fontAsset.pointSize
```

pointSize 是生成 SDF Atlas 时的采样基准。例如：

| 字体 | Point Size | fontSize=36 时的渲染比例 |
|------|-----------|-------------------------|
| LiberationSans SDF | 86 | 36/86 ≈ 0.42× |
| NotoSansSC SDF | 13 | 36/13 ≈ 2.77× |

同一 `fontSize=36` 在 NotoSansSC 下渲染为 LiberationSans 的 **6.6 倍**。

**查看字体 Point Size**：用 Grep 搜索 `.asset` 文件中的 `m_PointSize`。

**修复**：
1. **根治**：Font Asset Creator → Sampling Point Size 切为 Custom Size → 填与原字体相同的值
2. **临时**：等比缩放所有 fontSize，或切回原字体

**教训**：替换项目字体前，先检查新旧 SDF 的 Point Size 是否一致。不一致时要么重建字体统一 Point Size，要么全局调整 fontSize。

---

### 1.2 同一 Text 内不同字符大小不一致

**现象**：像 `Start` 里的 "S""t""a""r""t" 显示正常，但 `<` `>` 明显偏小或偏大。

**根因**：主字体的 SDF Atlas 中不包含这些字符，TMP 自动 fallback 到后备字体渲染。不同字体的 Point Size 不同 → 同一个 Text 组件内，字符来自不同 Atlas → 渲染比例不同。

**识别方法**：场景层级中出现 `TMP SubMeshUI [主字体材质 + 后备字体 Atlas]` 子对象，即说明发生了 fallback。

**修复**：
1. **根治**：重建主字体时用 Extended ASCII 或自定义字符集，确保所有会用到的字符都在主 Atlas 中
2. **临时**：切回包含所有目标字符的原字体

---

### 1.3 中文项目使用英文字体 → 方块 □

**现象**：所有中文字符显示为 □。

**根因**：Font Asset 是 LiberationSans 等仅包含拉丁字符的字体，没有中文字形。

**修复**：
- 使用支持中文的 TMP 字体：`set_component_property TextMeshProUGUI font = "Assets/.../NotoSansSC SDF.asset"`
- 注意：切换后参考 1.1 检查 Point Size 是否一致

---

## 2. Slider 相关深坑

### 2.1 Slider 在 LayoutGroup 下高度为 0

**现象**：Slider 明明有 Background、Fill Area/Fill、Handle Slide Area/Handle 完整子结构，FillRect 和 HandleRect 引用也设置正确，但在 Game 视图中完全看不到。

**根因**：Slider 的父节点有 VerticalLayoutGroup（或 HorizontalLayoutGroup），LayoutGroup 每帧会重新计算并覆盖子元素的 `sizeDelta`。若 Slider 自身 `SizeDelta.y = 0`，LayoutGroup 不会自动分配高度。子对象虽有正确的 Anchor 拉伸，但全被父级 0 高度压缩到不可见。

**排查**：
```
get_component_inspector_properties RectTransform → SizeDelta.y > 0 ？
get_gameobject_components 父节点 → 是否有 VerticalLayoutGroup / HorizontalLayoutGroup ？
```

**修复**：给 Slider 添加 `LayoutElement` 并设 `preferredHeight`。

```
add_component LayoutElement
set_component_property LayoutElement preferredHeight = 20
```

LayoutGroup 会在计算尺寸时优先读取子元素的 LayoutElement 约束，从而给 Slider 分配正确高度。

**教训**：直接设 `sizeDelta` 在 LayoutGroup 下无效（会被每帧覆盖），**LayoutElement 是唯一绕过方式**。

---

### 2.2 Slider 缺子对象 → 无视觉反馈

**现象**：Slider 组件挂上去了，Interactable=true，但完全看不到轨道和手柄。

**根因**：只创建了 Slider GameObject + 挂 Slider 组件，没有创建子对象。Slider 依赖子对象来显示：
- Background → 轨道
- Fill Area / Fill → 填充进度条
- Handle Slide Area / Handle → 可拖拽手柄

三个引用 `Fill Rect`、`Handle Rect`、`Target Graphic` 全为 null。

**修复**：用 `create_ui_element(type="Slider")` 一次性创建完整层级，或手动补全子对象并设置引用。

---

## 3. Canvas 下创建 UI 子对象的正确方式

### 3.1 create_game_object vs create_ui_element

**现象**：在 Canvas 下用 `create_game_object` 创建子节点后，无法设置 RectTransform 属性，`set_component_property RectTransform` 报 "not found"。

**根因**：`create_game_object` 生成的是 `Transform`，不是 `RectTransform`。Canvas 下的所有子对象必须有 RectTransform 才能正常布局和渲染。

**修复**：Canvas 下创建任何 UI 子对象，始终用 `create_ui_element`：

| 需求 | 方法 |
|------|------|
| 透明容器（Fill Area 等） | `create_ui_element(type="Image", color="1,1,1,0")` |
| 可见元素 | `create_ui_element(type="Image/Button/...")` |

- ❌ `create_game_object` → Transform，Canvas 下无法正确定位
- ✓ `create_ui_element` → Image + RectTransform，可在 Canvas 中正常工作

---

## 4. CanvasScaler 方向匹配

**现象**：UI 在不同分辨率下缩放异常，元素位置偏移。

**根因**：`matchWidthOrHeight` 与游戏方向不匹配：
- `0` = 按宽度缩放（竖屏手游适用）
- `1` = 按高度缩放（横屏游戏适用）
- `0.5` = 综合缩放（通用桌面/平板）

**修复**：按游戏实际方向设置。横屏节奏游戏设为 `1`，竖屏手游设为 `0`。

---

## 引用

- [check.md](check.md) — 快速排查流程 + 参考卡
- [generate.md](generate.md) — Slider/Button/TMP 等组件的完整参数参考
- [SKILL.md](SKILL.md) — 完整工作流（规划→生成→检查）
