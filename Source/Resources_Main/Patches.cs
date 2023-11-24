using System.Xml;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Verse;
using UnityEngine;
using WVC;

namespace WVC
{

	public class PatchOperation_MMWM : PatchOperation
	{
		public string settingName;
		public PatchOperation active;
		public PatchOperation deActive;

		protected override bool ApplyWorker(XmlDocument xml)
		{
			if (WVC_MMWM.settings.GetEnabledSettings.Contains(settingName) && active != null)
			{
				return active.Apply(xml);
			}
			else if (!WVC_MMWM.settings.GetEnabledSettings.Contains(settingName) && deActive != null)
			{
				return deActive.Apply(xml);
			}
			return true;
		}
	}

}
