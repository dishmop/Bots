
while true do
	if RadioIsButtonDown(obj_2, "Key_RightShift") then
		fwPower = 1
	else
		fwPower = 0
	end

	turn = 0
	if RadioIsButtonDown(obj_2, "Key_X") then 
		turn = -1
	end
	
	if RadioIsButtonDown(obj_2, "Key_Z") then 
		turn = 1
	end	
	
	
	EngineSetPower(obj_4, turn + fwPower)
	EngineSetPower(obj_5, -turn + fwPower)
	
	if (RadioIsButtonDown(obj_2, "Key_Return")) then
		ConstructorTrigger(obj_1)
	end
end