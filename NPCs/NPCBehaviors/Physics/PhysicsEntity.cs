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

namespace XxDefinitions.NPCs.NPCBehaviors.Physics
{
	/// <summary>
	/// 模拟物理系统
	/// 被模拟的对象不计算碰撞箱，不碰撞物块，无重力
	/// </summary>
	public class PhysicsEntity:ModNPCBehaviorComponent<ModNPC>
	{

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override string BehaviorName => "PhysicsEntity";
		public override bool NetUpdateThis => false;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
		/// <summary>
		/// 重心相对npc中心的位置
		/// </summary>
		public Vector2 BarycenterOffsetCenter;
		/// <summary>
		/// 重心的实际位置
		/// </summary>
		public Vector2 RealBarycenter {
			get => (BarycenterOffsetCenter * new Vector2(npc.direction, 1)).RotatedBy(npc.rotation) + npc.Center;
		}
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
		public float Palstance = 0;
		/// <summary>
		/// 最大旋转角
		/// </summary>
		public float MaxRotation = (float)Math.PI / 2 * 0.9f;
		/// <summary>
		/// 角度修正，使得角度向0趋近
		/// </summary>
		public float RotationCorrection = 0.01f;
		/// <summary>
		/// 角速度阻力
		/// </summary>
		public float PalstanceCorrection = 0.02f;
		/// <summary>
		/// 创建物理实体
		/// </summary>
		/// <param name="modNPC">目标NPC</param>
		/// <param name="Barycenter">相对npc.position的重心</param>
		/// <param name="Mass">质量</param>
		/// <param name="RotaryInertia">转动惯量</param>
		public PhysicsEntity(ModNPC modNPC, Vector2 Barycenter, float Mass, float RotaryInertia) : base(modNPC)
		{
			this.BarycenterOffsetCenter = Barycenter - npc.Size / 2;
			this.Mass = Mass;
			this.RotaryInertia = RotaryInertia;
		}
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override void OnActivate()
		{
			npc.noTileCollide = true;
			base.OnActivate();
		}
		public override void Update()
		{
			Palstance -= npc.rotation * RotationCorrection;
			base.Update();
			npc.rotation += Palstance;
			Palstance *= 1 - PalstanceCorrection;
			float OldRotation = npc.rotation;
			npc.rotation = Terraria.Utils.Clamp(npc.rotation, -MaxRotation, MaxRotation);
			if (OldRotation != npc.rotation) Palstance = 0;
		}
		public override void OnNetUpdateSend(BinaryWriter writer)
		{
			writer.Write(Palstance);
		}
		public override void OnNetUpdateReceive(BinaryReader reader)
		{
			Palstance = reader.ReadSingle();
		}
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
		/// <summary>
		/// 施加力
		/// </summary>
		/// <param name="Pos">力的实际位置</param>
		/// <param name="Force">力*60 （每1/60秒的冲量）</param>
		public void AddForce(Vector2 Pos, Vector2 Force) {
			npc.velocity += Force / Mass ;
			Palstance += Utils.CalculateUtils.CrossProduct(Force, RealBarycenter - Pos) /RotaryInertia;
		}

	}
	/// <summary>
	/// 物理系统组件的基类，组件应加在PhysicsEntity中
	/// </summary>
	public abstract class PhysicsPart<T>: ModNPCBehavior<T> 
		where T:ModNPC
	{
		/// <summary>
		/// 目标组件
		/// </summary>
		public PhysicsEntity physicsEntity;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public PhysicsPart(T modNPC, PhysicsEntity physicsEntity) : base(modNPC) {
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
			this.physicsEntity = physicsEntity;
		}
		/// <summary>
		/// 施加力
		/// </summary>
		/// <param name="Pos">力的实际位置</param>
		/// <param name="Force">力的大小</param>
		public void AddForce(Vector2 Pos, Vector2 Force)
		{
			physicsEntity.AddForce(Pos,Force);
		}
		/// <summary>
		/// 重心相对npc中心的位置
		/// </summary>
		public Vector2 BarycenterOffsetCenter=>physicsEntity.BarycenterOffsetCenter;
		/// <summary>
		/// 重心的实际位置
		/// </summary>
		public Vector2 RealBarycenter
		{
			get => physicsEntity.RealBarycenter;
		}
		/// <summary>
		/// 转动惯量
		/// </summary>
		public float RotaryInertia => physicsEntity.RotaryInertia;
		/// <summary>
		/// 质量
		/// </summary>
		public float Mass => physicsEntity.Mass;
		/// <summary>
		/// 角速度
		/// </summary>
		public float Palstance { 
			get=> physicsEntity.Palstance;
			set =>  physicsEntity.Palstance=value;
		}
		/// <summary>
		/// 最大旋转角
		/// </summary>
		public float MaxRotation => physicsEntity.MaxRotation;
	}

	/// <summary>
	/// 可组合物理系统组件的基类，组件应加在PhysicsEntity中
	/// </summary>
	public abstract class PhysicsPartComponent<T> : ModNPCBehaviorComponent<T>
		where T : ModNPC
	{
		/// <summary>
		/// 目标组件
		/// </summary>
		public PhysicsEntity physicsEntity;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public PhysicsPartComponent(T modNPC, PhysicsEntity physicsEntity) : base(modNPC)
		{
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
			this.physicsEntity = physicsEntity;
		}
		/// <summary>
		/// 施加力
		/// </summary>
		/// <param name="Pos">力的实际位置</param>
		/// <param name="Force">力的大小</param>
		public void AddForce(Vector2 Pos, Vector2 Force)
		{
			physicsEntity.AddForce(Pos, Force);
		}
		/// <summary>
		/// 重心相对npc中心的位置
		/// </summary>
		public Vector2 BarycenterOffsetCenter => physicsEntity.BarycenterOffsetCenter;
		/// <summary>
		/// 重心的实际位置
		/// </summary>
		public Vector2 RealBarycenter
		{
			get => physicsEntity.RealBarycenter;
		}
		/// <summary>
		/// 转动惯量
		/// </summary>
		public float RotaryInertia => physicsEntity.RotaryInertia;
		/// <summary>
		/// 质量
		/// </summary>
		public float Mass => physicsEntity.Mass;
		/// <summary>
		/// 角速度
		/// </summary>
		public float Palstance
		{
			get => physicsEntity.Palstance;
			set => physicsEntity.Palstance = value;
		}
		/// <summary>
		/// 最大旋转角
		/// </summary>
		public float MaxRotation => physicsEntity.MaxRotation;
	}
}
