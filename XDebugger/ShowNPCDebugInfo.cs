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
	/// 用于显示NPCDebugInfo，调用ModNPC[ModNPCInfoString]与GlobalNPC[GlobalNPCInfoString] Action<NPC,List<string>>]]>
	/// </summary>
	[GlobalNPCInfoString("XDebugger.XDebugger", "DefShowNPCDebugInfo")]
	public class ShowNPCDebugInfo:GlobalNPC
	{
		/// <summary>
		/// 设为true使ShowNPCDebugInfo总是显示
		/// DrawTimeLeft=1;
		/// </summary>
		public static bool ShowAlways=false;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public static Action<NPC, List<(string, string)>> DefShowNPCDebugInfo => (npc, l) =>
		{
			l.Add(("DefShowNPCDebugInfo",
				$"Type:{npc.type} {npc.TypeName} aiStyle:{npc.aiStyle}\n"+
				$"ais:{npc.ai[0]},{npc.ai[1]},{npc.ai[2]},{npc.ai[3]}\n"+
				$"localAIs:{npc.localAI[0]},{npc.localAI[1]},{npc.localAI[2]},{npc.localAI[3]}\n" +
				$"friendly:{npc.friendly} boss:{npc.boss} town:{npc.townNPC}\n"
				));
		};
		public override bool InstancePerEntity => true;
		List<Action<NPC, List<(string, string)>>> actions=new List<Action<NPC, List<(string, string)>>>();
		List<TryGetXDebugger> actionsUsing = new List<TryGetXDebugger>();
		public override void SetDefaults(NPC npc)
		{
			if (npc == null) return;
			if(npc.modNPC!=null)
			{
				System.Reflection.MemberInfo memberInfo = npc.modNPC.GetType();
				List<ModNPCInfoString> xDebuggerModNPCInfos = new List<ModNPCInfoString>((ModNPCInfoString[])memberInfo.GetCustomAttributes(typeof(ModNPCInfoString), true));
				if (xDebuggerModNPCInfos.Count > 0)
				{
					foreach (var i in xDebuggerModNPCInfos)
					{
						Action<List<(string, string)>> action = i.GetInfoStringMethod(npc);
						if (action != null) { 
							actions.Add((n, l) => { action.Invoke(l); });
							actionsUsing.Add(i.tryGetXDebugger);
						}
					}
				}
			}
			List<GlobalNPCInfoString> xDebuggerGlobalNPCInfos;
			foreach (var j in globalNPCs) {
				xDebuggerGlobalNPCInfos = new List<GlobalNPCInfoString>((GlobalNPCInfoString[])j.Instance(npc).GetType().GetCustomAttributes(typeof(GlobalNPCInfoString), true));
				if (xDebuggerGlobalNPCInfos.Count > 0)
				{
					foreach (var i in xDebuggerGlobalNPCInfos)
					{
						Action<NPC,List<(string, string)>> action = i.GetInfoStringMethod(j);
						if (action != null)
						{
							actions.Add(action);
							actionsUsing.Add(i.tryGetXDebugger);
						}
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
			if (!XDebugger.DebugMode) return;
			if (ShowAlways) DrawTimeLeft = 1;
			if (DrawTimeLeft <= 0) return;
			Vector2 Pos = npc.position + new Vector2(npc.width, npc.height / 2) - Main.screenPosition;
			List<(string,string)> tooltips = new List<(string, string)>();
			//foreach (var i in actions)
			//{
			//	i(npc, tooltips);
			//}
			for (int i = 0; i < actions.Count; ++i) {
				if (actionsUsing[i].XDebuggerMode == 2) { 
					actions[i](npc, tooltips);
				}
			}
			string info = "";
			foreach (var i in tooltips)
			{
				info += i.Item2;
			}
			Pos -= new Vector2(0, Main.fontMouseText.MeasureString(info).Y / 2f);
			Terraria.Utils.DrawBorderString(spriteBatch, info, Pos, Color.White);
		}
	}
}
