using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Collections;
using System.IO;
using Terraria.ID;
using XxDefinitions.Behavior;
using XxDefinitions.NPCs.NPCBehaviors;

namespace XxDefinitions.NPCs.NPCBehaviors
{
	/// <summary>
	/// 使用组合模式的IModNPCBehavior
	/// </summary>
	public interface IModNPCBehaviorComponent : IBehaviorComponent<IModNPCBehavior>, IModNPCBehavior { }
	/// <summary>
	/// 使用组合模式的ModNPCBehavior
	/// </summary>
	public abstract class ModNPCBehaviorComponent<RealModNPC> : BehaviorComponent<IModNPCBehavior>, IModNPCBehaviorComponent//, IModNPCBehaviorComponent
		where RealModNPC : ModNPC
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
		public ModNPCBehaviorComponent(RealModNPC modNPC) { this.modNPC = modNPC; }
		#region Hooks
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public virtual void FindFrame(int frameHeight)
		{
			foreach (var behavior in this)
			{
				behavior.FindFrame(frameHeight);
			}
		}
		public virtual void HitEffect(int hitDirection, double damage)
		{
			foreach (var behavior in this)
			{
				behavior.HitEffect(hitDirection, damage);
			}
		}
		public virtual void BossHeadRotation(ref float rotation)
		{
			foreach (var behavior in this)
			{
				behavior.BossHeadRotation(ref rotation);
			}
		}
		public virtual void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
		{
			foreach (var behavior in this)
			{
				behavior.BossHeadSpriteEffects(ref spriteEffects);
			}
		}
		public virtual void BossHeadSlot(ref int index)
		{
			foreach (var behavior in this)
			{
				behavior.BossHeadSlot(ref index);
			}
		}
		public virtual bool? CanBeHitByItem(Player player, Item item)
		{
			bool? Result = null;
			foreach (var behavior in this)
			{
				bool? N = behavior.CanBeHitByItem(player, item);
				if (N != null) Result = N;
			}
			return Result;
			//return States[IState].CanBeHitByItem(player, item);
		}
		public virtual bool? CanBeHitByProjectile(Projectile projectile)
		{
			bool? Result = null;
			foreach (var behavior in this)
			{
				bool? N = behavior.CanBeHitByProjectile(projectile);
				if (N != null) Result = N;
			}
			return Result;
			//return States[IState].CanBeHitByProjectile(projectile);
		}
		public virtual bool CanChat()
		{
			bool Result = npc.townNPC;
			foreach (var behavior in this)
			{
				bool N = behavior.CanChat();
				if (N != npc.townNPC) Result = N;
			}
			return Result;
			//return States[IState].CanChat();
		}
		public virtual bool? CanHitNPC(NPC target)
		{
			bool? Result = null;
			foreach (var behavior in this)
			{
				bool? N = behavior.CanHitNPC(target);
				if (N != null) Result = N;
			}
			return Result;
			//return States[IState].CanHitNPC(target);
		}
		public virtual bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			bool Result = true;
			foreach (var behavior in this)
			{
				bool N = behavior.CanHitPlayer(target, ref cooldownSlot);
				if (N != true) Result = N;
			}
			return Result;
			//return States[IState].CanHitPlayer(target, ref cooldownSlot);
		}
		public virtual bool CheckConditions(int left, int right, int top, int bottom)
		{
			bool Result = true;
			foreach (var behavior in this)
			{
				bool N = behavior.CheckConditions(left, right, top, bottom);
				if (N != true) Result = N;
			}
			return Result;
			//return States[IState].CheckConditions(left, right, top, bottom);
		}
		public virtual void DrawBehind(int index)
		{
			foreach (var behavior in this)
			{
				behavior.DrawBehind(index);
			}
		}
		public virtual void DrawEffects(ref Color drawColor)
		{
			foreach (var behavior in this)
			{
				behavior.DrawEffects(ref drawColor);
			}
		}
		public virtual bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			bool? Result = null;
			foreach (var behavior in this)
			{
				bool? N = behavior.DrawHealthBar(hbPosition, ref scale, ref position);
				if (N != null) Result = N;
			}
			return Result;
			//return States[IState].DrawHealthBar(hbPosition, ref scale, ref position);
		}
		public virtual void DrawTownAttackGun(ref float scale, ref int item, ref int closeness)
		{
			foreach (var behavior in this)
			{
				behavior.DrawTownAttackGun(ref scale, ref item, ref closeness);
			}
		}
		public virtual void DrawTownAttackSwing(ref Texture2D item, ref int itemSize, ref float scale, ref Vector2 offset)
		{
			foreach (var behavior in this)
			{
				behavior.DrawTownAttackSwing(ref item, ref itemSize, ref scale, ref offset);
			}
		}
		public virtual Color? GetAlpha(Color drawColor)
		{
			//TemplateMod2.Logging.Debug("MNA Start Get Alpha");
			//Color? d = States[IState].GetAlpha(drawColor);
			//TemplateMod2.Logging.Debug("MNA End Get Alpha");
			Color? d = null;
			foreach (var behavior in this)
			{
				Color? N = behavior.GetAlpha(drawColor);
				if (N != null) d = N;
			}
			return d;
		}
		public virtual void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			foreach (var behavior in this)
			{
				behavior.ModifyHitByItem(player, item, ref damage, ref knockback, ref crit);
			}
		}
		public virtual void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			foreach (var behavior in this)
			{
				behavior.ModifyHitByProjectile(projectile, ref damage, ref knockback, ref crit, ref hitDirection);
			}
		}
		public virtual void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit)
		{
			foreach (var behavior in this)
			{
				behavior.ModifyHitNPC(target, ref damage, ref knockback, ref crit);
			}
		}
		public virtual void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			foreach (var behavior in this)
			{
				behavior.ModifyHitPlayer(target, ref damage, ref crit);
			}
		}
		public virtual string GetChat()
		{
			string D = "";
			foreach (var behavior in this)
			{
				string N = behavior.GetChat();
				if (N.Trim().Length != 0 && N != Language.GetTextValue("tModLoader.DefaultTownNPCChat")) D += " " + N;
			}
			return D;
		}
		public virtual void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			foreach (var behavior in this)
			{
				behavior.OnHitByItem(player, item, damage, knockback, crit);
			}
		}
		public virtual void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			foreach (var behavior in this)
			{
				behavior.OnHitByProjectile(projectile, damage, knockback, crit);
			}
		}
		public virtual void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			foreach (var behavior in this)
			{
				behavior.OnHitNPC(target, damage, knockback, crit);
			}
		}
		public virtual void OnHitPlayer(Player target, int damage, bool crit)
		{
			foreach (var behavior in this)
			{
				behavior.OnHitPlayer(target, damage, crit);
			}
		}
		public virtual void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			foreach (var behavior in this)
			{
				behavior.PostDraw(spriteBatch, drawColor);
			}
		}
		public virtual bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			bool Result = true;
			foreach (var behavior in this)
			{
				bool N = behavior.PreDraw(spriteBatch, drawColor);
				if (N != true) Result = N;
			}
			return Result;
		}
		public virtual void NPCLoot()
		{
			foreach (var behavior in this)
			{
				if (behavior.PreNPCLoot() && behavior.SpecialNPCLoot()) behavior.NPCLoot();
			}
		}
		public virtual bool PreNPCLoot()
		{
			bool Result = false;
			foreach (var behavior in this)
			{
				bool N = behavior.PreNPCLoot();
				if (N != false) Result = N;
			}
			return Result;
		}
		public virtual void BossLoot(ref string name, ref int potionType)
		{
			foreach (var behavior in this)
			{
				if (behavior.PreNPCLoot() && behavior.SpecialNPCLoot()) BossLoot(ref name, ref potionType);
			}
		}
		public virtual bool SpecialNPCLoot()
		{
			bool Result = false;
			foreach (var behavior in this)
			{
				bool N = behavior.SpecialNPCLoot();
				if (N != false) Result = N;
			}
			return Result;
			//return States[IState].SpecialNPCLoot();
		}
		public virtual void ResetEffects()
		{
			foreach (var behavior in this)
			{
				behavior.ResetEffects();
			}
		}
		public virtual bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			bool Result = true;
			foreach (var behavior in this)
			{
				bool N = behavior.StrikeNPC(ref damage, defense, ref knockback, hitDirection, ref crit);
				if (N != true) Result = N;
			}
			return Result;
			//return States[IState].StrikeNPC(ref damage, defense, ref knockback, hitDirection, ref crit);
		}
		public virtual void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			foreach (var behavior in this)
			{
				behavior.ScaleExpertStats(numPlayers, bossLifeScale);
			}
		}
		public virtual void SetupShop(Chest shop, ref int nextSlot)
		{
			foreach (var behavior in this)
			{
				behavior.SetupShop(shop, ref nextSlot);
			}
		}
		public virtual void SetChatButtons(ref string button, ref string button2)
		{
			foreach (var behavior in this)
			{
				behavior.SetChatButtons(ref button, ref button2);
			}
		}
		public virtual void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			foreach (var behavior in this)
			{
				behavior.TownNPCAttackCooldown(ref cooldown, ref randExtraCooldown);
			}
		}
		public virtual void TownNPCAttackMagic(ref float auraLightMultiplier)
		{
			foreach (var behavior in this)
			{
				behavior.TownNPCAttackMagic(ref auraLightMultiplier);
			}
		}
		public virtual void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			foreach (var behavior in this)
			{
				behavior.TownNPCAttackProj(ref projType, ref attackDelay);
			}
		}
		public virtual void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			foreach (var behavior in this)
			{
				behavior.TownNPCAttackProjSpeed(ref multiplier, ref gravityCorrection, ref randomOffset);
			}
		}
		public virtual void TownNPCAttackShoot(ref bool inBetweenShots)
		{
			foreach (var behavior in this)
			{
				behavior.TownNPCAttackShoot(ref inBetweenShots);
			}
		}
		public virtual void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			foreach (var behavior in this)
			{
				behavior.TownNPCAttackStrength(ref damage, ref knockback);
			}
		}
		public virtual void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight)
		{
			foreach (var behavior in this)
			{
				behavior.TownNPCAttackSwing(ref itemWidth, ref itemHeight);
			}
		}
		public virtual string TownNPCName()
		{
			string D = "";
			foreach (var behavior in this)
			{
				string N = behavior.TownNPCName();
				if (N.Trim().Length != 0 && N != Language.GetTextValue("tModLoader.DefaultTownNPCName")) D += " " + N;
			}
			return D;
			//return States[IState].TownNPCName();
		}
		public virtual void UpdateLifeRegen(ref int damage)
		{
			foreach (var behavior in this)
			{
				behavior.UpdateLifeRegen(ref damage);
			}
		}
		public virtual bool CheckActive()
		{
			bool Result = true;
			foreach (var behavior in this)
			{
				bool N = behavior.CheckActive();
				if (N != true) Result = N;
			}
			return Result;
			//return States[IState].CheckActive();
		}
		/// <summary>
		/// 会this.Dispose
		/// </summary>
		public virtual bool CheckDead()
		{
			bool Result = true;
			foreach (var behavior in this)
			{
				bool N = behavior.CheckDead();
				if (N != true) Result = N;
			}
			if (Result) this.Dispose();
			return Result;
			//return States[IState].CheckDead();
		}
		public virtual bool UsesPartyHat()
		{
			bool Result = true;
			foreach (var behavior in this)
			{
				bool N = behavior.UsesPartyHat();
				if (N != true) Result = N;
			}
			return Result;
			//return States[IState].UsesPartyHat();
		}
		public virtual void OnCatchNPC(Player player, Item item)
		{
			foreach (var behavior in this)
			{
				behavior.OnCatchNPC(player, item);
			}
		}
		public virtual void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			foreach (var behavior in this)
			{
				behavior.OnChatButtonClicked(firstButton, ref shop);
			}
		}
		public virtual bool CanGoToStatue(bool toKingStatue)
		{
			bool Result = false;
			foreach (var behavior in this)
			{
				bool N = behavior.CanGoToStatue(toKingStatue);
				if (N != false) Result = N;
			}
			return Result;
			//return States[IState].CanGoToStatue(toKingStatue);
		}
		public virtual void OnGoToStatue(bool toKingStatue)
		{
			foreach (var behavior in this)
			{
				behavior.OnGoToStatue(toKingStatue);
			}
		}
		#endregion
	}

	public class ModNPCBehaviorComponentState : ModNPCBehaviorComponent<ModNPC>
	{
		public readonly string name;
		public override string BehaviorName => name;

		public override bool NetUpdateThis => true;

		public ModNPCBehaviorComponentState(ModNPC npc,string name="State") : base(npc) {
			this.name = name;
		}
	}
}
