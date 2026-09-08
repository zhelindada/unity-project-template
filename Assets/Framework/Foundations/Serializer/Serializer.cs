using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Dada.Foundations
{
    /// <summary>
    /// 标记需要参与序列化的字段或属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SerializableMemberAttribute : Attribute
    {
        // 可扩展：如设置别名、默认值等
    }
}

