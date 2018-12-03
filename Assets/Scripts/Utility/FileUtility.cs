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
            static public void FileCopy(string scrPath, string dstPath, bool overwrite = true) {
                FileInfo scrFileInfo = new FileInfo(scrPath);
                if (!scrFileInfo.Exists)
                {
                    Debug.LogError("Found out scrPath:" + scrFileInfo.FullName);
                    return;
                }

                FileInfo dstFileInfo = new FileInfo(dstPath);
                if (!dstFileInfo.Directory.Exists)
                    dstFileInfo.Directory.Create();
                scrFileInfo.CopyTo(dstFileInfo.FullName, overwrite);
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
        
			/*
				long res_list_len
				list res_list_data
					<
						string name
						long startPos
						long len
					>
				list res_list
					<
						string fileBytes
					>
		
		

			static public void MergeFiles(string scrDirPath, string dstFilePath,string searchPattern = string.Empty, SearchOption searchOption = SearchOption.AllDirectories)
			{
				string tmpScrDirPath = scrDirPath + (!scrDirPath.EndsWith (@"/") && !scrDirPath.EndsWith (@"\"))?"/":string.Empty;
				if (!Directory.Exists (tmpScrDirPath)) {
					Debug.LogError ("Found out Directory. Path:" + tmpScrDirPath);
					return;
				}
				string[] scrfiles = Directory.GetFiles (tmpScrDirPath, searchPattern, searchOption);
				long curFilePosition = 0;
				for (int i = 0; i < scrfiles.Length; i++) {
					FileInfo fileInfo = new FileInfo (scrfiles[i]);
					if (!fileInfo.Exists) {
						Debug.LogError ("Found out File. Path:" + fileInfo.FullName);
						continue;
					}

					string pathName = fileInfo.FullName.Replace(scrDirPath,string.Empty);
					st
				}
			}

			static public void SplitFile(string scrFile, string dstDirPath, int buffSize = 1024)
			{
				
			}

			*/
		}
    }
}