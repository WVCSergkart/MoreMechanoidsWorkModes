using System.Collections.Generic;
using Verse;

namespace WVC_WorkModes
{

    public class WorkModeResearchRequirementDef : Def
	{
		// public MechWorkModeDef workMode;

		public List<MechWorkModeDef> workModes;

		public List<ResearchProjectDef> researchPrerequisites;

	}

	public class WorkModeExtension_SmartEscort : DefModExtension
	{
		public bool hideAssignGizmo = true;
	}

}
