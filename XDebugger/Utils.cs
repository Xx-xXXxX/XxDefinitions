using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ModLoader;

using XxDefinitions.XDebugger.UI;

using static XxDefinitions.XDebugger.Utils;

namespace XxDefinitions.XDebugger
{
	/// <summary>
	/// 方法
	/// </summary>
	public static class Utils
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		/// <summary>
		/// 用于绘画的方法
		/// </summary>
		public static class DrawUtils
		{
			/// <summary>
			/// 线
			/// </summary>
			public static Texture2D Line => ModContent.GetTexture("XxDefinitions/XDebugger/Line");
			public static void DrawLine(SpriteBatch spriteBatch, Vector2 From, float Distance, float Rotation, Color color, float LineWidth = 1)
			{
				//From -= Main.screenPosition;
				spriteBatch.Draw(Line, From, null, color, Rotation, new Vector2(0, 3), new Vector2(Distance / 64f, LineWidth / 8f), SpriteEffects.None, 0f);
			}
			public static void DrawLineBy(SpriteBatch spriteBatch, Vector2 From, Vector2 By, Color color, float LineWidth = 1)
			{
				DrawLine(spriteBatch, From, By.Length(), By.ToRotation(), color, LineWidth);
			}
			public static void DrawLineTo(SpriteBatch spriteBatch, Vector2 From, Vector2 To, Color color, float LineWidth = 1)
			{
				Vector2 By = To - From;
				DrawLine(spriteBatch, From, By.Length(), By.ToRotation(), color, LineWidth);
			}
			public static void DrawRect(SpriteBatch spriteBatch, Rectangle Rect, Color? color = null)
			{
				Color RealColor;
				if (color == null) RealColor = Color.GreenYellow;
				else RealColor = (Color)color;
				DrawLineTo(spriteBatch, Rect.TopLeft(), Rect.TopRight(), RealColor, 1);
				DrawLineTo(spriteBatch, Rect.TopLeft(), Rect.BottomLeft(), RealColor, 1);
				DrawLineTo(spriteBatch, Rect.BottomLeft(), Rect.BottomRight(), RealColor, 1);
				DrawLineTo(spriteBatch, Rect.TopRight(), Rect.BottomRight(), RealColor, 1);
			}
			public static void DrawCircle(SpriteBatch spriteBatch, Vector2 Pos, float r, int Accuracy = 60, float LineWidth = 1, Color? color = null)
			{
				Color RealColor;
				if (color == null) RealColor = Color.GreenYellow;
				else RealColor = (Color)color;
				Vector2 OldP = Pos + new Vector2(r, 0);
				Vector2 NewP;
				float Angle = 0;
				float Dangle = (float)Math.PI * 2 / Accuracy;
				for (int i = 0; i < Accuracy; ++i)
				{
					Angle += Dangle;
					NewP = Pos + new Vector2(r, 0).RotatedBy(Angle);
					DrawLineTo(spriteBatch, OldP, NewP, RealColor, LineWidth);
					OldP = NewP;
				}
			}
			public static void DrawEllipse(SpriteBatch spriteBatch, Vector2 Pos, float a, float b, int Accuracy = 60, float LineWidth = 1, Color? color = null)
			{
				Color RealColor;
				if (color == null) RealColor = Color.GreenYellow;
				else RealColor = (Color)color;
				Vector2 OldP = Pos + new Vector2(a, 0);
				Vector2 NewP;
				float Angle = 0;
				float Dangle = (float)Math.PI * 2 / Accuracy;
				for (int i = 0; i < Accuracy; ++i)
				{
					Angle += Dangle;
					NewP = Pos + new Vector2((float)(Math.Cos(Angle) * a), (float)(Math.Sin(Angle) * b));
					DrawLineTo(spriteBatch, OldP, NewP, RealColor, LineWidth);
					OldP = NewP;
				}
			}
			public static void DrawEllipse(SpriteBatch spriteBatch, Rectangle rect, int Accuracy = 60, float LineWidth = 1, Color? color = null)
			{
				DrawEllipse(spriteBatch, rect.Center.ToVector2(), rect.Width / 2f, rect.Height / 2f, Accuracy, LineWidth, color);
			}
		}
		/// <summary>
		/// 可用随时调用的添加绘画的方法
		/// 比如<code>
		/// public override void AI(){
		///		.AddDrawVector(npc.velocity,npc.Center);
		/// }
		/// </code>
		/// </summary>
		public static class AddDraw
		{
			public static void AddDrawString(string shownString, Vector2 pos)
			{
				UI.CustomDraw.DrawerList.Add(new UI.DrawString(shownString, pos));
			}
			public static void AddDrawString(string shownString, Vector2 pos, int time)
			{
				UI.CustomDraw.DrawerList.Add(new UI.DrawString(shownString, pos, time));
			}
			public static void AddDrawPoint(Vector2 pos, int DrawTime = 0)
			{
				UI.CustomDraw.DrawerList.Add(new UI.DrawString("\'", pos, DrawTime));
			}
			public static void AddDrawFunc(Action<SpriteBatch> DrawFunc)
			{
				UI.CustomDraw.DrawerList.Add(new CunsomDrawer((SpriteBatch spriteBatch, CunsomDrawer c) => { DrawFunc(spriteBatch); }));
			}
			public static void AddDrawFunc(Action<SpriteBatch> DrawFunc, int time)
			{
				UI.CustomDraw.DrawerList.Add(new CunsomDrawer((SpriteBatch spriteBatch, CunsomDrawer c) => { DrawFunc(spriteBatch); }, time));
			}
			public static void AddDrawLine(Vector2 FromR, float Distance, float Rotation, Color? color = null, float LineWidth = 1, int DrawTime = 0)
			{
				Color RealColor;
				if (color == null) RealColor = Color.GreenYellow;
				else RealColor = (Color)color;
				AddDrawFunc((spriteBatch) =>
				{
					Vector2 From = FromR - Main.screenPosition;
					DrawUtils.DrawLine(spriteBatch, From, Distance, Rotation, RealColor, LineWidth);
				}, DrawTime);
			}
			public static void AddDrawEllipse(Vector2 Or, float a, float b, Color? color = null, int Accuracy = 60, float LineWidth = 1, int DrawTime = 0)
			{
				Color RealColor;
				if (color == null) RealColor = Color.GreenYellow;
				else RealColor = (Color)color;
				AddDrawFunc((spriteBatch) =>
				{
					Vector2 O = Or - Main.screenPosition;
					DrawUtils.DrawEllipse(spriteBatch, O, a, b, Accuracy, LineWidth, color);
				}, DrawTime);
			}
			public static void AddDrawCircle(Vector2 Or, float r, Color? color = null, int Accuracy = 60, float LineWidth = 1, int DrawTime = 0)
			{
				Color RealColor;
				if (color == null) RealColor = Color.GreenYellow;
				else RealColor = (Color)color;
				AddDrawFunc((spriteBatch) =>
				{
					Vector2 O = Or - Main.screenPosition;
					DrawUtils.DrawCircle(spriteBatch, O, r, Accuracy, LineWidth, color);
				}, DrawTime);
			}
			public static void AddDrawLineTo(Vector2 From, Vector2 To, Color? color = null, float LineWidth = 1, int DrawTime = 0)
			{
				AddDrawLine(From, (To - From).Length(), (To - From).ToRotation(), color, LineWidth, DrawTime);
			}
			public static void AddDrawLineBy(Vector2 From, Vector2 By, Color? color = null, float LineWidth = 1, int DrawTime = 0)
			{
				AddDrawLine(From, By.Length(), By.ToRotation(), color, LineWidth, DrawTime);
			}
			public static void AddDrawRect(Rectangle Rect, Color? color = null, int DrawTime = 0)
			{
				Color RealColor;
				if (color == null) RealColor = Color.GreenYellow;
				else RealColor = (Color)color;
				AddDrawFunc((spriteBatch) =>
				{
					Rectangle Nrectangle = Rect;
					Nrectangle.X -= (int)Main.screenPosition.X;
					Nrectangle.Y -= (int)Main.screenPosition.Y;
					DrawUtils.DrawRect(spriteBatch, Nrectangle, color);
				}, DrawTime);
			}
			public static void AddDrawVector(Vector2 Pos, Vector2 Vec, Color? color = null, int DrawTime = 0)
			{
				Color RealColor;
				if (color == null) RealColor = Color.GreenYellow;
				else RealColor = (Color)color;
				float L = Math.Min(8, Vec.Length());
				AddDrawLineBy(Pos, Vec, RealColor, 1, DrawTime);
				AddDrawLineBy(Pos + Vec, new Vector2(L, 0).RotatedBy(Math.PI * 3 / 4 + Vec.ToRotation()), RealColor, 1, DrawTime);
				AddDrawLineBy(Pos + Vec, new Vector2(L, 0).RotatedBy(-Math.PI * 3 / 4 + Vec.ToRotation()), RealColor, 1, DrawTime);
			}
		}
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

		/// <summary>
		/// 加入对对应npc的DebugData
		/// </summary>
		public static void AddGetNPCDebugDataFunc(int type, Func<NPC, string> func)
		{
			if (NPCStringData.GetNPCDebugDataFuncs.ContainsKey(type))
				NPCStringData.GetNPCDebugDataFuncs[type] = func;
			else
				NPCStringData.GetNPCDebugDataFuncs.Add(type, func);
		}

		public static void DefNPCDebugInfo1(NPC nPC, List<string> tooltip)
		{
			tooltip.Add($"ai:{nPC.ai[0]} {nPC.ai[1]} {nPC.ai[2]} {nPC.ai[3]}\nlocalAi:{nPC.localAI[0]} {nPC.localAI[1]} {nPC.localAI[2]} {nPC.localAI[3]}\ntype:{nPC.type} name:{nPC.TypeName} aistyle:{nPC.aiStyle}");
		}

		public static XDebugger GetXDebugger(this Mod mod, String Name, bool Using = false)
		{
			return new XDebugger(mod, Name, Using);
		}
	}
	public partial class XDebugger
	{
		public void AddDrawString(string shownString, Vector2 pos)
		{
			UI.CustomDraw.DrawerList.Add(new UI.DrawString(shownString, pos));
		}
		public void AddDrawString(string shownString, Vector2 pos, int time)
		{
			if (Using)
				UI.CustomDraw.DrawerList.Add(new UI.DrawString(shownString, pos, time));
		}
		public void AddDrawPoint(Vector2 pos, int DrawTime = 0)
		{
			if (Using)
				UI.CustomDraw.DrawerList.Add(new UI.DrawString("\'", pos, DrawTime));
		}
		public void AddDrawFunc(Action<SpriteBatch> DrawFunc)
		{
			if (Using)
				UI.CustomDraw.DrawerList.Add(new CunsomDrawer((SpriteBatch spriteBatch, CunsomDrawer c) => { DrawFunc(spriteBatch); }));
		}
		public void AddDrawFunc(Action<SpriteBatch> DrawFunc, int time)
		{
			if (Using)
				UI.CustomDraw.DrawerList.Add(new CunsomDrawer((SpriteBatch spriteBatch, CunsomDrawer c) => { DrawFunc(spriteBatch); }, time));
		}
		public void AddDrawLine(Vector2 FromR, float Distance, float Rotation, Color? color = null, float LineWidth = 1, int DrawTime = 0)
		{
			if (Using)
			{
				Color RealColor;
				if (color == null) RealColor = Color.GreenYellow;
				else RealColor = (Color)color;
				AddDrawFunc((spriteBatch) =>
				{
					Vector2 From = FromR - Main.screenPosition;
					DrawUtils.DrawLine(spriteBatch, From, Distance, Rotation, RealColor, LineWidth);
				}, DrawTime);
			}
		}
		public void AddDrawEllipse(Vector2 Or, float a, float b, Color? color = null, int Accuracy = 60, float LineWidth = 1, int DrawTime = 0)
		{
			if (Using)
			{
				Color RealColor;
				if (color == null) RealColor = Color.GreenYellow;
				else RealColor = (Color)color;
				AddDrawFunc((spriteBatch) =>
				{
					Vector2 O = Or - Main.screenPosition;
					DrawUtils.DrawEllipse(spriteBatch, O, a, b, Accuracy, LineWidth, color);
				}, DrawTime);
			}
		}
		public void AddDrawCircle(Vector2 Or, float r, Color? color = null, int Accuracy = 60, float LineWidth = 1, int DrawTime = 0)
		{
			if (Using)
			{
				Color RealColor;
				if (color == null) RealColor = Color.GreenYellow;
				else RealColor = (Color)color;
				AddDrawFunc((spriteBatch) =>
				{
					Vector2 O = Or - Main.screenPosition;
					DrawUtils.DrawCircle(spriteBatch, O, r, Accuracy, LineWidth, color);
				}, DrawTime);
			}
		}
		public void AddDrawLineTo(Vector2 From, Vector2 To, Color? color = null, float LineWidth = 1, int DrawTime = 0)
		{
			if (Using)
				AddDrawLine(From, (To - From).Length(), (To - From).ToRotation(), color, LineWidth, DrawTime);
		}
		public void AddDrawLineBy(Vector2 From, Vector2 By, Color? color = null, float LineWidth = 1, int DrawTime = 0)
		{
			if (Using)
				AddDrawLine(From, By.Length(), By.ToRotation(), color, LineWidth, DrawTime);
		}
		public void AddDrawRect(Rectangle Rect, Color? color = null, int DrawTime = 0)
		{
			if (Using)
			{
				Color RealColor;
				if (color == null) RealColor = Color.GreenYellow;
				else RealColor = (Color)color;
				AddDrawFunc((spriteBatch) =>
				{
					Rectangle Nrectangle = Rect;
					Nrectangle.X -= (int)Main.screenPosition.X;
					Nrectangle.Y -= (int)Main.screenPosition.Y;
					DrawUtils.DrawRect(spriteBatch, Nrectangle, color);
				}, DrawTime);
			}
		}
		public void AddDrawVector(Vector2 Pos, Vector2 Vec, Color? color = null, int DrawTime = 0)
		{
			if (Using)
			{
				Color RealColor;
				if (color == null) RealColor = Color.GreenYellow;
				else RealColor = (Color)color;
				float L = Math.Min(8, Vec.Length());
				AddDrawLineBy(Pos, Vec, RealColor, 1, DrawTime);
				AddDrawLineBy(Pos + Vec, new Vector2(L, 0).RotatedBy(Math.PI * 3 / 4 + Vec.ToRotation()), RealColor, 1, DrawTime);
				AddDrawLineBy(Pos + Vec, new Vector2(L, 0).RotatedBy(-Math.PI * 3 / 4 + Vec.ToRotation()), RealColor, 1, DrawTime);
			}
		}

	}
}
