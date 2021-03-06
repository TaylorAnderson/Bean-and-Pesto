namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class HarmonixDrumKitMacProfile : Xbox360DriverMacProfile
	{
		public HarmonixDrumKitMacProfile()
		{
			Name = "Harmonix Drum Kit";
			Meta = "Harmonix Drum Kit on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0x1138,
				},
			};
		}
	}
	// @endcond
}


