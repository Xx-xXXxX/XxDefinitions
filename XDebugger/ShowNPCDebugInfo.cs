using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace XxDefinitions.XDebugger
{
	//[XDebuggerInfo()]
	public class ShowNPCDebugInfo:GlobalNPC
	{
		//List< XDebuggerInfo> XInfos;
		//public override void SetDefaults(NPC npc)
		//{
		//	System.Reflection.MemberInfo info = npc.GetType();
		//	XInfos =new List<XDebuggerInfo>( (XDebuggerInfo[])info.GetCustomAttributes(typeof(XDebuggerInfo), true));
		//	Type type = npc.GetType();
		//	var gns= type.GetField("globalNPCs");
		//	GlobalNPC[] globalNPCs=(GlobalNPC[])gns.GetValue(npc);
		//	foreach (var i in globalNPCs) {
		//		foreach (var j in (XDebuggerInfo[])i.GetType().GetCustomAttributes(typeof(XDebuggerInfo), true)) {
		//			XInfos.Add(j);
		//		}
		//	}
		//}
		//public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		//{
		//	Vector2 Pos = npc.position + new Vector2(npc.width, npc.height/2)-Main.screenPosition;
		//	List<Terraria.ModLoader.TooltipLine> tooltips = new List<TooltipLine>();
		//	foreach (var i in XInfos) {
		//		i.action(npc,tooltips);
		//	}

		//	string info = "";
		//	foreach (var i in tooltips) {
		//		info += i.text;
		//	}
		//	int lines= info.Count((c)=>c=='\n');
		//	Pos -= new Vector2(lines*12/2);
		//	Terraria.Utils.DrawBorderString(spriteBatch, info, Pos, Color.White);
		//}
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override bool InstancePerEntity => true;
		List<Action<NPC, List<string>>> actions=new List<Action<NPC, List<string>>>();
		public override void SetDefaults(NPC npc)
		{
			//Type t = npc.GetType();
			System.Reflection.MemberInfo memberInfo = this.GetType();
			List<XDebuggerRequires> xDebuggerInfos= new List<XDebuggerRequires>((XDebuggerRequires[])memberInfo.GetCustomAttributes(typeof(XDebuggerRequires), true));
			if (xDebuggerInfos.Count > 0) {
				foreach (var i in xDebuggerInfos) { 
					
				}
			}
		}
	}
}
