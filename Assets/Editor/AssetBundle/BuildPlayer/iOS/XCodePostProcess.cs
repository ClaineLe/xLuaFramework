using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;

using System.IO;
using System.Text;

namespace Framework.Editor
{
    namespace Utility
    { 

public class XCodePostProcess{

	#if RELEASE 
	private const string IPA_FLAG = "dis";
	private const string PLIST_FILENAME = "DisExportOptions.plist";
	private const string PROVISIONING_PROFILE = "591b6bba-f7fd-4185-a5dc-93f302d95374";
	private const string CODE_SIGN_IDENTITY = "iPhone Distribution: Qianbin Chen (B4B5XPZXDV)";
	#else
	private const string IPA_FLAG = "dev";
	private const string PLIST_FILENAME = "DevExportOptions.plist";
	private const string PROVISIONING_PROFILE = "3a02d483-171f-494c-9b6a-e5edd47bf60c";
	private const string CODE_SIGN_IDENTITY = "iPhone Developer: Qianbin Chen (X5Y746F5BW)";
	#endif

	[PostProcessBuild]
	static void OnPostProcessBuild(BuildTarget target, string path)
	{
		if (target == BuildTarget.iOS) {
			#if UNITY_EDITOR_OSX 
			{
				string projPath = UnityEditor.iOS.Xcode.PBXProject.GetPBXProjectPath (path);
				string targetGuidByName = UnityEditor.iOS.Xcode.PBXProject.GetUnityTargetName ();
				UnityEditor.iOS.Xcode.PBXProject proj = new UnityEditor.iOS.Xcode.PBXProject ();

				proj.ReadFromString (File.ReadAllText (projPath));
				string targetName = proj.TargetGuidByName (targetGuidByName);

				File.Copy(Application.dataPath + "/Editor/AssetBundle/BuildPlayer/iOS/" + PLIST_FILENAME, path + "/ExportOptions.plist",true);

				proj.SetBuildProperty (targetName, "PROVISIONING_PROFILE", PROVISIONING_PROFILE);
				proj.SetBuildProperty (targetName, "CODE_SIGN_IDENTITY", CODE_SIGN_IDENTITY);
				proj.SetBuildProperty (targetName, "ENABLE_BITCODE", "NO");
				File.WriteAllText (projPath, proj.WriteToString ());

				//xCodeToIPA(path, AssetBundleBuilder.GetExportName ());
			}
			#endif
		}
	}

	static private void xCodeToIPA(string projectPath, string ipaFileName){
		System.Diagnostics.Process myCustomProcess = new System.Diagnostics.Process();
		myCustomProcess.StartInfo.FileName = "osascript";
		myCustomProcess.StartInfo.Arguments = string.Format ("-e 'tell application \"Terminal\" \n activate \n do script \"cd {0} && sh {1} {2} {3}\" \n end tell'",Application.dataPath + "/Editor/Build/Build_iOS/", "shell-just-build.sh", projectPath, ipaFileName);
		myCustomProcess.StartInfo.UseShellExecute = false;
		myCustomProcess.StartInfo.RedirectStandardOutput = false;
		myCustomProcess.Start();
		myCustomProcess.WaitForExit ();
	}
}

    }
}