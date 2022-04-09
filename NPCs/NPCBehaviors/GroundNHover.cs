using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace XxDefinitions.NPCs.NPCBehaviors
{
	/// <summary>
	/// 选取N个点提供力
	/// </summary>
	[Obsolete("使用Physics")]
	public class GroundNHover:ModNPCBehavior<ModNPC>
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override bool NetUpdate => false;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
		/// <summary>
		/// 点
		/// </summary>
		public Vector2[] HoverPoint;
		/// <summary>
		/// 重心
		/// </summary>
		public Vector2 BarycenterOffsetCenter;
		/// <summary>
		/// 提供力的高度
		/// </summary>
		public float HoverHigh;
		/// <summary>
		/// 力的大小
		/// </summary>
		public float HoverForce;
		/// <summary>
		/// 转动惯量
		/// </summary>
		public float RotaryInertia;
		/// <summary>
		/// 质量
		/// </summary>
		public float Mass;
		/// <summary>
		/// 角速度
		/// </summary>
		public float Palstance=0;
		/// <summary>
		/// 最大转角
		/// </summary>
		public float MaxRotation=(float)Math.PI/2*0.9f;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public GroundNHover(ModNPC modNPC,Vector2 Barycenter,float Mass,float RotaryInertia, Vector2[] HoverPoint,float HoverHigh,float HoverForce) : base(modNPC) {
			this.HoverPoint = HoverPoint;
			this.BarycenterOffsetCenter = Barycenter-npc.Size/2;
			this.HoverHigh = HoverHigh;
			this.HoverForce = HoverForce;
			this.Mass = Mass;
			this.RotaryInertia = RotaryInertia;
		}
		public override void OnActivate()
		{
			//npc.noGravity = false;
			npc.noTileCollide = true;
		}
		public override void Update()
		{
			base.Update();
			bool Pushed = false;
			Vector2 RealBarycenter = (BarycenterOffsetCenter * new Vector2(npc.direction, 1)).RotatedBy(npc.rotation);
			foreach (var i in HoverPoint) {
				Vector2 RealPoint = (i*new Vector2(npc.direction,1)).RotatedBy(npc.rotation);
				float L = Utils.CalculateUtils.CanHitLineDistance(RealPoint+npc.Center,(float)Math.PI/2f,HoverHigh);
				if (L < HoverHigh) {
					Pushed = true;
					float D=(HoverHigh-L)/ HoverHigh;
					Vector2 RealHoverForce=new Vector2(0,-HoverForce*D);
					npc.velocity += RealHoverForce / Mass;
					Palstance += (RealPoint.X- RealBarycenter.X) * -HoverForce * D/ RotaryInertia;
				}
			}
			npc.rotation += Palstance;
			if (Pushed)
			{
				if (npc.velocity.Y > 3) npc.velocity.Y = 3;
				npc.velocity.Y *= 0.9f;
				Palstance *= 0.9f;
			}
			else {
				Palstance *= 0.96f;
			}
			npc.rotation *= 0.985f;
			if (npc.velocity.Y < -8) npc.velocity.Y = -8;
			npc.rotation=Terraria.Utils.Clamp(npc.rotation,-MaxRotation,MaxRotation);
		}
		public override void NetUpdateSend(BinaryWriter writer)
		{
			writer.Write(Palstance);
		}
		public override void NetUpdateReceive(BinaryReader reader)
		{
			Palstance = reader.ReadSingle();
		}
	}
}
