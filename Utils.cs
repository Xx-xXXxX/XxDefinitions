﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

using static System.Net.WebRequestMethods;

namespace XxDefinitions
{
	public static class Utils
	{
		public static void SpriteBatchUsingEffect(SpriteBatch spriteBatch) {
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
		public static void SpriteBatchEndUsingEffect(SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
	/// <summary>
	/// 获取符合正态分布的随机数
	/// </summary>
	public static double NextGaussian(this Terraria.Utilities.UnifiedRandom rand)
		{
			double u = -2 * Math.Log(rand.NextDouble());
			double v = 2 * Math.PI * rand.NextDouble();
			return (Math.Sqrt(u) * Math.Cos(v));
		}
		/// <summary>
		/// 获取符合正态分布的随机数
		/// </summary>
		public static double NextGaussian(this Terraria.Utilities.UnifiedRandom rand, double mu, double sigma) => rand.NextGaussian() * sigma + mu;
		public static Rectangle SetLeft(this Rectangle rect, int L) {
			rect.Width += L- rect.X;
			rect.X = L;
			return rect;
		}
		public static Rectangle SetRight(this Rectangle rect, int R) {
			rect.Width = R  - rect.X;return rect;
		}
		public static Rectangle SetTop(this Rectangle rect, int T) {
			rect.Height += T - rect.Y;
			rect.Y = T;
			return rect;
		}
		public static Rectangle SetBottom(this Rectangle rect, int B) {
			rect.Height = B - rect.X;return rect;
		}
		public static Rectangle SetLT(this Rectangle rect, Point point)
		{
			rect.SetLeft(point.X);
			rect.SetTop(point.Y);
			return rect;
		}
		public static Rectangle SetRB(this Rectangle rect, Point point)
		{
			rect.SetRight(point.X);
			rect.SetBottom(point.Y);
			return rect;
		}
		public static Rectangle MoveBy(this Rectangle rect, Point point)
		{
			rect.X -= point.X;
			rect.Y -= point.Y;
			return rect;
		}
		//public static Point operator +(Point A,Point B)
		public static bool NPCCanUse(NPC npc) => npc.active;
		public static bool PlayerCanUse(Player player) => player.active && !player.dead && !player.ghost;
		public static bool NPCCanFind(NPC npc) => npc.active && npc.CanBeChasedBy();
		public static bool PlayerCanFind(Player player) => player.active && !player.dead && !player.ghost;
		public static int Limit(int n, int l, int r)
		{
			if (n < l) n = l;
			if (n > r) n = r;
			return n;
		}
		public static int LimitCircular(int n, int l, int r) {
			int d = r - l;
			while (n < l) n += d;
			while (n >= r) n -= d;
			return n;
		}
		/*
		public static void LoadT() {
			Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Terraria.ModLoader.Default." + file + ".png");
			if (stream == null)
			{
				throw new ArgumentException("Given EquipType for PatreonItem or name is not valid. It is possible either does not match up with the classname. If you added a new EquipType, modify GetEquipTypeSuffix() and AddPatreonItemAndEquipType() first.");
			}
			return Texture2D.FromStream(Main.instance.GraphicsDevice, stream);
		}*/
		public static class CalculateUtils {
			/// <summary>
			/// 获取一个随n缓慢减小的值,从1到0
			/// <code>Sqrt(n + 1) - Sqrt(n)</code>
			/// </summary>
			public static double SlowlyDecreaseLim1To0(double n) => Math.Sqrt(n + 1) - Math.Sqrt(n);
			/// <summary>
			/// 获取一个随n缓慢减小的值,从1到0,SpeedParameter绝定该函数增长的速度。SpeedParameter越小，减小越慢。SpeedParameter应大于1
			/// <code>Math.Pow(n+1, 1 / SpeedParameter) - Math.Pow(n, 1 / SpeedParameter)</code>
			/// </summary>
			public static double SlowlyDecreaseLim1To0(double n, double SpeedParameter) => Math.Pow(n + 1, 1 / SpeedParameter) - Math.Pow(n, 1 / SpeedParameter);
			/// <summary>
			/// 用SlowlyDecreaseLim1To0获取一个随n缓慢增长的值,从0到1
			/// <code>1-SlowlyDecreaseLim1To0(n)</code>
			/// </summary>
			public static double SlowlyIncreaseLim0To1(double n) => 1 - SlowlyDecreaseLim1To0(n);
			/// <summary>
			/// 用SlowlyIncreaseLim0To1获取一个随n缓慢增长的值,从l到r
			/// </summary>
			public static double SlowlyIncreaseLim(double n, double l, double r) => SlowlyIncreaseLim0To1(n) * (r - l) + l;
			/// <summary><![CDATA[
			/// SlowlyIncrease 的原始函数
			/// 获取一个增长的值，n为参数，SpeedParameter绝定该函数增长的速度。SpeedParameter越大，增长越慢。
			/// SpeedParameter>e时会比log(n+1)小，SpeedParameter<1时比n大]]>
			/// <code>Exp(Pow(Ln(n+1), 1 / SpeedParameter));</code>
			/// </summary>
			public static double SlowlyIncreaseRaw(double n, double SpeedParameter) => Math.Exp(Math.Pow(Math.Log(n + 1), 1 / SpeedParameter));
			/// <summary>
			/// 获取一个缓慢增长的值，n为参数，SpeedParameter绝定该函数增长的速度。SpeedParameter越大，增长越慢。比log(n+1)大。SpeedParameter应大于0
			/// <code>Exp(Pow(Ln(n+1), 1/SlowlyIncreaseLim(SpeedParameter,1,E)));</code>
			/// </summary>
			public static double SlowlyIncrease(double n, double SpeedParameter) => Math.Exp(Math.Pow(Math.Log(n + 1), 1 / SlowlyIncreaseLim(SpeedParameter, 1, Math.E)));
			/// <summary>
			/// 获取一个快速增长的值，在n取1时达到正无穷，n为参数
			/// <code>Tan(n*PI/2)</code>
			/// </summary>
			public static double FastlyIncreaseToInf(double n) => Math.Tan(n * Math.PI / 2);
			/// <summary>
			/// 获取在Box中 到Point最近的点，可用于判断碰撞
			/// </summary>
			public static Vector2 GetNearestPoint(Rectangle Box, Vector2 Point)
			{
				Vector2 NearestPoint = Point;
				if (NearestPoint.X < Box.Left) NearestPoint.X = Box.Left;
				if (NearestPoint.X > Box.Right) NearestPoint.X = Box.Right;
				if (NearestPoint.Y < Box.Top) NearestPoint.Y = Box.Top;
				if (NearestPoint.Y > Box.Bottom) NearestPoint.Y = Box.Bottom;
				return NearestPoint;
			}
			public static bool CheckPointInCiecle(Vector2 Pos, float R, Vector2 Point)=> (Point - Pos).LengthSquared() <= R * R;
			/// <summary>
			/// 判断Box与 已Pos为圆心，已R为半径的圆 是否碰撞
			/// </summary>
			public static bool CheckAABBvCircleColliding(Rectangle Box, Vector2 Pos, float R) {
				Vector2 P = Utils.CalculateUtils.GetNearestPoint(Box, Pos);
				return CheckPointInCiecle(Pos, R, P);
			}
			public static float CrossProduct(Vector2 v1, Vector2 v2) => v1.X * v2.Y - v1.Y * v2.X;
			public static float? PredictWithVel(Vector2 OffsetPos, Vector2 TargetVel, float Speed) {
				float D = CrossProduct(OffsetPos, TargetVel) / (Speed * OffsetPos.Length());
				if (D > 1 || D < -1) return null;
				else return (float)Math.Asin(D) + OffsetPos.ToRotation();
			}
			//public static bool EntityCanFind(Entity item)=>item.
			//public static bool EntityInRange(Entity item,Vector2 pos,float R)=> item.active&&
			/// <summary>
			/// 以npc与Pos的距离为价值
			/// <code>(npc.Center - Pos).Length() - npc.Size.Length()</code>
			/// </summary>
			public static float NPCFindValue(NPC npc, Vector2 Pos) => (npc.Center - Pos).Length() - npc.Size.Length();
			/// <summary>
			/// 以player与Pos的距离为价值
			/// <code>(player.Center - Pos).Length() - player.Size.Length()</code>
			/// </summary>
			public static float PlayerFindValue(Player player, Vector2 Pos) => (player.Center - Pos).Length() - player.Size.Length();
			/// <summary>
			/// 根据价值搜索目标，找到价值最低的目标
			/// </summary>
			/// <param name="Pos">搜索位置</param>
			/// <param name="DefValue">初始价值</param>
			/// <param name="FindFriendly">将player和友好的npc作为可选目标</param>
			/// <param name="FindHostile">将敌对的npc作为可选目标</param>
			/// <param name="NPCCanFindFunc">确认npc可以作为目标</param>
			/// <param name="PlayerCanFindFunc">确认player可以作为目标</param>
			/// <param name="NPCFindValueFunc">npc的价值，默认NPCFindValue</param>
			/// <param name="PlayerFindValueFunc">player的价值，默认PlayerFindValue</param>
			/// <returns></returns>
			public static UnifiedTarget FindTargetClosest(Vector2 Pos,float DefValue,bool FindFriendly,bool FindHostile,Func<NPC,bool> NPCCanFindFunc= null, Func<Player, bool> PlayerCanFindFunc=null,Func<NPC,float> NPCFindValueFunc=null, Func<Player, float> PlayerFindValueFunc=null) {
				UnifiedTarget T = new UnifiedTarget();
				float Value = DefValue;
				if (NPCCanFindFunc == null) NPCCanFindFunc = NPCCanFind;
				if (PlayerCanFindFunc == null) PlayerCanFindFunc = PlayerCanFind;
				if (NPCFindValueFunc == null) NPCFindValueFunc = (npc) => NPCFindValue(npc, Pos);
				if (PlayerFindValueFunc == null) PlayerFindValueFunc = (player) => PlayerFindValue(player, Pos);
				foreach (var i in Main.npc) {
					if (NPCCanFindFunc(i)) {
						if ((FindFriendly && i.friendly)||(FindHostile&&!i.friendly)) {
							float newValue = NPCFindValueFunc(i);
							if (newValue < Value) {
								T.Set(i);
								Value = newValue;
							}
						}
					}
				}
				foreach (var i in Main.player)
				{
					if (PlayerCanFindFunc(i))
					{
						if (FindFriendly)
						{
							float newValue = PlayerFindValueFunc(i);
							if (newValue < Value)
							{
								T.Set(i);
								Value = newValue;
							}
						}
					}
				}
				return T;
			}
			public static int WeightingChoose(int I, params int[] values) {
				for (int i = 0; i < values.Length; ++i) {
					if (I < values[i]) return i;
					I -= values[i];
				}
				return -1;
			}
			//public static int WeightingChoose(int I, params int[] values) => WeightingChoose(I, values);
		}
		public static class SummonUtils {
			public static int SummonProjExplosion(Vector2 Position, float radius, int friendlyDamage, int hostileDamage, Color? color_, int Owner, bool npcProj) => Projectiles.ProjExplosion.SummonProjExplosion(Position, radius, friendlyDamage, hostileDamage, color_, Owner, npcProj);
			public static int SummonProjExplosionTrap(Vector2 Position, float radius, int friendlyDamage, int hostileDamage, Color? color_) => Projectiles.ProjExplosion.SummonProjExplosionTrap(Position, radius, friendlyDamage, hostileDamage, color_);
			static SummonUtils() {
				ModTranslation DRS = XxDefinitions.Instance.CreateTranslation("KilledBySummonDustExplosion");
				DRS.SetDefault("be kill with an explosion");
				DRS.AddTranslation(GameCulture.Chinese, "被炸死了");
				XxDefinitions.Instance.AddTranslation(DRS);
			}
			public static void SummonDustExplosion(Vector2 Position, float radius, int friendlyDamage, int hostileDamage, int DustType,int DustCircleNumber, int DustSpreadNumber,float DustSpeed,Action<Dust> DustDo=null,Func<Player, Terraria.DataStructures.PlayerDeathReason> MakeDeathReason=null) {
				for (int i = 0; i < DustSpreadNumber; i++)
				{
					Dust dust = Dust.NewDustPerfect(Position, DustType, Main.rand.NextVector2Circular(DustSpeed, DustSpeed));
					DustDo?.Invoke(dust);
				}
				for (int i = 0; i < DustSpreadNumber; i++) {
					Vector2 NewPos = Position + Main.rand.NextVector2Circular(radius, radius);
					Dust dust = Dust.NewDustPerfect(NewPos, DustType, Vector2.Zero);
					DustDo?.Invoke(dust);
				}
				if (friendlyDamage > 0 && hostileDamage > 0)
				{
					foreach (var i in Main.npc)
					{
						if (i.active)
						{
							if (!i.friendly && friendlyDamage > 0 && CalculateUtils.CheckAABBvCircleColliding(i.Hitbox, Position, radius)) i.StrikeNPC(friendlyDamage, 0, 0);
							if (i.friendly && hostileDamage > 0 && CalculateUtils.CheckAABBvCircleColliding(i.Hitbox, Position, radius)) i.StrikeNPC(hostileDamage, 0, 0);
						}
					}
				}
				if (hostileDamage > 0)
				{
					if (MakeDeathReason == null) {
						MakeDeathReason =(player)=> Terraria.DataStructures.PlayerDeathReason.ByCustomReason(player.name + " " + Language.GetTextValue("Mods.XxDefinitions.KilledBySummonDustExplosion"));
					}
					foreach (var i in Main.player)
					{
						if (i.active && CalculateUtils.CheckAABBvCircleColliding(i.Hitbox, Position, radius)) i.Hurt(MakeDeathReason(i),hostileDamage,0);
					}
				}
			}
		}
	}
}