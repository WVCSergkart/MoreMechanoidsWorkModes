using Verse;

namespace WVC_WorkModes
{

	[StaticConstructorOnStartup]
	public static class GraphicCache
	{

		//public static readonly CachedTexture Icon_MechsWorkTab = new("WVC/UI/Buttons/MechsWorkIcon");

		public static readonly CachedTexture Icon_CanShutdown_Yes = new("WVC/UI/WorkModes_General/CanShutdown_Yes");
		public static readonly CachedTexture Icon_CanShutdown_No = new("WVC/UI/WorkModes_General/CanShutdown_No");
		public static readonly CachedTexture Icon_RestrictShutdown = new("WVC/UI/WorkModes_General/Ui_RestrictZoneByGroup");
		public static readonly CachedTexture Icon_EscortTargetAssign = new("WVC/UI/WorkModes_General/EscortTargetAssign");
		public static readonly CachedTexture Icon_EscortTargetAssignReset = new("WVC/UI/WorkModes_General/EscortTargetAssignReset");

	}

}