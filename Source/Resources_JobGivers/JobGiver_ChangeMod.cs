// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{
    public class JobGiver_ChangeMode : ThinkNode_JobGiver
	{

		public MechWorkModeDef workMode;

		public string message = "WVC_WorkModes_ChangeModDefaultMessage";

		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn overseer = pawn.GetOverseer();
			if (workMode != null)
			{
				MechanitorControlGroup controlGroup = overseer.mechanitor.GetControlGroup(pawn);
				Messages.Message(message.Translate(controlGroup.WorkMode.label.CapitalizeFirst(), workMode.label.CapitalizeFirst()), null, MessageTypeDefOf.RejectInput, historical: false);
				controlGroup.SetWorkMode(workMode);
			}
			return null;
		}

	}
}
