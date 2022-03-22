using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XxDefinitions.XDebugger
{
	/// <summary>
	/// ModProjectile信息，<![CDATA[Action<List<(string,string)>>]]>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ModProjInfoString : XDebuggerRequires
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public ModProjInfoString(string XDebuggerFullName, string MethodName) : base(XDebuggerFullName, MethodName)
		{ }
		public Action<List<(string, string)>> GetInfoStringMethod(Terraria.Projectile proj)
		{
			return base.GetPropertyValue<Action<List<(string, string)>>>(proj);
		}
	}
	/// <summary>
	/// GlobalProjectile信息，<![CDATA[Action<Projectile,List<(string,string)>>]]>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class GlobalProjInfoString : XDebuggerRequires
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public GlobalProjInfoString(string XDebuggerFullName, string MethodName) : base(XDebuggerFullName, MethodName)
		{ }
		public Action<Terraria.Projectile, List<(string, string)>> GetInfoStringMethod(Terraria.ModLoader.GlobalProjectile globalProj)
		{
			return base.GetPropertyValue<Action<Terraria.Projectile, List<(string, string)>>>(globalProj);
		}
	}
}
