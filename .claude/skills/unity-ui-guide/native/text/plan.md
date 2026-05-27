# TextMeshPro — 规划

## 决策清单

1. **字体选择**：中文项目 → NotoSansSC 等中文字体。英文 → LiberationSans 即可。
2. **Point Size 检查**：如果是从现有字体切换，先检查新旧 SDF 的 `m_PointSize`。
3. **字号**：从 config `fonts.sizeByRole` 选择角色对应的字号
4. **颜色**：从 config 选择 textPrimary / textSecondary
5. **对齐**：标题居中、正文左对齐、按钮文字居中
6. **溢出处理**：短标签用 Overflow，长内容考虑 Wrapping + Truncate

## 字体切换清单

```
□ 检查新字体 Atlas Point Size
□ 如果 Point Size 与原字体不同 → 重建字体 or 调整所有 fontSize
□ 确认新字体包含目标字符集（中文/Extended ASCII）
□ 设置 font 属性（需完整路径，如 Assets/.../FontName SDF.asset）
```
