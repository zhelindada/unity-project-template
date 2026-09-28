using System.Collections.Generic;
using UnityEngine;

namespace Dada.Cores
{
    /// <summary>
    /// uGUI 屏幕注册配置——与 UITK.UITKConfig 平行。
    /// </summary>
    [CreateAssetMenu(menuName = "Dada/UGUI Config", fileName = "UGUIConfig")]
    public class UGUIConfig : ScriptableObject
    {
        public List<UGUIScreenDefinition> screens = new();

        public UGUIScreenDefinition Find(string screenId)
            => screens.Find(s => s.screenId == screenId);
    }
}
