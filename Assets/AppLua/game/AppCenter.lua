local CSGameObject = CS.UnityEngine.GameObject
local CSWidgetRoot = CS.Framework.Code.Widget.WidgetRoot

AppCenter = {
	
}

function AppCenter:Init()
	print("AppCenter - Init")
	local loginViewModel = ViewFactory:CreateView("Login")
	--loginViewModel.m_Text.Value = "Claine"
end

ViewFactory={}
function ViewFactory:CreateView( viewName )
	local goAsset = G_Manager.AssetMgr:LoadGameObject(viewName)
	local go = CSGameObject.Instantiate(goAsset)
	local widgetRoot = go:GetComponent(typeof(CSWidgetRoot))
	local view = LoginView:Create(widgetRoot,LoginView)
	view.m_Text.text = "asdfasdf"
	--return viewModel:Create(viewRoot,viewModel,view)
end