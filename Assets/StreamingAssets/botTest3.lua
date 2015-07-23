bot = ConstructBot()
obj_0 = ConstructFuelCell(bot)
obj_1 = ConstructAttachedEngine(obj_0, 2)
obj_2 = ConstructAttachedEngine(obj_0, 4)
BotLoadScript(bot, "botRun")


