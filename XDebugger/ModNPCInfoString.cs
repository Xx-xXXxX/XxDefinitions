using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
namespace XxDefinitions.XDebugger
{
	/// <summary>
	/// ModNPC信息，<![CDATA[Action<List<(string,string)>>]]>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ModNPCInfoString: XDebuggerRequires
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public ModNPCInfoString(string XDebuggerFullName, string MethodName):base(XDebuggerFullName,MethodName)
		{ }
		public Action<List<(string, string)>> GetInfoStringMethod(Terraria.ModLoader.ModNPC npc) {
			return base.GetPropertyValue<Action<List<(string, string)>>>(npc);
		}
	}
	/// <summary>
	/// GlobalNPC信息，<![CDATA[Action<Terraria.NPC,List<(string,string)>>]]>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class GlobalNPCInfoString : XDebuggerRequires
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public GlobalNPCInfoString(string XDebuggerFullName, string MethodName) : base(XDebuggerFullName, MethodName)
		{ }
		public Action<Terraria.NPC,List<(string, string)>> GetInfoStringMethod(Terraria.ModLoader.GlobalNPC globalNPC)
		{
			return base.GetPropertyValue<Action<Terraria.NPC,List<(string, string)>>>(globalNPC);
		}
	}
	/// <summary>
	/// ModNPC绘图，<![CDATA[Func<SpriteBatch,bool>，返回false阻止显示InfoString]]>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ModNPCInfoDraw : XDebuggerRequires
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public ModNPCInfoDraw(string XDebuggerFullName, string MethodName) : base(XDebuggerFullName, MethodName)
		{ }
		public Func<SpriteBatch,bool> GetInfoStringMethod(Terraria.ModLoader.ModNPC npc)
		{
			return base.GetPropertyValue<Func<SpriteBatch, bool>>(npc);
		}
	}

	/// <summary>
	/// GlobalNPC绘图，<![CDATA[Func<NPC,SpriteBatch,bool>，返回false阻止显示InfoString]]>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class GlobalNPCInfoDraw : XDebuggerRequires
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public GlobalNPCInfoDraw(string XDebuggerFullName, string MethodName) : base(XDebuggerFullName, MethodName)
		{ }
		public Func<Terraria.NPC,SpriteBatch,bool> GetInfoStringMethod(Terraria.ModLoader.GlobalNPC globalNPC)
		{
			return base.GetPropertyValue<Func<Terraria.NPC, SpriteBatch, bool>>(globalNPC);
		}
	}
}
