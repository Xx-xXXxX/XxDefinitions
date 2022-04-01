using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;

namespace XxDefinitions
{
	/// <summary>
	/// 设置NPC从平台下落的条件，一true即true，包含原版方法
	/// </summary>
	public static class SetNPCFallThroughPlatforms
	{
		private static bool loaded=false;
		private static void Load_() {
			funcs = new List<System.Func<NPC, bool>>();
			iDfuncs = new SortedList<int, System.Func<NPC, bool>>();
			On.Terraria.NPC.Collision_DecideFallThroughPlatforms += UseFuncs;
			loaded = true;
		}

		private static bool UseFuncs(On.Terraria.NPC.orig_Collision_DecideFallThroughPlatforms orig, NPC self)
		{
			bool Result=false;
			if (orig != null) Result = orig.Invoke(self);
			if (loaded)
			{
				if (iDfuncs != null)
				{ 
					if(iDfuncs.TryGetValue(self.type,out System.Func<NPC, bool> func))
						if(func!=null)
							Result |= func.Invoke(self);
				}
				if (Result) return true;
				foreach (var i in funcs)
				{
					if (i != null)
						Result |= i.Invoke(self);
				}
			}
			return Result;
		}
		private static SortedList<int, System.Func<NPC, bool>> iDfuncs;
		private static List<System.Func<NPC, bool>> funcs;
		/// <summary>
		/// 设置全局方法
		/// </summary>
		public static void Add(System.Func<NPC, bool> func) { Load(); funcs.Add(func); }
		/// <summary>
		/// 设置对该type的npc的方法
		/// </summary>
		public static void Add(int type, System.Func<NPC, bool> func) { Load();iDfuncs.Add(type, func); }
		/// <summary>
		/// 如果目标玩家的顶部比自己的底部低
		/// </summary>
		public static bool FallIfTargetPlayerHigher(NPC npc) {
			if (npc.HasPlayerTarget)
			{
				if (Main.player[npc.target].Hitbox.Top > npc.Hitbox.Bottom)
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// 如果目标NPC的顶部比自己的底部低
		/// </summary>
		public static bool FallIfTargetNPCHigher(NPC npc)
		{
			if (npc.HasNPCTarget)
			{
				if (Main.npc[npc.TranslatedTargetIndex].Hitbox.Top > npc.Hitbox.Bottom)
				{
					return true;
				}
			}
			return false;
		}
		private static void Unload_() {
			funcs = null;
			iDfuncs = null;
			On.Terraria.NPC.Collision_DecideFallThroughPlatforms -= UseFuncs;
			loaded = false;
		}
		internal static void Load() {
			if (!loaded) Load_();
		}
		internal static void UnLoad() {
			if (loaded) Unload_();
		}
	}
}
