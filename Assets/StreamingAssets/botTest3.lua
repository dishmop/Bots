bot = ConstructBot()
obj_0 = ConstructFuelCell(bot, 2)
obj_1 = ConstructAttachedConstructor(obj_0, 0, 1)
ConstructorSetBotDefinition(obj_1, "missile")
ConstructorActivate(obj_1, true)
ConstructorEnableAutoRepeat(obj_1, true)
obj_2 = ConstructAttachedEngine(obj_1, 2, 1)
obj_3 = ConstructAttachedEngine(obj_1, 4, 1)
BotLoadScript(bot, "botRun")
