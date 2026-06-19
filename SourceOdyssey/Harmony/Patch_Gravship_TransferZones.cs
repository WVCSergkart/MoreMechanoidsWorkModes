using HarmonyLib;
using System.Collections.Generic;
using Verse;
using RimWorld.Planet;

namespace WVC_WorkModes.Odyssey
{
	[HarmonyPatch(typeof(Gravship), "TransferZones")]
	public static class Patch_Gravship_TransferZones
	{
		public static void Postfix(Gravship __instance, Map oldMap, IntVec3 origin, HashSet<IntVec3> engineFloors)
        {
			//try
			//{
			//	__instance.GetComponent<WorldObjectComp_Zones>().TransferZones(oldMap, origin, engineFloors);
			//}
			//catch
			//{
			//	Log.Warning("Failed save zones. Trying use backup method.");
			//}
			try
			{
				ShutdownUtility.SaveZonesFromMap(oldMap, origin, engineFloors, __instance, ref StaticCollectionsClass.backupSavedZones);
			}
			catch
			{
				Log.Error("Failed save zones. Not your lucky day, Buddy.");
			}
		}
    }

}
