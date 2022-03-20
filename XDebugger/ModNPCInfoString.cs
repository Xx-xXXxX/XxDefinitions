using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions.XDebugger
{
	/// <summary>
	/// ModNPC信息
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ModNPCInfoString: XDebuggerRequires
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public ModNPCInfoString(string XDebuggerFullName, string MethodName):base(XDebuggerFullName,MethodName)
		{ }
		public Action<List<string>> GetInfoStringMethod(Terraria.NPC npc) {
			return base.GetPropertyValue<Action<List<string>>>(npc);
		}
	}
	/// <summary>
	/// ModNPC信息
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class GlobalNPCInfoString : XDebuggerRequires
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public GlobalNPCInfoString(string XDebuggerFullName, string MethodName) : base(XDebuggerFullName, MethodName)
		{ }
		public Action<Terraria.NPC,List<string>> GetInfoStringMethod(Terraria.ModLoader.GlobalNPC globalNPC)
		{
			return base.GetPropertyValue<Action<Terraria.NPC,List<string>>>(globalNPC);
		}
	}
}
