// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{

    public class ThinkNode_ConditionalResearchProjects : ThinkNode_Conditional
	{

		public MechWorkModeDef workMode;

		public List<ResearchProjectDef> allOfResearchProjects;

		public List<ResearchProjectDef> anyOfResearchProjects;

		public string message = "WVC_WorkModes_ChangeModDefaultMessage";

		// public override ThinkNode DeepCopy(bool resolve = true)
		// {
			// ThinkNode_ConditionalResearchProjects obj = (ThinkNode_ConditionalResearchProjects)base.DeepCopy(resolve);
			// obj.invert = invert;
			// obj.allOfResearchProjects = allOfResearchProjects;
			// obj.anyOfResearchProjects = anyOfResearchProjects;
			// return obj;
		// }

		protected override bool Satisfied(Pawn pawn)
		{
			if (AllProjectsFinished(allOfResearchProjects) || AnyProjectFinished(anyOfResearchProjects))
			{
				return true;
			}
			Pawn overseer = pawn.GetOverseer();
			if (workMode != null)
			{
				MechanitorControlGroup controlGroup = overseer.mechanitor.GetControlGroup(pawn);
				Messages.Message(message.Translate(controlGroup.WorkMode.label.CapitalizeFirst(), workMode.label.CapitalizeFirst()), null, MessageTypeDefOf.RejectInput, historical: false);
				controlGroup.SetWorkMode(workMode);
			}
			return false;
		}

		public static bool AllProjectsFinished(List<ResearchProjectDef> researchProjects)
		{
			if (!researchProjects.NullOrEmpty())
			{
				for (int i = 0; i < researchProjects?.Count; i++)
				{
					if (researchProjects[i] == null || !researchProjects[i].IsFinished)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public static bool AnyProjectFinished(List<ResearchProjectDef> researchProjects)
		{
			if (!researchProjects.NullOrEmpty())
			{
				for (int i = 0; i < researchProjects?.Count; i++)
				{
					if (researchProjects[i] != null && researchProjects[i].IsFinished)
					{
						return true;
					}
				}
			}
			return false;
		}

	}

}
