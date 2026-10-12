# unity-project-template

Unity **6000.6.0f1** 模板，使用 URP 17.6、新 Input System 和 UGUI 2.6 / UI Toolkit。

Framework 通过 Unity Package Manager 从 [unity-framework](https://github.com/zhelindada/unity-framework) 安装，不内嵌到 Assets。当前固定为 1.0.4 对应的 Git 提交 `85e736a599d0c05998396092dee0c47073a9ea3e`。

`Packages/manifest.json` 配置 UPM / Git 依赖与 OpenUPM 包源。使用 6000.6.0f1 首次打开项目后由 Package Manager 解析依赖并生成 `Packages/packages-lock.json`。R3 等 NuGet 运行库由 NuGetForUnity 按 `Assets/packages.config` 还原；UniTask 只使用 UPM 版本。

框架程序集为 `Dada.Foundations`、`Dada.Cores.Sim`、`Dada.Cores` 和 `Dada.Cores.UI`，不要复制旧 Framework 目录。版本与目录约定见 `.claude/conventions/tech-stack.md` 和 `project-structure.md`。

### 首次打开时提示找不到 R3

UPM 的 `com.cysharp.r3` 只提供 Unity 集成，核心 `R3.dll` 需要 NuGetForUnity 还原到 `Assets/Packages`。`Assets/NuGet.config` 必须只有一个 `<config>` 节点，`repositoryPath` 为 `./Packages`（相对于 Assets），不能指向项目外的用户缓存。

ZString 已由 Framework 的 UPM 依赖提供完整实现，不要再安装 NuGet ZString。若出现 `CS0433` 提示两份 `ZString`，移除 `Assets/packages.config` 中的 ZString 条目及已还原的 `Assets/Packages/ZString.*` 包目录，再重新编译。

若首次编译时出现 `R3` / `Observable<>` 等缺失错误，进入 Unity Safe Mode，等待 UPM 安装完成，再执行 **NuGet → Restore Packages**。还原后等待重新编译；不要修改 `Library/PackageCache` 内的框架脚本。

初始化时将 `Assets/$projname` 重命名为实际项目名称，并同步调整 Build Settings 中的场景路径。

## 插件导入

默认插件安装包在 `External/Packages`，分卷包先合并后导入。Feel 默认选择 6.1，避免同时导入 Feel 2022。

`External/OptionalPackages` 保存旧版可选安装包，不属于默认导入集合。Odin Validator 4.0.1.6 与默认 Inspector 4.0.2.4 不匹配；需要 Validator 时请先取得与 Inspector 相同 Major.Minor.Build 的正版安装包，再导入。

通过 `External/Packages/import_packages.py` 导入时，工具会在写入前检查 Odin 配套版本与插件文件冲突。默认拒绝不匹配的 Odin 组合和不同版本覆盖同一目标文件。
