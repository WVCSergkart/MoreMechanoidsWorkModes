namespace WVC_WorkModes
{
	public class MainButtonWorker_ToggleMechTab : RimWorld.MainButtonWorker_ToggleMechTab
	{

		public static bool enabled = false;

		public override bool Disabled => !enabled || base.Disabled;

	}

}
