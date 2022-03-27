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

namespace XxDefinitions.Projectiles.ProjBehaviors
{
	/// <summary>
	/// 自动设置目标，使用Utils.FindTargetClosest
	/// </summary>
	public class CheckTarget : ModProjBehavior<ModProjectile>
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
		public Func<bool> DoUpdate=null;
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
		public CheckTarget(ModProjectile modProjectile, IGetSetValue<UnifiedTarget> Target) : base(modProjectile) {
			this.Target = Target;
			DoUpdate=()=> modProjectile.projectile.timeLeft % 15 == 0;
		}
		public CheckTarget(ModProjectile modProjectile, IGetSetValue<UnifiedTarget> Target,int UpdateTime) : base(modProjectile)
		{
			this.Target = Target;
			DoUpdate = () => modProjectile.projectile.timeLeft % UpdateTime == 0;
		}
		/// <summary>
		/// 在进行查找之前检查，返回false阻止之后的检查
		/// </summary>
		public Func<IGetSetValue<UnifiedTarget>, bool> CheckBefore = null;
		public override void Update()
		{
			if (CheckBefore !=null&& !CheckBefore(Target)) return;

			if(DoUpdate==null) DoUpdate = () => projectile.timeLeft % 15 == 0;
			if (NPCCanBeTargeted == null) NPCCanBeTargeted = Utils.NPCCanFind;
			if (PlayerCanBeTargeted == null) PlayerCanBeTargeted = Utils.PlayerCanFind;
			if (DoUpdate.Invoke()||(
					Target.Value.IsNPC&&NPCCanBeTargeted(Target.Value.npc)||
					Target.Value.IsPlayer&&PlayerCanBeTargeted(Target.Value.player)||
					Target.Value.IsNull
				)) {
				Target.Value = Utils.CalculateUtils.FindTargetClosest(projectile.Center,DefaultValue,FindFriendly,FindHostile,NPCCanBeTargeted,PlayerCanBeTargeted,NPCValue,PlayerValue);
			}
		}
		public override void OnActivate()
		{
			Update();
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
		/// <summary>
		/// 设置优先搜索玩家的MinionAttackTarget
		/// </summary>
		/// <returns></returns>
		public CheckTarget SetForMinionAttackTarget() {
			CheckBefore = delegate (IGetSetValue<UnifiedTarget> target)
			{
				Player player = Main.player[projectile.owner];
				if (player.HasMinionAttackTargetNPC)
				{
					target.Value = UnifiedTarget.MakeNPC(player.MinionAttackTargetNPC);
					return false;
				}
				return true;
			};
			return this;
		}
	}
}
