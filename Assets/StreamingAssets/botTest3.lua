bot = ConstructBot()
obj_0 = ConstructFuelCell(bot, 4)
--ModuleEnableConsumable(obj_0, true)


obj_1 = ConstructAttachedConstructor(obj_0, 0, 1)
ConstructorSetBotDefinition(obj_1, "missile")
ConstructorActivate(obj_1, false)
ConstructorEnableAutoRepeat(obj_1, true)

obj_2 = ConstructAttachedRadio(obj_0, 2, 1)
obj_3 = ConstructAttachedAI(obj_0, 4, 1)
obj_4 = ConstructAttachedEngine(obj_1, 2, 2)
obj_5 = ConstructAttachedEngine(obj_1, 4, 2)

AISetRuntimeScript(obj_3, "botRunInput_2")
