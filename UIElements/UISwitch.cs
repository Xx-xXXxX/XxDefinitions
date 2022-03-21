﻿using Microsoft.Xna.Framework;
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
	/// <summary>
	/// 开关
	/// </summary>
	public class UISwitch: UIImageBottomEX, IGetValue<bool>,ISetValue<bool>,IGetSetValue<bool>
	{
		/// <summary>
		/// 开关图形
		/// </summary>
		public static StaticRefWithFunc<Texture2D> BottomTexture = new StaticRefWithFunc<Texture2D>(()=> Terraria.Graphics.TextureManager.Load("Images/UI/Settings_Toggle"));
		/// <summary>
		/// 开关图形
		/// </summary>
		public static Texture2DCutted BottomTexture0 => new Texture2DCutted(BottomTexture.Value, new Rectangle(0, 0, 13, 13), new Vector2(0, 0));
		/// <summary>
		/// 开关图形
		/// </summary>
		public static Texture2DCutted BottomTexture1 => new Texture2DCutted(BottomTexture.Value, new Rectangle(15, 0, 13, 13), new Vector2(0, 0));
		/// <summary>
		/// 被控制的值的IGetSetValue
		/// </summary>
		public IGetSetValue<bool> value;
		/// <summary>
		/// 被控制的值
		/// </summary>
		public bool Value {
			get => value.Value;
			set => this.value.Value = value;
		}
		/// <summary>
		/// 
		/// </summary>
		public UISwitch(IGetSetValue<bool> value,IGetValue<string> HoverString):base(new GetByDelegate<Texture2DCutted>(()=>value.Value? BottomTexture1: BottomTexture0), HoverString) {
			this.value = value;
			Top.Set(0,0f);
			Left.Set(0, 0f);
			Width.Set(14, 0f);
			Height.Set(14, 0f);
		}
		/// <summary>
		/// 设置开关中心
		/// </summary>
		public void SetCenter(Vector2 center,Vector2 precent) {
			Top.Set(center.Y - Height.Pixels / 2, precent.Y);
			Left.Set(center.X - Width.Pixels / 2, precent.X);
		}
		/// <summary>
		/// 按下
		/// </summary>
		public override void Click(UIMouseEvent evt)
		{
			Value = !Value;
		}

	}
	/// <summary>
	/// 在开关右边显示文本
	/// </summary>
	public class UINamedSwitch :UIElement
	{
		/// <summary>
		/// 开关
		/// </summary>
		public UISwitch Switch;
		/// <summary>
		/// 文本
		/// </summary>
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
