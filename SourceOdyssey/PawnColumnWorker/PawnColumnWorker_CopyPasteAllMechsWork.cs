using RimWorld;
using Verse;
using static RimWorld.MechClusterSketch;

namespace WVC_WorkModes
{
	public class PawnColumnWorker_CopyPasteAllMechsWork : PawnColumnWorker_CopyPaste
	{

		protected override bool AnythingInClipboard => wmClipboard != null;

		public static MechSettings wmClipboard;
		
		public class MechSettings
		{

			public bool restrictZoneByGroup = false;
			public Pawn escortTarget = null;
			public bool allowShutdown = true;

			public MechSettings()
			{

			}

			public MechSettings(Pawn mech)
			{
				CompMechSettings compMechSettings = mech.GetMechSettings();
				restrictZoneByGroup = compMechSettings.restrictZoneByGroup;
				escortTarget = compMechSettings.escortTarget;
				allowShutdown = compMechSettings.allowShutdown;
			}

			public void PasteSettings(Pawn mech)
			{
				CompMechSettings compMechSettings = mech.GetMechSettings();
				compMechSettings.allowShutdown = wmClipboard.allowShutdown;
				compMechSettings.escortTarget = wmClipboard.escortTarget;
				compMechSettings.restrictZoneByGroup = wmClipboard.restrictZoneByGroup;
			}

		}

		protected override void CopyFrom(Pawn p)
		{
			wmClipboard = new(p);
		}

		protected override void PasteTo(Pawn p)
		{
			wmClipboard?.PasteSettings(p);
		}

	}

}
