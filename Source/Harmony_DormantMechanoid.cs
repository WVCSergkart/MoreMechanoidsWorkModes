using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace WVC
{
	namespace HarmonyPatches
	{

		[HarmonyPatch(typeof(GenHostility), "IsActiveThreatTo")]
		public static class Patch_GenHostility_IsActiveThreatTo
		{

			[HarmonyPrefix]
			public static bool Prefix(ref bool __result, ref IAttackTarget target)
			{
				Pawn pawn = target.Thing as Pawn;
				Log.Error(pawn.Name + " is target.");
				if (pawn.IsColonyMechPlayerControlled && (pawn.needs.energy.IsSelfShutdown || pawn.needs.energy.IsLowEnergySelfShutdown))
				{
					Log.Error(pawn.Name + " is dormant colony mech.");
					__result = false;
					return false;
				}
				return true;
			}
		}

	}

}
