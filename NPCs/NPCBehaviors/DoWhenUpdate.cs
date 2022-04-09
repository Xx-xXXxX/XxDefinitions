using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace XxDefinitions.NPCs.NPCBehaviors
{
	/// <summary>
	/// 在相应时机执行
	/// </summary>
	public class DoWhenUpdate:ModNPCBehavior<ModNPC>
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override bool NetUpdate => false;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
		/// <summary>
		/// 执行时
		/// </summary>
		public Action UpdateAction;
		/// <summary>
		/// 激活时
		/// </summary>
		public Action ActivateAction;
		/// <summary>
		/// 暂停时
		/// </summary>
		public Action PauseAction;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public DoWhenUpdate(ModNPC modNPC, Action UpdateAction=null, Action ActivateAction=null, Action PauseAction=null) : base(modNPC) {

			this.UpdateAction = UpdateAction;
			this.ActivateAction = ActivateAction;
			this.PauseAction = PauseAction;
		}
		public override void OnActivate()
		{
			ActivateAction?.Invoke();
			base.OnActivate();
		}
		public override void Update()
		{
			UpdateAction?.Invoke();
			base.Update();
		}
		public override void OnPause()
		{
			PauseAction?.Invoke();
			base.OnPause();
		}
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
	}
}
