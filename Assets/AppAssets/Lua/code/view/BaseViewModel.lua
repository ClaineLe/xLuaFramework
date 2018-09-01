BaseViewModel = {
	m_ViewRoot,
	m_View,
}

function BaseViewModel:Create( viewRoot, viewModel, view )
	local obj = {}
	setmetatable( obj, { __index = viewModel })
	obj.m_ViewRoot = viewRoot
	obj.m_View = view:Create(obj,view)
	obj:init()
	return obj
end
