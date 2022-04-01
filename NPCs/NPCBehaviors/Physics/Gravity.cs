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

namespace XxDefinitions.NPCs.NPCBehaviors.Physics
{
	//重力
	public class Gravity : PhysicsPart<ModNPC>
	{
		public override string BehaviorName => "Gravity";

		public override bool NetUpdate =>false;
		public float MaxFallSpeed = 16f;
		public Gravity(PhysicsEntity physicsEntity) : base(physicsEntity.modNPC, physicsEntity) { }
		public override void Update()
		{
			AddForce(RealBarycenter,new Vector2(0,0.3f*Mass) );
			if (npc.velocity.Y > MaxFallSpeed) npc.velocity.Y = MaxFallSpeed;
		}
		public Gravity SetMaxFallSpeed(float speed) {
			MaxFallSpeed = speed;return this;
		}
	}
}
