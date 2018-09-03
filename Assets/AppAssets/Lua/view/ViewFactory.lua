local CSGameObject = CS.UnityEngine.GameObject
local CSWidgetRoot = CS.Framework.Code.Widget.WidgetRoot
ViewFactory={
	
}

local VIEWASSET_ROOTPATH = "Assets/AppAssets/Prefabs/View/"
local VIEWASSET_EXTENSION = ".prefab"

function ViewFactory:LoadViewRoot( viewType )
	local viewFullPath = self:_getViewPath(ViewDefine[viewType].p_Name) 
	local goAsset = Framework.m_AssetMgr:LoadGameObject(viewFullPath)
	local go = CSGameObject.Instantiate(goAsset)
	return go:GetComponent(typeof(CSWidgetRoot))
end

function ViewFactory:CreateView( viewType )
	local widgetRoot = self:LoadViewRoot(viewType)
	local view = LoginView:Create(widgetRoot)
	return LoginViewPresender:Create(view)
end


function ViewFactory:_getViewPath( viewName )
	return VIEWASSET_ROOTPATH .. viewName .. VIEWASSET_EXTENSION
end