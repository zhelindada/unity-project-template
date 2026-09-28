using System.Collections.Generic;
using UnityEngine;

namespace Dada.Cores
{
    /// <summary>
    /// UITK 屏幕注册配置——ScriptableObject，在 Inspector 中维护所有屏幕定义。
    /// 通过 UITKInstaller 注入到 VContainer 容器中。
    /// </summary>
    [CreateAssetMenu(menuName = "Dada/UITK Config", fileName = "UITKConfig")]
    public class UITKConfig : ScriptableObject
    {
        [Tooltip("所有屏幕的注册定义")]
        public List<ScreenDefinition> screens = new();

        public ScreenDefinition Find(string screenId)
        {
            return screens.Find(s => s.screenId == screenId);
        }
    }
}
