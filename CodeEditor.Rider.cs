using System;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Tools.CodeEditors;

[Title( "Rider" )]
public class Rider : ICodeEditor
{
	public void OpenFile( string path, int? line = null, int? column = null )
	{
		var solution = FindSolutions( System.IO.Path.GetDirectoryName( path ) );
		
		var args = new StringBuilder();
		args.Append( $"\"{solution}\" " );
		if ( line is not null )
			args.Append( $"--line {line} " );
		if ( column is not null )
			args.Append( $"--column {column} " );
		args.Append( $"\"{path}\"" );
		
		Launch( args.ToString() );
	}

	public void OpenSolution()
	{
		Launch( $"\"{AddonSolution()}\"" );
	}

	public bool IsInstalled() => !string.IsNullOrEmpty( FindRider() );

	private static void Launch( string arguments )
	{
		var startInfo = new System.Diagnostics.ProcessStartInfo
		{
			CreateNoWindow = true,
			Arguments = arguments,
			FileName = FindRider()
		};

		System.Diagnostics.Process.Start( startInfo );
	}

	private static string RiderPath;

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>" )]
	private static string FindRider()
	{
		if ( RiderPath != null )
		{
			return RiderPath;
		}
		
		foreach ( var p in System.Diagnostics.Process.GetProcessesByName( "rider64" ) )
		{
			RiderPath = p.MainModule.FileName;
			return RiderPath;
		}
		
		string value = null;
		using ( var key = Registry.ClassesRoot.OpenSubKey( @"Applications\\rider64.exe\\shell\\open\\command" ) )
		{
			value = key?.GetValue( "" ) as string;
		}

		if ( value == null )
		{
			return string.Empty;
		}

		// Given `"C:\Program Files\JetBrains\JetBrains Rider 2022.1.2\bin\rider64.exe" "%1"` grab the first bit
		Regex rgx = new Regex( "\"(.*)\" \".*\"", RegexOptions.IgnoreCase );
		var matches = rgx.Matches( value );
		if ( matches.Count == 0 || matches[0].Groups.Count < 2 )
		{
			return null;
		}

		RiderPath = matches[0].Groups[1].Value;
		return RiderPath;
	}
	
	private static string FindSolutions( string path )
	{
		if ( path == null || path.Length < 5 )
			throw new Exception( "Couldn't find solution file" );

		var addonFile = System.IO.Path.Combine( path, ".addon" );
		if ( System.IO.File.Exists( addonFile ) )
		{
			return AddonSolution();
		}

		var solutions = System.IO.Directory.EnumerateFiles( path, "*.sln" ).ToArray();
		if ( solutions.Length > 0 )
		{
			return string.Join( ";", solutions );
		}

		return FindSolutions( System.IO.Directory.GetParent( path ).FullName );
	}
	
	private static string AddonSolution()
	{
		return $"{Environment.CurrentDirectory}/s&box.sln";
	}
}
