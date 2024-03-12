// RimWorld.StatPart_Age
using RimWorld;
using Verse;

namespace WVC_WorkModes
{
	public class StatPart_DormantMode : StatPart
	{

		public StatDef stat;

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && req.Thing is Pawn pawn && ShutdownUtility.InShutdownMode(pawn))
			{
				val *= (WVC_MMWM.settings.mechBandwithCostInDormantMode * 0.01f);
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && req.Thing is Pawn pawn && ShutdownUtility.InShutdownMode(pawn))
			{
				return "WVC_WorkModes_DormantModeStatPart".Translate(WVC_MMWM.settings.mechBandwithCostInDormantMode);
			}
			return null;
		}

	}
}
