# unity-project-template

Framework 通过 Unity Package Manager 从 [unity-framework](https://github.com/zhelindada/unity-framework) 安装，不再内嵌到 `Assets/Framework`。

`Packages/manifest.json` 已配置 Framework、UniTask 和 VContainer 的 Git 依赖。使用 Unity 打开项目后，Package Manager 会解析并安装依赖，同时更新 `Packages/packages-lock.json`。

框架程序集名称为 `DadaFramework.Cores` 和 `DadaFramework.Foundations`。不要再复制旧的 Framework 目录，以免脚本、程序集和 GUID 重复。

