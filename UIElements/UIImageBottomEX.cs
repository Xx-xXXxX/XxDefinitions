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
using Terraria.UI;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
namespace XxDefinitions.UIElements
{
	public class UIImageBottomEX:UIElement
	{
		public IGetValue<Texture2DCutted> texture;
		public IGetValue<string> hoverString;
		public float visibilityActive = 1f;
		public float visibilityInactive = 0.4f;
		public UIImageBottomEX(IGetValue<Texture2DCutted> texture, IGetValue<string> hoverString=null) {
			this.texture = texture;
			this.hoverString = hoverString;
		}
		public virtual void DrawSelfVirtual(SpriteBatch spriteBatch) {
			texture.Value.Draw(spriteBatch, GetDimensions().Position(), Color.White * (base.IsMouseHovering ? visibilityActive : visibilityInactive), 0, 1f, SpriteEffects.None);
			if (IsMouseHovering && hoverString != null)
			{
				Main.hoverItemName = hoverString.Value;
			}
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			DrawSelfVirtual(spriteBatch);
		}
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
		}
		public void SetVisibility(float whenActive, float whenInactive)
		{
			visibilityActive = MathHelper.Clamp(whenActive, 0f, 1f);
			visibilityInactive = MathHelper.Clamp(whenInactive, 0f, 1f);
		}
	}
}
