using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using XxDefinitions.Behavior;
namespace XxDefinitions.NPCs.NPCBehaviors
{
	/// <summary>
	/// ModNPC的行为的接口
	/// </summary>
	public interface IModNPCBehavior : Behavior.IBehavior, IModNPCBehaviorHooks { }
	/// <summary>
	/// ModNPC的行为的基类
	/// </summary>
	public abstract class ModNPCBehavior<RealModNPC> : IModNPCBehavior
		where RealModNPC:ModNPC
	{
		/// <summary>
		/// 被操作的modNPC
		/// </summary>
		public RealModNPC modNPC;
		/// <summary>
		/// 被操作的npc
		/// </summary>
		public NPC npc => modNPC.npc;
		//public ItemTreeIndex<string> index;
		/// <summary>
		/// 初始化
		/// </summary>
		public ModNPCBehavior(RealModNPC modNPC) { this.modNPC = modNPC;}
		//public ModNPCBehavior(RealModNPC modNPC, ItemTreeIndex<string> index) { this.modNPC = modNPC; this.index = index; }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		#region IBehavior
		public virtual void Update() { }
		public virtual bool CanPause() { return true; }
		public virtual void Pause() { }
		public virtual bool CanContinue() { return true; }
		public virtual void Continue() { }
		public virtual void Start() { }
		public virtual void End() { }
		public virtual void NetUpdateSend(BinaryWriter writer) { }
		public virtual void NetUpdateReceive(BinaryReader reader) { }
		public virtual object Call(params object[] vs) { return null; }
		public virtual string BehaviorName => "ModNPCBehavior";
		public bool pausing=true;
		public bool Pausing
		{
			get => pausing;
			set
			{
				if (value) this.TryPause();
				else this.TryContinue();
			}
		}
		#endregion
		#region IModNPCBehaviorHooks
		/// <summary>
		/// Allows you to customize this NPC's stats in expert mode. This is useful because expert mode's doubling of damage and life might be too much sometimes (for example, with bosses). Also useful for scaling life with the number of players in the world.
		/// </summary>
		/// <param name="numPlayers"></param>
		/// <param name="bossLifeScale"></param>
		public virtual void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
		}
		/// <summary>
		/// Allows you to modify the frame from this NPC's texture that is drawn, which is necessary in order to animate NPCs.
		/// </summary>
		/// <param name="frameHeight"></param>
		public virtual void FindFrame(int frameHeight)
		{
		}
		public virtual void HitEffect(int hitDirection, double damage)
		{
		}
		/// <summary>
		/// Allows you to make the NPC either regenerate health or take damage over time by setting npc.lifeRegen. Regeneration or damage will occur at a rate of half of npc.lifeRegen per second. The damage parameter is the number that appears above the NPC's head if it takes damage over time.
		/// </summary>
		/// <param name="damage"></param>
		public virtual void UpdateLifeRegen(ref int damage)
		{
		}
		/// <summary>
		/// Allows you to call NPCLoot on your own when the NPC dies, rather then letting vanilla call it on its own. Useful for things like dropping loot from the nearest segment of a worm boss. Returns false by default.
		/// </summary>
		/// <returns>Return true to stop vanilla from calling NPCLoot on its own. Do this if you call NPCLoot yourself.</returns>
		public virtual bool SpecialNPCLoot()
		{
			return false;
		}
		/// <summary>
		/// Allows you to determine whether or not this NPC will drop anything at all. Return false to stop the NPC from dropping anything. Returns true by default.
		/// </summary>
		/// <returns></returns>
		public virtual bool PreNPCLoot()
		{
			return true;
		}
		/// <summary>
		/// Whether or not to run the code for checking whether this NPC will remain active. Return false to stop this NPC from being despawned and to stop this NPC from counting towards the limit for how many NPCs can exist near a player. Returns true by default.
		/// </summary>
		/// <returns></returns>
		public virtual bool CheckActive()
		{
			return true;
		}
		/// <summary>
		/// Whether or not this NPC should be killed when it reaches 0 health. You may program extra effects in this hook (for example, how Golem's head lifts up for the second phase of its fight). Return false to stop this NPC from being killed. Returns true by default.
		/// </summary>
		/// <returns></returns>
		public virtual bool CheckDead()
		{
			return true;
		}
		/// <summary>
		/// Allows you to make things happen when this NPC is caught. Ran Serverside
		/// </summary>
		/// <param name="player">The player catching this NPC</param>
		/// <param name="item">The item that will be spawned</param>
		public virtual void OnCatchNPC(Player player, Item item)
		{
		}
		/// <summary>
		/// Allows you to customize what happens when this boss dies, such as which name is displayed in the defeat message and what type of potion it drops.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="potionType"></param>
		public virtual void BossLoot(ref string name, ref int potionType)
		{
		}
		/// <summary>
		/// Allows you to determine whether this NPC can hit the given player. Return false to block this NPC from hitting the target. Returns true by default. CooldownSlot determines which of the player's cooldown counters to use (-1, 0, or 1), and defaults to -1.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="cooldownSlot"></param>
		/// <returns></returns>
		public virtual bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return true;
		}
		/// <summary>
		/// Allows you to modify the damage, etc., that this NPC does to a player.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="damage"></param>
		/// <param name="crit"></param>
		public virtual void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
		}
		/// <summary>
		/// Allows you to create special effects when this NPC hits a player (for example, inflicting debuffs).
		/// </summary>
		/// <param name="target"></param>
		/// <param name="damage"></param>
		/// <param name="crit"></param>
		public virtual void OnHitPlayer(Player target, int damage, bool crit)
		{
		}
		/// <summary>
		/// Allows you to determine whether this NPC can hit the given friendly NPC. Return true to allow hitting the target, return false to block this NPC from hitting the target, and return null to use the vanilla code for whether the target can be hit. Returns null by default.
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public virtual bool? CanHitNPC(NPC target)
		{
			return null;
		}
		/// <summary>
		/// Allows you to modify the damage, knockback, etc., that this NPC does to a friendly NPC.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="damage"></param>
		/// <param name="knockback"></param>
		/// <param name="crit"></param>
		public virtual void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit)
		{
		}
		/// <summary>
		/// Allows you to create special effects when this NPC hits a friendly NPC.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="damage"></param>
		/// <param name="knockback"></param>
		/// <param name="crit"></param>
		public virtual void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		}
		/// <summary>
		/// Allows you to determine whether this NPC can be hit by the given melee weapon when swung. Return true to allow hitting the NPC, return false to block hitting the NPC, and return null to use the vanilla code for whether the NPC can be hit. Returns null by default.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual bool? CanBeHitByItem(Player player, Item item)
		{
			return null;
		}
		/// <summary>
		/// Allows you to modify the damage, knockback, etc., that this NPC takes from a melee weapon.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="item"></param>
		/// <param name="damage"></param>
		/// <param name="knockback"></param>
		/// <param name="crit"></param>
		public virtual void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
		}
		/// <summary>
		/// Allows you to create special effects when this NPC is hit by a melee weapon.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="item"></param>
		/// <param name="damage"></param>
		/// <param name="knockback"></param>
		/// <param name="crit"></param>
		public virtual void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
		}
		/// <summary>
		/// Allows you to determine whether this NPC can be hit by the given projectile. Return true to allow hitting the NPC, return false to block hitting the NPC, and return null to use the vanilla code for whether the NPC can be hit. Returns null by default.
		/// </summary>
		/// <param name="projectile"></param>
		/// <returns></returns>
		public virtual bool? CanBeHitByProjectile(Projectile projectile)
		{
			return null;
		}
		/// <summary>
		/// Allows you to modify the damage, knockback, etc., that this NPC takes from a projectile. This method is only called for the owner of the projectile, meaning that in multi-player, projectiles owned by a player call this method on that client, and projectiles owned by the server such as enemy projectiles call this method on the server.
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="damage"></param>
		/// <param name="knockback"></param>
		/// <param name="crit"></param>
		/// <param name="hitDirection"></param>
		public virtual void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
		}
		/// <summary>
		/// Allows you to create special effects when this NPC is hit by a projectile.
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="damage"></param>
		/// <param name="knockback"></param>
		/// <param name="crit"></param>
		public virtual void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
		}
		/// <summary>
		/// Allows you to use a custom damage formula for when this NPC takes damage from any source. For example, you can change the way defense works or use a different crit multiplier. Return false to stop the game from running the vanilla damage formula; returns true by default.
		/// </summary>
		/// <param name="damage"></param>
		/// <param name="defense"></param>
		/// <param name="knockback"></param>
		/// <param name="hitDirection"></param>
		/// <param name="crit"></param>
		/// <returns></returns>
		public virtual bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			return true;
		}
		/// <summary>
		/// Allows you to customize the boss head texture used by this NPC based on its state.
		/// Set index to -1 to stop the texture from being displayed.
		/// </summary>
		/// <param name="index"></param>
		public virtual void BossHeadSlot(ref int index)
		{
		}
		/// <summary>
		/// Allows you to customize the rotation of this NPC's boss head icon on the map.
		/// </summary>
		/// <param name="rotation"></param>
		public virtual void BossHeadRotation(ref float rotation)
		{
		}
		/// <summary>
		/// Allows you to flip this NPC's boss head icon on the map.
		/// </summary>
		/// <param name="spriteEffects"></param>
		public virtual void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
		{
		}
		/// <summary>
		/// Allows you to determine the color and transparency in which this NPC is drawn. Return null to use the default color (normally light and buff color). Returns null by default.
		/// </summary>
		/// <param name="drawColor"></param>
		/// <returns></returns>
		public virtual Color? GetAlpha(Color drawColor)
		{
			return null;
		}
		/// <summary>
		/// Allows you to add special visual effects to this NPC (such as creating dust), and modify the color in which the NPC is drawn.
		/// </summary>
		/// <param name="drawColor"></param>
		public virtual void DrawEffects(ref Color drawColor)
		{
		}
		/// <summary>
		/// Allows you to draw things behind this NPC, or to modify the way this NPC is drawn. Return false to stop the game from drawing the NPC (useful if you're manually drawing the NPC). Returns true by default.
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="drawColor"></param>
		/// <returns></returns>
		public virtual bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			return true;
		}
		/// <summary>
		/// Allows you to draw things in front of this NPC. This method is called even if PreDraw returns false.
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="drawColor"></param>
		public virtual void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
		}
		/// <summary>
		/// Allows you to control how the health bar for this NPC is drawn. The hbPosition parameter is the same as Main.hbPosition; it determines whether the health bar gets drawn above or below the NPC by default. The scale parameter is the health bar's size. By default, it will be the normal 1f; most bosses set this to 1.5f. Return null to let the normal vanilla health-bar-drawing code to run. Return false to stop the health bar from being drawn. Return true to draw the health bar in the position specified by the position parameter (note that this is the world position, not screen position).
		/// </summary>
		/// <param name="hbPosition"></param>
		/// <param name="scale"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public virtual bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return null;
		}
		/// <summary>
		/// This is where you reset any fields you add to your subclass to their default states. This is necessary in order to reset your fields if they are conditionally set by a tick update but the condition is no longer satisfied. (Note: This hook is only really useful for GlobalNPC, but is included in ModNPC for completion.)
		/// </summary>
		public virtual void ResetEffects()
		{
		}
		/// <summary>
		/// Allows you to define special conditions required for this town NPC's house. For example, Truffle requires the house to be in an aboveground mushroom biome.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <param name="top"></param>
		/// <param name="bottom"></param>
		/// <returns></returns>
		public virtual bool CheckConditions(int left, int right, int top, int bottom)
		{
			return true;
		}
		/// <summary>
		/// Allows you to give this town NPC any name when it spawns. By default returns something embarrassing.
		/// </summary>
		/// <returns></returns>
		public virtual string TownNPCName()
		{
			return Language.GetTextValue("tModLoader.DefaultTownNPCName");
		}
		/// <summary>
		/// Allows you to determine whether this town NPC wears a party hat during a party. Returns true by default.
		/// </summary>
		/// <returns></returns>
		public virtual bool UsesPartyHat()
		{
			return true;
		}
		/// <summary>
		/// Allows you to determine whether this NPC can talk with the player. By default, returns if the NPC is a town NPC.
		/// </summary>
		/// <returns></returns>
		public virtual bool CanChat()
		{
			return npc.townNPC;
		}
		/// <summary>
		/// Allows you to give this NPC a chat message when a player talks to it. By default returns something embarrassing.
		/// </summary>
		/// <returns></returns>
		public virtual string GetChat()
		{
			return Language.GetTextValue("tModLoader.DefaultTownNPCChat");
		}
		/// <summary>
		/// Allows you to set the text for the buttons that appear on this NPC's chat window. A parameter left as an empty string will not be included as a button on the chat window.
		/// </summary>
		/// <param name="button"></param>
		/// <param name="button2"></param>
		public virtual void SetChatButtons(ref string button, ref string button2)
		{
		}
		/// <summary>
		/// Allows you to make something happen whenever a button is clicked on this NPC's chat window. The firstButton parameter tells whether the first button or second button (button and button2 from SetChatButtons) was clicked. Set the shop parameter to true to open this NPC's shop.
		/// </summary>
		/// <param name="firstButton"></param>
		/// <param name="shop"></param>
		public virtual void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
		}
		/// <summary>
		/// Allows you to add items to this NPC's shop. Add an item by setting the defaults of shop.item[nextSlot] then incrementing nextSlot. In the end, nextSlot must have a value of 1 greater than the highest index in shop.item that contains an item.
		/// </summary>
		/// <param name="shop"></param>
		/// <param name="nextSlot"></param>
		public virtual void SetupShop(Chest shop, ref int nextSlot)
		{
		}
		/// <summary>
		/// Whether this NPC can be telported to a King or Queen statue. Returns false by default.
		/// </summary>
		/// <param name="toKingStatue">Whether the NPC is being teleported to a King or Queen statue.</param>
		public virtual bool CanGoToStatue(bool toKingStatue)
		{
			return false;
		}
		/// <summary>
		/// Allows you to make things happen when this NPC teleports to a King or Queen statue.
		/// This method is only called server side.
		/// </summary>
		/// <param name="toKingStatue">Whether the NPC was teleported to a King or Queen statue.</param>
		public virtual void OnGoToStatue(bool toKingStatue)
		{
		}
		/// <summary>
		/// Allows you to determine the damage and knockback of this town NPC's attack before the damage is scaled. (More information on scaling in GlobalNPC.BuffTownNPCs.)
		/// </summary>
		/// <param name="damage"></param>
		/// <param name="knockback"></param>
		public virtual void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
		}
		/// <summary>
		/// Allows you to determine the cooldown between each of this town NPC's attack. The cooldown will be a number greater than or equal to the first parameter, and less then the sum of the two parameters.
		/// </summary>
		/// <param name="cooldown"></param>
		/// <param name="randExtraCooldown"></param>
		public virtual void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
		}
		/// <summary>
		/// Allows you to determine the projectile type of this town NPC's attack, and how long it takes for the projectile to actually appear. This hook is only used when the town NPC has an attack type of 0 (throwing), 1 (shooting), or 2 (magic).
		/// </summary>
		/// <param name="projType"></param>
		/// <param name="attackDelay"></param>
		public virtual void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
		}
		/// <summary>
		/// Allows you to determine the speed at which this town NPC throws a projectile when it attacks. Multiplier is the speed of the projectile, gravityCorrection is how much extra the projectile gets thrown upwards, and randomOffset allows you to randomize the projectile's velocity in a square centered around the original velocity. This hook is only used when the town NPC has an attack type of 0 (throwing), 1 (shooting), or 2 (magic).
		/// </summary>
		/// <param name="multiplier"></param>
		/// <param name="gravityCorrection"></param>
		/// <param name="randomOffset"></param>
		public virtual void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
		}
		/// <summary>
		/// Allows you to tell the game that this town NPC has already created a projectile and will still create more projectiles as part of a single attack so that the game can animate the NPC's attack properly. Only used when the town NPC has an attack type of 1 (shooting).
		/// </summary>
		/// <param name="inBetweenShots"></param>
		public virtual void TownNPCAttackShoot(ref bool inBetweenShots)
		{
		}
		/// <summary>
		/// Allows you to control the brightness of the light emitted by this town NPC's aura when it performs a magic attack. Only used when the town NPC has an attack type of 2 (magic)
		/// </summary>
		/// <param name="auraLightMultiplier"></param>
		public virtual void TownNPCAttackMagic(ref float auraLightMultiplier)
		{
		}
		/// <summary>
		/// Allows you to determine the width and height of the item this town NPC swings when it attacks, which controls the range of this NPC's swung weapon. Only used when the town NPC has an attack type of 3 (swinging).
		/// </summary>
		/// <param name="itemWidth"></param>
		/// <param name="itemHeight"></param>
		public virtual void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight)
		{
		}
		/// <summary>
		/// Allows you to customize how this town NPC's weapon is drawn when this NPC is shooting (this NPC must have an attack type of 1). Scale is a multiplier for the item's drawing size, item is the ID of the item to be drawn, and closeness is how close the item should be drawn to the NPC.
		/// </summary>
		/// <param name="scale"></param>
		/// <param name="item"></param>
		/// <param name="closeness"></param>
		public virtual void DrawTownAttackGun(ref float scale, ref int item, ref int closeness)
		{
		}
		/// <summary>
		/// Allows you to customize how this town NPC's weapon is drawn when this NPC is swinging it (this NPC must have an attack type of 3). Item is the Texture2D instance of the item to be drawn (use Main.itemTexture[id of item]), itemSize is the width and height of the item's hitbox (the same values for TownNPCAttackSwing), scale is the multiplier for the item's drawing size, and offset is the offset from which to draw the item from its normal position.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="itemSize"></param>
		/// <param name="scale"></param>
		/// <param name="offset"></param>
		public virtual void DrawTownAttackSwing(ref Texture2D item, ref int itemSize, ref float scale, ref Vector2 offset)
		{
		}
		public virtual void DrawBehind(int index)
		{
		}
		public virtual void NPCLoot()
		{
		}
		#endregion
	}
}
