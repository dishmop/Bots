count = 0;
while true do
	angle = count;
	EngineSetPower(obj_2,  math.sin(angle))
	EngineSetPower(obj_3,  math.cos(angle))
    count = count + 1
end
