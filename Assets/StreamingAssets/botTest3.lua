bot = ConstructBot()
obj_0 = ConstructFuelCell(bot, 4)


obj_1 = ConstructAttachedConstructor(obj_0, 0, 2)
ConstructorSetBotDefinition(obj_1, "missile_2")
ConstructorSetAlwaysReadyMode(obj_1)
ConstructorSetKickVelocity(obj_1, 0, 20)

obj_2 = ConstructAttachedRadio(obj_0, 2, 1)
obj_3 = ConstructAttachedAI(obj_0, 4, 1)
obj_4 = ConstructAttachedEngine(obj_1, 2, 2)
obj_5 = ConstructAttachedEngine(obj_1, 4, 2)

--ModuleEnableConsumable(obj_4, true)
--ModuleEnableConsumable(obj_5, true)

AISetRuntimeScript(obj_3, "botRunInput_3")
