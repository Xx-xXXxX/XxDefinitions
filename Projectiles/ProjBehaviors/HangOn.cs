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

using XxDefinitions.Behavior;

namespace XxDefinitions.Projectiles.ProjBehaviors
{
	/// <summary>
	/// 保持其在NPC的相对位置上
	/// 在entity无效（!active）时自动暂停
	/// </summary>
	public class HangOnNPC : ModProjBehavior<ModProjectile>
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override bool NetUpdate => false;
		/// <summary>
		/// 相对中心的位置
		/// </summary>
		public Vector2 Offset;
		/// <summary>
		/// 所在的npc
		/// </summary>
		public NPC entity;
		public HangOnNPC(ModProjectile modProjectile, NPC target, Vector2 Offset) : base(modProjectile)
		{
			entity = target; this.Offset = Offset;
		}
		public override void Update()
		{
			base.Update();
			if (!entity.active)
			{
				this.TryPause();
				if (!Active) return;
			}
			projectile.Center = entity.Center + (Offset * new Vector2(entity.direction, 1)).RotatedBy(entity.rotation);
		}
		public override void OnActivate()
		{
			Update();
		}
	}
	/// <summary>
	/// 保持其在Proj的相对位置上
	/// 在entity无效（!active）时自动暂停
	/// </summary>
	public class HangOnProj : ModProjBehavior<ModProjectile>
	{
		public override bool NetUpdate => false;
		/// <summary>
		/// 相对中心的位置
		/// </summary>
		public Vector2 Offset;
		/// <summary>
		/// 所在的npc
		/// </summary>
		public Projectile entity;
		public HangOnProj(ModProjectile modProjectile, Projectile target, Vector2 Offset) : base(modProjectile)
		{
			entity = target; this.Offset = Offset;
		}
		public override void Update()
		{
			base.Update();
			if (!entity.active)
			{
				this.TryPause();
				if (!Active) return;
			}
			projectile.Center = entity.Center + (Offset * new Vector2(entity.direction, 1)).RotatedBy(entity.rotation);
		}
		public override void OnActivate()
		{
			Update();
		}
	}
}
