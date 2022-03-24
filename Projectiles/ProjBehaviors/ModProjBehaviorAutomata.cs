using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using XxDefinitions.NPCs.NPCBehaviors;

namespace XxDefinitions.Projectiles.ProjBehaviors
{
	/// <summary>
	/// ModProjectile应用行为自动机的基类
	/// </summary>
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	public abstract class ModProjBehaviorAutomata:ModProjectile
	{
		public Behavior.BehaviorSet<IModProjBehavior> BehaviorSet = new Behavior.BehaviorSet<IModProjBehavior>();
		public IModProjBehavior BehaviorMain => BehaviorSet.BehaviorMain;
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			BehaviorSet.NetUpdateReceive(reader);
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			BehaviorSet.NetUpdateSend(writer);
		}
		public override void AI()
		{
			BehaviorSet.Update();
		}
		#region Hooks
		public override bool? CanHitNPC(NPC target)
		{
			bool? Result = null;
			foreach (var ID in BehaviorSet)
			{
				bool? N = BehaviorSet[ID].CanHitNPC(target);
				if (N != null) Result = N;
			}
			return Result;
			//return base.CanHitNPC(target);
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			foreach (var ID in BehaviorSet)
			{
				BehaviorSet[ID].ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			foreach (var ID in BehaviorSet)
			{
				BehaviorSet[ID].OnHitNPC(target, damage, knockback, crit);
			}
		}
		public override bool? CanCutTiles()
		{
			bool? Result = null;
			foreach (var ID in BehaviorSet)
			{
				bool? N = BehaviorSet[ID].CanCutTiles();
				if (N != null) Result = N;
			}
			return Result;
			//return base.CanCutTiles();
		}
		public override void CutTiles()
		{
			foreach (var ID in BehaviorSet)
			{
				BehaviorSet[ID].CutTiles();
			}
		}
		public override bool CanHitPlayer(Player target)
		{
			bool DefResult = true;
			bool Result = DefResult;
			foreach (var ID in BehaviorSet)
			{
				bool N = BehaviorSet[ID].CanHitPlayer(target);
				if (N != DefResult) Result = N;
			}
			return Result;
			//return base.CanHitPlayer(target);
		}
		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			foreach (var ID in BehaviorSet)
			{
				BehaviorSet[ID].ModifyHitPlayer(target, ref damage, ref crit);
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			foreach (var ID in BehaviorSet)
			{
				BehaviorSet[ID].OnHitPlayer(target, damage, crit);
			}
		}
		public override bool CanHitPvp(Player target)
		{
			bool DefResult = true;
			bool Result = DefResult;
			foreach (var ID in BehaviorSet)
			{
				bool N = BehaviorSet[ID].CanHitPvp(target);
				if (N != DefResult) Result = N;
			}
			return Result;
			//return base.CanHitPvp(target);
		}
		public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
		{
			foreach (var ID in BehaviorSet)
			{
				BehaviorSet[ID].ModifyHitPvp(target, ref damage, ref crit);
			}
		}
		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			foreach (var ID in BehaviorSet)
			{
				BehaviorSet[ID].OnHitPvp(target, damage, crit);
			}
		}
		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			foreach (var ID in BehaviorSet)
			{
				BehaviorSet[ID].DrawBehind(index, drawCacheProjsBehindNPCsAndTiles, drawCacheProjsBehindNPCs, drawCacheProjsBehindProjectiles, drawCacheProjsOverWiresUI);
			}
		}

		/// <summary>
		/// 会BehaviorSet.Dispose
		/// </summary>
		public override void Kill(int timeLeft)
		{
			foreach (var ID in BehaviorSet)
			{
				BehaviorSet[ID].Kill(timeLeft);
			}
			BehaviorSet.Dispose();
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			foreach (var ID in BehaviorSet)
			{
				BehaviorSet[ID].ModifyDamageHitbox(ref hitbox);
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			foreach (var ID in BehaviorSet)
			{
				BehaviorSet[ID].PostDraw(spriteBatch, lightColor);
			}
		}
		public override void UseGrapple(Player player, ref int type)
		{
			foreach (var ID in BehaviorSet)
			{
				BehaviorSet[ID].UseGrapple(player, ref type);
			}
		}
		public override void NumGrappleHooks(Player player, ref int numHooks)
		{
			foreach (var ID in BehaviorSet)
			{
				int n = 0;
				BehaviorSet[ID].NumGrappleHooks(player, ref n);
				numHooks += n;
			}
		}
		public override void GrapplePullSpeed(Player player, ref float speed)
		{
			foreach (var ID in BehaviorSet)
			{
				float n = 0;
				BehaviorSet[ID].GrapplePullSpeed(player, ref speed);
				speed += n;
			}
		}
		public override float GrappleRange()
		{
			float N = 0;
			foreach (var ID in BehaviorSet)
			{
				N += BehaviorSet[ID].GrappleRange();
			}
			return N;
		}
		public override bool? CanUseGrapple(Player player)
		{
			bool? Result = null;
			foreach (var ID in BehaviorSet)
			{
				bool? N = BehaviorSet[ID].CanUseGrapple(player);
				if (N != null) Result = N;
			}
			return Result;
		}
		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			foreach (var ID in BehaviorSet)
			{
				float n = 0;
				BehaviorSet[ID].GrappleRetreatSpeed(player, ref speed);
				speed += n;
			}
		}
		public override void GrappleTargetPoint(Player player, ref float grappleX, ref float grappleY)
		{
			//base.GrappleTargetPoint(player, ref grappleX, ref grappleY);
			float RX = 0, RY = 0;
			int n = 0;
			foreach (var ID in BehaviorSet)
			{
				float RXn = 0, RYn = 0;
				BehaviorSet[ID].GrappleTargetPoint(player, ref RXn, ref RYn);
				if (RXn != 0 || RXn != 0) { n += 1; RX += RXn; RY += RYn; }
			}
			if (n != 0) { RX /= n; RY /= n; }
			grappleX = RX;
			grappleY = RY;
		}
		public override bool? SingleGrappleHook(Player player)
		{
			bool? Result = null;
			foreach (var ID in BehaviorSet)
			{
				bool? N = BehaviorSet[ID].SingleGrappleHook(player);
				if (N != null) Result = N;
			}
			return Result;
			//return base.SingleGrappleHook(player);
		}
		public override bool CanDamage()
		{
			bool DefResult = false;
			bool Result = DefResult;
			foreach (var ID in BehaviorSet)
			{
				bool N = BehaviorSet[ID].CanDamage();
				if (N != DefResult) Result = N;
			}
			return Result;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			bool? Result = null;
			foreach (var ID in BehaviorSet)
			{
				bool? N = BehaviorSet[ID].Colliding(projHitbox, targetHitbox);
				if (N != null) Result = N;
			}
			return Result;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			bool DefResult = false;
			bool Result = DefResult;
			foreach (var ID in BehaviorSet)
			{
				bool N = BehaviorSet[ID].OnTileCollide(oldVelocity);
				if (N != DefResult) Result = N;
			}
			return Result;
		}
		public override bool PreKill(int timeLeft)
		{
			bool DefResult = true;
			bool Result = DefResult;
			foreach (var ID in BehaviorSet)
			{
				bool N = BehaviorSet[ID].PreKill(timeLeft);
				if (N != DefResult) Result = N;
			}
			return Result;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			bool DefResult = true;
			bool Result = DefResult;
			foreach (var ID in BehaviorSet)
			{
				bool N = BehaviorSet[ID].PreDraw(spriteBatch, lightColor);
				if (N != DefResult) Result = N;
			}
			return Result;
		}
		public override bool PreDrawExtras(SpriteBatch spriteBatch)
		{
			bool DefResult = true;
			bool Result = DefResult;
			foreach (var ID in BehaviorSet)
			{
				bool N = BehaviorSet[ID].PreDrawExtras(spriteBatch);
				if (N != DefResult) Result = N;
			}
			return Result;
		}
		public override bool ShouldUpdatePosition()
		{
			bool DefResult = true;
			bool Result = DefResult;
			foreach (var ID in BehaviorSet)
			{
				bool N = BehaviorSet[ID].ShouldUpdatePosition();
				if (N != DefResult) Result = N;
			}
			return Result;
		}
		public override bool MinionContactDamage()
		{
			bool DefResult = false;
			bool Result = DefResult;
			foreach (var ID in BehaviorSet)
			{
				bool N = BehaviorSet[ID].MinionContactDamage();
				if (N != DefResult) Result = N;
			}
			return Result;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			bool DefResult = true;
			bool Result = DefResult;
			foreach (var ID in BehaviorSet)
			{
				bool N = BehaviorSet[ID].TileCollideStyle(ref width, ref height, ref fallThrough);
				if (N != DefResult) Result = N;
			}
			return Result;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			Color? Result = null;
			foreach (var ID in BehaviorSet)
			{
				Color? N = BehaviorSet[ID].GetAlpha(lightColor);
				if (N != null) Result = N;
			}
			return Result;
		}
		#endregion
		/*
		#region ModProjHooks
		public override bool ShouldUpdatePosition()
		{
			return BehaviorNow.ShouldUpdatePosition();
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			return BehaviorNow.TileCollideStyle(ref width, ref height, ref fallThrough);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return BehaviorNow.OnTileCollide(oldVelocity);
		}

		public override bool? CanCutTiles()
		{
			return BehaviorNow.CanCutTiles();
		}

		public override void CutTiles()
		{
			BehaviorNow.CutTiles();
		}

		public override bool PreKill(int timeLeft)
		{
			return BehaviorNow.PreKill(timeLeft);
		}

		public override void Kill(int timeLeft)
		{
			BehaviorNow.Kill(timeLeft);
		}

		public override bool CanDamage()
		{
			return BehaviorNow.CanDamage();
		}

		public override bool MinionContactDamage()
		{
			return BehaviorNow.MinionContactDamage();
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			BehaviorNow.ModifyDamageHitbox(ref hitbox);
		}

		public override bool? CanHitNPC(NPC target)
		{
			return BehaviorNow.CanHitNPC(target);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			BehaviorNow.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			BehaviorNow.OnHitNPC(target, damage, knockback, crit);
		}

		public override bool CanHitPvp(Player target)
		{
			return BehaviorNow.CanHitPvp(target);
		}

		public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
		{
			BehaviorNow.ModifyHitPvp(target, ref damage, ref crit);
		}

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			BehaviorNow.OnHitPvp(target, damage, crit);
		}

		public override bool CanHitPlayer(Player target)
		{
			return BehaviorNow.CanHitPlayer(target);
		}

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			BehaviorNow.ModifyHitPlayer(target, ref damage, ref crit);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			BehaviorNow.OnHitPlayer(target, damage, crit);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return BehaviorNow.Colliding(projHitbox, targetHitbox);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return BehaviorNow.GetAlpha(lightColor);
		}

		public override bool PreDrawExtras(SpriteBatch spriteBatch)
		{
			return BehaviorNow.PreDrawExtras(spriteBatch);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return BehaviorNow.PreDraw(spriteBatch, lightColor);
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			BehaviorNow.PostDraw(spriteBatch, lightColor);
		}

		public override bool? CanUseGrapple(Player player)
		{
			return BehaviorNow.CanUseGrapple(player);
		}

		public override bool? SingleGrappleHook(Player player)
		{
			return BehaviorNow.SingleGrappleHook(player);
		}

		public override void UseGrapple(Player player, ref int type)
		{
			BehaviorNow.UseGrapple(player, ref type);
		}

		public override float GrappleRange()
		{
			return BehaviorNow.GrappleRange();
		}

		public override void NumGrappleHooks(Player player, ref int numHooks)
		{
			BehaviorNow.NumGrappleHooks(player, ref numHooks);
		}

		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			BehaviorNow.GrappleRetreatSpeed(player, ref speed);
		}

		public override void GrapplePullSpeed(Player player, ref float speed)
		{
			BehaviorNow.GrapplePullSpeed(player, ref speed);
		}

		public override void GrappleTargetPoint(Player player, ref float grappleX, ref float grappleY)
		{
			BehaviorNow.GrappleTargetPoint(player, ref grappleX, ref grappleY);
		}

		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			BehaviorNow.DrawBehind(index, drawCacheProjsBehindNPCsAndTiles, drawCacheProjsBehindNPCs, drawCacheProjsBehindProjectiles, drawCacheProjsOverWiresUI);
		}

		#endregion
		*/
	}
}
