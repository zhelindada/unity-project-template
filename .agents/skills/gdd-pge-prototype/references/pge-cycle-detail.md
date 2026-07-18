# PGE Cycle Detail

## Phase 1: Plan — 详细步骤

### 1.1 GDD 信息提取表

| GDD 章节 | 提取内容 | 落到 Plan Card 的哪里 |
|----------|---------|---------------------|
| `02-gameplay/01-player-mechanics.md` | 核心动词（移动/跳跃/攻击/收集） | 必须实现列表 |
| `02-gameplay/02-combat-design.md` | 敌人类型、伤害公式、血量 | 敌人配置、HUD 需求 |
| `03-level-design/02-level-one-pagers/` | 第一个关卡的地形、敌人点位 | 关卡灰盒布局 |
| `04-ui-ux/01-hud-wireframes.md` | HUD 元素（血条/分数/提示） | UI 元素列表 |
| `06-tech-design/01-architecture.md` | 输入系统、物理方案、渲染管线 | 技术约束 |

### 1.2 Plan Card 模板

```
📋 Plan Card — [游戏名称] 原型
═══════════════════════════════════════
核心验证假设:
  [一句话，可证伪]

必须实现:
  ✅ 机制 1: [如：WASD 移动]
  ✅ 机制 2: [如：空格跳跃]
  ✅ 机制 3: [如：1 种敌人巡逻+追击]

不做（原型范围外）:
  ❌ [如：攻击系统]
  ❌ [如：道具/背包]

关卡布局:
  [ASCII 俯视图或文字描述]
  地面尺寸: WxH
  墙壁/障碍: [位置]
  敌人点位: [(x,z)]

HUD 需求:
  - [如：左上角血条（Image Filled）]
  - [如：右上角击杀数（TMP Text）]

成功标准:
  ✅ 标准 1: [可测试描述]
  ✅ 标准 2: [可测试描述]
  ✅ 标准 3: [可测试描述]

风险点:
  ⚠️ 风险 1: [描述 + 缓解措施]
  ⚠️ 风险 2: [描述 + 缓解措施]
```

### 1.3 框架检测流程

```
检测项目根目录:
├── 有 Assets/ + .unity 文件? → Unity
│   ├── 读取 Packages/manifest.json → 确认渲染管线
│   ├── 读取 ProjectSettings/ProjectSettings.asset → 确认 Input System
│   └── 策略: 使用 gladekit-unity MCP 工具
├── 有 *.uproject? → Unreal
│   └── 策略: 输出 C++/Blueprint 代码 + 手动操作指引
├── 有 project.godot? → Godot
│   └── 策略: 输出 .gd 脚本 + .tscn 场景文件
└── 有 *.csproj / Cargo.toml? → 纯代码框架
    └── 策略: 直接生成代码文件
```

## Phase 2: Generate — 详细步骤

### 2.1 Unity 场景搭建序列（gladekit-unity MCP）

```
Step 1: 清理旧原型
  → destroy_game_object "Prototype" (如果存在)

Step 2: 创建地面
  → create_primitive Plane "Ground" (scale: 20,1,20)
  → set_transform position: 0,0,0
  → create_material "Materials/Ground_Grey.mat" (color: 0.4,0.4,0.4,1)
  → assign_material_to_renderer

Step 3: 创建玩家
  → create_primitive Capsule "Player"
  → set_transform position: 0,1,0
  → add_component CharacterController
  → set_tag "Player"
  → create_material "Materials/Player_Blue.mat" (color: 0,0.3,1,1)
  → assign_material_to_renderer

Step 4: 创建敌人
  → create_primitive Cube "Enemy_01" (position: 5,0.5,0)
  → set_tag "Enemy"
  → create_material "Materials/Enemy_Red.mat" (color: 1,0.1,0.1,1)
  → assign_material_to_renderer

Step 5: 创建墙壁/障碍
  → create_primitive Cube "Wall_North" (position: 0,2,-10, scale: 20,4,1)
  → create_primitive Cube "Wall_South" (position: 0,2,10, scale: 20,4,1)
  → create_primitive Cube "Wall_East" (position: 10,2,0, scale: 1,4,20)
  → create_primitive Cube "Wall_West" (position: -10,2,0, scale: 1,4,20)
  → create_material "Materials/Wall_Dark.mat" (color: 0.2,0.2,0.2,1)
  → assign_material_to_renderer (全部 4 个)

Step 6: GameManager
  → create_game_object "GameManager"
  → create_script GameManager.cs
  → add_component GameManager

Step 7: 脚本挂载
  → create_script PlayerController.cs → add_component PlayerController to "Player"
  → create_script EnemyAI.cs → add_component EnemyAI to "Enemy_01"
  → create_script CameraFollow.cs → add_component CameraFollow to "MainCamera"
  → set_script_component_property CameraFollow.target = "Player"

Step 8: UI Canvas
  → create_canvas "UI_Canvas"
  → create_ui_element Panel "HealthBar_BG" (color: 0.3,0.3,0.3,1, size: 200,20)
  → create_ui_element Image "HealthBar_Fill" (color: 1,0,0,1, type: Filled)
  → create_ui_element TMP "ScoreText" (text: "Score: 0", color: 1,1,1,1)
  → set_transform anchoredPosition: HealthBar_BG at (-400,280), ScoreText at (400,280)

Step 9: 编译验证
  → compile_scripts
  → 等待 status='idle'
  → 如果 errorCount>0 → 修复 → 回到 compile_scripts
  → 如果 errorCount=0 → 进入 Phase 3
```

### 2.2 脚本模板

PlayerController 模板（使用 CharacterController）：

```csharp
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
            controller = gameObject.AddComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
```

## Phase 3: Evaluate — 详细步骤

### 3.1 运行时验证流程

```
1. start_runtime_observation → 获取 startCursor
2. 进入 Play Mode（或提示用户点击 Play）
3. 等待 Play Mode 稳定（≥ 5 秒）
4. get_runtime_events(sinceCursor=startCursor) → 收集所有 Error/Exception
5. 退出 Play Mode
6. 对照检查清单逐项判定
```

### 3.2 Fix Card 模板

发现问题时输出：

```
🔧 Fix Card — PGE 第 N 轮
├── 问题: [如：NullReferenceException at PlayerController:23]
├── 根因: [如：CameraFollow.target 未在 Start 中找到 Player]
├── 修复: [如：使用 GameObject.FindGameObjectWithTag("Player")]
├── 影响文件: [如：CameraFollow.cs:15]
└── 预计修复轮数: 1
```

### 3.3 最终报告模板

```
═══════════════════════════════════════
  PGE 原型生成完成 — [游戏名称]
═══════════════════════════════════════

迭代总览:
  总轮数: N/5
  通过检查: M/7
  修复问题数: K

检查清单结果:
  ✅ 场景可运行不报错
  ✅ 玩家可移动/跳跃
  ✅ 核心循环可体验
  ✅ Camera 跟随正常
  ✅ UI 显示正确
  ✅ 帧率 ≥ 30fps
  ⚠️ 无 NullReferenceException (仍有 1 个已知非阻塞警告)

已知问题:
  1. [问题描述] — 影响: [低/中/高] — 建议: [修复方向]

验证结论: ✅ 可玩

下一步建议:
  - [如：手感数值需要人类测试调优]
  - [如：建议运行 gdd-gameplay 细化战斗系统后再次 PGE]
```
