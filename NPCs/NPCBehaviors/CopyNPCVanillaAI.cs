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
	/// 复制VanillaAI
	/// 用CopyNPCVanillaAI.ai,CopyNPCVanillaAI.localAI（Update时替换与还原npc.ai ...）
	/// </summary>
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
	public class CopyNPCVanillaAI:ModNPCBehavior<ModNPC>
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
		public const int aiSize = 4;
		public const int localAISize = 4;
		public readonly int type;
		public readonly int aiStyle;
		public override string BehaviorName =>$"CopyNPCVanillaAI:{type} {aiStyle}";
		public IGetSetValue<float>[] ai;
		public IGetSetValue<float>[] localAI;
		public CopyNPCVanillaAI(ModNPC modNPC, int type,int aiStyle, IGetSetValue<float> ai0 = null, IGetSetValue<float> ai1=null, IGetSetValue<float> ai2 = null, IGetSetValue<float> ai3 = null, IGetSetValue<float> lovalAI0 = null, IGetSetValue<float> lovalAI1 = null, IGetSetValue<float> lovalAI2 = null, IGetSetValue<float> lovalAI3 = null) : base(modNPC) {
			this.type = type;
			this.aiStyle = aiStyle;
			this.ai = new IGetSetValue<float>[]{ ai0, ai1, ai2, ai3 };
			this.localAI = new IGetSetValue<float>[] { lovalAI0, lovalAI1, lovalAI2, lovalAI3 };
		}
		public CopyNPCVanillaAI(ModNPC modNPC, int type, int aiStyle, IGetSetValue<float>[] ai = null,IGetSetValue<float>[] localAI=null) : base(modNPC)
		{
			this.type = type;
			this.aiStyle = aiStyle;
			this.ai = ai;
			this.localAI = localAI;
		}
		public override void Update()
		{

			int rtype = npc.type;
			int raiStyle = npc.aiStyle;
			npc.type = type;
			npc.aiStyle = aiStyle;
			float[] npcai = new float[aiSize];
			float[] npclocalAI = new float[localAISize];
			if (ai != null) {
				for (int i = 0; i < aiSize; ++i) {
					if (ai[i] != null) {
						npcai[i] = npc.ai[i];
						npc.ai[i] = ai[i].Value;
					}
				}
			}
			if (localAI != null)
			{
				for (int i = 0; i < localAISize; ++i)
				{
					if (localAI[i] != null)
					{
						npclocalAI[i] = npc.localAI[i];
						npc.localAI[i] = localAI[i].Value;
					}
				}
			}
			npc.VanillaAI();
			npc.type = rtype;
			npc.aiStyle = raiStyle;
			if (ai != null)
			{
				for (int i = 0; i < aiSize; ++i)
				{
					if (ai[i] != null)
					{
						ai[i].Value= npc.ai[i];
						npc.ai[i]= npcai[i];
					}
				}
			}
			if (localAI != null)
			{
				for (int i = 0; i < localAISize; ++i)
				{
					if (localAI[i] != null)
					{
						localAI[i].Value= npc.localAI[i];
						npc.localAI[i]= npclocalAI[i];
					}
				}
			}
		}
	}
}
