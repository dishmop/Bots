count = 0;
while true do
	angle = count;
	EngineSetPower(obj_4,  math.sin(angle))
	EngineSetPower(obj_5,  math.cos(angle))
    count = count + 0.1
end
