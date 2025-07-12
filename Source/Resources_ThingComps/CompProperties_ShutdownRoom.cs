using Verse;

namespace WVC_WorkModes
{
    public class CompProperties_ShutdownRoom : CompProperties
	{

		//public string uiIconAssign = "WVC/UI/WorkModes_General/EscortTargetAssign";

		//public string uiIconReset = "WVC/UI/WorkModes_General/EscortTargetAssignReset";

		// public float uiIconSize = 1.0f;

		public CompProperties_ShutdownRoom()
		{
			compClass = typeof(CompShutdownRoom);
		}
	}

	public class CompShutdownRoom : ThingComp
	{



	}

}
