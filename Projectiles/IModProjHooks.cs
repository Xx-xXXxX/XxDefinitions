using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
namespace XxDefinitions.Projectiles
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	public interface IModProjHooks
	{

		bool ShouldUpdatePosition();
		bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough);
		bool OnTileCollide(Vector2 oldVelocity);
		bool? CanCutTiles();
		void CutTiles();
		bool PreKill(int timeLeft);
		void Kill(int timeLeft);
		bool CanDamage();
		bool MinionContactDamage();
		void ModifyDamageHitbox(ref Rectangle hitbox);
		bool? CanHitNPC(NPC target);
		void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection);
		void OnHitNPC(NPC target, int damage, float knockback, bool crit);
		bool CanHitPvp(Player target);
		void ModifyHitPvp(Player target, ref int damage, ref bool crit);
		void OnHitPvp(Player target, int damage, bool crit);
		bool CanHitPlayer(Player target);
		void ModifyHitPlayer(Player target, ref int damage, ref bool crit);
		void OnHitPlayer(Player target, int damage, bool crit);
		bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox);
		Color? GetAlpha(Color lightColor);
		bool PreDrawExtras(SpriteBatch spriteBatch);
		bool PreDraw(SpriteBatch spriteBatch, Color lightColor);
		void PostDraw(SpriteBatch spriteBatch, Color lightColor);
		bool? CanUseGrapple(Player player);
		bool? SingleGrappleHook(Player player);
		void UseGrapple(Player player, ref int type);
		float GrappleRange();
		void NumGrappleHooks(Player player, ref int numHooks);
		void GrappleRetreatSpeed(Player player, ref float speed);
		void GrapplePullSpeed(Player player, ref float speed);
		void GrappleTargetPoint(Player player, ref float grappleX, ref float grappleY);
		void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI);
	}
}
