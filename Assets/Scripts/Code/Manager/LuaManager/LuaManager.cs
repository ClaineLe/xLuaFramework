using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace Framework
{
    namespace Code.Manager
    {
        public partial class ManagerName
        {
            public const string Lua = "LuaManager";
        }
        public partial class LuaManager : BaseManager<LuaManager>, IManager
        {
            private XLua.LuaEnv m_LuaEnv;

            private XLua.LuaFunction m_FrameworkStart;
            private XLua.LuaFunction m_FrameworkTick;
            private XLua.LuaFunction m_FrameworkRelease;

            private float lastGCTime = 0;
            private const float GCInterval = 1;//1 second 

            public void Init()
            {
                this.m_LuaEnv = new XLua.LuaEnv();
                this.m_LuaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
                this.m_LuaEnv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
                this.m_LuaEnv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);
                this.m_LuaEnv.AddLoader(CustomLoader);
            }

            public void Tick()
            {
                if (m_FrameworkTick != null)
                    m_FrameworkTick.Call();

                if (Time.time - lastGCTime > GCInterval)
                {
                    this.m_LuaEnv.Tick();
                    lastGCTime = Time.time;
                }
            }

            public void Release()
            {
                if (m_FrameworkRelease != null)
                    m_FrameworkRelease.Call();

                this.m_FrameworkStart = null;
                this.m_FrameworkTick = null;
                this.m_FrameworkRelease = null;

                this.m_LuaEnv = null;
            }

            public void DoLuaFile(string luaPath)
            {
                this.m_LuaEnv.DoString(string.Format("require '{0}'", luaPath));
            }

            public void HotFix(string hotfixStr)
            {
                this.m_LuaEnv.DoString(hotfixStr);
            }

            public byte[] CustomLoader(ref string filepath)
            {
#if UNITY_EDITOR
                string fullPath = Application.dataPath + "/" + filepath;
                return File.ReadAllBytes(fullPath.Replace('.', Path.DirectorySeparatorChar) + ".txt");
#else
                return null;
#endif
            }

            public bool StartUpLuaFramework()
            {
                this.DoLuaFile("code.Framework");
                XLua.LuaTable frameworkTable = m_LuaEnv.Global.Get<XLua.LuaTable>("Framework");
                m_FrameworkStart = frameworkTable.Get<XLua.LuaFunction>("Start");
                m_FrameworkTick = frameworkTable.Get<XLua.LuaFunction>("Tick");
                m_FrameworkRelease = frameworkTable.Get<XLua.LuaFunction>("Release");
                if (m_FrameworkStart != null)
                    m_FrameworkStart.Call(frameworkTable);
                return true;
            }
        }
    }
}
