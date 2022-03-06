using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
namespace XxDefinitions.XDebugger
{
	public static class NPCStringData
	{
		public static string GetNPCDebugDataDef(NPC npc) {
			string D = "'\n";
			D += $"ais:{npc.ai[0]},{npc.ai[1]},{npc.ai[2]},{npc.ai[3]}\n";
			D += $"localAIs:{npc.localAI[0]},{npc.localAI[1]},{npc.localAI[2]},{npc.localAI[3]}\n";
			D += $"Type:{npc.type} {npc.TypeName} aiStyle:{npc.aiStyle}\n";
			D += $"friendly:{npc.friendly} boss:{npc.boss} town:{npc.townNPC}\n";
			Action<NPC, string> F;
			GetNPCDebugDataHook.RemoveAll((N)=>!N.TryGetTarget(out F));
			foreach (var N in GetNPCDebugDataHook) {
				if (N.TryGetTarget(out F)) { F(npc, D); }
			}
			return D;
		}
		public static List<WeakReference<Action<NPC, string>>> GetNPCDebugDataHook = new List<WeakReference<Action<NPC, string>>>();
		
		public static string GetNPCDebugData(NPC npc)
		{
			if (GetNPCDebugDataFuncs.ContainsKey(npc.type)){
				return GetNPCDebugDataFuncs[npc.type](npc);
			}
			else 
			return GetNPCDebugDataDef(npc);
		}
		public static SortedList<int,Func<NPC, string>> GetNPCDebugDataFuncs=new SortedList<int, Func<NPC, string>>();


		public static void ShowNPCDebug(NPC npc)
		{
			if (ShowNPCDebugFuncs.ContainsKey(npc.type))
			{
				ShowNPCDebugFuncs[npc.type](npc);
			}
			else
				ShowNPCDebugDef(npc);
		}
		public static void ShowNPCDebugDef(NPC npc) {
			Utils.AddDraw.AddDrawString(GetNPCDebugData(npc),npc.position);
			Utils.AddDraw.AddDrawRect(npc.Hitbox);
			Utils.AddDraw.AddDrawVector(npc.Center,npc.velocity*6);
		}
		//Action<SpriteBatch>
		public static SortedList<int, Action<NPC>> ShowNPCDebugFuncs = new SortedList<int, Action<NPC>>();

		public static void AddGetNPCDebugDataFunc(int type, Func<NPC, string> func)
		{
			if (GetNPCDebugDataFuncs.ContainsKey(type))
				NPCStringData.GetNPCDebugDataFuncs[type] = func;
			else
				NPCStringData.GetNPCDebugDataFuncs.Add(type, func);
		}
	}
}
