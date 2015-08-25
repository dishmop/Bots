bot = ConstructBot()
obj_4 = ConstructAI(bot, 1)
obj_0 = ConstructAttachedFuelCell(obj_4, 0, 4)
obj_1 = ConstructAttachedConstructor(obj_0, 0, 3)
ConstructorSetBotDefinition(obj_1, "missile")
ConstructorActivate(obj_1, false)
ConstructorEnableAutoRepeat(obj_1, true)
obj_2 = ConstructAttachedEngine(obj_1, 2, 2)
obj_3 = ConstructAttachedEngine(obj_1, 4, 2)

--obj_5 = ConstructAttachedSolarCell(obj_2, 2, 0.25)
--obj_6 = ConstructAttachedSolarCell(obj_3, 4, 0.25)

AISetRuntimeScript(obj_4, "botRun")
