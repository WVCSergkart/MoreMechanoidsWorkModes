using HarmonyLib;
using RimWorld;
using Verse;
using RimWorld.Planet;

namespace WVC_WorkModes.Odyssey
{
	[HarmonyPatch(typeof(GravshipPlacementUtility), "CopyZonesIntoMap")]
	public static class Patch_GravshipPlacementUtility_CopyZonesIntoMap
	{
		public static void Postfix(Gravship gravship, Map map, IntVec3 root)
		{
			try
			{
				//bool useBackUp = false;
				//if (!StaticCollectionsClass.backupSavedZones.NullOrEmpty())
				//{
				//	useBackUp = true;
				//}
				//if (useBackUp)
				//{
				//}
				//else
				//{
				//	foreach (WorldObjectComp objectComp in gravship.AllComps)
				//	{
				//		if (objectComp is WorldObjectComp_Zones zones)
				//		{
				//			//Log.Error("1");
				//			zones.CopyZonesIntoMap(gravship, map, root);
				//		}
				//	}
				//}
				ShutdownUtility.CopyZonesIntoMap(ref StaticCollectionsClass.backupSavedZones, gravship, map, root);
			}
			catch
			{
				Log.Error("Failed load zones.");
			}
		}
	}

}
