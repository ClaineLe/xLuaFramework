
LoginViewPresender = {
	
} 

setmetatable( LoginViewPresender, { __index = BasePresender})

function LoginViewPresender:SetLabel( label )
	self.m_View.m_Text.text = label
end