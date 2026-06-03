模板采用分层架构，`Assets/` 下的目录结构即约定：

- `Assets/$projname/Scripts/` — 游戏业务代码，详见tech-stack.md
- `Assets/$projname/Designs/` — 游戏设计方案
- `Assets/Dada/` — 框架层和可复用核心脚本（非业务逻辑）
- `Assets/Config/` — 配置类 ScriptableObject 资产存放处
- `Assets/Art/` — 所有美术资源，按类型分子目录（Animations / Atlas / Audios / Fonts / Materials / Prefabs / Spines / Sprites / Textures）
- `Assets/Scenes/` — 场景文件，支持子场景（subscene）目录结构