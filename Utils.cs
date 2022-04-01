using Microsoft.Xna.Framework;
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

using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace XxDefinitions
{
	/// <summary>
	/// 方法
	/// </summary>
	public static class Utils
	{
		/// <summary>
		/// 启用Effect(Immediate)
		/// </summary>
		public static void SpriteBatchUsingEffect(SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
		/// <summary>
		/// 结束启用Effect(Deferred)
		/// </summary>
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
		/// <summary>
		/// 在其他边不变的情况下设置左边界
		/// </summary>
		public static Rectangle SetLeft(this Rectangle rect, int L)
		{
			rect.Width += L - rect.X;
			rect.X = L;
			return rect;
		}
		/// <summary>
		/// 在其他边不变的情况下设置右边界
		/// </summary>
		public static Rectangle SetRight(this Rectangle rect, int R)
		{
			rect.Width = R - rect.X; return rect;
		}
		/// <summary>
		/// 在其他边不变的情况下设置上边界
		/// </summary>
		public static Rectangle SetTop(this Rectangle rect, int T)
		{
			rect.Height += T - rect.Y;
			rect.Y = T;
			return rect;
		}
		/// <summary>
		/// 在其他边不变的情况下设置下边界
		/// </summary>
		public static Rectangle SetBottom(this Rectangle rect, int B)
		{
			rect.Height = B - rect.X; return rect;
		}
		/// <summary>
		/// 设置左上角
		/// </summary>
		public static Rectangle SetLT(this Rectangle rect, Point point)
		{
			rect.SetLeft(point.X);
			rect.SetTop(point.Y);
			return rect;
		}
		/// <summary>
		/// 设置右下角
		/// </summary>
		public static Rectangle SetRB(this Rectangle rect, Point point)
		{
			rect.SetRight(point.X);
			rect.SetBottom(point.Y);
			return rect;
		}
		/// <summary>
		/// 移动rect
		/// </summary>
		public static Rectangle MoveBy(this Rectangle rect, Point point)
		{
			rect.X += point.X;
			rect.Y += point.Y;
			return rect;
		}
		//public static Point operator +(Point A,Point B)
		/// <summary>
		/// 判断npc是否活动
		/// </summary>
		public static bool NPCCanUse(this NPC npc) => npc.active;
		/// <summary>
		/// 判断player是否活动
		/// </summary>
		public static bool PlayerCanUse(this Player player) => player.active && !player.dead && !player.ghost;
		/// <summary>
		/// 判断npc是否可以追踪
		/// </summary>
		public static bool NPCCanFind(this NPC npc) => NPCCanUse(npc) && npc.CanBeChasedBy();
		/// <summary>
		/// 判断npc是否可以追踪，避开Tile
		/// </summary>
		public static bool NPCCanFindNoTile(this NPC npc, Vector2 pos) => NPCCanFind(npc) && Terraria.Collision.CanHitLine(npc.Center, 1, 1, pos, 1, 1);
		/// <summary>
		/// 判断玩家是否可以追踪
		/// </summary>
		public static bool PlayerCanFind(this Player player) => PlayerCanUse(player);
		/// <summary>
		/// 判断npc是否可以追踪，避开Tile
		/// </summary>
		public static bool PlayerCanFindNoTile(this Player player, Vector2 pos) => PlayerCanUse(player) && Terraria.Collision.CanHitLine(player.Center, 1, 1, pos, 1, 1);
		/// <summary>
		/// 将n限制在[l,r]
		/// </summary>
		public static int Limit(int n, int l, int r)
		{
			if (n < l) n = l;
			if (n > r) n = r;
			return n;
		}
		/// <summary>
		/// 返回n在[l,r)中循环的结果
		/// </summary>
		public static int LimitLoop(int n, int l, int r)
		{
			int d = r - l;
			while (n < l) n += d;
			while (n >= r) n -= d;
			return n;
		}
		/// <summary>
		/// 返回n在[l,r)中循环的结果
		/// </summary>
		public static double LimitLoop(double n, double l, double r)
		{
			double d = r - l;
			while (n < l) n += d;
			while (n >= r) n -= d;
			return n;
		}
		/// <summary>
		/// 返回n在[l,r)中循环的结果
		/// </summary>
		public static float LimitLoop(float n, float l, float r)
		{
			float d = r - l;
			while (n < l) n += d;
			while (n >= r) n -= d;
			return n;
		}
		/// <summary>
		/// 用于计算的方法
		/// </summary>
		public static class CalculateUtils
		{
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
			/// 获取 在Box中 到Point最近的点，可用于判断碰撞
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
			/// <summary>
			/// 获取 在Box中 到Point最远的点，可用于判断碰撞
			/// </summary>
			public static Vector2 GetFarestPoint(Rectangle Box, Vector2 Point)
			{
				Vector2 NearestPoint = Point;
				if (Box.Right - NearestPoint.X > NearestPoint.X - Box.Left) NearestPoint.X = Box.Right;
				else NearestPoint.X = Box.Left;
				if (Box.Bottom - NearestPoint.Y > NearestPoint.Y - Box.Top) NearestPoint.Y = Box.Bottom;
				else NearestPoint.Y = Box.Top;
				return NearestPoint;
			}
			/// <summary>
			/// 判断点是否在圆内
			/// </summary>
			/// <param name="Pos">圆心</param>
			/// <param name="R">半径</param>
			/// <param name="Point">目标点</param>
			/// <returns></returns>
			public static bool CheckPointInCircle(Vector2 Pos, float R, Vector2 Point) => (Point - Pos).LengthSquared() <= R * R;
			/// <summary>
			/// 判断Box与 Pos为圆心，R为半径的圆 是否碰撞
			/// </summary>
			public static bool CheckAABBvCircleColliding(Rectangle Box, Vector2 Pos, float R)
			{
				Vector2 P = GetNearestPoint(Box, Pos);
				return CheckPointInCircle(Pos, R, P);
			}
			/// <summary>
			/// 判断Box与 Pos为圆心，半径MinR到MaxR的圆环 是否碰撞
			/// </summary>
			public static bool CheckAABBvAnnulusColliding(Rectangle Box, Vector2 Pos, float MaxR, float MinR)
			{
				Vector2 PMin = GetNearestPoint(Box, Pos);
				Vector2 PMax = GetFarestPoint(Box, Pos);
				return CheckPointInCircle(Pos, MaxR, PMin) && !CheckPointInCircle(Pos, MinR, PMax);
			}
			/// <summary>
			/// 计算二维向量叉乘的长
			/// </summary>
			public static float CrossProduct(Vector2 v1, Vector2 v2) => v1.X * v2.Y - v1.Y * v2.X;
			/// <summary>
			/// 根据相对位置，相对速度，固定发射速度进行预判
			/// </summary>
			/// <param name="OffsetPos">相对位置</param>
			/// <param name="OffsetVel">相对速度</param>
			/// <param name="Speed">固定发射速度</param>
			public static float? PredictWithVel(Vector2 OffsetPos, Vector2 OffsetVel, float Speed)
			{
				float D = CrossProduct(OffsetPos, OffsetVel) / (Speed * OffsetPos.Length());
				if (D > 1 || D < -1) return null;
				else return (float)Math.Asin(D) + OffsetPos.ToRotation();
			}
			/// <summary>
			/// 预判，返回速度，如果没有，返回Vector2.Normalize(OffsetVel) * Speed)
			/// </summary>
			/// <param name="OffsetPos"></param>
			/// <param name="OffsetVel"></param>
			/// <param name="Speed"></param>
			/// <returns></returns>
			public static Vector2 PredictWithVelDirect(Vector2 OffsetPos, Vector2 OffsetVel, float Speed)
			{
				//float? D= PredictWithVel(OffsetPos, OffsetVel, Speed);
				//return (D.HasValue ? D.Value.ToRotationVector2() * Speed : Vector2.Normalize(OffsetVel) * Speed);
				return (PredictWithVel(OffsetPos, OffsetVel, Speed) ?? OffsetVel.ToRotation()).ToRotationVector2() * Speed;
			}
			//public static bool EntityCanFind(Entity item)=>item.
			//public static bool EntityInRange(Entity item,Vector2 pos,float R)=> item.active&&
			/// <summary>
			/// 以npc与Pos的距离为价值
			/// <code>(npc.Center - Pos).Length() - npc.Size.Length()</code>
			/// </summary>
			public static float NPCFindValue(NPC npc, Vector2 Pos) => (npc.Center - Pos).Length() - npc.Size.Length() / 2;
			/// <summary>
			/// 以player与Pos的距离为价值
			/// <code>(player.Center - Pos).Length() - player.Size.Length()</code>
			/// </summary>
			public static float PlayerFindValue(Player player, Vector2 Pos) => (player.Center - Pos).Length() - player.Size.Length() / 2;
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
			public static UnifiedTarget FindTargetClosest(Vector2 Pos, float DefValue, bool FindFriendly, bool FindHostile, Func<NPC, bool> NPCCanFindFunc = null, Func<Player, bool> PlayerCanFindFunc = null, Func<NPC, float> NPCFindValueFunc = null, Func<Player, float> PlayerFindValueFunc = null)
			{
				UnifiedTarget T = new UnifiedTarget();
				float Value = DefValue;
				if (NPCCanFindFunc == null) NPCCanFindFunc = NPCCanFind;
				if (PlayerCanFindFunc == null) PlayerCanFindFunc = PlayerCanFind;
				if (NPCFindValueFunc == null) NPCFindValueFunc = (npc) => NPCFindValue(npc, Pos);
				if (PlayerFindValueFunc == null) PlayerFindValueFunc = (player) => PlayerFindValue(player, Pos);
				foreach (var i in Main.npc)
				{
					if (NPCCanFindFunc(i))
					{
						if ((FindFriendly && i.friendly) || (FindHostile && !i.friendly))
						{
							float newValue = NPCFindValueFunc(i);
							if (newValue < Value)
							{
								T.Set(i);
								Value = newValue;
							}
						}
					}
				}
				if (FindFriendly)
				{
					foreach (var i in Main.player)
					{
						if (PlayerCanFindFunc(i))
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
			/// <summary>
			/// 加权选择
			/// </summary>
			/// <returns>
			/// 返回选中的第几项
			/// 返回-1表示I超过权的和</returns>
			public static int WeightedChoose(int I, params int[] values)
			{
				for (int i = 0; i < values.Length; ++i)
				{
					if (I < values[i]) return i;
					I -= values[i];
				}
				return -1;
			}
			/// <summary>
			/// 计算以origin为起始点，向direction方向移动，直到碰到方块或到达最远距离的距离，不考虑斜方块和半砖
			/// </summary>
			public static float CanHitLineDistance(Vector2 origin, float direction, float MaxDistance = 2200f,bool fallThroughPlasform=true)
			{
				
				if (MaxDistance == 0) return 0f;
				bool first = true;
				Vector2 Offset =  direction.ToRotationVector2() * MaxDistance;
				Vector2 Unit = Vector2.Normalize(Offset);
				foreach (var i in EnumTilesInLine(origin, origin + Offset)) {
					Tile tile = Main.tile[i.X, i.Y];
					if (tile != null && !tile.inActive() && tile.active() && Main.tileSolid[tile.type]&&(Main.tileSolidTop[tile.type]? !fallThroughPlasform:true)) {
						if (first) return 0f;
						Vector2 tilePos = i.ToVector2() * 16;
						Vector2 HitT =Unit*( tilePos.Y - origin.Y)/Unit.Y;
						Vector2 HitB = Unit * (tilePos.Y + 16 - origin.Y) / Unit.Y;
						Vector2 HitL = Unit * (tilePos.X - origin.X) / Unit.X;
						Vector2 HitR = Unit * (tilePos.X + 16 - origin.X) / Unit.X;
						float HitTD = Vector2.Dot(HitT,Unit);
						float HitBD = Vector2.Dot(HitB, Unit);
						float HitLD = Vector2.Dot(HitL, Unit);
						float HitRD = Vector2.Dot(HitR, Unit);
						List<float> vs = new List<float>() { HitBD, HitLD, HitRD };
						//vs.Sort();
						//return vs[1];
						float vs1= HitTD; float vs2= HitBD;
						foreach (var j in vs) {
							if (j < vs1)
							{
								vs2 = vs1; vs1 = j;
							}
							else if (j < vs2) {
								vs2 = j;
							}
						}
						return vs2;
					}
					first = false;
				}
				return MaxDistance;
			}

			/// <summary>
			/// 计算以origin为起始点，向direction方向移动，直到碰到方块或到达最远距离的距离
			/// </summary>
			public static float CanHitLineDistancePerfect(Vector2 origin, float direction, float MaxDistance = 2200f, bool fallThroughPlasform = true)
			{

				if (MaxDistance == 0) return 0f;
				bool first = true;
				Vector2 Offset = direction.ToRotationVector2() * MaxDistance;
				Vector2 Unit = Vector2.Normalize(Offset);
				foreach (var i in EnumTilesInLine(origin, origin + Offset))
				{
					Tile tile = Main.tile[i.X, i.Y];
					if (tile != null && !tile.inActive() && tile.active() && Main.tileSolid[tile.type] && (Main.tileSolidTop[tile.type] ? !fallThroughPlasform : true))
					{
						if (first) return 0f;
						Vector2 tilePos = i.ToVector2() * 16;
						List<Vector2> Points=new List<Vector2>();
						if (tile.halfBrick())
						{
							Points.Add(tilePos + new Vector2(0, 8));
							Points.Add(tilePos + new Vector2(16, 8));
							Points.Add(tilePos + new Vector2(16, 16));
							Points.Add(tilePos + new Vector2(0, 16));
						}
						else
						{
							switch (tile.slope())
							{
								case 0:
									{
										Points.Add(tilePos + new Vector2(0, 0));
										Points.Add(tilePos + new Vector2(16, 0));
										Points.Add(tilePos + new Vector2(16, 16));
										Points.Add(tilePos + new Vector2(0, 16));
									}
									break;
								case 1:
									{
										Points.Add(tilePos + new Vector2(0, 0));
										Points.Add(tilePos + new Vector2(16, 16));
										Points.Add(tilePos + new Vector2(0, 16));
									}
									break;
								case 2:
									{
										Points.Add(tilePos + new Vector2(16, 0));
										Points.Add(tilePos + new Vector2(16, 16));
										Points.Add(tilePos + new Vector2(0, 16));
									}
									break;
								case 3:
									{
										Points.Add(tilePos + new Vector2(0, 0));
										Points.Add(tilePos + new Vector2(16, 0));
										Points.Add(tilePos + new Vector2(0, 16));
									}
									break;
								case 4:
									{
										Points.Add(tilePos + new Vector2(0, 0));
										Points.Add(tilePos + new Vector2(16, 0));
										Points.Add(tilePos + new Vector2(16, 16));
									}
									break;
							}
						}
						//List<float> Collidings = new List<float>();
						//for (int k = 0; k < Points.Count-1; ++k) {
						//	Collidings.Add(LineCollitionDistance(origin, direction, Points[k], (Points[k+1]-Points[k]).ToRotation()));
						//}
						//Collidings.Add(LineCollitionDistance(origin, direction, Points[Points.Count - 1], (Points[Points.Count - 1] - Points[0]).ToRotation()));
						//Collidings.Sort();
						//return Collidings[Collidings.Count-3];
						List<Vector2> Collidings = new List<Vector2>();
						Vector2 End = direction.ToRotationVector2() * MaxDistance + origin;
						foreach (var k in Points.EnumPairs()) {
							Collidings.AddRange(Terraria.Collision.CheckLinevLine(origin, End,k.Item1,k.Item2));
						}
						if (Collidings.Count <= 0) continue;
						float Distance = MaxDistance;
						foreach (var k in Collidings) {
							float NowDistance = (k - origin).Length();
							if (NowDistance < Distance) Distance = NowDistance;
						}
						return Distance;
					}
					first = false;
				}
				return MaxDistance;
			}
			/// <summary>
			/// 线1 过原点，方向Direction1 与 线2 过Point2 ，方向Direction2 交点到原点的距离（反向为负）
			/// </summary>
			public static float LineCollitionDistance(float Direction1, Vector2 Point2, float Direction2) {
				float P2Angle = Point2.ToRotation();
				float Angle1 = P2Angle - Direction1;
				float Angle2 = (float)Math.PI - P2Angle + Direction2;
				float Angle3 = (float)Math.PI - Angle1 - Angle2;
				float Distance =(float)( Point2.Length() / Math.Sin(Angle3) * Math.Sin(Angle2));
				return Distance;
			}
			public static float LineCollitionDistance(Vector2 Point1, float Direction1, Vector2 Point2, float Direction2) => LineCollitionDistance(Direction1,Point2-Point1,Direction2);
			public static float LineCollitionDistance(Vector2 Point1a, Vector2 Point1b, Vector2 Point2a, Vector2 Point2b)
			{
				return LineCollitionDistance((Point1b - Point1a).ToRotation(),Point2a-Point1a,(Point2b-Point2a).ToRotation());
			}
			/// <summary>
			/// 计算碰撞箱以Velocity速度移动，碰撞后最终速度
			/// </summary>
			/// <param name="rect"></param>
			/// <param name="Velocity"></param>
			/// <param name="StopWhenHit">是否在碰到物块时结束计算</param>
			/// <param name="fallThrough">是否穿过平台</param>
			/// <param name="fall2">在Velocity.Y>1时穿过平台</param>
			/// <param name="gravDir"></param>
			/// <returns></returns>
			public static Vector2 TileCollisionPerfect(Rectangle rect, Vector2 Velocity, bool StopWhenHit = false, bool fallThrough = false, bool fall2 = false, int gravDir = 1)
			{
				Vector2 Position = rect.Location.ToVector2();
				int Width = rect.Width;
				int Height = rect.Height;
				Vector2 NewPosition = Position;
				float PL = Math.Min(Width, Height);
				Vector2 PV = Vector2.Normalize(Velocity) * PL;
				Vector2 PV0 = PV;
				int i = 1;
				bool RealFall = fallThrough || (Velocity.Y > 1 && fall2);
				for (; PL * i < Velocity.Length(); ++i)
				{
					Vector2 NPL = Collision.TileCollision(NewPosition, PV, Width, Height, RealFall, false, gravDir);
					NewPosition += NPL;
					if (NPL != PV && StopWhenHit) return NewPosition - Position;
					PV = NPL;
				}
				NewPosition += Collision.TileCollision(NewPosition, (PV / PV0.Length()) * (Velocity.Length() - PL * (i - 1)), Width, Height, RealFall, false, gravDir);
				return NewPosition - Position;
			}
			/// <summary>
			/// 对蠕虫的头执行，加载全身，需要npc最后的速度
			/// </summary>
			/// <param name="npc">头</param>
			/// <param name="Nextnpc">下一个npc</param>
			/// <param name="End">是否结束</param>
			/// <param name="projLength">体节的长度，应比npc的长度稍小</param>
			/// <param name="AngleAmount">体节转向角度缩小比例</param>
			public static void UpdateWornPositionNPC(NPC npc, Func<NPC, NPC> Nextnpc, Func<NPC, bool> End, float projLength, float AngleAmount)
			{
				while (End(npc))
				{
					NPC Nownpc = Nextnpc(npc);
					Vector2 ToPos = npc.Center + npc.velocity + new Vector2(-0.5f * projLength, 0).RotatedBy(npc.rotation);
					Vector2 OffsetPos = ToPos - Nownpc.Center;
					float OffsetAngle = npc.rotation.AngleLerp(OffsetPos.ToRotation(), 1 - AngleAmount);
					Nownpc.Center = ToPos - OffsetAngle.ToRotationVector2() * projLength / 2;
					Nownpc.rotation = OffsetAngle;
					Nownpc.velocity = Vector2.Zero;
					npc = Nownpc;
				}
			}
			/// <summary>
			/// 对蠕虫的头执行，加载全身，需要npc最后的速度
			/// </summary>
			/// <param name="npc">头</param>
			/// <param name="Nextnpc">下一个npc</param>
			/// <param name="End">是否结束</param>
			/// <param name="projLength">体节的长度，应比npc的长度稍小</param>
			public static void UpdateWornPositionNPC(NPC npc, Func<NPC, NPC> Nextnpc, Func<NPC, bool> End, float projLength)
			{
				while (End(npc))
				{
					NPC Nownpc = Nextnpc(npc);
					Vector2 ToPos = npc.Center + npc.velocity + new Vector2(-0.5f * projLength, 0).RotatedBy(npc.rotation);
					Vector2 OffsetPos = ToPos - Nownpc.Center;
					float OffsetAngle = OffsetPos.ToRotation();
					Nownpc.Center = ToPos - OffsetAngle.ToRotationVector2() * projLength / 2;
					Nownpc.rotation = OffsetAngle;
					Nownpc.velocity = Vector2.Zero;
					npc = Nownpc;
				}
			}
			/// <summary>
			/// 对蠕虫的头执行，加载全身，需要projectile最后的速度
			/// </summary>
			/// <param name="projectile">头</param>
			/// <param name="NextProj">下一个proj</param>
			/// <param name="End">是否结束</param>
			/// <param name="projLength">体节的长度，应比proj的长度稍小</param>
			/// <param name="AngleAmount">体节转向角度缩小比例</param>
			public static void UpdateWornPositionProj(Projectile projectile, Func<Projectile, Projectile> NextProj, Func<Projectile, bool> End, float projLength, float AngleAmount)
			{
				while (!End(projectile))
				{
					Projectile NowProj1 = NextProj(projectile);
					Vector2 ToPos = projectile.Center + projectile.velocity + new Vector2(-0.5f * projLength, 0).RotatedBy(projectile.rotation);
					Vector2 OffsetPos = ToPos - NowProj1.Center;
					float OffsetAngle = projectile.rotation.AngleLerp(OffsetPos.ToRotation(), 1 - AngleAmount);
					NowProj1.Center = ToPos - OffsetAngle.ToRotationVector2() * projLength / 2;
					NowProj1.rotation = OffsetAngle;
					NowProj1.velocity = Vector2.Zero;
					projectile = NowProj1;
				}
			}
			/// <summary>
			/// 对蠕虫的头执行，加载全身，需要projectile最后的速度
			/// </summary>
			/// <param name="projectile">头</param>
			/// <param name="NextProj">下一个proj</param>
			/// <param name="End">是否结束</param>
			/// <param name="projLength">体节的长度，应比proj的长度稍小</param>
			public static void UpdateWornPositionProj(Projectile projectile, Func<Projectile, Projectile> NextProj, Func<Projectile, bool> End, float projLength)
			{
				while (!End(projectile))
				{
					Projectile NowProj1 = NextProj(projectile);
					Vector2 ToPos = projectile.Center + projectile.velocity + new Vector2(-0.5f * projLength, 0).RotatedBy(projectile.rotation);
					Vector2 OffsetPos = ToPos - NowProj1.Center;
					float OffsetAngle = OffsetPos.ToRotation();
					NowProj1.Center = ToPos - OffsetAngle.ToRotationVector2() * projLength / 2;
					NowProj1.rotation = OffsetAngle;
					NowProj1.velocity = Vector2.Zero;
					projectile = NowProj1;
				}
			}
			/// <summary>
			/// 获取两个向量的夹角的cos值
			/// </summary>
			public static float AngleCos(Vector2 a, Vector2 b) => (Vector2.Dot(a, b)) / (a.Length() * b.Length());
			/// <summary>
			/// 获取两个向量的夹角
			/// </summary>
			public static float Angle(Vector2 a, Vector2 b) => (float)Math.Acos(AngleCos(a, b));
			/// <summary>
			/// 枚举线上的物块
			/// </summary>
			public static IEnumerable<Point> EnumTilesInLine(Vector2 Start, Vector2 End)
			{
				if (Start.Y == End.Y)
				{
					int Y = (int)(Start.Y / 16);
					if (Y < 1 || Y >= Main.maxTilesY) yield break;
					int L = (int)(Start.X / 16);
					int R = (int)(End.X / 16);
					if (L > R) Terraria.Utils.Swap(ref L, ref R);
					if (L < 1) L = 1;
					if (L >= Main.maxTilesX) L = Main.maxTilesX - 1;
					if (R + 1 < 1) R = 0;
					if (R + 1 >= Main.maxTilesX) R = Main.maxTilesX - 2;
					for (int i = L; i < R + 1; ++i)
						yield return new Point(i, Y);
					yield break;
				}
				//Vector2 Point = Start;
				Vector2 Offset = End - Start;
				Vector2 Unit = Vector2.Normalize(Offset);
				bool GoRight = Unit.X > 0;
				bool GoDown = Unit.Y > 0;
				Vector2 UnitY16 = Unit * (16 / Unit.Y);
				float FromX = Start.X;
				float FromY = Start.Y;
				int FromXP = (int)(FromX / 16);
				int FromYP = (int)(FromY / 16);
				int NextYP;
				float NextX;
				int NextXP;
				int EndYP = (int)(End.Y / 16f);
				if (GoDown)
				{
					NextYP = FromYP + 1;
					NextX = FromX + UnitY16.X * (NextYP * 16 - FromY) / 16;
					NextXP = (int)(NextX / 16f);
					while (FromYP < EndYP)
					{
						if (GoRight)
							for (int i = FromXP; i <= NextXP; ++i)
								yield return new Point(i, FromYP);
						else
							for (int i = FromXP; i >= NextXP; --i)
								yield return new Point(i, FromYP);
						FromX = NextX;
						FromXP = (int)(FromX / 16);
						FromYP = NextYP;
						NextYP = FromYP + 1;
						NextX = FromX + UnitY16.X;
						NextXP = (int)(NextX / 16f);
					}
					NextX = End.X;
					NextXP = (int)(NextX / 16f);
					if (GoRight)
						for (int i = FromXP; i <= NextXP; ++i)
							yield return new Point(i, FromYP);
					else
						for (int i = FromXP; i >= NextXP; --i)
							yield return new Point(i, FromYP);
				}
				else
				{
					NextYP = FromYP;
					NextX = FromX + UnitY16.X * (NextYP * 16 - FromY) / 16;
					NextXP = (int)(NextX / 16f);
					while (FromYP > EndYP)
					{
						if (GoRight)
							for (int i = FromXP; i <= NextXP; ++i)
								yield return new Point(i, FromYP);
						else
							for (int i = FromXP; i >= NextXP; --i)
								yield return new Point(i, FromYP);
						FromX = NextX;
						FromXP = (int)(FromX / 16);
						FromYP = NextYP - 1;
						NextYP = FromYP;
						NextX = FromX - UnitY16.X;
						NextXP = (int)(NextX / 16f);
					}
					NextX = End.X;
					NextXP = (int)(NextX / 16f);
					if (GoRight)
						for (int i = FromXP; i <= NextXP; ++i)
							yield return new Point(i, FromYP);
					else
						for (int i = FromXP; i >= NextXP; --i)
							yield return new Point(i, FromYP);
				}
			}
			
			/// <summary>
			/// 确定点是否在线的上方（世界上线的下方）
			/// </summary>
			public static bool PointAboveLine(Vector2 Point, Vector2 Start, Vector2 End)
			{
				Vector2 Unit = Vector2.Normalize(End - Start);
				Vector2 OffsetPoint = Point - Start;
				Vector2 PointXOnLine = Unit / Unit.X * OffsetPoint.X;
				return OffsetPoint.Y > PointXOnLine.Y;
			}
			/// <summary>
			/// 确定点是否在线的上方（世界上线的下方）
			/// </summary>
			public static bool PointAboveLine(Vector2 Point, Vector2 Line)
			{
				Vector2 Unit = Vector2.Normalize(Line);
				Vector2 OffsetPoint = Point;
				Vector2 PointXOnLine = Unit / Unit.X * OffsetPoint.X;
				return OffsetPoint.Y > PointXOnLine.Y;
			}
		}
		public static Tile ToTile(this Vector2 vector)
		{
			Point point = (vector / 16).ToPoint();
			return Main.tile[point.X, point.Y];
		}
		/// <summary>
		/// 生成方法
		/// </summary>
		public static class SummonUtils
		{
			/// <summary>
			/// 生成作为弹幕的爆炸
			/// </summary>
			/// <param name="Position">爆炸位置</param>
			/// <param name="radius">爆炸半径</param>
			/// <param name="friendlyDamage">对hostile的npc的伤害</param>
			/// <param name="hostileDamage">对player和友好npc的伤害</param>
			/// <param name="color_">爆炸颜色</param>
			/// <param name="Owner">弹幕的所有者</param>
			/// <param name="npcProj">弹幕是否为npc的弹幕，注意npc的弹幕的所有者为Main.myplayer</param>
			/// <returns>爆炸弹幕的id</returns>
			public static int SummonProjExplosion(Vector2 Position, float radius, int friendlyDamage, int hostileDamage, Color? color_, int Owner, bool npcProj) => Projectiles.ProjExplosion.SummonProjExplosion(Position, radius, friendlyDamage, hostileDamage, color_, Owner, npcProj);
			/// <summary>
			/// 生成作为弹幕，陷阱的爆炸
			/// </summary>
			/// <param name="Position">爆炸位置</param>
			/// <param name="radius">爆炸半径</param>
			/// <param name="friendlyDamage">对hostile的npc的伤害</param>
			/// <param name="hostileDamage">对player和友好npc的伤害</param>
			/// <param name="color_">爆炸颜色</param>
			/// <returns>爆炸弹幕的id</returns>
			public static int SummonProjExplosionTrap(Vector2 Position, float radius, int friendlyDamage, int hostileDamage, Color? color_) => Projectiles.ProjExplosion.SummonProjExplosionTrap(Position, radius, friendlyDamage, hostileDamage, color_);
			static SummonUtils()
			{
				ModTranslation DRS = XxDefinitions.Instance.CreateTranslation("KilledBySummonDustExplosion");
				DRS.SetDefault("be kill with an explosion");
				DRS.AddTranslation(GameCulture.Chinese, "被炸死了");
				XxDefinitions.Instance.AddTranslation(DRS);
			}
			/// <summary>
			/// 对半径中的对象造成伤害，并产生粒子
			/// </summary>
			/// <param name="Position">爆炸位置</param>
			/// <param name="radius">爆炸半径</param>
			/// <param name="friendlyDamage">对hostile的npc的伤害</param>
			/// <param name="hostileDamage">对player和友好npc的伤害</param>
			/// <param name="DustType">粒子类型</param>
			/// <param name="DustCircleNumber">粒子在爆炸圆中的数量</param>
			/// <param name="DustSpreadNumber">粒子在中心扩散的数量</param>
			/// <param name="DustSpeed">扩散的粒子的速度</param>
			/// <param name="DustDo">对每个生成的粒子的操作</param>
			/// <param name="MakeDeathReason">杀死玩家的原因，默认为 player.name + " " + Language.GetTextValue("Mods.XxDefinitions.KilledBySummonDustExplosion") </param>
			public static void SummonDustExplosion(Vector2 Position, float radius, int friendlyDamage, int hostileDamage, int DustType, int DustCircleNumber, int DustSpreadNumber, float DustSpeed, Action<Dust> DustDo = null, Func<Player, Terraria.DataStructures.PlayerDeathReason> MakeDeathReason = null)
			{
				for (int i = 0; i < DustSpreadNumber; i++)
				{
					Dust dust = Dust.NewDustPerfect(Position, DustType, Main.rand.NextVector2Circular(DustSpeed, DustSpeed));
					DustDo?.Invoke(dust);
				}
				for (int i = 0; i < DustCircleNumber; i++)
				{
					Vector2 NewPos = Position + Main.rand.NextVector2Circular(radius, radius);
					Dust dust = Dust.NewDustPerfect(NewPos, DustType, Vector2.Zero);
					DustDo?.Invoke(dust);
				}
				if (friendlyDamage > 0 || hostileDamage > 0)
				{
					foreach (var i in Main.npc)
					{
						if (i.active && !i.dontTakeDamage)
						{
							if (!i.friendly && friendlyDamage > 0 && CalculateUtils.CheckAABBvCircleColliding(i.Hitbox, Position, radius)) i.StrikeNPC(friendlyDamage, 0, 0);
							if (i.friendly && !i.dontTakeDamageFromHostiles && hostileDamage > 0 && CalculateUtils.CheckAABBvCircleColliding(i.Hitbox, Position, radius)) i.StrikeNPC(hostileDamage, 0, 0);
						}
					}
				}
				if (hostileDamage > 0)
				{
					if (MakeDeathReason == null)
					{
						MakeDeathReason = (player) => Terraria.DataStructures.PlayerDeathReason.ByCustomReason(player.name + " " + Language.GetTextValue("Mods.XxDefinitions.KilledBySummonDustExplosion"));
					}
					foreach (var i in Main.player)
					{
						if (i.active && CalculateUtils.CheckAABBvCircleColliding(i.Hitbox, Position, radius)) i.Hurt(MakeDeathReason(i), hostileDamage, 0);
					}
				}
			}
		}
		/// <summary>
		/// 如果value有值且为真
		/// </summary>
		public static bool IsTrue(this bool? value) => value.HasValue && value.Value == true;
		/// <summary>
		/// 判断是否为默认值
		/// </summary>
		public static bool IsDef<T>(this T obj) => obj == null || obj.Equals(default(T));
		/// <summary>
		/// 判断是否为空
		/// </summary>
		public static bool IsNull(this object obj) => obj == null;
		/// <summary>
		/// 在范围内
		/// </summary>
		public static bool InRange(this float obj, float value = 0, float range = float.Epsilon)
		{
			float d = (float)Math.Abs(obj - value);
			return d < range;
		}
		/// <summary>
		/// 在范围内
		/// </summary>
		public static bool InRange(this double obj, double value = 0, double range = float.Epsilon)
		{
			double d = (double)Math.Abs(obj - value);
			return d < range;
		}
		/// <summary><![CDATA[
		/// 从IGetValue<T>到Func<T>]]>
		/// </summary>
		public static Func<T> ToGetDelegate<T>(this IGetValue<T> get) => () => get.Value;
		/// <summary><![CDATA[
		/// 从ISetValue<T>到Action<T>]]>
		/// </summary>
		public static Action<T> ToSetDelegate<T>(this ISetValue<T> set) => (v) => set.Value = v;
		/// <summary><![CDATA[
		/// 从Func<T>到IGetValue<T>]]>
		/// </summary>
		public static GetByDelegate<T> ToGet<T>(this Func<T> func) => (GetByDelegate<T>)func;
		/// <summary><![CDATA[
		/// 从Action<T>到ISetValue<T>]]>
		/// </summary>
		public static SetByDelegate<T> ToSet<T>(this Action<T> action) => (SetByDelegate<T>)action;
		/// <summary>
		/// 将Get和Set结合成Ref
		/// </summary>
		public static RefByDelegate<T> Combine<T>(this IGetValue<T> get, ISetValue<T> set) => new RefByDelegate<T>(get.ToGetDelegate(), set.ToSetDelegate());
		/// <summary>
		/// 将Get和Set结合成Ref
		/// </summary>
		public static RefByDelegate<T> Combine<T>(this ISetValue<T> set, IGetValue<T> get) => new RefByDelegate<T>(get.ToGetDelegate(), set.ToSetDelegate());
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public static T Max<T>(params T[] vs)
			where T:IComparable
		{
			T V = vs[0];
			foreach (var i in vs) if (i.CompareTo(V)>0) V = i;
			return V;
		}
		public static T Min<T>(params T[] vs)
			where T : IComparable
		{
			T V = vs[0];
			foreach (var i in vs) if (i.CompareTo(V) < 0) V = i;
			return V;
		}
		public static T SecondMax<T>(params T[] vs)
			where T:IComparable
		{
			T MaxV = vs[0];
			T MaxV2 = vs[1];
			foreach (var i in vs) {
				if (i.CompareTo(MaxV) > 0)
				{
					MaxV2 = MaxV;
					MaxV = i;
				}
				else if (i.CompareTo(MaxV2) > 0) {
					MaxV2 = i;
				}
			}
			return MaxV2;
		}
		public static T SecondMin<T>(params T[] vs)
			where T : IComparable
		{
			T V = vs[0];
			T V2 = vs[1];
			foreach (var i in vs)
			{
				if (i.CompareTo(V) < 0)
				{
					V2 = V;
					V = i;
				}
				else if (i.CompareTo(V2) < 0)
				{
					V2 = i;
				}
			}
			return V2;
		}
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
		/// <summary>
		/// 获取Player的位置的IGetValue
		/// </summary>
		public static IGetValue<Vector2> GetPlayerCenter(int id, Vector2 Offset = default)
		{
			return (Get<Vector2>)(() => Main.player[id].Center + Offset);
		}
		/// <summary>
		/// 获取Player的位置的IGetValue
		/// </summary>
		public static IGetValue<Vector2> GetPlayerCenter(Player player, Vector2 Offset = default)
		{
			return (Get<Vector2>)(() => player.Center + Offset);
		}
		/// <summary>
		/// 获取NPC的位置的IGetValue
		/// </summary>
		public static IGetValue<Vector2> GetNPCCenter(int id, Vector2 Offset = default)
		{
			return (Get<Vector2>)(() => Main.npc[id].Center + Offset);
		}
		/// <summary>
		/// 获取NPC的位置的IGetValue
		/// </summary>
		public static IGetValue<Vector2> GetNPCCenter(NPC npc, Vector2 Offset = default)
		{
			return (Get<Vector2>)(() => npc.Center + Offset);
		}
		/// <summary>
		/// 获取Proj的位置的IGetValue
		/// </summary>
		public static IGetValue<Vector2> GetProjCenter(int id, Vector2 Offset = default)
		{
			return (Get<Vector2>)(() => Main.projectile[id].Center + Offset);
		}
		/// <summary>
		/// 获取Proj的位置的IGetValue
		/// </summary>
		public static IGetValue<Vector2> GetProjCenter(Projectile projectile, Vector2 Offset = default)
		{
			return (Get<Vector2>)(() => projectile.Center + Offset);
		}
		/// <summary>
		/// 获取NPC的目标位置的IGetValue
		/// </summary>
		public static IGetValue<Vector2> GetNPCTargetCenter(int id, IGetValue<Vector2> Default, Vector2 Offset = default)
		{
			return (Get<Vector2>)(() =>
			{
				UnifiedTarget target = new UnifiedTarget() { NPCTarget = Main.npc[id].target };
				if (target.IsPlayer) return target.player.Center + Offset;
				else if (target.IsNPC) return target.npc.Center + Offset;
				else return Default.Value;
			});
		}
		/// <summary>
		/// 获取NPC的目标位置的IGetValue
		/// </summary>
		public static IGetValue<Vector2> GetNPCTargetCenter(NPC npc, IGetValue<Vector2> Default, Vector2 Offset = default)
		{
			return (Get<Vector2>)(() =>
			{
				UnifiedTarget target = new UnifiedTarget() { NPCTarget = npc.target };
				if (target.IsPlayer) return target.player.Center + Offset;
				else if (target.IsNPC) return target.npc.Center + Offset;
				else return Default.Value;
			});
		}
		/// <summary>
		/// 获取entity的位置的IGetValue
		/// </summary>
		public static IGetValue<Vector2> GetEntityCenter(Entity entity, Vector2 Offset = default)
		{
			return (Get<Vector2>)(() => entity.Center + Offset);
		}
		/// <summary>
		/// 获取目标位置的IGetValue
		/// </summary>
		public static IGetValue<Vector2> GetNPCTargetCenter(UnifiedTarget target, IGetValue<Vector2> Default, Vector2 Offset = default)
		{
			if (!target.IsNull) return GetEntityCenter(target.entity);
			else return null;
		}
		/// <summary>
		/// 获取表示NPC的目标的UnifiedTarget
		/// </summary>
		public static IGetSetValue<UnifiedTarget> GetNPCTarget(NPC npc) => new RefByDelegate<UnifiedTarget>(
			() => new UnifiedTarget() { NPCTarget = npc.target },
			(v) => npc.target = v.NPCTarget
			);
		/// <summary>
		/// 获取表示NPC的目标的UnifiedTarget
		/// </summary>
		public static IGetSetValue<UnifiedTarget> GetNPCTarget(int id) => new RefByDelegate<UnifiedTarget>(
			() => new UnifiedTarget() { NPCTarget = Main.npc[id].target },
			(v) => Main.npc[id].target = v.NPCTarget
			);
		/// <summary>
		/// 设置Vector2的长度
		/// </summary>
		public static Vector2 SetLength(this Vector2 vector, float Length)
		{
			return vector *= (Length / vector.Length());
		}
		/// <summary>
		/// 保持entity在世界中（否则会直接被active=false或报错）
		/// </summary>
		/// <param name="entity"></param>
		public static void SetInWorld(Entity entity) {

			if (entity.position.Y + entity.velocity.Y < 16)
				entity.position.Y = 16 - entity.velocity.Y;

			if (entity.position.X + entity.velocity.X < 16)
				entity.position.X = 16 - entity.velocity.X;

			if (entity.position.Y+entity.height + entity.velocity.Y >Main.maxTilesY*16- 16)
				entity.position.Y = Main.maxTilesY * 16 - 16 - entity.velocity.Y - entity.height;

			if (entity.position.X + entity.width + entity.velocity.X > Main.maxTilesX * 16 - 16)
				entity.position.X = Main.maxTilesX * 16 - 16 - entity.velocity.X -entity.width;
		}
		/// <summary>
		/// 从相对位置到世界位置
		/// </summary>
		public static Vector2 OffsetToWorld(this Vector2 offset, Vector2 center, float rotation, int direction) { 
			return (offset * new Vector2(direction, 1)).RotatedBy(rotation) + center;
		}
		/// <summary>
		/// 从相对entity的中心的位置到世界位置
		/// </summary>
		public static Vector2 OffsetToWorld(this Vector2 offset, NPC entity) {
			return (offset * new Vector2(entity.direction, 1)).RotatedBy(entity.rotation) + entity.Center;
		}
		/// <summary>
		/// 从相对entity的中心的位置到世界位置
		/// </summary>
		public static Vector2 OffsetToWorld(this Vector2 offset, Projectile entity)
		{
			return (offset * new Vector2(entity.direction, 1)).RotatedBy(entity.rotation) + entity.Center;
		}
		/// <summary>
		/// 遍历枚举全部相邻两元素组成的二元组，包括尾和首
		/// 每个元素都会出现两次
		/// </summary>
		public static IEnumerable<(T,T)> EnumPairs<T>(this IEnumerable<T> ts) {
			List < T > l= ts.ToList();
			for (int i = 0; i < l.Count - 1; ++i) yield return (l[i],l[i+1]);
			yield return (l[l.Count - 1], l[0]);
		}
	}
}
