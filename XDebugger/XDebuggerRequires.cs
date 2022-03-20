using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Terraria.ModLoader;

namespace XxDefinitions.XDebugger
{
	/// <summary>
	/// 根据需要获取属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Class,AllowMultiple =true)]
	public class XDebuggerRequires:Attribute
	{
		/// <summary>
		/// 属性的名称
		/// </summary>
		public readonly string PropertyName;
		/// <summary>
		/// 使用的XDebugger的全名
		/// </summary>
		public readonly string XDebuggerFullName;
		/// <summary>
		/// 尝试获取XDebugger
		/// </summary>
		public readonly TryGetXDebugger tryGetXDebugger;
		Type t;
		PropertyInfo propertyInfo;
		/// <summary>
		/// 使用反射获取属性值
		/// </summary>
		/// <param name="XDebuggerFullName">使用的XDebugger的全名</param>
		/// <param name="PropertyName">属性的名称</param>
		public XDebuggerRequires(string XDebuggerFullName, string PropertyName) {
			this.XDebuggerFullName = XDebuggerFullName;
			this.PropertyName = PropertyName;
			this.tryGetXDebugger = TryGetXDebugger.GetTryGetXDebugger(XDebuggerFullName);
		}
		public void ResetReflect() {
			t = null; propertyInfo = null;
		}
		public T GetPropertyValue<T>(object obj) {
			XDebugger xDebugger = tryGetXDebugger.xDebugger;
			if (xDebugger != null && xDebugger.Using)
			{
				if (t == null)
				{
					t = obj.GetType();
				}
				if (propertyInfo == null)
				{
					propertyInfo = t.GetProperty(PropertyName);
				}
				if (propertyInfo == null) {
					t = null;
					throw new ArgumentException($"obj {obj.ToString()} 不包含 {PropertyName}", "obj");
				}
				if (!propertyInfo.CanRead) {
					t = null; propertyInfo = null;
					throw new ArgumentException($"obj {obj.ToString()} 的 {PropertyName} 的类型 {propertyInfo.PropertyType} 不可读", "obj");
				}
				if (propertyInfo.PropertyType != typeof(T)) {
					t = null;propertyInfo = null;
					throw new ArgumentException($"obj {obj.ToString()} 的 {PropertyName} 的类型 {propertyInfo.PropertyType} 不是 {typeof(T)}", "obj"); 
				}
				
				return (T)propertyInfo.GetValue(obj);
			}
			else {
				return default(T);
			}
		}
	}
}
