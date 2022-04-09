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
	/// <summary>
	/// 重力
	/// </summary>
	public class Gravity : PhysicsPart<ModNPC>
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override bool NetUpdate =>false;
		/// <summary>
		/// 最大下落速度
		/// </summary>
		public float MaxFallSpeed = 16f;
		/// <summary>
		/// 1/60s的加速度
		/// </summary>
		public float g =0.3f;
		public Gravity(PhysicsEntity physicsEntity) : base(physicsEntity.modNPC, physicsEntity) { }
		public override void Update()
		{
			AddForce(RealBarycenter,new Vector2(0, g * Mass) );
			if (npc.velocity.Y > MaxFallSpeed) npc.velocity.Y = MaxFallSpeed;
		}
		public Gravity Set(float MaxFallSpeed, float g=0.3f) {
			this.MaxFallSpeed = MaxFallSpeed;
			this.g = g;
			return this;
		}
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
	}
}
