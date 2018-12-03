using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    namespace Game
    {
        public class AppInfo {
            public string appVersion;
            public int resVersion;
            public long listContentSize;
            public eChange change;
            public ePlatform platform;

            public override string ToString()
            {
                JObject json = new JObject();
                json["appVersion"] = appVersion;
                json["resVersion"] = resVersion;
                json["listContentSize"] = listContentSize;
                json["change"] = change.ToString();
                json["platform"] = platform.ToString();
                return json.ToString();
            }

            public static AppInfo ValuleOf(string content)
            {
                JObject json = JObject.Parse(content);
                AppInfo appInfo = new AppInfo();
                appInfo.appVersion = json["appVersion"].Value<string>();
                appInfo.resVersion = json["resVersion"].Value<int>();
                appInfo.listContentSize = json["listContentSize"].Value<long>();
                appInfo.change = (eChange)System.Enum.Parse(typeof(eChange), json["change"].Value<string>());
                appInfo.platform = (ePlatform)System.Enum.Parse(typeof(ePlatform), json["platform"].Value<string>());
                return appInfo;
            }
        }

        public class ClientBundleInfo {
            public int resVersion;
            public List<BundleBaseInfo> bundleList = new List<BundleBaseInfo>();

            public override string ToString()
            {
                string content = resVersion + "\n";
                for (int i = 0; i < bundleList.Count; i++) {
                    content += bundleList[i].ToString() + "\n";
                }
                return content.Trim();
            }

            public static ClientBundleInfo ValuleOf(string content)
            {
                string[] datas = content.Trim().Split('\n');
                ClientBundleInfo clientBundleInfo = new ClientBundleInfo();
                clientBundleInfo.resVersion = int.Parse(datas[0]);
                clientBundleInfo.bundleList = new List<BundleBaseInfo>();
                for (int i = 1; i < datas.Length; i++)
                {
                    clientBundleInfo.bundleList.Add(BundleBaseInfo.ValuleOf(datas[i]));
                }
                return clientBundleInfo;
            }
        }

        public class BundleInfo
        {
            public string name;
            public string md5;
            public long position;
            public long len;
            public override string ToString()
            {
                return string.Format("{0},{1},{2},{3}", md5, name, position, len);
            }

            public static BundleInfo ValuleOf(string content)
            {
                string[] datas = content.Trim().Split(',');
                BundleInfo bundleInfo = new BundleInfo();
                if (datas.Length == 4)
                {
                    bundleInfo.md5 = datas[0];
                    bundleInfo.name = datas[1];
                    bundleInfo.position = long.Parse(datas[2]);
                    bundleInfo.len = long.Parse(datas[3]);
                }
                return bundleInfo;
            }
        }

        public class BundleBaseInfo
        {
            public string name;
            public string md5;
            public override string ToString()
            {
                return string.Format("{0},{1}",md5,name);
            }

            public static BundleBaseInfo ValuleOf(string content)
            {
                string[] datas = content.Trim().Split(',');
                BundleBaseInfo baseInfo = new BundleBaseInfo();
                if (datas.Length == 2)
                {
                    baseInfo.md5 = datas[0];
                    baseInfo.name = datas[1];
                }
                else {
                    Debug.LogError("parse content fail. content:" + content);
                }
                return baseInfo;
            }

            public override bool Equals(object dstObj)
            {
                BundleBaseInfo info = dstObj as BundleBaseInfo;
                return (info.name == name) && (info.md5 == md5);
            }
        }

    }
}