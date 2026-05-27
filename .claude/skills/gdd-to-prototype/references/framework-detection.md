# Framework Detection & Adaptation

## 自动检测规则

按优先级依次检查以下信号：

### 1. Unity
**检测信号（任一命中即确认为 Unity）：**
- 存在 `Assets/` 目录且包含 `.unity` 文件
- 存在 `ProjectSettings/` 目录
- 存在 `Packages/manifest.json`
- 存在 `*.sln` 且 `.csproj` 引用 `UnityEngine`

**确认后读取：**
- `ProjectSettings/ProjectSettings.asset` → 确认 Input System (New/Old)
- `Packages/manifest.json` → 确认渲染管线（URP/HDRP/Built-in）
- `ProjectSettings/ProjectVersion.txt` → 确认 Unity 版本

**可用工具：** gladekit-unity MCP（场景操作、脚本创建、组件添加、编译检查）

### 2. Unreal Engine
**检测信号：**
- 存在 `*.uproject` 文件
- 存在 `Source/` 和 `Content/` 目录
- 存在 `Config/DefaultEngine.ini`

**可用工具：** 无直接 MCP 工具，输出文件路径 + C++/Blueprint 代码，指导用户手动操作

### 3. Godot
**检测信号：**
- 存在 `project.godot` 文件
- 存在 `.godot/` 目录

**可用工具：** 无直接 MCP 工具，输出 `.tscn` 场景文件 + `.gd` 脚本

### 4. 纯代码框架（Raylib/MonoGame/LÖVE 等）
**检测信号：**
- 存在 `*.csproj` / `Cargo.toml` / `main.lua` 等
- 无上述引擎特征

**可用工具：** 直接生成代码文件

## 框架适配矩阵

| 需求 | Unity | Unreal | Godot | 纯代码 |
|------|-------|--------|-------|--------|
| 场景/关卡 | Scene + GameObject | Level + Actor | Scene + Node | 代码构建 |
| 脚本 | C# MonoBehaviour | C++/Blueprint | GDScript/C# | 框架语言 |
| 物理 | Rigidbody + Collider | PhysicsBody + Collision | RigidBody + CollisionShape | 框架 API |
| UI | Canvas + TMP | UMG Widget | Control nodes | 框架 API |
| 输入 | InputSystem / Input | Enhanced Input | Input Map | 框架 API |
| 相机 | Camera component | CameraActor + SpringArm | Camera3D/2D | 手动矩阵 |

## 适配原则

1. **优先使用 MCP 工具**：如果连接了 gladekit-unity，直接从 GDD 生成 Unity 场景
2. **无工具时输出代码**：为 Unreal/Godot 输出完整文件内容 + 放置说明
3. **保持原型脚本跨框架逻辑一致**：PlayerController 的核心逻辑（移动速度、跳跃力、碰撞检测）用注释标注物理参数，方便迁移
4. **标注框架特定细节**：
   - Unity: `// Unity: 使用 InputSystem 或 Input.GetAxis`
   - Unreal: `// UE: 使用 Enhanced Input Action`
   - Godot: `# Godot: 使用 InputMap`
