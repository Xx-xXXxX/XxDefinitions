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
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	/// <summary>
	/// 一直推
	/// </summary>
	public class Pusher:PhysicsPart<ModNPC>
	{
		public override string BehaviorName => "Pusher";
		public override bool NetUpdate => false;
		/// <summary>
		/// 相对位置
		/// </summary>
		public Vector2 Offset;
		/// <summary>
		/// 力，向下为正
		/// </summary>
		public IGetValue<Vector2> Force;
		public Pusher(PhysicsEntity physicsEntity,Vector2 Offset, IGetValue<Vector2> Force) : base(physicsEntity.modNPC, physicsEntity) {
			this.Offset = Offset;
			this.Force = Force;
		}
		public override void Update()
		{
			AddForce(Offset.OffsetToWorld(npc),Force.Value);
		}
	}
}
