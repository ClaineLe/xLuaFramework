
local CSWidgetTxt = CS.Framework.Util.Bindable.BindablePropertyText

LoginViewModel = {
	m_Text,
	m_Image,
	m_Btn_Confirm,
}

ViewDefine["Login"].ViewModel = LoginViewModel
setmetatable( LoginViewModel, { __index = BaseViewModel})

function LoginViewModel:init()
	
	self["m_Text"] = self.m_ViewRoot:GetWidgets()[0]

	print(self.m_Text)
	self.m_Text.text = "asdfasdfasdf"
end
