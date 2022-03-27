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
	/// 确定召唤物是否应该活动，包括距离太远瞬移
	/// </summary>
	public class CheckMinionActive:ModProjBehavior<ModProjectile>
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override string BehaviorName => "CheckMinionActive";
		public override bool NetUpdate =>false;
		/// <summary>
		/// 该召唤物的buff的type
		/// </summary>
		public readonly int BuffType;
		public CheckMinionActive(ModProjectile modProjectile, int BuffType,int TeleportLength=16*400) : base(modProjectile) {
			this.BuffType = BuffType;
			this.TeleportLength = TeleportLength;
		}
		/// <summary>
		/// 与玩家距离太远传送的距离，<![CDATA[<]]>0不传送
		/// </summary>
		public int TeleportLength;
		public override void Update()
		{
			Player player = Main.player[projectile.owner];
			if (player.dead || !player.active)
			{
				player.ClearBuff(BuffType);
			}
			if (!player.HasBuff(BuffType))
			{
				projectile.Kill();
			}
			if (TeleportLength>0&&(projectile.Center - player.Center).Length() > TeleportLength) projectile.Center = player.Center;
		}
	}
}
