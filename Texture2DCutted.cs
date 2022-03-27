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
		/// 旋转，建议直角
		/// </summary>
		public float Rotation;
		/// <summary>
		/// 放缩
		/// </summary>
		public Vector2 Scale;
		/// <summary>
		/// 截取Texture2D
		/// </summary>
		/// <param name="texture">被截取的Texture2D</param>
		/// <param name="rectangle">截取的区域</param>
		/// <param name="origin">中心位置</param>
		/// <param name="Rotation">旋转，建议直角</param>
		/// <param name="Scale">放缩</param>
		public Texture2DCutted(Texture2D texture, Rectangle rectangle, Vector2 origin,float Rotation=0,Vector2 Scale=default) {
			this.texture = texture;
			this.rectangle = rectangle;
			this.origin = origin;
			this.Rotation = Rotation;
			if (Scale == default) Scale = Vector2.One;
			this.Scale = Scale;
		}
		/// <summary>
		/// 替代SpriteBatch中的对应参数绘图
		/// </summary>
		public void Draw(SpriteBatch sb,Vector2 position,Color color,float rotation,float scale,SpriteEffects spriteEffects, float layerDepth = 0f) 
		{
			sb.Draw(texture,position,rectangle,color,rotation+this.Rotation,origin, this.Scale*scale, spriteEffects, layerDepth);
		}
		/// <summary>
		/// 替代SpriteBatch中的对应参数绘图，scale只有在贴图是直角旋转时有效，使用RealScale()
		/// </summary>
		public void Draw(SpriteBatch sb, Vector2 position, Color color, float rotation, Vector2 scale, SpriteEffects spriteEffects, float layerDepth = 0f)
		{
			Vector2 RScale = RealScale(scale)*Scale;
			sb.Draw(texture, position, rectangle, color, rotation+this.Rotation, origin, RScale, spriteEffects, layerDepth);

		}
		/// <summary>
		/// 替代SpriteBatch中的对应参数绘图
		/// </summary>
		public void Draw(SpriteBatch sb, Rectangle destinationRectangle, Color color, float rotation, SpriteEffects effects, float layerDepth=0f) {
			sb.Draw(texture,destinationRectangle,rectangle,color,rotation+this.Rotation, origin,effects,layerDepth);
		}
		/// <summary>
		/// 实际放缩，只有在贴图是直角旋转时正常，否则返回(或许是)近似值
		/// </summary>
		public Vector2 RealScale(Vector2 scale) {
			if ((this.Rotation % ((float)Math.PI / 2)).InRange(0, 0.01f))
			{
				Vector2 Size = scale.RotatedBy(-Rotation);
				Size = new Vector2(Math.Abs(Size.X), Math.Abs(Size.Y));
				return Size;
			}
			else
			{
				Vector2 Size = scale;
				Size = new Vector2(
						Utils.Max((float)Math.Abs(Size.X * Math.Cos(-Rotation)), (float)Math.Abs(Size.Y * Math.Sin(-Rotation)), (float)Math.Abs(Size.X * Math.Cos(-Rotation) - Size.Y * Math.Sin(-Rotation))),
						Utils.Max((float)Math.Abs(Size.X * Math.Sin(-Rotation)), (float)Math.Abs(Size.Y * Math.Cos(-Rotation)), (float)Math.Abs(Size.X * Math.Sin(-Rotation) - Size.Y * Math.Cos(-Rotation)))
					);
				return Size;
			}
		}
		/// <summary>
		/// 实际长，只有在贴图是直角旋转时正常，否则返回(或许是)近似值
		/// </summary>
		public Vector2 RealSize()
		{
			if ((this.Rotation % ((float)Math.PI / 2)).InRange(0, 0.01f))
			{
				Vector2 Size = new Vector2(rectangle.Width * Scale.X, rectangle.Height * Scale.Y);
				Size.RotatedBy(Rotation);
				Size = new Vector2(Math.Abs(Size.X), Math.Abs(Size.Y));
				return Size;
			}
			else {
				Vector2 Size = new Vector2(rectangle.Width * Scale.X, rectangle.Height * Scale.Y);
				Size = new Vector2(
						Utils.Max((float)Math.Abs(Size.X*Math.Cos(Rotation)), (float)Math.Abs(Size.Y*Math.Sin(Rotation)),(float)Math.Abs(Size.X * Math.Cos(Rotation)- Size.Y * Math.Sin(Rotation))),
						Utils.Max((float)Math.Abs(Size.X * Math.Sin(Rotation)), (float)Math.Abs(Size.Y * Math.Cos(Rotation)), (float)Math.Abs(Size.X * Math.Sin(Rotation) - Size.Y * Math.Cos(Rotation)))
					);
				return Size;
			}
		}
	}
}
