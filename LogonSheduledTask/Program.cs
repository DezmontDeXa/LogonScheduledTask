using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogonSheduledTask
{
	//class Program
	//{
	//	static void Main(string[] args)
	//	{
	//		LogonSheduledTaskManager.CreateTask("SomeTask", Assembly.GetEntryAssembly().Location);

	//		Thread.Sleep(10000);

	//		LogonSheduledTaskManager.DeleteTask("SomeTask");
	//	}
	//}

	public static class LogonSheduledTaskManager
	{
		/// <summary>
		/// Create a scheduled task
		/// </summary>
		/// <param name="name">Name of task</param>
		/// <param name="execPath">Full path to executable file. If NULL then Assembly.GetEntryAssembly().Location</param>
		/// <param name="workingDir">Working directory. If NULL then directory of executable file</param>
		public static void CreateTask(string name, string execPath = null, string workingDir = null)
		{
			if (execPath == null)
				execPath = Assembly.GetEntryAssembly().Location;

			if (workingDir == null)
				workingDir = Path.GetDirectoryName(execPath);

			string xmlConfigFileContent = Properties.Resources.xmlconfig;

			xmlConfigFileContent = xmlConfigFileContent.Replace("[Command]", execPath);
			xmlConfigFileContent = xmlConfigFileContent.Replace("[WorkingDirectory]", workingDir);

			string tmpFile = Path.GetTempFileName();

			tmpFile = Path.ChangeExtension(tmpFile, "xml");

			File.WriteAllText(tmpFile, xmlConfigFileContent);

			var info = new ProcessStartInfo("Schtasks.exe", $"/Create /TN \"{name}\" /IT /F /XML \"{tmpFile}\"");


			Process.Start(info);


		}

		public static void DeleteTask(string name)
		{
			Process.Start("Schtasks.exe", $"/Delete /TN \"{name}\" /f");
		}
	}
}
