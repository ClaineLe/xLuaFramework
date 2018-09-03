local CSManager = CS.Framework.Game.Manager


Framework = {
	m_LuaMgr,
    m_AssetMgr,
    m_TimerMgr,
    m_SoundMgr,
    m_PoolsMgr,
    m_NetWorkMgr,
    m_LevenMgr,
    m_EventMgr,
}

function Framework:LoadLuaScript()
    require 'cfg.FrameworkConfig'

    require 'util.ProtBufUtility'
    require 'util.FrameworkUtility'

    require 'view.BaseView'
    require 'view.BasePresender'
    require 'view.Login.LoginView'
    require 'view.Login.LoginViewPresender'
    require 'view.ViewFactory'
    require 'view.ViewDefine'

end

function Framework:InitManagers()
    self.m_LuaMgr 		= CSManager.LuaMgr
	self.m_AssetMgr 	= CSManager.AssetMgr
	self.m_TimerMgr 	= CSManager.TimerMgr
	self.m_SoundMgr 	= CSManager.SoundMgr
	self.m_PoolsMgr 	= CSManager.PoolsMgr
	self.m_NetWorkMgr 	= CSManager.NetWorkMgr
	self.m_LevenMgr		= CSManager.LevenMgr
	self.m_EventMgr		= CSManager.EventMgr
end

function Framework:Start( )
    self:LoadLuaScript()
    self:InitManagers()
    self:StartUp()
end

function Framework:Tick()
    --print("Framework:Tick")
end

function Framework:Release()
	--print("Framework:Release")
end

function Framework:StartUp()
	print("Framework:StartUp")
	local loginPresender = ViewFactory:CreateView(View.Login)
	loginPresender:SetLabel("Test_Claine")
end