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
		/// <summary>
		/// 会BehaviorSet.Dispose
		/// </summary>
		public override bool CheckDead()
		{
			bool Result = true;
			foreach (var State in BehaviorSet)
			{
				bool N = BehaviorSet[State].CheckDead();
				if (N != true) Result = N;
			}
			if (Result) BehaviorSet.Dispose();
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

	public abstract class ModNPCBehaviorsAutomataComponent :ModNPC
	{
		//public Behavior.BehaviorSet<IModNPCBehavior> BehaviorMain = new Behavior.BehaviorSet<IModNPCBehavior>();
		public IModNPCBehaviorComponent BehaviorMain;
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			BehaviorMain.NetUpdateReceive(reader);
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			BehaviorMain.NetUpdateSend(writer);
		}
		public override void AI()
		{
			BehaviorMain.Update();
		}

		public override void BossHeadRotation(ref float rotation)
		{
			((IModNPCBehaviorHooks)BehaviorMain).BossHeadRotation(ref rotation);
		}

		public override void BossHeadSlot(ref int index)
		{
			((IModNPCBehaviorHooks)BehaviorMain).BossHeadSlot(ref index);
		}

		public override void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
		{
			((IModNPCBehaviorHooks)BehaviorMain).BossHeadSpriteEffects(ref spriteEffects);
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			((IModNPCBehaviorHooks)BehaviorMain).BossLoot(ref name, ref potionType);
		}

		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).CanBeHitByItem(player, item);
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).CanBeHitByProjectile(projectile);
		}

		public override bool CanChat()
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).CanChat();
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).CanGoToStatue(toKingStatue);
		}

		public override bool? CanHitNPC(NPC target)
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).CanHitNPC(target);
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).CanHitPlayer(target, ref cooldownSlot);
		}

		public override bool CheckActive()
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).CheckActive();
		}

		public override bool CheckConditions(int left, int right, int top, int bottom)
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).CheckConditions(left, right, top, bottom);
		}

		public override bool CheckDead()
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).CheckDead();
		}

		public override void DrawBehind(int index)
		{
			((IModNPCBehaviorHooks)BehaviorMain).DrawBehind(index);
		}

		public override void DrawEffects(ref Color drawColor)
		{
			((IModNPCBehaviorHooks)BehaviorMain).DrawEffects(ref drawColor);
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).DrawHealthBar(hbPosition, ref scale, ref position);
		}

		public override void DrawTownAttackGun(ref float scale, ref int item, ref int closeness)
		{
			((IModNPCBehaviorHooks)BehaviorMain).DrawTownAttackGun(ref scale, ref item, ref closeness);
		}

		public override void DrawTownAttackSwing(ref Texture2D item, ref int itemSize, ref float scale, ref Vector2 offset)
		{
			((IModNPCBehaviorHooks)BehaviorMain).DrawTownAttackSwing(ref item, ref itemSize, ref scale, ref offset);
		}

		public override void FindFrame(int frameHeight)
		{
			((IModNPCBehaviorHooks)BehaviorMain).FindFrame(frameHeight);
		}

		public override Color? GetAlpha(Color drawColor)
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).GetAlpha(drawColor);
		}

		public override string GetChat()
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).GetChat();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			((IModNPCBehaviorHooks)BehaviorMain).HitEffect(hitDirection, damage);
		}

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			((IModNPCBehaviorHooks)BehaviorMain).ModifyHitByItem(player, item, ref damage, ref knockback, ref crit);
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			((IModNPCBehaviorHooks)BehaviorMain).ModifyHitByProjectile(projectile, ref damage, ref knockback, ref crit, ref hitDirection);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit)
		{
			((IModNPCBehaviorHooks)BehaviorMain).ModifyHitNPC(target, ref damage, ref knockback, ref crit);
		}

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			((IModNPCBehaviorHooks)BehaviorMain).ModifyHitPlayer(target, ref damage, ref crit);
		}

		public override void NPCLoot()
		{
			((IModNPCBehaviorHooks)BehaviorMain).NPCLoot();
		}

		public override void OnCatchNPC(Player player, Item item)
		{
			((IModNPCBehaviorHooks)BehaviorMain).OnCatchNPC(player, item);
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			((IModNPCBehaviorHooks)BehaviorMain).OnChatButtonClicked(firstButton, ref shop);
		}

		public override void OnGoToStatue(bool toKingStatue)
		{
			((IModNPCBehaviorHooks)BehaviorMain).OnGoToStatue(toKingStatue);
		}

		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			((IModNPCBehaviorHooks)BehaviorMain).OnHitByItem(player, item, damage, knockback, crit);
		}

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			((IModNPCBehaviorHooks)BehaviorMain).OnHitByProjectile(projectile, damage, knockback, crit);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			((IModNPCBehaviorHooks)BehaviorMain).OnHitNPC(target, damage, knockback, crit);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			((IModNPCBehaviorHooks)BehaviorMain).OnHitPlayer(target, damage, crit);
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			((IModNPCBehaviorHooks)BehaviorMain).PostDraw(spriteBatch, drawColor);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).PreDraw(spriteBatch, drawColor);
		}

		public override bool PreNPCLoot()
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).PreNPCLoot();
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			((IModNPCBehaviorHooks)BehaviorMain).ScaleExpertStats(numPlayers, bossLifeScale);
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			((IModNPCBehaviorHooks)BehaviorMain).SetChatButtons(ref button, ref button2);
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			((IModNPCBehaviorHooks)BehaviorMain).SetupShop(shop, ref nextSlot);
		}

		public override bool SpecialNPCLoot()
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).SpecialNPCLoot();
		}

		public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).StrikeNPC(ref damage, defense, ref knockback, hitDirection, ref crit);
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			((IModNPCBehaviorHooks)BehaviorMain).TownNPCAttackCooldown(ref cooldown, ref randExtraCooldown);
		}

		public override void TownNPCAttackMagic(ref float auraLightMultiplier)
		{
			((IModNPCBehaviorHooks)BehaviorMain).TownNPCAttackMagic(ref auraLightMultiplier);
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			((IModNPCBehaviorHooks)BehaviorMain).TownNPCAttackProj(ref projType, ref attackDelay);
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			((IModNPCBehaviorHooks)BehaviorMain).TownNPCAttackProjSpeed(ref multiplier, ref gravityCorrection, ref randomOffset);
		}

		public override void TownNPCAttackShoot(ref bool inBetweenShots)
		{
			((IModNPCBehaviorHooks)BehaviorMain).TownNPCAttackShoot(ref inBetweenShots);
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			((IModNPCBehaviorHooks)BehaviorMain).TownNPCAttackStrength(ref damage, ref knockback);
		}

		public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight)
		{
			((IModNPCBehaviorHooks)BehaviorMain).TownNPCAttackSwing(ref itemWidth, ref itemHeight);
		}

		public override string TownNPCName()
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).TownNPCName();
		}

		public override void UpdateLifeRegen(ref int damage)
		{
			((IModNPCBehaviorHooks)BehaviorMain).UpdateLifeRegen(ref damage);
		}

		public override bool UsesPartyHat()
		{
			return ((IModNPCBehaviorHooks)BehaviorMain).UsesPartyHat();
		}

		public override void ResetEffects()
		{
			((IModNPCBehaviorHooks)BehaviorMain).ResetEffects();
		}
	}
}
