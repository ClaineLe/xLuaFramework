local CSManager = CS.Framework.Game.Manager

G_Manager = {
    LuaMgr,
    AssetMgr,
    TimerMgr,
    SoundMgr,
    PoolsMgr,
    NetWorkMgr,
    LevenMgr,
    EventMgr,
}

function G_Manager:Init()
	self.LuaMgr 	= CSManager.LuaMgr
	self.AssetMgr 	= CSManager.AssetMgr
	self.TimerMgr 	= CSManager.TimerMgr
	self.SoundMgr 	= CSManager.SoundMgr
	self.PoolsMgr 	= CSManager.PoolsMgr
	self.NetWorkMgr = CSManager.NetWorkMgr
	self.LevenMgr	= CSManager.LevenMgr
	self.EventMgr	= CSManager.EventMgr
end