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
	public class CustomDraw : UIState
	{
		public interface IDrawer {
			void DrawFunc(SpriteBatch spriteBatch);
			void AI();
			bool Quit();
		}
		public abstract class Drawer<RealType>:IDrawer { 
			public abstract void DrawFunc(SpriteBatch spriteBatch);
			public abstract void AI();
			public abstract bool Quit();
		}
		public static List<IDrawer> DrawerList=new List<IDrawer>();
		public override void OnInitialize()
		{
		}
		public static bool Visible = true;
		public override void Draw(SpriteBatch spriteBatch)
		{
			foreach (var n in DrawerList)
			{
				n.DrawFunc(spriteBatch);
			}
		}
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
	public class CunsomDrawer : Drawer<CunsomDrawer>
	{
		public delegate void DDrawF(SpriteBatch spriteBatch, CunsomDrawer d);
		public delegate void DAI(CunsomDrawer d);
		public DDrawF dDrawF;
		public DAI dAI;
		public double timeleft;
		public Vector2 pos;
		public Vector2 vel;
		public object customData;
		public override void DrawFunc(SpriteBatch spriteBatch)
		{
			dDrawF(spriteBatch, this);
		}
		public override void AI()
		{
			if (dAI != null)
				dAI(this);
			else
				pos += vel;
			timeleft -= 1;
		}
		public override bool Quit()
		{
			return timeleft <=0;
		}
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
	public class DrawString : Drawer<DrawString>
	{
		public string ShownString;
		public Vector2 Pos;
		public double timeleft;
		public override void AI() { timeleft -= 1; }
		public DrawString(string shownString, Vector2 pos)
		{
			ShownString = shownString;
			Pos = pos;
			timeleft = 0;
		}
		public DrawString(string shownString, Vector2 pos,int time)
		{
			ShownString = shownString;
			Pos = pos;
			timeleft = time;
		}
		public override void DrawFunc(SpriteBatch spriteBatch)
		{
			Terraria.Utils.DrawBorderString(spriteBatch, ShownString, Pos - Main.screenPosition, Color.White);
		}
		public override bool Quit()
		{
			return timeleft <=0;
		}
	}
}