--bot = ConstructBot()
--obj_0 = ConstructAttachedEngine(bot, 2)
--obj_1 = ConstructAttachedSolarCell(obj_0, 3, 0.25)


bot = ConstructBot()
obj_0 = ConstructEngine(bot, 2)
obj_1 = ConstructAttachedSolarCell(obj_0, 3, 0.25)
EngineSetPower(obj_0, 1)

--ModuleEnableConsumable(obj_0, true)
--EngineSetPower(obj_0, 1)
--obj_1 = ConstructAttachedFuelCell(obj_0, 0, 1)



