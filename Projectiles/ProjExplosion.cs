using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XxDefinitions.Projectiles
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	public class ProjExplosion:ModProjectile
	{
		public Color color;
		public float R {
			get => projectile.scale * 49;
			set => projectile.scale = R / 49;
		}
		public static int TDamage=100000;
		public static int SummonProjExplosionTrap(Vector2 Position, float radius, int friendlyDamage, int hostileDamage, Color? color_)
		{
			Color color = color_ ?? Color.White;
			Projectile P = Projectile.NewProjectileDirect(Position, Vector2.Zero, ModContent.ProjectileType<ProjExplosion>(), TDamage, 0, Main.myPlayer,  hostileDamage, friendlyDamage);
			P.npcProj = false;
			P.trap = true;
			if (friendlyDamage != 0)
				P.friendly = true;
			if (hostileDamage != 0)
				P.hostile = true;
			//P.ai[0] = color.R;
			//P.ai[1] = color.G;
			//P.localAI[0] = color.B;
			//P.localAI[1] = color.A;
			ProjExplosion m = (ProjExplosion)P.modProjectile;m.color = color;
			P.scale = radius / 49;
			P.rotation = (float)(Main.rand.NextFloat() * Math.PI * 2);
			return P.identity;
		}
		/// <summary>
		/// 生成爆炸
		/// </summary>
		/// <param name="Position">爆炸的位置</param>
		/// <param name="friendlyDamage">对敌对NPC的伤害</param>
		/// <param name="hostileDamage">对友好NPC和玩家的伤害</param>
		/// <param name="radius">爆炸的半径</param>
		/// <param name="color_">爆炸的颜色</param>
		/// <param name="Owner">爆炸的所有者</param>
		/// <param name="npcProj">是否属于npc</param>
		/// <returns></returns>
		public static int SummonProjExplosion(Vector2 Position, float radius, int friendlyDamage,int hostileDamage,Color? color_,int Owner,bool npcProj) {
			Color color = color_ ?? Color.White;
			Projectile P= Projectile.NewProjectileDirect(Position, Vector2.Zero, ModContent.ProjectileType<ProjExplosion>(), TDamage, 0, Owner, hostileDamage,friendlyDamage );
			P.npcProj = npcProj;
			if(friendlyDamage!=0)
			P.friendly = true;
			if(hostileDamage != 0)
			P.hostile = true;
			//P.ai[0] = color.R;
			//P.ai[1] = color.G;
			//P.localAI[0] = color.B;
			//P.localAI[1] = color.A;
			ProjExplosion m = (ProjExplosion)P.modProjectile; m.color = color;
			P.scale = radius / 49;
			P.rotation = (float)(Main.rand.NextFloat() * Math.PI * 2);
			return P.identity;
		}
		public static int[] TimesPerFrame = new int[] {1,2,2,3,3,2,2};
		public static int[] TimesTotalPerFrame;
		static ProjExplosion() {
			TimesTotalPerFrame = new int[7];
			int t = 0;
			for (int i = 0; i < 7; ++i) {
				TimesTotalPerFrame[i] = t;
				t += TimesPerFrame[i];
			}
		}
		public static int TimePerFrame=>4;
		public static int TimeMax => TimesTotalPerFrame[6];
		public override void SetDefaults()
		{
			projectile.width = 98;               //The width of projectile hitbox
			projectile.height = 98;              //The height of projectile hitbox
			projectile.aiStyle = -1;             //The ai style of the projectile, please reference the source code of Terraria
			projectile.penetrate = -1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			projectile.timeLeft = TimeMax;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			projectile.alpha = 255;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			projectile.light = 0.5f;            //How much light emit around the projectile
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = false;          //Can the projectile collide with tiles?
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return Utils.CalculateUtils.CheckAABBvCircleColliding(targetHitbox,projectile.Center,R);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Rectangle drawRect=new Rectangle(0,0,98,98);
			drawRect.Y +=Utils.CalculateUtils.WeightedChoose((TimeMax-projectile.timeLeft), TimesPerFrame) *98;
			spriteBatch.Draw(
				ModContent.GetTexture(Texture),
				projectile.Center - Main.screenPosition,
				drawRect, color,
				projectile.rotation, new Vector2(49,49), projectile.scale, SpriteEffects.None, 0f
				);
			return false;
		}
		public override bool CanDamage()
		{
			return projectile.timeLeft==TimeMax- TimesTotalPerFrame[1];
		}
		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			//damage = (int)projectile.ai[0];
			//Main.NewText($"{damage},{(float)damage / TDamage},{projectile.ai[0]},{(int)((float)damage / TDamage * projectile.ai[0])},{crit}");
			damage = (int)((float)damage / TDamage * projectile.ai[0]);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!projectile.npcProj) target.immune[projectile.owner] = 0;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if(target.friendly) damage = (int)((float)damage / TDamage * projectile.ai[0]);
			else damage = (int)((float)damage / TDamage * projectile.ai[1]);
		}
		public override bool PreKill(int timeLeft)
		{
			return timeLeft==0;
		}
	}
}
