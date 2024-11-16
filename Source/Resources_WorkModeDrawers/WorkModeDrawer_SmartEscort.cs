using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace WVC_WorkModes
{

	public class WorkModeDrawer_SmartEscort : WorkModeDrawer
	{
		protected override bool DrawIconAtTarget => false;

		public override void DrawControlGroupMouseOverExtra(MechanitorControlGroup group)
		{
			// GlobalTargetInfo targetForLine = GetTargetForLine(group);
			List<Pawn> mechsForReading = group.MechsForReading;
			Map currentMap = Find.CurrentMap;
			foreach (Pawn mech in mechsForReading)
			{
				GlobalTargetInfo targetForMech = GetTargetForMech(group, mech);
				if (!targetForMech.IsValid || targetForMech.Map != currentMap)
				{
					return;
				}
				Vector3 vector = targetForMech.Cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MoteOverhead);
				GenDraw.DrawLineBetween(vector, mech.DrawPos, SimpleColor.White, 0.1f);
				GenDraw.DrawCircleOutline(mech.DrawPos, 0.5f);
			}
			// if (DrawIconAtTarget)
			// {
				// if (iconMat == null)
				// {
					// iconMat = MaterialPool.MatFrom(def.uiIcon);
				// }
				// Matrix4x4 matrix = Matrix4x4.TRS(vector, Quaternion.identity, IconScale);
				// Graphics.DrawMesh(MeshPool.plane14, matrix, iconMat, 0);
			// }
		}

		public static GlobalTargetInfo GetTargetForMech(MechanitorControlGroup group, Pawn mech)
		{
			CompMechSettings compSmartEscort = mech.TryGetComp<CompMechSettings>();
			if (compSmartEscort != null && compSmartEscort.escortTarget != null)
			{
				return compSmartEscort.escortTarget;
			}
			return group.Tracker.Pawn;
		}

		// public override GlobalTargetInfo GetTargetForLine(MechanitorControlGroup group)
		// {
			// return group.Tracker.Pawn;
		// }
	}

}
