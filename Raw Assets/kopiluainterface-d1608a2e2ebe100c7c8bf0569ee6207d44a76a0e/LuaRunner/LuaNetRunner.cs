using System;
using LuaInterface;
using System.Threading;
using System.Collections.Generic;
/*
 * Application to run Lua scripts that can use LuaInterface
 * from the console
 * 
 * Author: Fabio Mascarenhas
 * Version: 1.0
 */
namespace LuaRunner
{
    public class TestClass
    {


        public void TestFuncC()
        {
            Console.WriteLine("Calling test function from lua");
        }

     

    }

    public class LuaNetRunner
    {
        /*
		 * Runs the Lua script passed as the first command-line argument.
		 * It passed all the command-line arguments to the script.
		 */
        [STAThread]     // steffenj: testluaform.lua "Load" button complained with an exception that STAThread was missing
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                // For attaching from the debugger
                // Thread.Sleep(20000);

                // Test enumerator thingy
                /*
                TestClass test = new TestClass();
                test.RestartLoop();
                while (!test.HasLoopFinished())
                {
                    test.CallLoop();
                }
                */
                TestClass test = new TestClass();
                //TestHookClass testObj = new TestHookClass(args[0]);
                Lua lua = new Lua();
                lua.RegisterFunction("TestFunc", test, test.GetType().GetMethod("TestFuncC"));
                try
                {
                    //Console.WriteLine("DoFile(" + args[0] + ");");
                    lua.DoFileASync(args[0], 1);

                    while (!lua.isFinishedASync)
                    {
                        lua.ResumeAsync();

                    }

                    Console.WriteLine("Done DoFileASync()");
                }
                catch (Exception e)
                {
                    // steffenj: BEGIN error message improved, output is now in decending order of importance (message, where, stacktrace)
                    // limit size of strack traceback message to roughly 1 console screen height
                    string trace = e.StackTrace;
                    if (e.StackTrace.Length > 1300)
                        trace = e.StackTrace.Substring(0, 1300) + " [...] (traceback cut short)";

                    Console.WriteLine();
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.Source + " raised a " + e.GetType().ToString());
                    Console.WriteLine(trace);

                    // wait for keypress if there is an error
                    // Console.ReadKey();
                    // steffenj: END error message improved
                }

            }
            Console.ReadLine();



        }
    }

}
