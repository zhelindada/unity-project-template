# TextMeshPro — 文本显示

TextMeshProUGUI 是 Unity UI 中的标准文本组件，用于 Canvas 下的所有文字显示。

## 关键参数

```
Font Asset:     (TMP SDF 字体)   ← ★ 必须设置，否则空白
Font Size:      24                ← 从 config 读取
Font Color:     (1,1,1,1)        ← 文字渲染颜色
Face Color:     (1,1,1,1)        ← 面颜色
Outline Color:  (0,0,0,1)
Outline Thickness: 0
```

## 字号参考（config）

| 角色 | 字号范围 | 默认 |
|------|---------|------|
| title | 28-48 | 36 |
| heading | 22-34 | 28 |
| body | 18-28 | 24 |
| caption | 14-22 | 18 |
| button | 18-28 | 22 |

## 字体选择

**中文项目必须使用中文字体**（如 NotoSansSC SDF）。LiberationSans 等英文字体不含中文字形，中文会显示为 □。

## 字体 Point Size 陷阱

不同 SDF 字体有不同的 **Sampling Point Size**。TMP 渲染缩放公式：

```
实际像素大小 ≈ fontSize / fontAsset.pointSize
```

| 字体 | Point Size | fontSize=36 效果 |
|------|-----------|-----------------|
| LiberationSans | 86 | 0.42× |
| NotoSansSC | 13（Auto Sizing 自动生成） | 2.77×（大 6.6 倍！） |

**规则**：
- 换字体前先检查新旧 SDF 的 `m_PointSize`（Grep `.asset` 文件）
- 不一致时要么重建字体统一 Point Size，要么全局调整 fontSize
- 重建字体 → Font Asset Creator → Sampling Point Size 切为 **Custom Size**

## Fallback 渲染

当主字体缺少某个字符时，TMP 自动 fallback 到后备字体。不同字体的 Atlas Point Size 不同会导致同一 Text 内字符大小不一致。

**识别**：场景层级出现 `TMP SubMeshUI [主字体材质 + 后备字体 Atlas]` 子对象。

**修复**：重建字体时用 Extended ASCII 字符集，确保所有符号都在主 Atlas 中。

## 对齐与溢出

```
Horizontal Alignment: Center / Left / Right
Vertical Alignment:   Middle / Top / Bottom
Wrapping:             true
Overflow:             Overflow / Ellipsis / Masking / Truncate
Rich Text:            true
```
