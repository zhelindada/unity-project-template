# TextMeshPro — 检查

## 快速验证

```
□ Font Asset 非 null
□ Font Asset 支持目标字符集（中文 → 中文字体）
□ Font Color.a > 0（文字可见）
□ Font Size ≥ 12px（config 硬性约束）
□ 文字 alpha ≥ 0.8（正文可读性约束）
□ 无 TMP SubMeshUI 子对象（无 fallback，或 fallback 比例正常）
```

## 常见错误

| 问题 | 原因 | 修复 |
|------|------|------|
| 中文显示为 □ | 字体不含中文字形 | 换为中文字体 |
| 切字体后文字变大/变小 | 新旧 SDF Point Size 不同 | 重建字体统一 Point Size |
| 同一 Text 内字符大小不一 | 缺字符 fallback 到不同比例字体 | 重建字体包含全部字符 |
| 文字完全不可见 | color.a = 0 或 font = null | 检查 Color 和 Font Asset |
| 设置 font 属性失败 | 路径不完整 | 用完整 Asset 路径 |
