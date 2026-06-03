## 关键依赖

| 包 | 用途 |
|---|---|
| `jp.hadashikick.vcontainer` (1.17.0) | DI 容器，项目核心 IoC 框架 |
| `com.cysharp.messagepipe` (1.8.1) | 消息管道，事件/消息解耦通信 |
| `com.cysharp.messagepipe.vcontainer` (1.8.1) | MessagePipe 与 VContainer 集成 |
| `com.cysharp.unitask` (2.5.10) | 零 GC 的 async/await 实现 |
| `com.cysharp.r3` (1.3.0) | 响应式编程（Rx）库 |
| `com.cysharp.zstring` (2.6.0) | 零分配字符串格式化 |
| `com.unity.addressables` (1.22.3) | 资源管理（Addressables 系统） |
| `com.esotericsoftware.spine.spine-unity` (4.2.x) | 2D 骨骼动画 |
| `com.unity.cinemachine` (2.10.3) | 相机系统 |
| `com.tayx.graphy` (3.0.5) | 性能监控（FPS/内存等） |
| `com.unity.test-framework` (1.1.33) | 单元测试框架 |


## 核心框架

代码框架在 Asset/Framework 下，存放独立于项目的通用代码，这些代码可以被复用，如果没有需要不需要更改。

当前项目专用的代码放到 Asset/$projname/Scripts下,且需要按照现有结构存放。

Constants - 静态/常量类型，如 文件路径。命名空间为 $projname.Constants
Data - 数据类型，存放纯数据的类型，如业务用enum, Game, ***Data, 命名空间为 $projname.Data
Controllers - 非UI类型控制器，用于存放继承了MonoBehaviour的控制器类，需要挂在游戏内。命名空间为 $projname.Controllers
Services - 服务类，用于存放无需挂载的游戏逻辑类型，由vcontainer等DI框架初始化并管理。命名空间为 $projname.Services
Views - 用于处理UI逻辑, 比如UIDocument的控制器,UIDocument,Canvas配套脚本 等。 命名空间为 $projname.Views
