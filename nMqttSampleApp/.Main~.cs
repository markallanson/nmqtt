using System;
using System.Windows.Forms;

namespace nMqttSampleApp
{
	class Program
	{
		[STAThread]
		public static void Main (string[] args)
		{
			Application.EnableVisualStyles();
			Application.Run(new Shell());
		}
	}
}
