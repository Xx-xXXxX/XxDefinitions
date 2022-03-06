using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace XxDefinitions.NPCs.NPCBehaviors
{
	/// <summary>
	/// ModNPC应用行为自动机的基类
	/// </summary>
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	public abstract class ModNPCBehaviorsAutomata : ModNPC
	{
		public Behavior.BehaviorSet<IModNPCBehavior> BehaviorSet =new Behavior.BehaviorSet<IModNPCBehavior>();
		public IModNPCBehavior BehaviorMain => BehaviorSet.BehaviorMain;
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
		public override void FindFrame(int frameHeight)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].FindFrame(frameHeight);
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].HitEffect(hitDirection, damage);
			}
		}
		public override void BossHeadRotation(ref float rotation)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].BossHeadRotation(ref rotation);
			}
		}
		public override void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].BossHeadSpriteEffects(ref spriteEffects);
			}
		}
		public override void BossHeadSlot(ref int index)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].BossHeadSlot(ref index);
			}
		}
		public override bool? CanBeHitByItem(Player player, Item item)
		{
			bool? Result = null;
			foreach (var State in BehaviorSet)
			{
				bool? N = BehaviorSet[State].CanBeHitByItem(player, item);
				if (N != null) Result = N;
			}
			return Result;
			//return States[IState].CanBeHitByItem(player, item);
		}
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			bool? Result = null;
			foreach (var State in BehaviorSet)
			{
				bool? N = BehaviorSet[State].CanBeHitByProjectile(projectile);
				if (N != null) Result = N;
			}
			return Result;
			//return States[IState].CanBeHitByProjectile(projectile);
		}
		public override bool CanChat()
		{
			bool Result = npc.townNPC;
			foreach (var State in BehaviorSet)
			{
				bool N = BehaviorSet[State].CanChat();
				if (N != npc.townNPC) Result = N;
			}
			return Result;
			//return States[IState].CanChat();
		}
		public override bool? CanHitNPC(NPC target)
		{
			bool? Result = null;
			foreach (var State in BehaviorSet)
			{
				bool? N = BehaviorSet[State].CanHitNPC(target);
				if (N != null) Result = N;
			}
			return Result;
			//return States[IState].CanHitNPC(target);
		}
		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			bool Result = true;
			foreach (var State in BehaviorSet)
			{
				bool N = BehaviorSet[State].CanHitPlayer(target, ref cooldownSlot);
				if (N != true) Result = N;
			}
			return Result;
			//return States[IState].CanHitPlayer(target, ref cooldownSlot);
		}
		public override bool CheckConditions(int left, int right, int top, int bottom)
		{
			bool Result = true;
			foreach (var State in BehaviorSet)
			{
				bool N = BehaviorSet[State].CheckConditions(left, right, top, bottom);
				if (N != true) Result = N;
			}
			return Result;
			//return States[IState].CheckConditions(left, right, top, bottom);
		}
		public override void DrawBehind(int index)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].DrawBehind(index);
			}
		}
		public override void DrawEffects(ref Color drawColor)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].DrawEffects(ref drawColor);
			}
		}
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			bool? Result = null;
			foreach (var State in BehaviorSet)
			{
				bool? N = BehaviorSet[State].DrawHealthBar(hbPosition, ref scale, ref position);
				if (N != null) Result = N;
			}
			return Result;
			//return States[IState].DrawHealthBar(hbPosition, ref scale, ref position);
		}
		public override void DrawTownAttackGun(ref float scale, ref int item, ref int closeness)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].DrawTownAttackGun(ref scale, ref item, ref closeness);
			}
		}
		public override void DrawTownAttackSwing(ref Texture2D item, ref int itemSize, ref float scale, ref Vector2 offset)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].DrawTownAttackSwing(ref item, ref itemSize, ref scale, ref offset);
			}
		}
		public override Color? GetAlpha(Color drawColor)
		{
			//TemplateMod2.Logging.Debug("MNA Start Get Alpha");
			//Color? d = States[IState].GetAlpha(drawColor);
			//TemplateMod2.Logging.Debug("MNA End Get Alpha");
			Color? d = null;
			foreach (var State in BehaviorSet)
			{
				Color? N = BehaviorSet[State].GetAlpha(drawColor);
				if (N != null) d = N;
			}
			return d;
		}
		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].ModifyHitByItem(player, item, ref damage, ref knockback, ref crit);
			}
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].ModifyHitByProjectile(projectile, ref damage, ref knockback, ref crit, ref hitDirection);
			}
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].ModifyHitNPC(target, ref damage, ref knockback, ref crit);
			}
		}
		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].ModifyHitPlayer(target, ref damage, ref crit);
			}
		}
		public override string GetChat()
		{
			string D = "";
			foreach (var State in BehaviorSet)
			{
				string N = BehaviorSet[State].GetChat();
				if (N.Trim().Length != 0 && N != Language.GetTextValue("tModLoader.DefaultTownNPCChat")) D += " " + N;
			}
			return D;
		}
		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].OnHitByItem(player, item, damage, knockback, crit);
			}
		}
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].OnHitByProjectile(projectile, damage, knockback, crit);
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].OnHitNPC(target, damage, knockback, crit);
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].OnHitPlayer(target, damage, crit);
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].PostDraw(spriteBatch, drawColor);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			bool Result = true;
			foreach (var State in BehaviorSet)
			{
				bool N = BehaviorSet[State].PreDraw(spriteBatch, drawColor);
				if (N != true) Result = N;
			}
			return Result;
		}
		public override void NPCLoot()
		{
			foreach (var State in BehaviorSet)
			{
				if (BehaviorSet[State].PreNPCLoot() && BehaviorSet[State].SpecialNPCLoot()) BehaviorSet[State].NPCLoot();
			}
		}
		public override bool PreNPCLoot()
		{
			bool Result = false;
			foreach (var State in BehaviorSet)
			{
				bool N = BehaviorSet[State].PreNPCLoot();
				if (N != false) Result = N;
			}
			return Result;
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			foreach (var State in BehaviorSet)
			{
				if (BehaviorSet[State].PreNPCLoot() && BehaviorSet[State].SpecialNPCLoot()) BossLoot(ref name, ref potionType);
			}
		}
		public override bool SpecialNPCLoot()
		{
			bool Result = false;
			foreach (var State in BehaviorSet)
			{
				bool N = BehaviorSet[State].SpecialNPCLoot();
				if (N != false) Result = N;
			}
			return Result;
			//return States[IState].SpecialNPCLoot();
		}
		public override void ResetEffects()
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].ResetEffects();
			}
		}
		public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			bool Result = true;
			foreach (var State in BehaviorSet)
			{
				bool N = BehaviorSet[State].StrikeNPC(ref damage, defense, ref knockback, hitDirection, ref crit);
				if (N != true) Result = N;
			}
			return Result;
			//return States[IState].StrikeNPC(ref damage, defense, ref knockback, hitDirection, ref crit);
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].ScaleExpertStats(numPlayers, bossLifeScale);
			}
		}
		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].SetupShop(shop, ref nextSlot);
			}
		}
		public override void SetChatButtons(ref string button, ref string button2)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].SetChatButtons(ref button, ref button2);
			}
		}
		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].TownNPCAttackCooldown(ref cooldown, ref randExtraCooldown);
			}
		}
		public override void TownNPCAttackMagic(ref float auraLightMultiplier)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].TownNPCAttackMagic(ref auraLightMultiplier);
			}
		}
		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].TownNPCAttackProj(ref projType, ref attackDelay);
			}
		}
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].TownNPCAttackProjSpeed(ref multiplier, ref gravityCorrection, ref randomOffset);
			}
		}
		public override void TownNPCAttackShoot(ref bool inBetweenShots)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].TownNPCAttackShoot(ref inBetweenShots);
			}
		}
		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].TownNPCAttackStrength(ref damage, ref knockback);
			}
		}
		public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].TownNPCAttackSwing(ref itemWidth, ref itemHeight);
			}
		}
		public override string TownNPCName()
		{
			string D = "";
			foreach (var State in BehaviorSet)
			{
				string N = BehaviorSet[State].TownNPCName();
				if (N.Trim().Length != 0 && N != Language.GetTextValue("tModLoader.DefaultTownNPCName")) D += " " + N;
			}
			return D;
			//return States[IState].TownNPCName();
		}
		public override void UpdateLifeRegen(ref int damage)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].UpdateLifeRegen(ref damage);
			}
		}
		public override bool CheckActive()
		{
			bool Result = true;
			foreach (var State in BehaviorSet)
			{
				bool N = BehaviorSet[State].CheckActive();
				if (N != true) Result = N;
			}
			return Result;
			//return States[IState].CheckActive();
		}
		public override bool CheckDead()
		{
			bool Result = true;
			foreach (var State in BehaviorSet)
			{
				bool N = BehaviorSet[State].CheckDead();
				if (N != true) Result = N;
			}
			return Result;
			//return States[IState].CheckDead();
		}
		public override bool UsesPartyHat()
		{
			bool Result = true;
			foreach (var State in BehaviorSet)
			{
				bool N = BehaviorSet[State].UsesPartyHat();
				if (N != true) Result = N;
			}
			return Result;
			//return States[IState].UsesPartyHat();
		}
		public override void OnCatchNPC(Player player, Item item)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].OnCatchNPC(player, item);
			}
		}
		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].OnChatButtonClicked(firstButton, ref shop);
			}
		}
		public override bool CanGoToStatue(bool toKingStatue)
		{
			bool Result = false;
			foreach (var State in BehaviorSet)
			{
				bool N = BehaviorSet[State].CanGoToStatue(toKingStatue);
				if (N != false) Result = N;
			}
			return Result;
			//return States[IState].CanGoToStatue(toKingStatue);
		}
		public override void OnGoToStatue(bool toKingStatue)
		{
			foreach (var State in BehaviorSet)
			{
				BehaviorSet[State].OnGoToStatue(toKingStatue);
			}
		}
		#endregion
	}
}
