
LoginView = {
	m_Text = "",
	m_Image = "",

}

ViewDefine["Login"].View = LoginView
setmetatable( LoginView, { __index = BaseView})

function LoginView:init()
	print("LoginView:init")
end
