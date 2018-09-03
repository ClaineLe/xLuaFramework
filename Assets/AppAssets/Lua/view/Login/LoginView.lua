
LoginView = {
	p_Name = "LoginView",
	m_Text,
	m_Image,
}
setmetatable( LoginView, { __index = BaseView})

function LoginView:init()
	print("LoginView:init")
end
