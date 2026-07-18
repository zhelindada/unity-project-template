# Unity 6 UI Toolkit MVVM Pattern

## 核心概念

Unity 6 内置 runtime data binding，三要素：

| 角色 | 职责 | 实现 |
|------|------|------|
| Model | 数据 + 业务逻辑 | ScriptableObject / 纯 C# 对象 |
| View | 视觉呈现 | UXML + USS |
| ViewModel | 暴露可绑定属性 + 处理命令 | MonoBehaviour 或纯 C# 对象 |

## Data Source 定义

```csharp
using Unity.Properties;
using UnityEngine;

[CreateAssetMenu]
public class PlayerDataSO : ScriptableObject
{
    [SerializeField, DontCreateProperty] int m_Health;
    [SerializeField, DontCreateProperty] string m_Name;

    [CreateProperty] public int Health => m_Health;
    [CreateProperty] public string Name => m_Name;

    public void TakeDamage(int damage)
    {
        m_Health -= damage;
        NotifyPropertyChanged(nameof(Health));
    }
}
```

- `[CreateProperty]` — 标注可绑定属性，编译期生成 property bag
- `[DontCreateProperty]` — 排除不需要绑定的字段
- `NotifyPropertyChanged()` — 手动通知 UI 更新

## 绑定方式

### A) UXML 声明式绑定

```xml
<ui:UXML xmlns:ui="UnityEngine.UIElements">
    <ui:VisualElement name="player-panel">
        <Bindings>
            <ui:DataBinding property="dataSource" data-source-path="PlayerData" />
        </Bindings>
        <ui:Label name="health-label">
            <Bindings>
                <ui:DataBinding property="text" data-source-path="Health" binding-mode="ToTarget" />
            </Bindings>
        </ui:Label>
    </ui:VisualElement>
</ui:UXML>
```

### B) C# 过程式绑定

```csharp
label.SetBinding("text", new DataBinding
{
    dataSource = playerData,
    dataSourcePath = new PropertyPath(nameof(PlayerDataSO.Health)),
    bindingMode = BindingMode.ToTarget
});
```

## Binding 模式

| 模式 | 用途 |
|------|------|
| `ToTarget` | 只读显示（Label、进度条） |
| `TwoWay` | 交互控件（Slider、TextField） |
| `ToSource` | 输入专用 |
| `ToTargetOnce` | 一次性快照 |

## 已知限制

- **Button clickable 无法绑定** — 必须用 `RegisterCallback<ClickEvent>()`
- **List/Tree 绑定较复杂** — 大型列表需自定义 `ListView` data source
- **动画绑定不支持** — 动画不应走 binding，走 Presenter 层

## 性能优化

大量绑定时实现以下接口：

```csharp
public class DataSource : INotifyBindablePropertyChanged
{
    public event BindablePropertyChangedEventHandler propertyChanged;
    // 只更新变化的属性，而不是全量刷新
}
```
