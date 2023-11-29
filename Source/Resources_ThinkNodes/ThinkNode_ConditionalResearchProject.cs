// RimWorld.JobGiver_GetEnergy_SelfShutdown
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace WVC_WorkModes
{

    public class ThinkNode_ConditionalResearchProjects : ThinkNode_Conditional
	{

		public List<ResearchProjectDef> allOfResearchProjects;

		public List<ResearchProjectDef> anyOfResearchProjects;

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

	public class ThinkNode_ConditionalWorkModesWithResearchRequirement : ThinkNode_Conditional
	{

		public MechWorkModeDef workMode;

		public string message = "WVC_WorkModes_ChangeModDefaultMessage";

		public List<WorkModeResearchRequirementDef> workModeResearchRequirementDefs;

		protected override bool Satisfied(Pawn pawn)
		{
			foreach (WorkModeResearchRequirementDef def in workModeResearchRequirementDefs)
			{
			// Log.Error("0");
				Pawn overseer = pawn.GetOverseer();
				List<MechWorkModeDef> workModes = def.workModes;
				MechanitorControlGroup controlGroup = overseer.mechanitor.GetControlGroup(pawn);
				// Log.Error("1");
				if (workModes.Contains(controlGroup.WorkMode))
				{
					// Log.Error("2");
					if (ThinkNode_ConditionalResearchProjects.AllProjectsFinished(def.researchPrerequisites))
					{
						// Log.Error("2.1");
						return true;
					}
					// Log.Error("3");
					Messages.Message(message.Translate(controlGroup.WorkMode.label.CapitalizeFirst(), workMode.label.CapitalizeFirst()), null, MessageTypeDefOf.RejectInput, historical: false);
					controlGroup.SetWorkMode(workMode);
				}
			}
			// Log.Error("4");
			return false;
		}

	}

}
