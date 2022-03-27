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
using XxDefinitions;
namespace XxDefinitions.Projectiles.ProjBehaviors
{
	/// <summary>
	/// 跟踪目标
	/// </summary>
	public class Chase : ModProjBehavior<ModProjectile>
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override string BehaviorName => "Chase";

		public override bool NetUpdate => false;
		/// <summary>
		/// 目标位置
		/// </summary>
		public IGetValue<Vector2> Target;
		/// <summary>
		/// 减速比例，每帧执行
		/// </summary>
		public float SpeedReduce= 0.4f/16;
		/// <summary>
		/// 最大速度
		/// </summary>
		public float MaxSpeed=16;
		/// <summary>
		/// 加速度
		/// </summary>
		public float Acceleration=0.4f;
		/// <summary>
		/// 目标范围半径
		/// </summary>
		public float Range= 64;
		/// <summary>
		/// 最小减速度值，低于该速度不会减速
		/// </summary>
		public float MinReduceSpeed=4;
		public Chase(ModProjectile modProjectile, IGetValue<Vector2> Target,float Range=64, float MaxSpeed=16, float Acceleration=0.4f,float MinReduceSpeed=4) : base(modProjectile) {
			this.Target = Target;
			this.Range = Range;
			this.Acceleration = Acceleration;
			this.MaxSpeed = MaxSpeed;
			this.SpeedReduce= Acceleration / MaxSpeed;
			this.MinReduceSpeed = MinReduceSpeed;
		}
		public Vector2 ChaseMinionOwner(Vector2 Offset) {
			return Main.player[projectile.owner].Center + Offset;
		}
		public Chase SetChaseTarget(IGetValue<UnifiedTarget> target, Vector2 Offset, IGetValue<Vector2> NullTarget=null)
		{
			this.Target = (Get<Vector2>)delegate {
				if (target.Value.IsNPC) return target.Value.npc.Center + Offset;
				else if (target.Value.IsPlayer) return target.Value.player.Center + Offset;
				else return NullTarget?.Value ??(Main.player[projectile.owner].Center+Offset);
			};
			return this;
		}
		public override void Update()
		{
			Vector2 offset = Target.Value-projectile.Center;
			if (offset.Length() > Range) {
				projectile.velocity += Vector2.Normalize(offset) * Acceleration;
			}
			if(projectile.velocity.Length()> MinReduceSpeed)
				projectile.velocity *= 1 - SpeedReduce;
		}
	}
}
