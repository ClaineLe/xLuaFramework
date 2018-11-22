using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    namespace Game
    {
        public class PackerInfo {
            public string fromVersion;
            public string toVersion;
            public string packerName;
            public int fileCnt;
            public long packerSize;
            public long totalSize;
        }

        public class BundleInfo
        {
            public int state;//-1:删除， 0：修改， 1：增加
            public string Name;
            public string MD5;
            public long Size;

            public override string ToString()
            {
                return "state:" + state + ", name:" + Name + ", md5:" + MD5 + ", size:" + Size;
            }
        }
    }
}