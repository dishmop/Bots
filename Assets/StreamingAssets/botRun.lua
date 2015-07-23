count = 0;
while true do
	angle = count;
--	Log(math.sin(angle))
	EngineSetPower(obj_1, math.sin(angle))
	EngineSetPower(obj_2, math.cos(angle))
    count = count + 1
end
