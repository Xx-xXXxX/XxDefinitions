using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Drawing;
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
	/// 圆形轮子
	/// </summary>
	public class CircularWhell:PhysicsPartComponent<ModNPC>
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override bool NetUpdateThis => false;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
		/// <summary>
		/// 相对位置
		/// </summary>
		public Vector2 Offset;
		/// <summary>
		/// 半径
		/// </summary>
		public float R;
		/// <summary>
		/// 弹性
		/// </summary>
		public float Elasticity;
		/// <summary>
		/// 模拟精度
		/// </summary>
		public int N;
		/// <summary>
		/// 轮胎的半径
		/// </summary>
		public float TireR;
		/// <summary>
		/// 转动提供的动力（？），顺时针产生的动力为正
		/// </summary>
		public IGetValue<float> RotatePower; 
		public CircularWhell(PhysicsEntity physicsEntity,Vector2 Offset,float R, float TireR, IGetValue<float> RotatePower, float Elasticity=0.5f,int N=16) : base(physicsEntity.modNPC, physicsEntity) {
			this.Offset = Offset;
			this.R = R;
			this.TireR = TireR;
			this.Elasticity = Elasticity;
			this.N = N;
			this.RotatePower = RotatePower;
		}
		private bool ChildrenActivated = false;
		public override void Update()
		{
			base.Update();
			bool Collided=false;
			for (float i = 0; i < Math.PI * 2; i += (float)Math.PI / 4)
			{
				Vector2 RealCenter = Offset.OffsetToWorld(npc);
				Vector2 RealNext = //(Offset.RotatedBy(Palstance)).OffsetToWorld(npc)+npc.velocity;
					(Offset).OffsetToWorld(npc.Center + npc.velocity, npc.rotation + Palstance, npc.direction);
				float BL = Utils.CalculateUtils.CanHitLineDistancePerfect(RealNext, i, R + TireR);
				if (BL < R+ TireR)
				{
					Vector2 DVel = (BL-R-TireR) * i.ToRotationVector2();
					if(BL < R)
						npc.position += (BL - R) * i.ToRotationVector2() * (1 - Elasticity) * 1f;
					AddForce(RealCenter, Elasticity * DVel * Mass );
					Collided = true;
					float DF = Utils. Min((TireR+R - BL),4);
					AddForce(RealCenter, DF * RotatePower.Value*(i-(float)Math.PI/2).ToRotationVector2());
				}
			}

			if (Collided)
			{
				if (!ChildrenActivated)
				{
					foreach (var i in GetUsings()) i.TryActivate();
					ChildrenActivated = true;
				}
			}
			else
			{
				if (ChildrenActivated)
				{
					foreach (var i in GetUsings()) i.TryPause();
					ChildrenActivated = false;
				}
			}
		}
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
	}
}
