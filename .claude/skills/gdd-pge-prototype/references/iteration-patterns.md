# Iteration Patterns

## 问题分类 → 修复模式速查

### 编译错误类

| 错误类型 | 常见原因 | 修复模式 |
|---------|---------|---------|
| `CS0246: 找不到类型或命名空间` | using 缺失 / 类名拼写错误 | 添加 `using UnityEngine;` 或修正类名 |
| `CS0103: 名称不存在` | 变量未声明 / 方法名错误 | 检查拼写，确认变量在作用域内 |
| `CS1061: 不包含定义` | 类型不匹配 / API 版本差异 | 确认组件类型正确（如 Rigidbody vs Rigidbody2D） |
| `CS0029: 无法隐式转换` | GetComponent 返回类型错误 | 使用正确的泛型参数 |
| `CS0117: 不包含定义` | Input System API 差异 | 确认新旧 Input System，改用对应 API |

### 运行时错误类

| 错误类型 | 常见原因 | 修复模式 |
|---------|---------|---------|
| `NullReferenceException` | 未赋值的 serialized field | 在 Start() 中用 FindObjectOfType / GetComponent 自动查找 |
| `MissingReferenceException` | GameObject 已销毁但仍被引用 | 在 OnDestroy 中清理引用，或在访问前判空 |
| `UnassignedReferenceException` | Inspector 中未拖入引用 | 同上，添加自动查找 fallback |
| `IndexOutOfRangeException` | 数组/列表越界 | 添加 `if (index < list.Count)` 保护 |

### 物理/碰撞类

| 问题 | 根因 | 修复 |
|------|------|------|
| 角色穿过地面 | Ground 无 Collider | 给 Plane 添加 MeshCollider（默认自带） |
| 角色穿墙 | 墙壁无 Collider | 给墙壁 Cube 确认 BoxCollider 存在 |
| 角色跳跃后悬浮 | gravity 方向为正 / 值太小 | 确保 gravity = -15~-25，向下 |
| 角色不落地 | CharacterController.isGrounded 误判 | 检查 skinWidth ≤ 0.08，stepOffset ≥ 0.1 |
| 敌人浮空 | 未处理 Y 轴速度 | 在 EnemyAI.Update 中添加简易重力 |
| 两个物体重叠区撞飞 | Collider 未设为 Trigger | 道具/触发器用 `isTrigger=true` |

### Camera 类

| 问题 | 根因 | 修复 |
|------|------|------|
| Camera 不动 | target 未赋值 | Start() 中 `target = GameObject.FindGameObjectWithTag("Player")?.transform` |
| Camera 剧烈抖动 | LateUpdate 在 FixedUpdate 之后执行 | 用 `Time.deltaTime` 而非 `Time.fixedDeltaTime` |
| Camera 初始位置错误 | offset 使用了 world-space 而非 target-relative | 确保 offset 相对于 target：`target.position + offset` |
| Camera 初始在原点看天空 | 没有在场景中设置初始位置 | set_transform 将 MainCamera 放到 Player 后方上方 |

### UI 类

| 问题 | 根因 | 修复 |
|------|------|------|
| Canvas 空白 | EventSystem 缺失 | create_event_system |
| TMP 文字不显示 | TMP Essential Resources 缺失 | import_tmp_essential_resources |
| 血条不更新 | 未在 Update 中刷新 fillAmount | 添加 `healthBar.fillAmount = currentHealth / maxHealth` |
| UI 位置偏移 | anchoredPosition 设置错误 | 确认 Canvas Scaler 模式，修正锚点位置 |

### 输入类

| 问题 | 根因 | 修复 |
|------|------|------|
| WASD 无效 | Input Manager 轴未配置 | 确认 Project Settings > Input Manager 中 Horizontal/Vertical 存在 |
| 空格跳跃无效 | "Jump" 按钮名大小写错误 | 使用 `Input.GetButtonDown("Jump")` — J 大写 |
| New Input System 报错 | 项目使用新系统但脚本用旧 API | 读取 ProjectConfiguration，确认使用正确 API |

## 多轮迭代策略

### 第 1 轮（编译门禁）
目标：编译通过。
- 修复所有 CS 编译错误
- 不关注逻辑正确性，只关注编译

### 第 2 轮（启动门禁）
目标：进入 Play Mode 不崩。
- 修复所有 NullReferenceException / MissingReferenceException
- 此时原型只需"不报错"，"不要求功能完整"

### 第 3 轮（功能门禁）
目标：核心循环可走通。
- 修复玩法功能缺失（移动/跳跃/敌人行为）
- 修复 Camera 跟随

### 第 4 轮（体验门禁）
目标：UI 正确 + 帧率稳定。
- 修复 HUD 显示问题
- 消除明显的卡顿/抖动

### 第 5 轮（最终确认）
目标：全部检查通过，输出报告。
- 逐项验证检查清单
- 标注剩余已知问题

## 迭代终止判断

```
第 N 轮后：
├── 全部通过 → 退出循环 ✅
├── 有进展（通过数增加）→ 继续下一轮
├── 无进展（连续 2 轮）→ 暂停，报告用户：
│   "PGE 在第 N 轮后无新进展。当前: M/7 通过。
│    失败项: [E?] ...
│    请决定：继续尝试 / 调整原型范围 / 接受当前状态"
└── 达到 5 轮 → 输出原型 + 已知问题清单，标注 ⚠️
```
