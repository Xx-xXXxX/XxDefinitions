using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
namespace XxDefinitions.Projectiles.ProjBehaviors
{
	/// <summary>
	/// ModProj的行为的接口
	/// </summary>
	public interface IModProjBehavior : Behavior.IBehavior, IModProjHooks { }
	/// <summary>
	/// ModProj的行为的基类
	/// </summary>
	/// <typeparam name="RealModProjType"></typeparam>
	public abstract class ModProjBehavior<RealModProjType>:Behavior.Behavior, IModProjBehavior
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
		public ModProjBehavior(RealModProjType modProjectile) { this.modProjectile = modProjectile; }
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		#region IModProjHooks
		/// <summary>
		/// Whether or not this projectile should update its position based on factors such as its velocity, whether it is in liquid, etc. Return false to make its velocity have no effect on its position. Returns true by default.
		/// </summary>
		public virtual bool ShouldUpdatePosition()
		{
			return true;
		}
		/// <summary>
		/// Allows you to determine how this projectile interacts with tiles. Width and height determine the projectile's hitbox for tile collision, and default to -1. Leave them as -1 to use the projectile's real size. Fallthrough determines whether the projectile can fall through platforms, etc., and defaults to true.
		/// </summary>
		/// <param name="width">Width of the hitbox.</param>
		/// <param name="height">Height of the hitbox.</param>
		/// <param name="fallThrough">If the projectile can fall through platforms etc.</param>
		public virtual bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			return true;
		}
		/// <summary>
		/// Allows you to determine what happens when this projectile collides with a tile. OldVelocity is the velocity before tile collision. The velocity that takes tile collision into account can be found with projectile.velocity. Return true to allow the vanilla tile collision code to take place (which normally kills the projectile). Returns true by default.
		/// </summary>
		/// <param name="oldVelocity">The velocity of the projectile upon collision.</param>
		public virtual bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}
		/// <summary>
		/// Return true or false to specify if the projectile can cut tiles, like vines. Return null for vanilla decision.
		/// </summary>
		public virtual bool? CanCutTiles()
		{
			return null;
		}
		/// <summary>
		/// Code ran when the projectile cuts tiles. Only runs if CanCutTiles() returns true. Useful when programming lasers and such.
		/// </summary>
		public virtual void CutTiles()
		{
		}
		/// <summary>
		/// Allows you to determine whether the vanilla code for Kill and the Kill hook will be called. Return false to stop them from being called. Returns true by default. Note that this does not stop the projectile from dying.
		/// </summary>
		public virtual bool PreKill(int timeLeft)
		{
			return true;
		}
		/// <summary>
		/// Allows you to control what happens when this projectile is killed (for example, creating dust or making sounds). Also useful for creating retrievable ammo. Called on all clients and the server in multiplayer, so be sure to use `if (projectile.owner == Main.myPlayer)` if you are spawning retrievable ammo. (As seen in ExampleJavelinProjectile)
		/// </summary>
		public virtual void Kill(int timeLeft)
		{
		}
		/// <summary>
		/// Whether or not this projectile is capable of killing tiles (such as grass) and damaging NPCs/players. Return false to prevent it from doing any sort of damage. Returns true by default.
		/// </summary>
		public virtual bool CanDamage()
		{
			return true;
		}
		/// <summary>
		/// Whether or not this minion can damage NPCs by touching them. Returns false by default. Note that this will only be used if this projectile is considered a pet.
		/// </summary>
		public virtual bool MinionContactDamage()
		{
			return false;
		}
		/// <summary>
		/// Allows you to change the hitbox used by this projectile for damaging players and NPCs.
		/// </summary>
		/// <param name="hitbox"></param>
		public virtual void ModifyDamageHitbox(ref Rectangle hitbox)
		{
		}
		/// <summary>
		/// Allows you to determine whether this projectile can hit the given NPC. Return true to allow hitting the target, return false to block this projectile from hitting the target, and return null to use the vanilla code for whether the target can be hit. Returns null by default.
		/// </summary>
		/// <param name="target">The target.</param>
		public virtual bool? CanHitNPC(NPC target)
		{
			return null;
		}
		/// <summary>
		/// Allows you to modify the damage, knockback, etc., that this projectile does to an NPC. This method is only called for the owner of the projectile, meaning that in multi-player, projectiles owned by a player call this method on that client, and projectiles owned by the server such as enemy projectiles call this method on the server.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="damage">The modifiable damage.</param>
		/// <param name="knockback">The modifiable knockback.</param>
		/// <param name="crit">The modifiable crit.</param>
		/// <param name="hitDirection">The modifiable hit direction.</param>
		public virtual void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
		}
		/// <summary>
		/// Allows you to create special effects when this projectile hits an NPC (for example, inflicting debuffs). This method is only called for the owner of the projectile, meaning that in multi-player, projectiles owned by a player call this method on that client, and projectiles owned by the server such as enemy projectiles call this method on the server.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="damage">The damage.</param>
		/// <param name="knockback">The knockback.</param>
		/// <param name="crit">The critical hit.</param>
		public virtual void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		}
		/// <summary>
		/// Allows you to determine whether this projectile can hit the given opponent player. Return false to block this projectile from hitting the target. Returns true by default.
		/// </summary>
		/// <param name="target">The target</param>
		public virtual bool CanHitPvp(Player target)
		{
			return true;
		}
		/// <summary>
		/// Allows you to modify the damage, etc., that this projectile does to an opponent player.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="damage">The modifiable damage.</param>
		/// <param name="crit">The modifiable crit.</param>
		public virtual void ModifyHitPvp(Player target, ref int damage, ref bool crit)
		{
		}
		/// <summary>
		/// Allows you to create special effects when this projectile hits an opponent player.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="damage">The damage.</param>
		/// <param name="crit">The critical hit.</param>
		public virtual void OnHitPvp(Player target, int damage, bool crit)
		{
		}
		/// <summary>
		/// Allows you to determine whether this hostile projectile can hit the given player. Return false to block this projectile from hitting the target. Returns true by default.
		/// </summary>
		/// <param name="target">The target.</param>
		public virtual bool CanHitPlayer(Player target)
		{
			return true;
		}
		/// <summary>
		/// Allows you to modify the damage, etc., that this hostile projectile does to a player.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="damage">The modifiable damage.</param>
		/// <param name="crit">The modifiable crit.</param>
		public virtual void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
		}
		/// <summary>
		/// Allows you to create special effects when this hostile projectile hits a player.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="damage">The damage.</param>
		/// <param name="crit">The critical hit.</param>
		public virtual void OnHitPlayer(Player target, int damage, bool crit)
		{
		}
		/// <summary>
		/// Allows you to use custom collision detection between this projectile and a player or NPC that this projectile can damage. Useful for things like diagonal lasers, projectiles that leave a trail behind them, etc.
		/// </summary>
		/// <param name="projHitbox">The hitbox of the projectile.</param>
		/// <param name="targetHitbox">The hitbox of the target.</param>
		/// <returns>Whether they collide or not.</returns>
		public virtual bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return null;
		}
		/// <summary>
		/// Allows you to determine the color and transparency in which this projectile is drawn. Return null to use the default color (normally light and buff color). Returns null by default.
		/// </summary>
		public virtual Color? GetAlpha(Color lightColor)
		{
			return null;
		}
		/// <summary>
		/// Allows you to draw things behind this projectile. Returns false to stop the game from drawing extras textures related to the projectile (for example, the chains for grappling hooks), useful if you're manually drawing the extras. Returns true by default.
		/// </summary>
		public virtual bool PreDrawExtras(SpriteBatch spriteBatch)
		{
			return true;
		}
		/// <summary>
		/// Allows you to draw things behind this projectile, or to modify the way this projectile is drawn. Return false to stop the game from drawing the projectile (useful if you're manually drawing the projectile). Returns true by default.
		/// </summary>
		public virtual bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return true;
		}
		/// <summary>
		/// Allows you to draw things in front of a projectile. This method is called even if PreDraw returns false.
		/// </summary>
		public virtual void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
		}
		/// <summary>
		/// This code is called whenever the player uses a grappling hook that shoots this type of projectile. Use it to change what kind of hook is fired (for example, the Dual Hook does this), to kill old hook projectiles, etc.
		/// </summary>
		public virtual bool? CanUseGrapple(Player player)
		{
			return null;
		}
		/// <summary>
		/// Whether or not a grappling hook can only have one hook per player in the world at a time. Return null to use the vanilla code. Returns null by default.
		/// </summary>
		public virtual bool? SingleGrappleHook(Player player)
		{
			return null;
		}
		/// <summary>
		/// This code is called whenever the player uses a grappling hook that shoots this type of projectile. Use it to change what kind of hook is fired (for example, the Dual Hook does this), to kill old hook projectiles, etc.
		/// </summary>
		public virtual void UseGrapple(Player player, ref int type)
		{
		}
		/// <summary>
		/// How far away this grappling hook can travel away from its player before it retracts.
		/// </summary>
		public virtual float GrappleRange()
		{
			return 300f;
		}
		/// <summary>
		/// How many of this type of grappling hook the given player can latch onto blocks before the hooks start disappearing. Change the numHooks parameter to determine this; by default it will be 3.
		/// </summary>
		public virtual void NumGrappleHooks(Player player, ref int numHooks)
		{
		}
		/// <summary>
		/// The speed at which the grapple retreats back to the player after not hitting anything. Defaults to 11, but vanilla hooks go up to 24.
		/// </summary>
		public virtual void GrappleRetreatSpeed(Player player, ref float speed)
		{
		}
		/// <summary>
		/// The speed at which the grapple pulls the player after hitting something. Defaults to 11, but the Bat Hook uses 16.
		/// </summary>
		public virtual void GrapplePullSpeed(Player player, ref float speed)
		{
		}
		public virtual void GrappleTargetPoint(Player player, ref float grappleX, ref float grappleY)
		{
		}
		/// <summary>
		/// When used in conjunction with "projectile.hide = true", allows you to specify that this projectile should be drawn behind certain elements. Add the index to one and only one of the lists. For example, the Nebula Arcanum projectile draws behind NPCs and tiles.
		/// </summary>
		public virtual void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
		}
		#endregion
	}
}
