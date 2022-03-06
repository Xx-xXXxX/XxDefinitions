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
namespace XxDefinitions.NPCs
{
	public interface IModNPCBehaviorHooks
	{
		void BossHeadRotation(ref float rotation);
		void BossHeadSlot(ref int index);
		void BossHeadSpriteEffects(ref SpriteEffects spriteEffects);
		void BossLoot(ref string name, ref int potionType);
		bool? CanBeHitByItem(Player player, Item item);
		bool? CanBeHitByProjectile(Projectile projectile);
		bool CanChat();
		bool CanGoToStatue(bool toKingStatue);
		bool? CanHitNPC(NPC target);
		bool CanHitPlayer(Player target, ref int cooldownSlot);
		bool CheckActive();
		bool CheckConditions(int left, int right, int top, int bottom);
		bool CheckDead();
		void DrawBehind(int index);
		void DrawEffects(ref Color drawColor);
		bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position);
		void DrawTownAttackGun(ref float scale, ref int item, ref int closeness);
		void DrawTownAttackSwing(ref Texture2D item, ref int itemSize, ref float scale, ref Vector2 offset);
		void FindFrame(int frameHeight);
		Color? GetAlpha(Color drawColor);
		string GetChat();
		void HitEffect(int hitDirection, double damage);
		void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit);
		void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection);
		void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit);
		void ModifyHitPlayer(Player target, ref int damage, ref bool crit);
		void NPCLoot();
		void OnCatchNPC(Player player, Item item);
		void OnChatButtonClicked(bool firstButton, ref bool shop);
		void OnGoToStatue(bool toKingStatue);
		void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit);
		void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit);
		void OnHitNPC(NPC target, int damage, float knockback, bool crit);
		void OnHitPlayer(Player target, int damage, bool crit);
		void PostDraw(SpriteBatch spriteBatch, Color drawColor);
		bool PreDraw(SpriteBatch spriteBatch, Color drawColor);
		bool PreNPCLoot();
		void ScaleExpertStats(int numPlayers, float bossLifeScale);
		void SetChatButtons(ref string button, ref string button2);
		void SetupShop(Chest shop, ref int nextSlot);
		bool SpecialNPCLoot();
		bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit);
		void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown);
		void TownNPCAttackMagic(ref float auraLightMultiplier);
		void TownNPCAttackProj(ref int projType, ref int attackDelay);
		void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset);
		void TownNPCAttackShoot(ref bool inBetweenShots);
		void TownNPCAttackStrength(ref int damage, ref float knockback);
		void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight);
		string TownNPCName();
		void UpdateLifeRegen(ref int damage);
		bool UsesPartyHat();
		void ResetEffects();
	}
}
