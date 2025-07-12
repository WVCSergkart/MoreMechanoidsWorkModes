using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace WVC_WorkModes
{

    [Obsolete]
    public class CompProperties_SmartEscort : CompProperties
	{

		public string uiIconAssign = "WVC/UI/WorkModes_General/EscortTargetAssign";

		public string uiIconReset = "WVC/UI/WorkModes_General/EscortTargetAssignReset";

		// public float uiIconSize = 1.0f;

		public CompProperties_SmartEscort()
		{
			compClass = typeof(CompSmartEscort);
		}
	}

	[Obsolete]
	public class CompSmartEscort : ThingComp
	{



	}

}
