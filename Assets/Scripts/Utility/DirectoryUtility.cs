using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Framework
{
    namespace Util
    {
        public class DirectoryUtility
        {
            /// <summary>
            /// 复制文件夹到目标路径
            /// </summary>
            /// <param name="scrPath">源文件夹路径</param>
            /// <param name="dstPath">目标文件夹路径</param>
            static public void DirectoryCopyTo(string scrPath, string dstPath){
            }

            static public bool Exists(string path){
                return Directory.Exists(path);
            }

            static public void Clear(string path)
            {
                Delete(path);
                Directory.CreateDirectory(path);
            }

            static public void Delete(string path)
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
        }
    }
}