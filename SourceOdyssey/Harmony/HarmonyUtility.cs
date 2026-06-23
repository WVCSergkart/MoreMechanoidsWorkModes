using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using Verse;

namespace WVC_WorkModes.Odyssey
{
	public static class HarmonyUtility
	{

		private static Harmony cachedHarmony;
		public static Harmony Harmony
		{
			get
			{
				if (cachedHarmony == null)
				{
					cachedHarmony = new Harmony("wvc.sergkart.biotech.moremechanoidsworkmodes");
				}
				return cachedHarmony;
			}
		}

		public static void ZonesPatch()
		{
			try
			{
				HarmonyUtility.Harmony.Patch(AccessTools.Method(typeof(Gravship), "TransferZones"), prefix: new HarmonyMethod(typeof(HarmonyUtility).GetMethod(nameof(HarmonyUtility.Patch_Gravship_TransferZones))));
				HarmonyUtility.Harmony.Patch(AccessTools.Method(typeof(GravshipPlacementUtility), "CopyZonesIntoMap"), prefix: new HarmonyMethod(typeof(HarmonyUtility).GetMethod(nameof(HarmonyUtility.Patch_GravshipPlacementUtility_CopyZonesIntoMap))));
			}
			catch (Exception arg)
			{
				Log.Warning("Failed patch gravship. Reason: " + arg.Message);
			}
		}

		public static void Patch_Gravship_TransferZones(Gravship __instance, Map oldMap, IntVec3 origin, HashSet<IntVec3> engineFloors)
		{
			try
			{
				ShutdownUtility.CopyZonesFromMap(oldMap, origin, engineFloors, __instance, ref StaticCollectionsClass.backupSavedZones);
			}
			catch
			{
				Log.Error("Failed copy zones. Not your lucky day, Buddy.");
			}
		}
		public static void Patch_GravshipPlacementUtility_CopyZonesIntoMap(Gravship gravship, Map map, IntVec3 root)
		{
			try
			{
				ShutdownUtility.PasteZonesIntoMap(ref StaticCollectionsClass.backupSavedZones, gravship, map, root);
			}
			catch
			{
				Log.Error("Failed paste zones.");
			}
		}

	}
}