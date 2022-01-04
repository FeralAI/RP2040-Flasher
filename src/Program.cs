using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace RP2040_Flasher
{
	class Program
	{
		static FileInfo FirmwareFile { get; set; }

		static void Main(string[] args)
		{
			if (args.Length != 1)
			{
				Console.WriteLine("Please provide a .uf2 to flash via command line or drag-and-drop on the .exe file.");
				return;
			}

			if (!File.Exists(args[0]))
			{
				Console.WriteLine("Invalid .uf2 file, please use a valid file.");
				return;
			}

			FirmwareFile = new FileInfo(args[0]);
			Console.WriteLine($"Using .uf2 file for flashing: {FirmwareFile.FullName}");

			while (true)
			{
				Console.WriteLine("Waiting for RP2040 drive...");
				FlashLoop();
			}
		}

		static void FlashLoop()
		{
			DriveInfo driveInfo = System.IO.DriveInfo.GetDrives().FirstOrDefault(d => d.VolumeLabel == "RPI-RP2");
			while (driveInfo == null)
				driveInfo = System.IO.DriveInfo.GetDrives().FirstOrDefault(d => d.VolumeLabel == "RPI-RP2");

			Console.WriteLine($"Found RP2040 at {driveInfo.Name}");
			Console.WriteLine($"Flashing {FirmwareFile.FullName} to RP2040, please wait...");
			try
			{

				File.Copy(FirmwareFile.FullName, Path.Combine(driveInfo.Name, FirmwareFile.Name));	
				Console.WriteLine("Flashing complete!");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Flash failed with exception: {ex.Message}");
			}

			Thread.Sleep(3000); // Wait a few seconds for the drive to disconnect and the Pico to reload
		}
	}
}
