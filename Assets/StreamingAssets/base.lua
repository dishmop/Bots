bot = ConstructBot()
BotEnableAnchor(bot, true);
obj_0 = ConstructConstructor(bot, 4)
ConstructorSetBotDefinition(obj_0, "botTest3")
ConstructorActivate(obj_0, true)
ConstructorEnableAutoRepeat(obj_0, false)
obj_1 = ConstructAttachedFuelCell(obj_0, 2, 1)
obj_2 = ConstructAttachedFuelCell(obj_0, 4, 1)

obj_3 = ConstructAttachedSolarCell(obj_1, 2, 1)
obj_4 = ConstructAttachedSolarCell(obj_2, 4, 1)

obj_7 = ConstructAttachedSolarCell(obj_1, 4, 1)

obj_5 = ConstructAttachedSolarCell(obj_3, 2, 1)
obj_6 = ConstructAttachedSolarCell(obj_4, 4, 1)

--ModuleEnableConsumable(obj_1, false)

