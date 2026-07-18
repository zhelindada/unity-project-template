# Evaluation Checklist

## 检查清单总览

共 7 项必检 + 3 项建议检查。必检项全部通过才算原型合格。

## 必检项（7 项）

### E1. 场景可运行不报错
- **测试方法**: `get_runtime_events` 收集所有 Error/Exception 级别日志
- **通过标准**: 0 条 Error 级别日志，0 条 Exception
- **常见失败**:
  - NullReferenceException（未赋值的 public field）
  - MissingReferenceException（已销毁但仍在引用的 GameObject）
  - UnassignedReferenceException（Inspector 中未拖入引用）

### E2. 玩家可移动 + 核心交互
- **测试方法**: 进入 Play Mode，按 WASD 确认角色移动，按空格确认跳跃
- **通过标准**: 
  - WASD → 角色在水平面移动，方向正确
  - 空格 → 角色垂直跳跃，落回地面
  - 移动无卡顿/瞬移/穿墙
- **常见失败**:
  - 角色不动 → 检查 Input Manager 设置（Horizontal/Vertical 轴是否存在）
  - 角色飞天 → 重力向量方向错误
  - 角色穿墙 → 墙壁缺少 Collider

### E3. 核心 Gameplay 循环可完整体验
- **测试方法**: 从场景启动开始，在 30 秒内完成一次核心循环
- **通过标准**: 循环完整可走通（如：移动靠近敌人 → 敌人追击 → 玩家逃脱或死亡）
- **常见失败**:
  - 敌人不动 → EnemyAI 未找到 Player（tag 未设置）
  - 无敌方反馈 → 缺乏状态变化的视觉提示

### E4. Camera 跟随正常
- **测试方法**: 移动角色，观察 Camera 行为
- **通过标准**:
  - Camera 平滑跟随 Player
  - 不穿墙（或至少不穿出关卡边界）
  - 不剧烈抖动
- **常见失败**:
  - Camera 不动 → target 未正确赋值
  - 剧烈抖动 → LateUpdate 中 SmoothDamp 参数过小
  - 穿墙 → 原型阶段可接受（标注 ⚠️）

### E5. UI 显示正确
- **测试方法**: 进入 Play Mode，检查 Canvas 下所有 UI 元素
- **通过标准**:
  - HUD 元素可见且在屏幕正确位置
  - 文字清晰可读（TMP 字体未缺失）
  - 动态元素（血条/分数）初始值正确
- **常见失败**:
  - UI 不显示 → Canvas 未激活 / EventSystem 缺失
  - 中文乱码 → TMP 字体不支持中文（原型用英文）

### E6. 帧率 ≥ 30fps
- **测试方法**: 进入 Play Mode，查看 Game 视图 Stats 面板
- **通过标准**: FPS ≥ 30（允许短暂掉帧）
- **常见失败**:
  - 极低帧率 → 无限循环 / 每帧 GetComponent / 过多 Debug.Log
- **注意**: 编辑器内帧率偏低是正常的，Game 视图实测为准

### E7. 无 NullReferenceException
- **测试方法**: 同 E1，但专门过滤 NullReferenceException
- **通过标准**: 运行 10 秒内 0 条 NullReferenceException
- **说明**: 这是最常见的运行时崩溃原因，单独列出以确保重视

## 建议检查项（3 项）

### R1. 物理参数合理
- CharacterController 的 stepOffset ≤ 0.3（避免卡楼梯感）
- 重力值 -15~-25（太大会飘，太小会沉）
- 移动速度 3-8（太慢会无聊，太快会失控）

### R2. 边界安全
- 角色不会掉出关卡（如有边界墙）
- 角色不能无限跳跃（isGrounded 检查有效）
- 快速连按空格不会导致异常状态

### R3. Inspector 清理
- 所有 public/serialized field 都有默认值或已赋值
- 无 "(Missing)" 引用
- 无不必要的 Debug.Log 输出

## 检查执行顺序

```
E1 ──→ E2 ──→ E3 ──→ E4 ──→ E5 ──→ E6 ──→ E7
                                           ↓
                              全部通过 → [R1,R2,R3] 建议项
```

E1 是前置条件——如果场景不能无错运行，后续测试不可信。

## 判定矩阵

| 必检项 | 建议项 | 结论 |
|--------|--------|------|
| 7/7 通过 | 不限 | ✅ 可玩 |
| 5-6/7 通过 | 不限 | ⚠️ 有条件可玩（标注失败项） |
| ≤4/7 通过 | 不限 | ❌ 需人工介入 |
