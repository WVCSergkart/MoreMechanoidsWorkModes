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

		private static Harmony cachedHarmony;
		public static Harmony Harmony
		{
			get
			{
				if (cachedHarmony == null)
				{
					cachedHarmony = new Harmony("wvc.sergkart.races.biotech");
				}
				return cachedHarmony;
			}
		}

		static WVC_WorkModesOdyssey_Main()
		{
			Harmony.PatchAll();
		}

	}

}
