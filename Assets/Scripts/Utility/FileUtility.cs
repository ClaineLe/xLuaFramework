using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Framework
{
    namespace Util
    {
        public class FileUtility
        {
			static public long GetFileSize(string scrFileFullPath){
				FileInfo fileInfo = new FileInfo(scrFileFullPath);
				if(fileInfo.Exists)
					return fileInfo.Length;
				return 0;
			}
			
            /// <summary>
            /// 复制文件到目标路径
            /// </summary>
            /// <param name="scrPath">源文件路径</param>
            /// <param name="dstPath">目标文件路径</param>
            static public void FileCopy(string scrPath, string dstPath) {
                FileInfo scrFileInfo = new FileInfo(scrPath);
                if (!scrFileInfo.Exists)
                {
                    Debug.LogError("Found out scrPath:" + scrFileInfo.FullName);
                    return;
                }

                FileInfo dstFileInfo = new FileInfo(dstPath);
                if (!dstFileInfo.Directory.Exists)
                    dstFileInfo.Directory.Create();
                scrFileInfo.CopyTo(dstFileInfo.FullName);
            }

            static public void DirCopy(string scrPath, string dstPath, string[] filterExtensions = null)
            {
                List<string> filters = new List<string>();
                if(filterExtensions != null && filterExtensions.Length>0)
                    filters = new List<string>(filterExtensions);

                DirectoryInfo scrDirInfo = new DirectoryInfo(scrPath);
                if(!scrDirInfo.Exists)
                {
                    Debug.LogError("Found out scrPath:" + scrDirInfo.FullName);
                    return;
                }

                DirectoryInfo dstDirInfo = new DirectoryInfo(dstPath);
                if (!dstDirInfo.Exists)
                    dstDirInfo.Create();

                FileInfo[] scrFileInfos = scrDirInfo.GetFiles();
                for (int i = 0; i < scrFileInfos.Length; i++)
                {
                    if (filters.Exists(a => a.Equals(scrFileInfos[i].Extension)))
                        continue;
                    FileCopy(scrFileInfos[i].FullName, Path.Combine(dstPath, scrFileInfos[i].Name));
                }
            }
        }
    }
}