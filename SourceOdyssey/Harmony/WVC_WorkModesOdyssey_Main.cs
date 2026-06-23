using HarmonyLib;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;
using System;

namespace WVC_WorkModes.Odyssey
{

	[StaticConstructorOnStartup]
	public static class WVC_WorkModesOdyssey_Main
	{
		static WVC_WorkModesOdyssey_Main()
		{
			if (ModsConfig.OdysseyActive)
			{
				HarmonyUtility.ZonesPatch();
			}
			if (WVC_MMWM.settings.enableMechsWorkTab)
			{
				MechsWorkTabUtility.ApplyNullRefPatch();
			}
		}

	}

}
