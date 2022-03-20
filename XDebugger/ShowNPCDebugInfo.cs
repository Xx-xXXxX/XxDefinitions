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
	/// <summary><![CDATA[
	/// 用于显示NPCDebugInfo，调用ModNPC[XDebuggerRequires] Action<List<string>>与GlobalNPC[XDebuggerRequires] Action<NPC,List<string>>]]>
	/// </summary>
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
			System.Reflection.MemberInfo memberInfo = npc.modNPC.GetType();
			List<ModNPCInfoString> xDebuggerModNPCInfos= new List<ModNPCInfoString>((ModNPCInfoString[])memberInfo.GetCustomAttributes(typeof(ModNPCInfoString), true));
			if (xDebuggerModNPCInfos.Count > 0) {
				foreach (var i in xDebuggerModNPCInfos) {
					Action<List<string>> action = i.GetInfoStringMethod(npc);
					if (action != null) actions.Add( (n, l) => { action.Invoke(l); });
				}
			}
			List<GlobalNPCInfoString> xDebuggerGlobalNPCInfos;
			foreach (var j in globalNPCs) {
				xDebuggerGlobalNPCInfos = new List<GlobalNPCInfoString>((GlobalNPCInfoString[])j.Instance(npc).GetType().GetCustomAttributes(typeof(GlobalNPCInfoString), true));
				if (xDebuggerGlobalNPCInfos.Count > 0)
				{
					foreach (var i in xDebuggerGlobalNPCInfos)
					{
						Action<NPC,List<string>> action = i.GetInfoStringMethod(j);
						if (action != null) actions.Add(action);
					}
				}
			}
		}
		internal static IList<GlobalNPC> globalNPCs;
		static ShowNPCDebugInfo() {
			Type type = typeof(Terraria.ModLoader.NPCLoader);
			var fieldinfo= type.GetField("globalNPCs",System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.NonPublic);
			globalNPCs =(IList<GlobalNPC>)fieldinfo.GetValue(null);
		}
		public int DrawTimeLeft = 0;
		public override void PostAI(NPC npc)
		{
			if (DrawTimeLeft > 0) DrawTimeLeft -= 1;
		}
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			if (DrawTimeLeft <= 0) return;
			Vector2 Pos = npc.position + new Vector2(npc.width, npc.height / 2) - Main.screenPosition;
			List<string> tooltips = new List<string>();
			foreach (var i in actions)
			{
				i(npc, tooltips);
			}

			string info = "";
			foreach (var i in tooltips)
			{
				info += i;
			}
			int lines = info.Count((c) => c == '\n');
			Pos -= new Vector2(lines * 12 / 2);
			Terraria.Utils.DrawBorderString(spriteBatch, info, Pos, Color.White);
		}
	}
}
