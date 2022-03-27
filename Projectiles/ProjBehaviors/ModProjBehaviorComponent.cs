using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using XxDefinitions.Behavior;
using XxDefinitions.Projectiles.ProjBehaviors;
using Microsoft.Xna.Framework.Graphics;

namespace XxDefinitions.Projectiles.ProjBehaviors
{
	/// <summary>
	/// 使用组合模式的IModProjBehavior
	/// </summary>
	public interface IModProjBehaviorComponent : IBehaviorComponent<IModProjBehavior>, IModProjBehavior { }
	/// <summary>
	/// 使用组合模式的ModProjBehavior
	/// </summary>
	public abstract class ModProjBehaviorComponent<RealModProjType>:BehaviorComponent<IModProjBehavior>, IModProjBehaviorComponent
		where RealModProjType:ModProjectile
	{
		/// <summary>
		/// 被操作的modProj
		/// </summary>
		public RealModProjType modProjectile;
		/// <summary>
		/// 被操作的projectile
		/// </summary>
		public Projectile projectile => modProjectile.projectile;
		/// <summary>
		/// 初始化
		/// </summary>
		public ModProjBehaviorComponent(RealModProjType modProjectile) { this.modProjectile = modProjectile; }

		#region Hooks
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public virtual bool? CanHitNPC(NPC target)
		{
			bool? Result = null;
			foreach (var behavior in this)
			{
				bool? N = behavior.CanHitNPC(target);
				if (N != null) Result = N;
			}
			return Result;
			//return base.CanHitNPC(target);
		}
		public virtual void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			foreach (var behavior in this)
			{
				behavior.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
			}
		}
		public virtual void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			foreach (var behavior in this)
			{
				behavior.OnHitNPC(target, damage, knockback, crit);
			}
		}
		public virtual bool? CanCutTiles()
		{
			bool? Result = null;
			foreach (var behavior in this)
			{
				bool? N = behavior.CanCutTiles();
				if (N != null) Result = N;
			}
			return Result;
			//return base.CanCutTiles();
		}
		public virtual void CutTiles()
		{
			foreach (var behavior in this)
			{
				behavior.CutTiles();
			}
		}
		public virtual bool CanHitPlayer(Player target)
		{
			bool DefResult = true;
			bool Result = DefResult;
			foreach (var behavior in this)
			{
				bool N = behavior.CanHitPlayer(target);
				if (N != DefResult) Result = N;
			}
			return Result;
			//return base.CanHitPlayer(target);
		}
		public virtual void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			foreach (var behavior in this)
			{
				behavior.ModifyHitPlayer(target, ref damage, ref crit);
			}
		}
		public virtual void OnHitPlayer(Player target, int damage, bool crit)
		{
			foreach (var behavior in this)
			{
				behavior.OnHitPlayer(target, damage, crit);
			}
		}
		public virtual bool CanHitPvp(Player target)
		{
			bool DefResult = true;
			bool Result = DefResult;
			foreach (var behavior in this)
			{
				bool N = behavior.CanHitPvp(target);
				if (N != DefResult) Result = N;
			}
			return Result;
			//return base.CanHitPvp(target);
		}
		public virtual void ModifyHitPvp(Player target, ref int damage, ref bool crit)
		{
			foreach (var behavior in this)
			{
				behavior.ModifyHitPvp(target, ref damage, ref crit);
			}
		}
		public virtual void OnHitPvp(Player target, int damage, bool crit)
		{
			foreach (var behavior in this)
			{
				behavior.OnHitPvp(target, damage, crit);
			}
		}
		public virtual void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			foreach (var behavior in this)
			{
				behavior.DrawBehind(index, drawCacheProjsBehindNPCsAndTiles, drawCacheProjsBehindNPCs, drawCacheProjsBehindProjectiles, drawCacheProjsOverWiresUI);
			}
		}

		/// <summary>
		/// 会this.Dispose
		/// </summary>
		public virtual void Kill(int timeLeft)
		{
			foreach (var behavior in this)
			{
				behavior.Kill(timeLeft);
			}
			this.Dispose();
		}
		public virtual void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			foreach (var behavior in this)
			{
				behavior.ModifyDamageHitbox(ref hitbox);
			}
		}
		public virtual void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			foreach (var behavior in this)
			{
				behavior.PostDraw(spriteBatch, lightColor);
			}
		}
		public virtual void UseGrapple(Player player, ref int type)
		{
			foreach (var behavior in this)
			{
				behavior.UseGrapple(player, ref type);
			}
		}
		public virtual void NumGrappleHooks(Player player, ref int numHooks)
		{
			foreach (var behavior in this)
			{
				int n = 0;
				behavior.NumGrappleHooks(player, ref n);
				numHooks += n;
			}
		}
		public virtual void GrapplePullSpeed(Player player, ref float speed)
		{
			foreach (var behavior in this)
			{
				float n = 0;
				behavior.GrapplePullSpeed(player, ref speed);
				speed += n;
			}
		}
		public virtual float GrappleRange()
		{
			float N = 0;
			foreach (var behavior in this)
			{
				N += behavior.GrappleRange();
			}
			return N;
		}
		public virtual bool? CanUseGrapple(Player player)
		{
			bool? Result = null;
			foreach (var behavior in this)
			{
				bool? N = behavior.CanUseGrapple(player);
				if (N != null) Result = N;
			}
			return Result;
		}
		public virtual void GrappleRetreatSpeed(Player player, ref float speed)
		{
			foreach (var behavior in this)
			{
				float n = 0;
				behavior.GrappleRetreatSpeed(player, ref speed);
				speed += n;
			}
		}
		public virtual void GrappleTargetPoint(Player player, ref float grappleX, ref float grappleY)
		{
			//base.GrappleTargetPoint(player, ref grappleX, ref grappleY);
			float RX = 0, RY = 0;
			int n = 0;
			foreach (var behavior in this)
			{
				float RXn = 0, RYn = 0;
				behavior.GrappleTargetPoint(player, ref RXn, ref RYn);
				if (RXn != 0 || RXn != 0) { n += 1; RX += RXn; RY += RYn; }
			}
			if (n != 0) { RX /= n; RY /= n; }
			grappleX = RX;
			grappleY = RY;
		}
		public virtual bool? SingleGrappleHook(Player player)
		{
			bool? Result = null;
			foreach (var behavior in this)
			{
				bool? N = behavior.SingleGrappleHook(player);
				if (N != null) Result = N;
			}
			return Result;
			//return base.SingleGrappleHook(player);
		}
		public virtual bool CanDamage()
		{
			bool DefResult = false;
			bool Result = DefResult;
			foreach (var behavior in this)
			{
				bool N = behavior.CanDamage();
				if (N != DefResult) Result = N;
			}
			return Result;
		}
		public virtual bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			bool? Result = null;
			foreach (var behavior in this)
			{
				bool? N = behavior.Colliding(projHitbox, targetHitbox);
				if (N != null) Result = N;
			}
			return Result;
		}
		public virtual bool OnTileCollide(Vector2 oldVelocity)
		{
			bool DefResult = false;
			bool Result = DefResult;
			foreach (var behavior in this)
			{
				bool N = behavior.OnTileCollide(oldVelocity);
				if (N != DefResult) Result = N;
			}
			return Result;
		}
		public virtual bool PreKill(int timeLeft)
		{
			bool DefResult = true;
			bool Result = DefResult;
			foreach (var behavior in this)
			{
				bool N = behavior.PreKill(timeLeft);
				if (N != DefResult) Result = N;
			}
			return Result;
		}
		public virtual bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			bool DefResult = true;
			bool Result = DefResult;
			foreach (var behavior in this)
			{
				bool N = behavior.PreDraw(spriteBatch, lightColor);
				if (N != DefResult) Result = N;
			}
			return Result;
		}
		public virtual bool PreDrawExtras(SpriteBatch spriteBatch)
		{
			bool DefResult = true;
			bool Result = DefResult;
			foreach (var behavior in this)
			{
				bool N = behavior.PreDrawExtras(spriteBatch);
				if (N != DefResult) Result = N;
			}
			return Result;
		}
		public virtual bool ShouldUpdatePosition()
		{
			bool DefResult = true;
			bool Result = DefResult;
			foreach (var behavior in this)
			{
				bool N = behavior.ShouldUpdatePosition();
				if (N != DefResult) Result = N;
			}
			return Result;
		}
		public virtual bool MinionContactDamage()
		{
			bool DefResult = false;
			bool Result = DefResult;
			foreach (var behavior in this)
			{
				bool N = behavior.MinionContactDamage();
				if (N != DefResult) Result = N;
			}
			return Result;
		}
		public virtual bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			bool DefResult = true;
			bool Result = DefResult;
			foreach (var behavior in this)
			{
				bool N = behavior.TileCollideStyle(ref width, ref height, ref fallThrough);
				if (N != DefResult) Result = N;
			}
			return Result;
		}
		public virtual Color? GetAlpha(Color lightColor)
		{
			Color? Result = null;
			foreach (var behavior in this)
			{
				Color? N = behavior.GetAlpha(lightColor);
				if (N != null) Result = N;
			}
			return Result;
		}
		#endregion
	}
	public class ModProjBehaviorComponentState : ModProjBehaviorComponent<ModProjectile>
	{
		public readonly string name;
		public override string BehaviorName => name;
		public override bool NetUpdate => true;
		public ModProjBehaviorComponentState(ModProjectile projectile,string name= "State") : base(projectile) { this.name = name; }
	}
}
