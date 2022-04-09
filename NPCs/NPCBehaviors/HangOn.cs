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

using XxDefinitions.Behavior;
namespace XxDefinitions.NPCs.NPCBehaviors
{
	/// <summary>
	/// 保持其在NPC的相对位置上
	/// 在entity无效（!active）时自动暂停
	/// </summary>
	public class HangOnNPC:ModNPCBehavior<ModNPC>
	{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public override bool NetUpdate =>false;
		/// <summary>
		/// 相对中心的位置
		/// </summary>
		public Vector2 Offset;
		/// <summary>
		/// 所在的npc
		/// </summary>
		public NPC entity;
		public HangOnNPC(ModNPC modNPC, NPC target, Vector2 Offset) : base(modNPC) {
			entity = target;this.Offset = Offset;
		}
		public override void Update()
		{
			base.Update();
			if (!entity.active) { 
				this.TryPause();
				if (!Active) return;
			}
			npc.Center = entity.Center + (Offset * new Vector2(entity.direction, 1)).RotatedBy(entity.rotation);
		}
		public override void OnActivate()
		{
			Update();
		}
	}
	/// <summary>
	/// 保持其在Proj的相对位置上
	/// 在entity无效（!active）时自动暂停
	/// </summary>
	public class HangOnProj : ModNPCBehavior<ModNPC>
	{
		public override bool NetUpdate => false;
		/// <summary>
		/// 相对中心的位置
		/// </summary>
		public Vector2 Offset;
		/// <summary>
		/// 所在的npc
		/// </summary>
		public Projectile entity;
		public HangOnProj(ModNPC modNPC, Projectile target, Vector2 Offset) : base(modNPC)
		{
			entity = target; this.Offset = Offset;
		}
		public override void Update()
		{
			base.Update();
			if (!entity.active)
			{
				this.TryPause();
				if (!Active) return;
			}
			npc.Center = entity.Center + (Offset * new Vector2(entity.direction, 1)).RotatedBy(entity.rotation);
		}
		public override void OnActivate()
		{
			Update();
		}
	}
}
