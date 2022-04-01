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

using XxDefinitions.NPCs.NPCBehaviors;

namespace XxDefinitions.NPCs.NPCBehaviors
{
	/// <summary>
	/// 自动设置目标，使用Utils.FindTargetClosest
	/// </summary>
	public class CheckTarget : ModNPCBehavior<ModNPC>
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override string BehaviorName => "CheckTarget";

		public override bool NetUpdate =>false;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
		/// <summary>
		/// 找到的目标
		/// </summary>
		public IGetSetValue<UnifiedTarget> Target;
		/// <summary>
		/// 是否进行更新，在间隔期，只会判断目标是否可用，并在不可用时更新
		/// </summary>
		public Func<bool> DoCalculate=null;
		/// <summary>
		/// 玩家是否可以成为目标
		/// </summary>
		public Func<Player, bool> PlayerCanBeTargeted=null;
		/// <summary>
		/// NPC是否可以成为目标
		/// </summary>
		public Func<NPC, bool> NPCCanBeTargeted = null;
		/// <summary>
		/// 玩家的值
		/// </summary>
		public Func<Player, float> PlayerValue = null;
		/// <summary>
		/// NPC的值
		/// </summary>
		public Func<NPC, float> NPCValue=null;
		/// <summary>
		/// 是否寻找友善生物，包括玩家和友好NPC
		/// </summary>
		public bool FindFriendly = false;
		/// <summary>
		/// 是否寻找邪恶生物
		/// </summary>
		public bool FindHostile = true;
		/// <summary>
		/// 初始价值，相当于距离
		/// </summary>
		public float DefaultValue = 2048;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public CheckTarget(ModNPC modNPC, IGetSetValue<UnifiedTarget> Target) : base(modNPC) {
			this.Target = Target;
			DoCalculate=()=> modNPC.npc.timeLeft % 15 == 0;
		}
		public CheckTarget(ModNPC modNPC, IGetSetValue<UnifiedTarget> Target,int UpdateTime) : base(modNPC)
		{
			this.Target = Target;
			DoCalculate = () => modNPC.npc.timeLeft % UpdateTime == 0;
		}
		public override void Update()
		{
			if(DoCalculate==null) DoCalculate = () => npc.timeLeft % 15 == 0;

			if (NPCCanBeTargeted == null) NPCCanBeTargeted =Utils.NPCCanFind;
			if (PlayerCanBeTargeted == null) PlayerCanBeTargeted = Utils.PlayerCanFind;

			if (DoCalculate.Invoke()||(
					Target.Value.IsNPC&&!NPCCanBeTargeted(Target.Value.npc)||
					Target.Value.IsPlayer&&!PlayerCanBeTargeted(Target.Value.player)||
					Target.Value.IsNull
				)) {
				Claculate();
			}
		}
		public override void OnActivate()
		{
			Claculate();
			base.OnActivate();
		}
		public void Claculate() { 
			Target.Value = Utils.CalculateUtils.FindTargetClosest(npc.Center, DefaultValue, FindFriendly, FindHostile, NPCCanBeTargeted, PlayerCanBeTargeted, NPCValue, PlayerValue); 
		}
		/// <summary>
		/// 设置为搜索邪恶生物
		/// </summary>
		public CheckTarget SetForHostileNPC(Func<NPC, bool> NPCCanBeTargeted = null, Func<NPC, float> NPCValue = null) {
			this.NPCCanBeTargeted = NPCCanBeTargeted;
			this.NPCValue = NPCValue;
			FindFriendly = false;
			FindHostile = true;
			this.PlayerCanBeTargeted = (p) => false;
			return this;
		}
		/// <summary>
		/// 设置为搜索玩家
		/// </summary>
		public CheckTarget SetForPlayer(Func<Player, bool> PlayerCanBeTargeted=null, Func<Player, float> PlayerValue=null)
		{
			this.PlayerCanBeTargeted = PlayerCanBeTargeted;
			this.PlayerValue = PlayerValue;
			FindFriendly = true;
			FindHostile = false;
			this.NPCCanBeTargeted = (p) => false;
			return this;
		}
	}
}
