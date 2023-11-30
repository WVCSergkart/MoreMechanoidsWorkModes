using System.Linq;
using System.Xml;
using Verse;

namespace WVC_WorkModes
{

    public class PatchOperation_Settings : PatchOperation
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
