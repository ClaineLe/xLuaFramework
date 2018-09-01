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
        public class LuaManager : BaseManager<LuaManager>, IManager
        {
            public const string AppLuaPath = "Assets/AppAssets/Lua/";

            private XLua.LuaEnv m_LuaEnv;

            private UnityAction m_LuaStart;
            private UnityAction m_LuaUpdate;
            private UnityAction m_LuaRelease;

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

            public byte[] CustomLoader(ref string filepath)
            {
#if UNITY_EDITOR
                return File.ReadAllBytes(AppLuaPath + filepath.Replace('.','\\') + ".lua");
#else
                string str = string.Empty;
#endif
            }
            public void DoLuaFile(string luaPath)
            {
                this.m_LuaEnv.DoString(string.Format("require '{0}'", luaPath));
            }

            public void HotFix(string hotfixStr)
            {
                this.m_LuaEnv.DoString(hotfixStr);
            }

            public bool StartUpLuaFramework()
            {
                this.DoLuaFile("game.StartUp");
                m_LuaStart = m_LuaEnv.Global.Get<UnityAction>("LuaStart");
                m_LuaUpdate = m_LuaEnv.Global.Get<UnityAction>("LuaUpdate");
                m_LuaRelease = m_LuaEnv.Global.Get<UnityAction>("LuaRelease");

                if (m_LuaStart != null)
                    m_LuaStart();

                return true;
            }

            public void Tick()
            {
                if (m_LuaUpdate != null)
                    m_LuaUpdate();

                if (Time.time - lastGCTime > GCInterval)
                {
                    this.m_LuaEnv.Tick();
                    lastGCTime = Time.time;
                }
            }

            public void Release()
            {
                if (m_LuaRelease != null)
                    m_LuaRelease();

                this.m_LuaStart = null;
                this.m_LuaUpdate = null;
                this.m_LuaRelease = null;

                this.m_LuaEnv = null;
            }
        }

    }
}
