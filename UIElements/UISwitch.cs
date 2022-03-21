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
using static System.Net.Mime.MediaTypeNames;

namespace XxDefinitions.UIElements
{
	public class UISwitch: UIImageBottomEX, IGetValue<bool>,ISetValue<bool>,IGetSetValue<bool>
	{
		public static StaticRefWithFunc<Texture2D> BottomTexture = new StaticRefWithFunc<Texture2D>(()=> Terraria.Graphics.TextureManager.Load("Images/UI/Settings_Toggle"));
		public static Texture2DCutted BottomTexture0 => new Texture2DCutted(BottomTexture.Value, new Rectangle(0, 0, 13, 13), new Vector2(0, 0));
		public static Texture2DCutted BottomTexture1 => new Texture2DCutted(BottomTexture.Value, new Rectangle(15, 0, 13, 13), new Vector2(0, 0));
		public IGetSetValue<bool> value;
		public bool Value {
			get => value.Value;
			set => this.value.Value = value;
		}
		public UISwitch(IGetSetValue<bool> value,IGetValue<string> HoverString):base(new GetByDelegate<Texture2DCutted>(()=>value.Value? BottomTexture1: BottomTexture0), HoverString) {
			this.value = value;
			Top.Set(0,0f);
			Left.Set(0, 0f);
			Width.Set(14, 0f);
			Height.Set(14, 0f);
		}
		public void SetCenter(Vector2 center,Vector2 precent) {
			Top.Set(center.Y - Height.Pixels / 2, precent.Y);
			Left.Set(center.X - Width.Pixels / 2, precent.X);
		}
		public override void Click(UIMouseEvent evt)
		{
			Value = !Value;
		}

	}

	public class UINamedSwitch :UIElement
	{
		public UISwitch Switch;
		public UIText Text;
		public string Name;
		public UINamedSwitch(UISwitch Switch,string Name) {
			this.Switch = Switch;
			this.Name = Name;
			Text = new UIText(Name);
			Text.Top.Set(0, 0f);
			Text.Left.Set(14, 0f);
			Text.Width.Set(0, 1f);
			Text.Height.Set(-14, 1f);
		}
		public UINamedSwitch(UISwitch Switch, UIText Text)
		{
			this.Switch = Switch;
			this.Text = Text;
			Name = Text.Text;
		}
		public override void OnInitialize()
		{
			Append(Switch);
			Append(Text);
		}
	}
}
