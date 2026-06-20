namespace WVC_WorkModes
{

	public class MainButtonWorker_ToggleMechTab : RimWorld.MainButtonWorker_ToggleMechTab
	{

		public static bool enabled = false;

		public override bool Disabled
		{
			get
			{
				if (MechsWorkTabUtility.MechTabEnabled)
				{
					return base.Disabled;
				}
				return true;
			}
		}

		public override bool Visible
		{
			get
			{
				if (def.buttonVisible)
				{
					return base.Visible;
				}
				return false;
			}
		}
	}

}
