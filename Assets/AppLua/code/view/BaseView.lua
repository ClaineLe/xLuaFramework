BaseView = {
}


function BaseView:Create( widgetRoot, View )
	local obj = {}
	setmetatable( obj, { __index = View })
	local widgets = widgetRoot:GetWidgets()
	for i=1,widgets.Length do
		if obj[widgets[i-1].RefName] then
			obj[widgets[i-1].RefName] = widgets[i-1]
		else
			print("found out " .. widgets[i-1].RefName .. " " .. type(widgets[i-1].RefName))
		end
	end
	obj:init()
	return obj
end
