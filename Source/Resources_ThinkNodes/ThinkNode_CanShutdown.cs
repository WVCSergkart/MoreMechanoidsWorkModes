// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{
	public class ThinkNode_CanShutdown : ThinkNode_Conditional
	{

		private static HashSet<Pawn> cachedNonShutdownMechs;
		public static HashSet<Pawn> NonShutdownMechs
		{
			get
			{
				if (cachedNonShutdownMechs == null)
				{
					HashSet<Pawn> pawns = new();
					foreach (Pawn mech in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive)
					{
						if (!mech.RaceProps.IsMechanoid)
						{
							continue;
						}
						if (mech.GetMechSettings()?.allowShutdown != false)
						{
							continue;
						}
						pawns.Add(mech);
					}
					cachedNonShutdownMechs = pawns;
				}
				return cachedNonShutdownMechs;
			}
		}

		public static void ResetCache()
		{
			cachedNonShutdownMechs = null;
		}

		public static bool CanShutdown(Pawn mech)
		{
			return !NonShutdownMechs.Contains(mech);
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return CanShutdown(pawn);
		}

	}

}
