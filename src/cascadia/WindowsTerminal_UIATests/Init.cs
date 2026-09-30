//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ExperimentalTabTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------

using Microsoft.PowerToys.UITest;
using System;
using System.Diagnostics;
using System.IO;
using WEX.Logging.Interop;
using WEX.TestExecution.Markup;

namespace Host.Tests.UIA
{
    [TestClass]
    class Init
    {
        static Process appDriver;
        static ScreenRecording sr;

        [AssemblyInitialize]
        public static void SetupAll(TestContext context)
        {
            sr = new ScreenRecording(Path.Combine(context.TestDeploymentDir, "Recordings"));
            _ = sr.StartRecordingAsync();
            Log.Comment("Searching for WinAppDriver in the same directory where this test was launched from...");
            string winAppDriver = Path.Combine(context.TestDeploymentDir, "WinAppDriver", "WinAppDriver.exe");

            if (!File.Exists(winAppDriver))
            {
                winAppDriver = Path.Combine(context.TestDeploymentDir, "WinAppDriver.exe");
            }

            Log.Comment($"Attempting to launch WinAppDriver at: {winAppDriver}");
            Log.Comment($"Working directory: {Environment.CurrentDirectory}");

            appDriver = Process.Start(winAppDriver);
        }

        [AssemblyCleanup]
        public static void CleanupAll()
        {
            try
            {
                    try
                    {
                        sr.StopRecordingAsync().GetAwaiter().GetResult();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to stop screen recording: {ex.Message}");
                    }
                appDriver.Kill();
            }
            catch
            {

            }
        }
    }
}
