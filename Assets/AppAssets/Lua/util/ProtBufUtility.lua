ProtBufUtility = {
	m_pb = nil,
	m_protoc = nil,
}

local ProtBufFilePath = 'Assets/AppLua/protobuf.proto'

function ProtBufUtility:Create( )
	local obj = {}
	setmetatable( obj, { __index = ProtBufUtility })
	obj:init()
	return obj
end

function ProtBufUtility:init( )
	print("ProtBufUtility")
	self.m_pb = require 'pb'
	self.m_protoc = require 'protoc'
	local protbufStr = G_Manager.AssetMgr.LoadTextAsset(ProtBufFilePath).text
	assert(self.m_protoc:load(protbufStr))
end


function ProtBufUtility:Encode( msgName, data )
	return assert(self.m_pb.encode(msgName, data))
end

function ProtBufUtility:Decode( msgName, bytes )
	return assert(self.m_pb.decode(msgName, bytes))
end