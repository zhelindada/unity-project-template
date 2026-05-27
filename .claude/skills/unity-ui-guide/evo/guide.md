# Evo UI 完整使用指南

> 基于 [Evo UI 官方文档](https://evo.michsky.com/docs/evo-ui) 整理。

---

## 1. 安装与配置

### 安装步骤

1. 将 Evo UI 包导入兼容的 Unity 项目（Unity 6 或 2022.3）
2. Unity 弹出依赖检测弹窗 → 点击 **Install/Upgrade** 自动安装缺失包
3. 必需依赖：**2D Sprite**、**Unity UI (uGUI)**、**TextMesh Pro**
4. 如使用旧 Input Manager，在 **Project Settings → Player → Active Input Handling** 中配置

### 更新注意事项

通过 Unity Package Manager 更新 Evo UI 后：
- **仔细审查 import 对话框**再确认
- **取消勾选**任何你修改过的文件，特别是：
  - 自定义 Styler Preset
  - Resources 文件夹中编辑过的图标或预设
- Unity 可能会覆盖你的自定义更改

---

## 2. Styler 系统

Styler 是 Evo UI 最核心的架构特性——一个模块化的全局样式控制系统。

### 架构模型

Styler 由两部分组成：

```
Styler Preset (主题数据)  ────→  Styler Object (主题接收器)
   ScriptableObject                 附加到 UI GameObject 上
   存储颜色/字体/精灵/音频           绑定到 Preset 的某个 ID
```

修改 Preset 的值 → 所有绑定的 Styler Object 自动更新。

### Styler Preset

- **类型**：ScriptableObject
- **存储内容**：Audio Clips、Colors、Fonts (TMP_FontAsset)、Sprites
- 每种资源按 **字符串 ID** 索引
- **默认位置**：`Resources/Styler Presets/Default`
- **打开方式**：Tools → Evo UI → Open Default Styler（快捷键 `Ctrl+Shift+M`）

**创建 Preset：**
- Project 窗口右键 → Create → Evo → UI → Styler Preset
- 推荐：复制已有 Preset 来创建 Light/Dark/High Contrast 等主题变体

### Styler Object

附加到 GameObject 上的组件，将 Preset 应用到该对象。

**支持的目标类型：**
| 类型 | 适用组件 |
|------|---------|
| Graphic | Image、RawImage、ProceduralRect 等 |
| Image | 基于 Sprite 的元素 |
| TMP Text | TextMeshPro 文本组件 |
| Interactive | Button、Switch 等有状态的交互控件 |

**添加方式：** 选中对象 → Add Component → 搜索 "Styler Object"

**绕过全局样式 (Override)：**
1. 启用 **Use Custom Color** → 该对象忽略 Preset 中对应 ID 的颜色
2. 直接移除 Styler Object 组件

**交互状态支持：**
Styler Object 可挂钩到交互组件，自动为各状态（hover/pressed/disabled）应用主题色。每个状态可引用 Styler color ID，也可启用 Use Custom Color 进行局部覆盖。

**Override Alpha：**
- `overrideAlpha` (bool) + `overrideAlphaValue` (float)
- 可独立于 Preset 颜色调整透明度

### Styler Browser

**Tools → Evo UI → Open Styler Browser**

功能：
- **管理** — 创建、复制、重命名、删除 Preset
- **Apply to Scene** — 将选中的 Preset 立即应用到场景中所有支持的对象
- **Apply to Selection** — 仅应用到选中的 StylerObject 或实现 IStylerHandler 的组件
- Apply to Scene 支持 **Undo (Ctrl+Z)**，可安全试验主题

### 全局主题切换（运行时）

```csharp
using UnityEngine;
using Evo.UI;

public class ThemeManager : MonoBehaviour
{
    public StylerPreset darkTheme;
    public StylerPreset lightTheme;

    public void SetDarkTheme()
    {
        Styler.ApplyPreset(darkTheme);
    }

    public void SetLightTheme()
    {
        Styler.ApplyPreset(lightTheme);
    }
}
```

`Styler.ApplyPreset()` 会更新场景中所有实现了 `IStylerHandler` 的对象（包含所有 StylerObject）。

### StylerPreset API

```csharp
// 获取
AudioClip sfx     = preset.GetAudio("Click");
Color accent      = preset.GetColor("Accent");
TMP_FontAsset f   = preset.GetFont("Regular");

// 添加
preset.AddColor("Warning", accentColor);
preset.AddAudio("Error SFX", clickSound);
preset.AddFont("Header", newFont);

// 修改
preset.SetColor("Warning", Color.yellow);
preset.SetAudio("Error SFX", clickSound);
preset.SetFont("Header", newFont);

// 移除
preset.RemoveColor("Warning");
preset.RemoveAudio("Error SFX");
preset.RemoveFont("Header");
```

### StylerObject API

```csharp
stylerObject.preset = preset;
stylerObject.objectType = StylerObject.ObjectType.Graphic;
stylerObject.colorID = "Primary";
stylerObject.fontID = "Regular";
stylerObject.overrideAlpha = true;
stylerObject.overrideAlphaValue = 0.7f;
stylerObject.useCustomColor = true;
stylerObject.UpdateStyler();       // 提交所有更改
```

### Styler 最佳实践

- 为不同模式创建多个 Preset（如色盲模式），运行时切换
- 使用 `Styler.ApplyPreset()` 一键切换整场景主题
- 对需要固定外观的特定元素使用 Use Custom Color 覆盖

---

## 3. 图标系统

- **Icon Selector**：Tools → Evo UI → Open Icon Selector
- 内置图标浏览器，无需手动在数百个图标中寻找
- 图标作为 Evo UI 包的一部分提供

---

## 4. Procedural Rect

- 程序化生成的矩形形状
- 位于 Evo UI 组件菜单中
- 可用于面板背景、分隔线等基础形状元素

---

## 5. Evo Localization

- 与 Evo UI 集成的本地化系统
- 详情见官方文档的 Evo Localization 页面

---

## 6. UI 元素创建

### 创建方式对比

| 方式 | 何时使用 | 引用配置 |
|------|---------|---------|
| Create 菜单 | 新建完整 UI 元素 | 自动配置 |
| Add Component | 给已有对象追加功能 | 需手动配置 |

### 创建流程

1. **顶部菜单** → Evo UI 子菜单，或 **右键 Hierarchy** → Evo UI
2. 选择目标元素类型
3. 元素自动创建完毕，所有引用已正确设置

### Add Component 方式

1. 选中 GameObject → Add Component
2. 浏览 **Evo → UI** 或搜索组件名
3. 手动分配所有必需引用

---

## 7. 自定义编辑器 GUI

**Tools → Evo UI → Disable Custom Editor** 可关闭自定义编辑器。

注意：
- 部分复杂组件会忽略此设置（如 List View、UI Animator）
- 此选项主要用于修改源码时避免编辑器脚本干扰

---

## 8. Dynamic Scale（动态缩放）

按钮组件的一个重要特性，三条关键规则：

| 场景 | Dynamic Scale |
|------|--------------|
| 固定尺寸按钮 | **禁用** |
| 按钮在 Layout Group 内 | **禁用**（与布局组的尺寸逻辑冲突） |
| 按钮文本自动增长、按钮需自适应 | **启用** |

---

## 9. 条件编译

Evo UI 自动定义 `EVO_UI` 编译符号，可用于条件编译：

```csharp
#if EVO_UI

void Start()
{
    Debug.Log("Evo UI is installed!");
}

#endif
```

---

## 10. 工作流程

### 创建新 UI 的标准流程

1. **确定是否使用 Styler** — 新项目建议全用 Styler 驱动，确保一致性
2. **设置 Styler Preset** — 在 Default Preset 中配置项目配色和字体
3. **用 Create 菜单创建 UI 元素** — 确保引用自动配置
4. **根据需要 Fine-tune** — 用 Use Custom 覆盖特定属性
5. **运行时主题切换** — 通过 `Styler.ApplyPreset()` 实现

### MCP 操作指南

使用 GladeKit 操作 Evo UI 时：

1. 创建 Canvas → 使用 `create_canvas`
2. 创建 UI 元素 → 使用 `create_ui_element`（Evo UI 元素基于 uGUI，最终仍是 GameObject + RectTransform + 组件）
3. Evo 组件 → 通过 `add_component` 添加（如 `StylerObject`）
4. 设置 Preset → 通过 `set_component_property` 或代码 API

### 排查清单

- [ ] Evo UI 包是否正确导入（检查 `EVO_UI` define symbol）
- [ ] Styler Preset 是否存在于 `Resources/Styler Presets/`
- [ ] Styler Object 是否已挂载且正确配置 colorID/fontID
- [ ] Layout Group 内的按钮 Dynamic Scale 是否已禁用
- [ ] 更新包后自定义 Preset 是否被覆盖
