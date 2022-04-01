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

namespace XxDefinitions.NPCs.NPCBehaviors
{
	public class HangOnNPC:ModNPCBehavior<ModNPC>
	{
		public override string BehaviorName => "HangOn";
		public override bool NetUpdate =>false;
		public Vector2 Offset;
		public NPC entity;
		public HangOnNPC(ModNPC modNPC, NPC target, Vector2 Offset) : base(modNPC) {
			entity = target;this.Offset = Offset;
		}
		public override void Update()
		{
			base.Update();
			npc.Center = entity.Center + (Offset * new Vector2(entity.direction, 1)).RotatedBy(entity.rotation);
		}
		public override void OnActivate()
		{
			Update();
		}
	}
	public class HangOnProj : ModNPCBehavior<ModNPC>
	{
		public override string BehaviorName => "HangOn";
		public override bool NetUpdate => false;
		public Vector2 Offset;
		public Projectile entity;
		public HangOnProj(ModNPC modNPC, Projectile target, Vector2 Offset) : base(modNPC)
		{
			entity = target; this.Offset = Offset;
		}
		public override void Update()
		{
			base.Update();
			npc.Center = entity.Center + (Offset * new Vector2(entity.direction, 1)).RotatedBy(entity.rotation);
		}
		public override void OnActivate()
		{
			Update();
		}
	}
}
