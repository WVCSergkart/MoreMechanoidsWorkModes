using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace WVC_WorkModes
{

	public class HediffCompProperties_RemoveIfIsNotDormant : HediffCompProperties
	{

		// public GeneDef geneDef;

		// public IntRange checkInterval = new (56720, 72032);

		public HediffCompProperties_RemoveIfIsNotDormant()
		{
			compClass = typeof(HediffComp_RemoveIfIsNotDormant);
		}

	}

	public class HediffComp_RemoveIfIsNotDormant : HediffComp
	{

		public Need_MechEnergy mechEnergy;

		public override bool CompShouldRemove
		{
			get
			{
				if (mechEnergy == null)
				{
					mechEnergy = Pawn?.needs?.energy;
				}
				if (mechEnergy == null || !mechEnergy.IsSelfShutdown)
				{
					return true;
				}
				return false;
			}
		}

		// public override void CompPostPostRemoved()
		// {
			// Pawn.GetOverseer()?.mechanitor?.Notify_BandwidthChanged();
		// }

	}


}
