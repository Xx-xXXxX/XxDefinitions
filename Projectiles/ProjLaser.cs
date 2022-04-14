using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using XxDefinitions;
using static XxDefinitions.Utils;
namespace XxDefinitions.Projectiles
{
	/// <summary>
	/// 激光
	/// </summary>
	public class ProjLaser:ModProjectile
	{
		/// <summary>
		/// 激光的Type
		/// </summary>
		public static int Type=>ModContent.ProjectileType<ProjLaser>();
		/// <summary>
		/// 激光的默认伤害，用于计算伤害浮动，自动转换
		/// </summary>
		public const int DefDamage= 100000;
		/// <summary>
		/// 生成玩家的激光
		/// </summary>
		/// <param name="Pos">激光位置</param>
		/// <param name="MaxDistance">激光最大距离</param>
		/// <param name="Rotation">激光角度</param>
		/// <param name="Collide">激光是否碰撞方块</param>
		/// <param name="Width">激光的宽度</param>
		/// <param name="friendlyDamage">对友善生物的伤害</param>
		/// <param name="hostileDamage">对邪恶生物的伤害</param>
		/// <param name="color_">颜色</param>
		/// <param name="Owner">玩家ID</param>
		/// <param name="timeleft">时间</param>
		/// <param name="HitCooldown">击中无敌帧</param>
		/// <returns>ProjLaser</returns>
		public static ProjLaser SummonProjLaserPlayer(Vector2 Pos,uint MaxDistance,float Rotation,bool Collide,float Width, int friendlyDamage, int hostileDamage, Color? color_, int Owner,int timeleft=30,int HitCooldown=6) {
			Color color = color_ ?? Color.White;
			Projectile proj= Projectile.NewProjectileDirect(Pos, Rotation.ToRotationVector2(), Type, DefDamage, 0, Owner);
			ProjLaser projLaser=(ProjLaser)proj.modProjectile;
			if (friendlyDamage != 0)
			{
				proj.friendly = true;
				projLaser.FriendlyDamage = friendlyDamage;
			}
			else {
				proj.friendly = false;
				projLaser.FriendlyDamage = 0;
			}
			if (hostileDamage != 0)
			{
				proj.hostile = true;
				projLaser.HostileDamage = hostileDamage;
			}
			else {

				proj.hostile = false;
				projLaser.HostileDamage = 0;
			}
			projLaser.MaxDistance = MaxDistance;
			projLaser.Rotation = Rotation;
			projLaser.Collide = Collide;
			projLaser.color = color;
			projLaser.HitCooldown = HitCooldown;
			projLaser.Width = Width;
			proj.timeLeft = timeleft;
			return projLaser;
		}
		/// <summary>
		/// 生成NPC的激光
		/// </summary>
		/// <param name="Pos">激光位置</param>
		/// <param name="MaxDistance">激光最大距离</param>
		/// <param name="Rotation">激光角度</param>
		/// <param name="Collide">激光是否碰撞方块</param>
		/// <param name="Width">激光的宽度</param>
		/// <param name="friendlyDamage">对友善生物的伤害</param>
		/// <param name="hostileDamage">对邪恶生物的伤害</param>
		/// <param name="color_">颜色</param>
		/// <param name="timeleft">时间</param>
		/// <returns>ProjLaser</returns>
		public static ProjLaser SummonProjLaserNPC(Vector2 Pos, uint MaxDistance, float Rotation, bool Collide, float Width, int friendlyDamage, int hostileDamage, Color? color_, int timeleft = 30)
		{
			Color color = color_ ?? Color.White;
			Projectile proj = Projectile.NewProjectileDirect(Pos, Rotation.ToRotationVector2(), Type, DefDamage, 0, Main.myPlayer);
			ProjLaser projLaser = (ProjLaser)proj.modProjectile;
			if (friendlyDamage != 0)
			{
				proj.friendly = true;
				projLaser.FriendlyDamage = friendlyDamage;
			}
			else
			{
				proj.friendly = false;
				projLaser.FriendlyDamage = 0;
			}
			if (hostileDamage != 0)
			{
				proj.hostile = true;
				projLaser.HostileDamage = hostileDamage;
			}
			else
			{

				proj.hostile = false;
				projLaser.HostileDamage = 0;
			}
			projLaser.MaxDistance = MaxDistance;
			projLaser.Rotation = Rotation;
			projLaser.Collide = Collide;
			projLaser.color = color;
			projLaser.Width = Width;
			proj.npcProj = true;
			proj.timeLeft = timeleft;
			return projLaser;
		}
		/// <summary>
		/// 生成陷阱的激光
		/// </summary>
		/// <param name="Pos">激光位置</param>
		/// <param name="MaxDistance">激光最大距离</param>
		/// <param name="Rotation">激光角度</param>
		/// <param name="Collide">激光是否碰撞方块</param>
		/// <param name="Width">激光的宽度</param>
		/// <param name="friendlyDamage">对友善生物的伤害</param>
		/// <param name="hostileDamage">对邪恶生物的伤害</param>
		/// <param name="color_">颜色</param>
		/// <param name="timeleft">时间</param>
		/// <returns>ProjLaser</returns>
		public static ProjLaser SummonProjLaserTrap(Vector2 Pos, uint MaxDistance, float Rotation, bool Collide, float Width, int friendlyDamage, int hostileDamage, Color? color_, int timeleft = 30)
		{
			Color color = color_ ?? Color.White;
			Projectile proj = Projectile.NewProjectileDirect(Pos, Rotation.ToRotationVector2(), Type, DefDamage, 0, Main.myPlayer);
			ProjLaser projLaser = (ProjLaser)proj.modProjectile;
			if (friendlyDamage != 0)
			{
				proj.friendly = true;
				projLaser.FriendlyDamage = friendlyDamage;
			}
			else
			{
				proj.friendly = false;
				projLaser.FriendlyDamage = 0;
			}
			if (hostileDamage != 0)
			{
				proj.hostile = true;
				projLaser.HostileDamage = hostileDamage;
			}
			else
			{

				proj.hostile = false;
				projLaser.HostileDamage = 0;
			}
			projLaser.MaxDistance = MaxDistance;
			projLaser.Rotation = Rotation;
			projLaser.Collide = Collide;
			projLaser.color = color;
			projLaser.Width = Width;
			proj.trap = true;
			proj.timeLeft = timeleft;
			return projLaser;
		}
		/// <summary>
		/// 激光的颜色
		/// </summary>
		public Color color=Color.White;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Laser");
			DisplayName.AddTranslation(GameCulture.Chinese,"激光");
		}
		/// <summary>
		/// 激光的长度
		/// </summary>
		public float Distance
		{
			get => projectile.velocity.Length();
			set => projectile.velocity *= value / projectile.velocity.Length();
		}
		/// <summary>
		/// 激光的方向
		/// </summary>
		public float Rotation
		{
			get => projectile.velocity.ToRotation();
			set => projectile.velocity = projectile.velocity.RotatedBy(value - Rotation);
		}
		/// <summary>
		/// 激光的方向和长度
		/// </summary>
		public Vector2 LaserVel
		{
			get => projectile.velocity;
			set => projectile.velocity = value;
		}
		/// <summary>
		/// 对玩家和友好NPC的伤害
		/// </summary>
		public int FriendlyDamage {
			get => BitOperate.ToInt(projectile.ai[0]);
			set => projectile.ai[0] = BitOperate.ToFloat(value);
		}
		/// <summary>
		/// 对敌人的伤害
		/// </summary>
		public int HostileDamage
		{
			get => BitOperate.ToInt(projectile.ai[1]);
			set => projectile.ai[1] = BitOperate.ToFloat(value);
		}
		private static UIntSeparatorFactory IntSeparatorFactory = new UIntSeparatorFactory(new uint[] {2,8192});
		private UIntSeparator Data1 => IntSeparatorFactory.Build(new RefByDelegate<uint>(() => LocalAI0uInt, (v) => LocalAI0uInt = v));
		private uint LocalAI0uInt
		{
			get => BitOperate.ToUInt(projectile.localAI[0]);
			set => projectile.localAI[0] = BitOperate.ToFloat(value);
		}
		/// <summary>
		/// 是否计算碰撞方块
		/// </summary>
		public bool Collide {
			get => Data1[0]==1;
			set=> Data1[0]=value?1u:0u;
		}
		/// <summary>
		/// 激光的最大长度，不超过8192（否则循环）
		/// </summary>
		public uint MaxDistance {
			get => Data1[1];
			set => Data1[1] = value;
		}
		/// <summary>
		/// 击中NPC的无敌帧
		/// </summary>
		public int HitCooldown
		{
			get => projectile.localNPCHitCooldown;
			set => projectile.localNPCHitCooldown = value;
		}
		/// <summary>
		/// 激光绘图的宽度
		/// </summary>
		public float drawWidth;
		/// <summary>
		/// 激光绘图的宽度
		/// </summary>
		public float DrawWidth {
			get {
				if (drawWidth == 0) drawWidth = Body.RealSize().Y;
				return drawWidth;
			}
			set => drawWidth = value;
		}
		/// <summary>
		/// 激光的宽度（使用默认的）
		/// </summary>
		public float Width
		{
			get => projectile.scale * DrawWidth;
			set => projectile.scale = value / DrawWidth;
		}
		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hide = false;
			projectile.usesLocalNPCImmunity = true;
			projectile.ignoreWater = true;
			//Data1 = IntSeparatorFactory.Build(new RefByDelegate<uint>(() => LocalAI0uInt,(v)=>LocalAI0uInt=v));
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float d = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.Location.ToVector2(), new Vector2(targetHitbox.Width, targetHitbox.Height), projectile.position, projectile.position + LaserVel,Width,ref d);
		}
		public override void PostAI()
		{
			projectile.position -= projectile.velocity;
		}
		public override void AI()
		{
			if ((!ai?.Invoke(this)).IsTrue()) return;
			StartTime += 1;
			if (Pos!=null)
				projectile.position = Pos.Value;
			if (rotation != null) Rotation = rotation.Value;
			if (distance != null) Distance = distance.Value;
			else
			{
				if (Collide)
				{
					Distance = Utils.CalculateUtils.CanHitLineDistancePerfect(projectile.Center, Rotation, MaxDistance,true);
				}
				else
				{
					Distance = MaxDistance;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float d = 1f;
			if (StartTime < StartTimeMax) d = (float)StartTime / StartTimeMax;
			if (projectile.timeLeft < EndTimeMax) d = (float)projectile.timeLeft / EndTimeMax;
			Vector2 Scale = new Vector2(1f, d);
			float HeadLength =Head.RealSize().X*0.9f;
			float BodyLength =Body.RealSize().X*0.9f;
			float TailLength = Tail.RealSize().X * 0.9f;
			Vector2 O;
			for (float i = HeadLength/2f; i < Distance- TailLength/2f; i += BodyLength)
			{
				O = projectile.Center + new Vector2(i, 0).RotatedBy(Rotation);
				if(DrawBody ==null|| DrawBody(O,Rotation,Scale))
					Body.Draw(spriteBatch, O - Main.screenPosition, color, Rotation, Scale, SpriteEffects.None);
			}
			O= projectile.Center + new Vector2(Distance - TailLength / 2f, 0).RotatedBy(Rotation);
			if (DrawBody == null || DrawBody(O, Rotation, Scale))
				Body.Draw(spriteBatch, O - Main.screenPosition, color, Rotation, Scale, SpriteEffects.None);
			if (DrawTail == null || DrawTail(projectile.Center, Rotation, Scale))
				Tail.Draw(spriteBatch, projectile.Center - Main.screenPosition, color, Rotation, Scale, SpriteEffects.None);
			if (DrawHead == null || DrawHead(projectile.Center + LaserVel, Rotation, Scale))
				Head.Draw(spriteBatch, projectile.Center + LaserVel - Main.screenPosition, color, Rotation, Scale, SpriteEffects.None);
			return false;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (target.friendly) damage = (int)((float)damage / DefDamage * HostileDamage);
			else damage = (int)((float)damage / DefDamage * FriendlyDamage);
		}
		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			//damage = (int)projectile.ai[0];
			//Main.NewText($"{damage},{(float)damage / TDamage},{projectile.ai[0]},{(int)((float)damage / TDamage * projectile.ai[0])},{crit}");
			damage = (int)((float)damage / DefDamage * HostileDamage);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			OnHitNPCEffect?.Invoke(this,target,damage,crit);
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			OnHitPlayerEffect?.Invoke(this,target,damage,crit);
		}

		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 unit = projectile.velocity;
			Terraria.Utils.PlotTileLine(projectile.Center, projectile.Center +LaserVel, Width, DelegateMethods.CutTiles);
		}
		/// <summary>
		/// 绘图的头（结束的位置）
		/// </summary>
		public static Texture2DCutted Head => new Texture2DCutted(Main.projectileTexture[Type], new Rectangle(0, 52, 28, 26), new Vector2(28 * .5f, 26 * .5f), -(float)Math.PI * 0.5f);
		/// <summary>
		/// 绘图的尾（开始的位置）
		/// </summary>
		public static Texture2DCutted Tail => new Texture2DCutted(Main.projectileTexture[Type], new Rectangle(0, 0, 28, 26), new Vector2(28 * .5f, 26 * .5f), -(float)Math.PI * 0.5f);
		/// <summary>
		/// 绘图的中间
		/// </summary>
		public static Texture2DCutted Body => new Texture2DCutted(Main.projectileTexture[Type], new Rectangle(0, 26, 28, 26), new Vector2(28 * .5f, 26 * .5f), -(float)Math.PI * 0.5f);

		/// <summary>
		/// 位置，如果存在，自动设置，否则保持原来的位置
		/// </summary>
		public IGetValue<Vector2> Pos=null;
		/// <summary>
		/// 角度，如果存在，自动设置
		/// </summary>
		public IGetValue<float> rotation=null;
		/// <summary>
		/// 距离，如果存在，自动设置，否则计算
		/// </summary>
		public IGetValue<float> distance = null;
		/// <summary>
		/// 击中NPC的效果
		/// </summary>
		public Action<ProjLaser, NPC, int, bool> OnHitNPCEffect = null;
		/// <summary>
		/// 击中Player的效果
		/// </summary>
		public Action<ProjLaser,Player, int, bool> OnHitPlayerEffect = null;
		/// <summary>
		/// 计算前执行，返回false终止
		/// </summary>
		public Func<ProjLaser,bool> ai = null;
		/// <summary>
		/// 从生成开始的时间
		/// </summary>
		public int StartTime=0;
		/// <summary>
		/// 开始的时间，用于激光开始变粗（碰撞箱不变）
		/// </summary>
		public int StartTimeMax = 10;
		/// <summary>
		/// 结束的时间，用于激光开始变细（碰撞箱不变）
		/// </summary>
		public int EndTimeMax = 10;
		/// <summary>
		/// 在画对应部分前的的操作，返回false终止操作
		/// </summary>
		public delegate bool DrawFunc(Vector2 Position, float rotation, Vector2 Scale);
		/// <summary>
		/// 画身体之前
		/// </summary>
		public DrawFunc DrawBody;
		/// <summary>
		/// 画尾之前
		/// </summary>
		public DrawFunc DrawTail;
		/// <summary>
		/// 画头之前
		/// </summary>
		public DrawFunc DrawHead;
	}
}
