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

namespace XxDefinitions.NPCs.NPCBehaviors.Physics
{
	/// <summary>
	/// 简单的模拟轮子，简单而有效
	/// </summary>
	public class EasyWhell : PhysicsPartComponent<ModNPC>
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override bool NetUpdateThis => false;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
		/// <summary>
		/// 相对中心位置
		/// </summary>
		public Vector2 Offset;
		/// <summary>
		/// 力的大小
		/// </summary>
		public float force;
		/// <summary>
		/// 高度
		/// </summary>
		public float height;
		/// <summary>
		/// 触地时的牵引力
		/// </summary>
		public IGetValue<float> WhellForce;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public EasyWhell(PhysicsEntity physicsEntity, Vector2 Offset, float force, float height,IGetValue<float> WhellForce) : base(physicsEntity.modNPC, physicsEntity)
		{
			this.Offset = Offset;
			this.force = force;
			this.height = height;
			this.WhellForce = WhellForce;
		}
		private bool ChildrenActivated = false;
		public override void OnActivate()
		{
			base.OnActivate();
			ChildrenActivated = true;
		}
		public override void OnPause()
		{
			base.OnPause();
			ChildrenActivated = false;
		}
		public override void Update()
		{
			Vector2 Pos = Offset.OffsetToWorld(npc);
			float ToGroundDistance = Utils.CalculateUtils.CanHitLineDistance(Pos, (float)Math.PI / 2f, height);
			if (ToGroundDistance < height)
			{
				if (!ChildrenActivated)
				{
					foreach (var i in GetUsings()) i.TryActivate();
					ChildrenActivated = true;
				}
				AddForce(Pos, new Vector2(WhellForce.Value, force * (ToGroundDistance - height) / height));
				
				if (npc.velocity.Y > 4) npc.velocity.Y = 4;
				npc.velocity.Y *= 0.96f;
				Palstance *= 0.97f;
			}
			else
				if (ChildrenActivated)
			{
				foreach (var i in GetUsings()) i.TryPause();
				ChildrenActivated = false;
			}
			base.Update();
		}
	}
}
