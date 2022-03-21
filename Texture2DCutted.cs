using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace XxDefinitions
{
	/// <summary>
	/// 截取的Texture2D
	/// </summary>
	public class Texture2DCutted
	{
		/// <summary>
		/// 被截取的Texture2D
		/// </summary>
		public Texture2D texture;
		/// <summary>
		/// 截取的区域
		/// </summary>
		public Rectangle rectangle;
		/// <summary>
		/// 中心位置
		/// </summary>
		public Vector2 origin;
		/// <summary>
		/// 截取Texture2D
		/// </summary>
		/// <param name="texture">被截取的Texture2D</param>
		/// <param name="rectangle">截取的区域</param>
		/// <param name="origin">中心位置</param>
		public Texture2DCutted(Texture2D texture, Rectangle rectangle, Vector2 origin) {
			this.texture = texture;
			this.rectangle = rectangle;
			this.origin = origin;
		}
		/// <summary>
		/// 替代SpriteBatch中的对应参数绘图
		/// </summary>
		public void Draw(SpriteBatch sb,Vector2 position,Color color,float rotation,float scale,SpriteEffects spriteEffects, float layerDepth = 0f) 
		{
			sb.Draw(texture,position,rectangle,color,rotation,origin, scale, spriteEffects, layerDepth);
		}
		/// <summary>
		/// 替代SpriteBatch中的对应参数绘图
		/// </summary>
		public void Draw(SpriteBatch sb, Vector2 position, Color color, float rotation, Vector2 scale, SpriteEffects spriteEffects, float layerDepth = 0f)
		{
			sb.Draw(texture, position, rectangle, color, rotation, origin, scale, spriteEffects, layerDepth);

		}
		/// <summary>
		/// 替代SpriteBatch中的对应参数绘图
		/// </summary>
		public void Draw(SpriteBatch sb, Rectangle destinationRectangle, Color color, float rotation, SpriteEffects effects, float layerDepth=0f) {
			sb.Draw(texture,destinationRectangle,rectangle,color,rotation,origin,effects,layerDepth);	
		}
	}
}
