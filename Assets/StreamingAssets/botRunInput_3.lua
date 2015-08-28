
while true do
	fwPower = 1
	turn = 0.5
	
	EngineSetPower(obj_4, turn + fwPower)
	EngineSetPower(obj_5, -turn + fwPower)
end
