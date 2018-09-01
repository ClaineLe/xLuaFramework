function LoadLuaScript()
    require 'global.G_Manager'
    
    require 'cfg.AppConfig'

    require 'util.ProtBufUtility'
    require 'util.AppUtility'

    require 'game.StartUp'
    require 'game.AppCenter'

    require 'code.view.BaseView'
    require 'code.view.BaseViewModel'

    require 'view.Login.LoginView'
    require 'view.Login.LoginViewModel'

end

function InitGlobal()
    G_Manager:Init()
end

ViewDefine = {
    ["Login"] = {},
}

function LuaStart()
    print("-----------LuaStart")
    LoadLuaScript()
    InitGlobal()
    AppCenter:Init()
end

function LuaUpdate()
    --print("-----------LuaUpdate")
end

function LuaRelease()
    print("-----------LuaRelease")
end