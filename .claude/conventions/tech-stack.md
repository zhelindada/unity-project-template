# 技术栈

目标 Editor：Unity 6000.6.0f1。UPM 依赖以 `Packages/manifest.json` 为准，NuGet 运行库以 `Assets/packages.config` 为准。

## 关键依赖

| 依赖 | 版本 / 来源 | 用途 |
| --- | --- | --- |
| Dada Unity Framework | 1.0.10，Git 提交 818a0f554586df5a03f560310c492ee0092a9a53 | 游戏框架 |
| VContainer | 1.19.0，UPM Git | DI |
| UniTask | 2.5.11，UPM Git | Unity 异步操作；不重复安装 NuGet UniTask |
| MessagePipe / VContainer 集成 | 1.8.2，Framework 的 OpenUPM 依赖 | 消息通信 |
| R3 | 1.3.1，UPM 集成 + NuGet 运行库 | 响应式数据 |
| ZString | 2.6.0，Framework 的 OpenUPM 依赖；不重复安装 NuGet ZString | 字符串格式化 |
| Addressables | 2.10.3，Framework 依赖 | 资源管理 |
| Input System | 1.20.0 | 新输入系统，示例使用 Input Actions 或设备 API |
| Universal RP | 17.6.0 | 渲染；自定义 Pass 使用 RenderGraph |
| UGUI / TextMeshPro | 2.6.0 | UGUI；TMP 已随 UGUI 提供 |
| Cinemachine | 6.6.0 | 相机；使用 Unity.Cinemachine 命名空间 |
| Timeline | 6.6.0 | 时间轴 |
| Test Framework | 1.8.0 | Unity 测试 |

OpenUPM 包源覆盖 `com.cysharp`。Spine、Graphy 是按项目需求另行安装的可选插件，不属于当前基础依赖。

## 核心框架

Framework 通过远程 UPM 安装到 `Packages/com.dada.unity-framework/`，不内嵌到 `Assets/Framework` 或 `Assets/Dada`。

当前程序集：`Dada.Data`、`Dada.Foundations`、`Dada.Cores.Sim`、`Dada.Cores`、`Dada.Cores.UI`。业务代码按需引用；框架修改在独立仓库维护，通过更新已验证的 Git 提交引入。

业务代码存放在 `Assets/$projname/Scripts/`：

- Constants：常量，命名空间 `$projname.Constants`。
- Data：业务数据，命名空间 `$projname.Data`。
- Controllers：游戏内 MonoBehaviour，命名空间 `$projname.Controllers`。
- Services：由 DI 管理的游戏逻辑，命名空间 `$projname.Services`。
- Views：UI 控制器，命名空间 `$projname.Views`。
