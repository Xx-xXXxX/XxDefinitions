using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace XxDefinitions.Projectiles.ProjBehaviors
{
	/// <summary>
	/// 复制VanillaAI
	/// 用 CopyProjVanillaAI.ai 和 CopyProjVanillaAI.localAI（Update时替换与还原projectile.ai ...）
	/// </summary>
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	public class CopyProjVanillaAI : ModProjBehavior<ModProjectile>
	{
		//	int type = npc.type;
		//	bool num = npc.modNPC != null && npc.modNPC.aiType > 0;
		//			if (num)
		//			{
		//				npc.type = npc.modNPC.aiType;
		//			}
		//	npc.VanillaAI();
		//			if (num)
		//			{
		//				npc.type = type;
		//			}
		public const int aiSize = 2;
		public const int localAISize = 2;
		public bool netUpdate;
		public override bool NetUpdate => netUpdate;
		public readonly int type;
		public readonly int aiStyle;
		public override string BehaviorName =>$"CopyNPCVanillaAI:{type}";
		public IGetSetValue<float>[] ai;
		public IGetSetValue<float>[] localAI;
		public CopyProjVanillaAI(ModProjectile modProjectile, int type,int aiStyle, IGetSetValue<float> ai0 = null, IGetSetValue<float> ai1=null,IGetSetValue<float> lovalAI0 = null, IGetSetValue<float> lovalAI1 = null) : base(modProjectile) {
			this.type = type;
			this.aiStyle = aiStyle;
			this.ai = new IGetSetValue<float>[]{ ai0, ai1};
			this.localAI = new IGetSetValue<float>[] { lovalAI0, lovalAI1};
		}
		public CopyProjVanillaAI(ModProjectile modProjectile, int type,int aiStyle, IGetSetValue<float>[] ai = null,IGetSetValue<float>[] localAI=null) : base(modProjectile)
		{
			this.type = type;
			this.aiStyle = aiStyle;
			this.ai = ai;
			this.localAI = localAI;
		}
		public override void Update()
		{
			int rtype = projectile.type;
			int raiStyle = projectile.aiStyle;
			bool rnetUpdate = projectile.netUpdate;
			projectile.type = type;
			projectile.aiStyle = aiStyle;
			projectile.netUpdate = netUpdate;
			float[] npcai = new float[aiSize];
			float[] npclocalAI = new float[localAISize];
			if (ai != null) {
				for (int i = 0; i < aiSize; ++i) {
					if (ai[i] != null) {
						npcai[i] = projectile.ai[i];
						projectile.ai[i] = ai[i].Value;
					}
				}
			}
			if (localAI != null)
			{
				for (int i = 0; i < localAISize; ++i)
				{
					if (localAI[i] != null)
					{
						npclocalAI[i] = projectile.localAI[i];
						projectile.localAI[i] = localAI[i].Value;
					}
				}
			}
			projectile.VanillaAI();
			projectile.type = rtype;
			projectile.aiStyle = aiStyle;
			netUpdate = projectile.netUpdate;
			projectile.netUpdate = rnetUpdate | netUpdate;
			if (ai != null)
			{
				for (int i = 0; i < aiSize; ++i)
				{
					if (ai[i] != null)
					{
						ai[i].Value= projectile.ai[i];
						projectile.ai[i]= npcai[i];
					}
				}
			}
			if (localAI != null)
			{
				for (int i = 0; i < localAISize; ++i)
				{
					if (localAI[i] != null)
					{
						localAI[i].Value= projectile.localAI[i];
						projectile.localAI[i]= npclocalAI[i];
					}
				}
			}
		}
		public override void NetUpdateSend(BinaryWriter writer)
		{
			for (int i = 0; i < aiSize; ++i) writer.Write(ai[i].Value);
		}
		public override void NetUpdateReceive(BinaryReader reader)
		{
			for (int i = 0; i < aiSize; ++i) ai[i].Value = reader.ReadSingle();
		}
	}
}
