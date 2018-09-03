BasePresender = {
	m_View,
}
function BasePresender:Create(view)
	local obj = {}
	setmetatable( obj, { __index = self })
	obj.m_View = view
	obj:init()
	return obj
end

--虚方法，子类重写
function BasePresender:init( )
end