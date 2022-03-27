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
	/// 设置Projectile悬浮时横向移动时的方向
	/// </summary>
	public class HoverRotation : ModProjBehavior<ModProjectile>
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override string BehaviorName => "HoverRotation";

		public override bool NetUpdate =>false;
		public HoverRotation(ModProjectile modProjectile) : base(modProjectile) { }

		public override void Update()
		{
			projectile.rotation = (float)(-Math.Atan2(0.3f, projectile.velocity.X / 80f)+Math.PI/2f);
		}
	}
}
