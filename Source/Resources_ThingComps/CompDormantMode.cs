using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace WVC_WorkModes
{

	[Obsolete]
	public class CompProperties_DormantMode : CompProperties
	{

		public int frequency = 2344;

		public List<HediffDef> hediffs;

		public CompProperties_DormantMode()
		{
			compClass = typeof(CompDormantMode);
		}

	}

	[Obsolete]
	public class CompDormantMode : ThingComp
	{

		public int nextTick = 131;

		public CompProperties_DormantMode Props => (CompProperties_DormantMode)props;

		public Need_MechEnergy mechEnergy;

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (respawningAfterLoad)
			{
				nextTick = Props.frequency;
			}
		}

		public override void CompTick()
		{
			Tick(1);
		}

		public override void CompTickRare()
		{
			Tick(150);
		}

		public override void CompTickLong()
		{
			Tick(1500);
		}

		public void Tick(int tick)
		{
			nextTick -= tick;
			if (nextTick > 0)
			{
				return;
			}
			nextTick = Props.frequency;
			if (parent.Faction != Faction.OfPlayer)
			{
				return;
			}
			Pawn pawn = parent as Pawn;
			if (mechEnergy == null)
			{
				mechEnergy = pawn?.needs?.energy;
			}
			if (mechEnergy == null || !mechEnergy.IsSelfShutdown)
			{
				return;
			}
			foreach (HediffDef hediff in Props.hediffs)
			{
				if (!pawn.health.hediffSet.HasHediff(hediff))
				{
					pawn.health.AddHediff(hediff);
				}
			}
		}

	}

}
