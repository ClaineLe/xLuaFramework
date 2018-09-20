BaseView = {
}


function BaseView:Create( widgetRoot )
	local obj = {}
	setmetatable( obj, { __index = self })
	local widgets = widgetRoot:GetWidgets()
	for i=1,widgets.Length do
		obj[widgets[i-1].RefName] = widgets[i-1]
	end
	obj:init()
	return obj
end

--虚方法，子类重写
function BaseView:init()
end