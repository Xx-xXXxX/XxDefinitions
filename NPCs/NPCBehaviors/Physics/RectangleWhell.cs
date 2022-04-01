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
	/// 如果离地距离近则产生力
	/// 组件在产生力时执行
	/// </summary>
	public class RectangleWhell:PhysicsPartComponent<ModNPC>
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override string BehaviorName => "RectangleWhell";
		public override bool NetUpdateThis => false;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
		/// <summary>
		/// 轮子的相对位置
		/// </summary>
		public Vector2 Offset;
		/// <summary>
		/// 轮子的长宽
		/// </summary>
		public int Size;
		/// <summary>
		/// 弹性，位置的修正有(1-Elasticity)作用于position,Elasticity产生Force
		/// </summary>
		public float Elasticity;
		/// <summary>
		/// 悬挂长度
		/// </summary>
		public float SuspensionHigh=4f;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public RectangleWhell(PhysicsEntity physicsEntity, Vector2 Offset, int Size,float Elasticity=0.5f) : base(physicsEntity.modNPC, physicsEntity)
		{
			this.Offset = Offset;
			this.Size = Size;
			this.Elasticity = Elasticity;
		}

		bool ChildrenActivated = false;
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
			base.Update();
			Vector2 RealCenter = Offset.OffsetToWorld(npc);
			Vector2 RealNext = //(Offset.RotatedBy(Palstance)).OffsetToWorld(npc)+npc.velocity;
				(Offset+new Vector2(0, SuspensionHigh)).OffsetToWorld(npc.Center+npc.velocity,npc.rotation+ Palstance,npc.direction);
			Vector2 RealV0 = RealNext - RealCenter;
			Vector2 RealV = Terraria.Collision.TileCollision(RealCenter-new Vector2(Size/2), RealV0, Size,Size,false,true,0);
			//float L2 = 1.414f * Size / 2;
			//for (float i = 0; i < Math.PI * 2; i += (float)Math.PI / 4)
			//{
			//	float BL = Utils.CalculateUtils.CanHitLineDistance(RealNext, i, L2);
			//	RealV += (BL - L2) * i.ToRotationVector2();
			//}
			if (RealV != RealV0)
			{
				if (!ChildrenActivated)
				{
					foreach (var i in GetUsings()) i.TryActivate();
					ChildrenActivated = true;
				}
				Vector2 DVel = RealV - RealV0;
				npc.position += DVel * (1 - Elasticity)*1f;
				AddForce(RealCenter, Elasticity*DVel*Mass);
			}
			else {
				if (ChildrenActivated)
				{
					foreach (var i in GetUsings()) i.TryPause();
					ChildrenActivated = false;
				}
			}
		}
	}
}
