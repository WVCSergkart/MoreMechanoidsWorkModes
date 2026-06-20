using RimWorld;
using Verse;

namespace WVC_WorkModes
{
	//public class MainButtonDef_WithInit : RimWorld.MainButtonDef
	//{

	//	public PawnTableDef pawnTableDef;

	//	public override void ResolveReferences()
	//	{
	//		base.ResolveReferences();
	//		//if (WVC_MMWM.settings.enableMechsWorkTab)
	//		//{
	//		//}
	//		if (pawnTableDef != null)
	//		{
	//			HarmonyUtility.Init(pawnTableDef);
	//		}
	//	}
	//}

	[StaticConstructorOnStartup]
	public static class PostInitializationOdyssey
	{
		static PostInitializationOdyssey()
		{
			if (WVC_MMWM.settings.enableMechsWorkTab)
			{
				MechsWorkTabUtility.InitPawnTable();
				WVC_MMWM.SetMechsTab();
			}
		}

	}

}
