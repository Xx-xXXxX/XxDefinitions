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

namespace XxDefinitions.Projectiles.ProjBehaviors
{
	public class HangOnNPC : ModProjBehavior<ModProjectile>
	{
		public override string BehaviorName => "HangOn";
		public override bool NetUpdate => false;
		public Vector2 Offset;
		public NPC entity;
		public HangOnNPC(ModProjectile modProjectile, NPC target, Vector2 Offset) : base(modProjectile)
		{
			entity = target; this.Offset = Offset;
		}
		public override void Update()
		{
			base.Update();
			projectile.Center = entity.Center + (Offset * new Vector2(entity.direction, 1)).RotatedBy(entity.rotation);
		}
		public override void OnActivate()
		{
			Update();
		}
	}
	public class HangOnProj : ModProjBehavior<ModProjectile>
	{
		public override string BehaviorName => "HangOn";
		public override bool NetUpdate => false;
		public Vector2 Offset;
		public Projectile entity;
		public HangOnProj(ModProjectile modProjectile, Projectile target, Vector2 Offset) : base(modProjectile)
		{
			entity = target; this.Offset = Offset;
		}
		public override void Update()
		{
			base.Update();
			projectile.Center = entity.Center + (Offset * new Vector2(entity.direction, 1)).RotatedBy(entity.rotation);
		}
		public override void OnActivate()
		{
			Update();
		}
	}
}
