using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using RimWorld;
using UnityEngine;
using Verse;
using System;
using RimWorld.Planet;

namespace WVC_WorkModes.Odyssey
{

	[StaticConstructorOnStartup]
	public static class WVC_WorkModesOdyssey_Main
	{
		static WVC_WorkModesOdyssey_Main()
		{
			new Harmony("wvc.sergkart.biotech.moremechanoidsworkmodes").PatchAll();
		}
	}

	[HarmonyPatch(typeof(Gravship), "TransferZones")]
	public static class Patch_Gravship_TransferZones
	{
		public static void Postfix(Gravship __instance, Map oldMap, IntVec3 origin, HashSet<IntVec3> engineFloors)
        {
            try
			{
				//if (__instance.AllComps.NullOrEmpty())
				//{
				//	__instance.PostMake();
				//	Log.Warning("Gravship comps were not initialized, trying to initialize them.");
				//}
				__instance.GetComponent<WorldObjectComp_Zones>().TransferZones(oldMap, origin, engineFloors);
            }
            catch
            {
				Log.Warning("Failed save zones. Trying use backup method.");
				try
				{
					WorldObjectComp_Zones.backupSavedZones = new();
					WorldObjectComp_Zones.SaveZonesFromMap(oldMap, origin, engineFloors, __instance, WorldObjectComp_Zones.backupSavedZones);
				}
				catch
				{
					Log.Error("Failed save zones anyway. Thanks for broken gravship comps Ludeon.");
				}
			}
        }
    }

	[HarmonyPatch(typeof(GravshipPlacementUtility), "CopyZonesIntoMap")]
	public static class Patch_GravshipPlacementUtility_CopyZonesIntoMap
	{
		public static void Postfix(Gravship gravship, Map map, IntVec3 root)
		{
			try
			{
				bool useBackUp = false;
				if (!WorldObjectComp_Zones.backupSavedZones.NullOrEmpty())
                {
					useBackUp = true;
				}
				if (useBackUp)
				{
					WorldObjectComp_Zones.CopyZonesIntoMap(WorldObjectComp_Zones.backupSavedZones, gravship, map, root);
					WorldObjectComp_Zones.backupSavedZones = null;
				}
				else
                {
					foreach (WorldObjectComp objectComp in gravship.AllComps)
					{
						if (objectComp is WorldObjectComp_Zones zones)
						{
							//Log.Error("1");
							zones.CopyZonesIntoMap(gravship, map, root);
						}
					}
				}
			}
			catch
			{
				Log.Error("Failed load zones.");
			}
		}
	}

}
