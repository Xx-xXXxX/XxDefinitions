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
namespace XxDefinitions.NPCs.NPCBehaviors.Physics
{
	/// <summary>
	/// 如果离地距离近则产生力
	/// 组件在产生力时执行
	/// </summary>
	public class VerticalGroundHover:PhysicsPartComponent<ModNPC>
	{
		public override string BehaviorName => "VerticalGroundHover";
		public override bool NetUpdateThis => false;
		public Vector2 Offset;
		public float force;
		public float height;
		public VerticalGroundHover(PhysicsEntity physicsEntity,Vector2 Offset,float force,float height) : base(physicsEntity.modNPC, physicsEntity) {
			this.Offset = Offset;
			this.force = force;
			this.height = height;
		}
		public bool ChildrenActivated=false;
		public override void OnActivate()
		{
			base.OnActivate();
			ChildrenActivated = true;
		}
		public override void OnPause()
		{
			base.OnPause();
			ChildrenActivated = false;
		}
		public override void Update()
		{
			Vector2 Pos = Offset.OffsetToWorld(npc);
			float ToGroundDistance = Utils.CalculateUtils.CanHitLineDistance(Pos,(float)Math.PI/2f,height);
			if (ToGroundDistance < height) {
				if (!ChildrenActivated)
				{
					foreach (var i in GetUsings()) i.TryActivate();
					ChildrenActivated = true;
				}
				AddForce(Pos,new Vector2(0,force*(ToGroundDistance-height)/height));
			}
			else
				if (ChildrenActivated)
				{
					foreach (var i in GetUsings()) i.TryPause();
					ChildrenActivated = false;
				}
			base.Update();
		}
	}
}
