using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace WVC_WorkModes
{

	[Obsolete]
	public class Hediff_DormantMode : HediffWithComps
	{

		private int cachedBandwidthCost;

		private HediffStage curStage;

		public override HediffStage CurStage
		{
			get
			{
				if (curStage == null)
				{
					StatModifier statModifier = new()
					{
						stat = StatDefOf.BandwidthCost,
						value = cachedBandwidthCost * 0.01f
					};
					curStage = new HediffStage
					{
						statFactors = new List<StatModifier> { statModifier }
					};
				}
				return curStage;
			}
		}

		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			RecacheBand();
		}

		public void RecacheBand()
		{
			curStage = null;
			cachedBandwidthCost = WVC_MMWM.settings.mechBandwithCostInDormantMode;
			pawn.GetOverseer()?.mechanitor?.Notify_BandwidthChanged();
		}

		public override void PostRemoved()
		{
			base.PostRemoved();
			RecacheBand();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref cachedBandwidthCost, "cachedBandwidthCost", 0);
		}
	}


}
