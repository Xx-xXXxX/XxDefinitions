using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions.XDebugger
{

	/// <summary>
	/// 声明一个公开的XDebugger给其他人使用
	/// <code><![CDATA[
	/// [AnnouncedXDebugger("T.a")]
	/// class T:Mod{}
	/// ]]></code>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	[Obsolete("使用AnnounceAttribute")]
	public class AnnouncedXDebugger:Attribute
	{
		/// <summary>
		/// 使用的XDebugger的全名
		/// </summary>
		public readonly string XDebuggerFullName;
		/// <summary>
		/// 尝试获取XDebugger
		/// </summary>
		public readonly TryGetXDebugger tryGetXDebugger;
		/// <summary>
		/// 声明一个公开的XDebugger给其他人使用
		/// </summary>
		public AnnouncedXDebugger(string FullName) {
			XxDefinitions.Logv1.Debug($"Announce XDebugger {FullName}");
			this.XDebuggerFullName = FullName;
			this.tryGetXDebugger = TryGetXDebugger.GetTryGetXDebugger(FullName);
		}
	}
	/// <summary>
	/// 表示该XDebugger是公用的，该XDebugger可以在AnnouncedDebuggers.Value中找到
	/// 可以在XDebuggerHelper(Mod)中开关
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	[Obsolete("使用new XDebugger(Announce:true)")]
	public class AnnounceAttribute : Attribute { }
}
