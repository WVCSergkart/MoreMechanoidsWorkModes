using RimWorld;

namespace WVC_WorkModes
{
	public class MainButtonDef : RimWorld.MainButtonDef
	{

		public PawnTableDef pawnTableDef;

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			//if (WVC_MMWM.settings.enableMechsWorkTab)
			//{
			//}
			if (pawnTableDef != null)
			{
				HarmonyUtility.Init(pawnTableDef);
			}
		}
	}

}
