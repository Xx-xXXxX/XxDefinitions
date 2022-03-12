using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using static XxDefinitions.XDebugger.UI.CustomDraw;

namespace XxDefinitions.XDebugger.UI
{
	/// <summary>
	/// 操作被添加的UI
	/// </summary>
	public class CustomDraw : UIState
	{

		/// <summary>
		/// 用于可添加的绘画的接口
		/// </summary>
		public interface IDrawer {
			/// <summary>
			/// 绘画函数
			/// </summary>
			void DrawFunc(SpriteBatch spriteBatch);
			/// <summary>
			/// 操作
			/// </summary>
			void AI();
			/// <summary>
			/// 是否结束
			/// </summary>
			/// <returns>true结束</returns>
			bool Quit();
		}
		/// <summary>
		/// 用于可添加的绘画的基类
		/// </summary>
		public abstract class Drawer<RealType>:IDrawer
		{
			/// <summary>
			/// 绘画函数
			/// </summary>
			public abstract void DrawFunc(SpriteBatch spriteBatch);
			/// <summary>
			/// 操作
			/// </summary>
			public abstract void AI();
			/// <summary>
			/// 是否结束
			/// </summary>
			/// <returns>true结束</returns>
			public abstract bool Quit();
		}
		internal static List<IDrawer> DrawerList=new List<IDrawer>();
		/// <summary>
		/// 
		/// </summary>
		public override void OnInitialize()
		{
		}
		internal static bool Visible = true;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="spriteBatch"></param>
		public override void Draw(SpriteBatch spriteBatch)
		{
			foreach (var n in DrawerList)
			{
				n.DrawFunc(spriteBatch);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			if (!Main.gamePaused)
			{
				DrawerList.RemoveAll((n) => n.Quit());
				foreach (var n in DrawerList)
				{
					n.AI();
				}
			}
		}
	}

	/// <summary>
	/// 用于可添加的绘画的类
	/// </summary>
	public class CunsomDrawer : Drawer<CunsomDrawer>
	{
		/// <summary>
		/// 绘画函数委派
		/// </summary>
		public delegate void DDrawF(SpriteBatch spriteBatch, CunsomDrawer d);
		/// <summary>
		/// 操作函数委派
		/// </summary>
		public delegate void DAI(CunsomDrawer d);
		/// <summary>
		/// 绘画函数
		/// </summary>
		public DDrawF dDrawF;
		/// <summary>
		/// 操作函数
		/// </summary>
		public DAI dAI;
		/// <summary>
		/// 剩余时间
		/// </summary>
		public double timeleft;
		/// <summary>
		/// 位置
		/// </summary>
		public Vector2 pos;
		/// <summary>
		/// 速度
		/// </summary>
		public Vector2 vel;
		/// <summary>
		/// 自定义参数
		/// </summary>
		public object customData;
		/// <summary>
		/// 绘画
		/// </summary>
		/// <param name="spriteBatch"></param>
		public override void DrawFunc(SpriteBatch spriteBatch)
		{
			dDrawF(spriteBatch, this);
		}
		/// <summary>
		/// 操作
		/// </summary>
		public override void AI()
		{
			if (dAI != null)
				dAI(this);
			else
				pos += vel;
			timeleft -= 1;
		}
		/// <summary>
		/// 结束
		/// </summary>
		/// <returns></returns>
		public override bool Quit()
		{
			return timeleft <=0;
		}
		/// <summary>
		/// 初始化
		/// </summary>
		public CunsomDrawer(DDrawF _DrawF, int _timeleft = 0, object _customData = null, Vector2 _pos = default, DAI _AI = null, Vector2 _vel = default)
		{
			dDrawF = _DrawF;
			timeleft = _timeleft;
			customData = _customData;
			pos = _pos;
			vel = _vel;
			dAI = _AI;
		}
	}
	/// <summary>
	/// 添加绘画字符串
	/// </summary>
	public class DrawString : Drawer<DrawString>
	{
		/// <summary>
		/// 绘画出的字符串
		/// </summary>
		public string ShownString;
		/// <summary>
		/// 位置
		/// </summary>
		public Vector2 Pos;
		/// <summary>
		/// 剩余时间
		/// </summary>
		public double timeleft;
		/// <summary>
		/// 操作
		/// </summary>
		public override void AI() { timeleft -= 1; }
		/// <summary>
		/// 初始化
		/// </summary>
		public DrawString(string shownString, Vector2 pos)
		{
			ShownString = shownString;
			Pos = pos;
			timeleft = 0;
		}
		/// <summary>
		/// 初始化
		/// </summary>
		public DrawString(string shownString, Vector2 pos,int time)
		{
			ShownString = shownString;
			Pos = pos;
			timeleft = time;
		}

		/// <summary>
		/// 画
		/// </summary>
		public override void DrawFunc(SpriteBatch spriteBatch)
		{
			Terraria.Utils.DrawBorderString(spriteBatch, ShownString, Pos - Main.screenPosition, Color.White);
		}
		/// <summary>
		/// 退出
		/// </summary>
		public override bool Quit()
		{
			return timeleft <=0;
		}
	}
}