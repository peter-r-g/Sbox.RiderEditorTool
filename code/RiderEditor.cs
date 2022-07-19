using System;
using System.Diagnostics;
using System.IO;
using Tools;

namespace RiderEditor;

public static class RiderEditor
{
	private const string RiderCookie = "RiderEditor.ExePath";
	private const string JetBrainsFolder = "C:\\Program Files\\JetBrains";

	[Menu( "Editor", "Rider/Open Rider" )]
	public static void OpenRiderEditor()
	{
		if ( !Cookie.TryGetString( RiderCookie, out var riderInstallation ) ||
		     string.IsNullOrWhiteSpace( riderInstallation ) )
		{
			Log.Error( "No Rider installation selected. Use Rider/Choose Rider Installation" );
			return;
		}

		if ( !File.Exists( riderInstallation ) )
		{
			Log.Error( $"Rider no longer exists at \"{riderInstallation}\" Cookie will be wiped." );
			Cookie.SetString( "RiderEditor.ExePath", string.Empty );
			return;
		}
		
		var process = new Process();
		var startInfo = new ProcessStartInfo
		{
			FileName = riderInstallation,
			Arguments = $"\"{Environment.CurrentDirectory}\\s&box.sln\""
		};
		process.StartInfo = startInfo;
		process.Start();
	}
	
	[Menu( "Editor", "Rider/Choose Rider Installation" )]
	public static void RiderEditorInstallation()
	{
		var dialog = new FileDialog( null ) {Title = "Find Rider Installation"};
		dialog.SetNameFilter( "rider64.exe" );
		if ( Directory.Exists( JetBrainsFolder ) )
			dialog.Directory = JetBrainsFolder;

		if ( dialog.Execute() )
			Cookie.SetString( RiderCookie, dialog.SelectedFile );
	}
}
