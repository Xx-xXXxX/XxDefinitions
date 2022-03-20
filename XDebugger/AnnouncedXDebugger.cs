using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions.XDebugger
{

	/// <summary>
	/// 声明一个公开的XDebugger给其他人使用
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
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
		public AnnouncedXDebugger(string FullName) {
			this.XDebuggerFullName = FullName;
			tryGetXDebugger = TryGetXDebugger.GetTryGetXDebugger(FullName);
		}
	}
}
