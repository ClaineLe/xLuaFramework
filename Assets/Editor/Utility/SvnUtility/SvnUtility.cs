using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace Framework.Editor
{
    namespace Utility
    { 

public class SvnUtility : MonoBehaviour
        {
            public static int GetSvnVersion(string dirPath)
            {
#if UNITY_EDITOR_OSX
                return GetSvnVersionForMac(dirPath);
#else
        return GetSvnVersionForWin (dirPath);
#endif
            }

            static private int GetSvnVersionForMac(string dirPath)
            {
                string shell = @"echo `svn info " + dirPath + @" | grep 'Last Changed Rev'|awk '{print $4}'`";

                string shellPath = "shell-get-svnversion.sh";
                if (System.IO.File.Exists(shellPath))
                    System.IO.File.Delete(shellPath);
                System.IO.File.WriteAllText(shellPath, shell);
                string command = "/bin/bash";
                Process process = new Process();
                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = shellPath;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                int svnVersion = 0;
                int.TryParse(process.StandardOutput.ReadToEnd(), out svnVersion);
                System.IO.File.Delete(shellPath);
                return svnVersion;
            }

            static private int GetSvnVersionForWin(string dirPath)
            {
                string tmpPath = "svn_version_tmp.txt";
                string destPath = "svn_version_dst.txt";
                if (System.IO.File.Exists(tmpPath))
                    System.IO.File.Delete(tmpPath);
                System.IO.File.WriteAllText(tmpPath, "$WCREV$");

                Process p = new Process();
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = Application.dataPath + "/Editor/Utility/SvnUtility/SubWCRev.exe";
                p.StartInfo.Arguments = dirPath + " " + tmpPath + " " + destPath;
                p.StartInfo.UseShellExecute = false;
                p.Start();
                p.WaitForExit();

                int svnVersion = int.Parse(System.IO.File.ReadAllText(destPath));

                if (System.IO.File.Exists(tmpPath))
                    System.IO.File.Delete(tmpPath);

                if (System.IO.File.Exists(destPath))
                    System.IO.File.Delete(destPath);

                return svnVersion;
            }
        }

    }
}