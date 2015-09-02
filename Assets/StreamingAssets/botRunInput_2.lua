
while true do
	fwPower = RadioGetAxisValue(obj_2, "Joystick_LeftStick_Y")
	turn = RadioGetAxisValue(obj_2, "Joystick_LeftStick_X")
	
	EngineSetPower(obj_4, turn + fwPower)
	EngineSetPower(obj_5, -turn + fwPower)
	
	if (RadioIsButtonDown(obj_2, "JoystickButton_B")) then
		ConstructorTrigger(obj_1)
	end
end
