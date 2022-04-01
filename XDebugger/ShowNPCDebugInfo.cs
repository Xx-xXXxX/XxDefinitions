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
	[GlobalNPCInfoString("XDebugger.XDebugger", "DefShowNPCDebugInfoString")]
	[GlobalNPCInfoDraw("XDebugger.XDebugger", "DefShowNPCDebugInfoDraw")]
	public class ShowNPCDebugInfo:GlobalNPC
	{
		/// <summary>
		/// 设为true使ShowNPCDebugInfo总是显示
		/// DrawTimeLeft=1;
		/// </summary>
		public static bool ShowAlways=false;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public static Action<NPC, List<(string, string)>> DefShowNPCDebugInfoString => (npc, l) =>
		{
			l.Add(("DefShowNPCDebugInfoString",
				$"Type:{npc.type} {npc.TypeName} aiStyle:{npc.aiStyle}\n"+
				$"ais:{npc.ai[0]},{npc.ai[1]},{npc.ai[2]},{npc.ai[3]}\n"+
				$"localAIs:{npc.localAI[0]},{npc.localAI[1]},{npc.localAI[2]},{npc.localAI[3]}\n" +
				$"friendly:{npc.friendly} boss:{npc.boss} town:{npc.townNPC}"
				));
		}; 
		public static Func<NPC,SpriteBatch,bool> DefShowNPCDebugInfoDraw => (npc, sb) =>
		{
			Utils.DrawUtils.DrawRect(sb,npc.Hitbox.MoveBy((-Main.screenPosition).ToPoint()));
			Utils.DrawUtils.DrawVector(sb, npc.Center - Main.screenPosition, npc.velocity * 10);
			UnifiedTarget target = new UnifiedTarget() { NPCTarget = npc.target };
			if (!target.IsNull)
			Utils.DrawUtils.DrawVector(sb, npc.Center - Main.screenPosition, target.entity.Center- npc.Center,Color.Red);
			return true;
		};
		public override bool InstancePerEntity => true;
		List<Action<NPC, List<(string, string)>>> StringActions=new List<Action<NPC, List<(string, string)>>>();
		List<TryGetXDebugger> StringActionsUsing = new List<TryGetXDebugger>();
		List<Func<NPC, SpriteBatch,bool>> DrawActions=new List<Func<NPC, SpriteBatch, bool>>();
		List<TryGetXDebugger> DrawActionsUsing = new List<TryGetXDebugger>();
		public override void SetDefaults(NPC npc)
		{
			if (npc == null) return;
			//if (npc.type >= 580 && npc.modNPC = null) return;
			StringActions.Clear();//解决CloneDefaults重复
			StringActionsUsing.Clear();
			DrawActions.Clear();
			DrawActionsUsing.Clear();
			if (npc.modNPC!=null)
			{
				System.Reflection.MemberInfo memberInfo = npc.modNPC.GetType();
				List<ModNPCInfoString> xDebuggerModNPCInfos = new List<ModNPCInfoString>((ModNPCInfoString[])memberInfo.GetCustomAttributes(typeof(ModNPCInfoString), true));
				if (xDebuggerModNPCInfos.Count > 0)
				{
					foreach (var i in xDebuggerModNPCInfos)
					{
						Action<List<(string, string)>> action = i.GetInfoStringMethod(npc.modNPC);
						if (action != null) { 
							StringActions.Add((n, l) => { action.Invoke(l); });
							StringActionsUsing.Add(i.tryGetXDebugger);
						}
					}
				}
				List<ModNPCInfoDraw> xDebuggerModNPCInfosDraw = new List<ModNPCInfoDraw>((ModNPCInfoDraw[])memberInfo.GetCustomAttributes(typeof(ModNPCInfoDraw), true));
				if (xDebuggerModNPCInfosDraw.Count > 0)
				{
					foreach (var i in xDebuggerModNPCInfosDraw)
					{
						var action = i.GetInfoStringMethod(npc.modNPC);
						if (action != null)
						{
							DrawActions.Add((n, sb) => {return action.Invoke(sb); });
							DrawActionsUsing.Add(i.tryGetXDebugger);
						}
					}
				}
			}
			List<GlobalNPCInfoString> xDebuggerGlobalNPCInfos;
			List<GlobalNPCInfoDraw> xDebuggerGlobalNPCInfosDraw;
			foreach (var j in globalNPCs) {
				xDebuggerGlobalNPCInfos = new List<GlobalNPCInfoString>((GlobalNPCInfoString[])j.Instance(npc).GetType().GetCustomAttributes(typeof(GlobalNPCInfoString), true));
				if (xDebuggerGlobalNPCInfos.Count > 0)
				{
					foreach (var i in xDebuggerGlobalNPCInfos)
					{
						Action<NPC,List<(string, string)>> action = i.GetInfoStringMethod(j);
						if (action != null)
						{
							StringActions.Add(action);
							StringActionsUsing.Add(i.tryGetXDebugger);
						}
					}
				}
				xDebuggerGlobalNPCInfosDraw = new List<GlobalNPCInfoDraw>((GlobalNPCInfoDraw[])j.Instance(npc).GetType().GetCustomAttributes(typeof(GlobalNPCInfoDraw), true));
				if (xDebuggerGlobalNPCInfosDraw.Count > 0)
				{
					foreach (var i in xDebuggerGlobalNPCInfosDraw)
					{
						var action = i.GetInfoStringMethod(j);
						if (action != null)
						{
							DrawActions.Add(action);
							DrawActionsUsing.Add(i.tryGetXDebugger);
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
			bool ShowString = true;
			for (int i = 0; i < DrawActions.Count; i++)
			{
				if (DrawActionsUsing[i].XDebuggerMode == 2) {
					ShowString= ShowString&&DrawActions[i](npc,spriteBatch);
				}
			}
			if(ShowString)
			{
				Vector2 Pos = npc.position + new Vector2(npc.width, npc.height / 2) - Main.screenPosition;
				List<(string, string)> tooltips = new List<(string, string)>();
				//foreach (var i in actions)
				//{
				//	i(npc, tooltips);
				//}
				for (int i = 0; i < StringActions.Count; ++i)
				{
					if (StringActionsUsing[i].XDebuggerMode == 2)
					{
						StringActions[i](npc, tooltips);
					}
				}
				string info = "";
				foreach (var i in tooltips)
				{
					info += i.Item2+"\n";
				}
				Pos -= new Vector2(0, Main.fontMouseText.MeasureString(info).Y / 2f);
				Terraria.Utils.DrawBorderString(spriteBatch, info, Pos, Color.White);
			}
		}
	}
}
