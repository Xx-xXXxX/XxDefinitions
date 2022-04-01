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
	public class DoWhenUpdate:ModNPCBehavior<ModNPC>
	{
		public override string BehaviorName => "DoWhenUpdate";
		public override bool NetUpdate => false;
		public Action UpdateAction;
		public Action ActivateAction;
		public Action PauseAction;
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
	}
}
