﻿using System;
using System.IO;
using IL2CPU.API.Attribs;
using PrismAPI.Graphics;
using GoCode = GoOS.Commands.Run;

namespace GoOS.GUI.Apps.GoIDE
{
    public class IDEFrame : Window
    {
        [ManifestResourceStream(ResourceName = "GoOS.Resources.GUI.GoIDE.run.bmp")] static byte[] runRaw;
        static Canvas RunImage = Image.FromBitmap(runRaw, false);

        Button SaveButton;
        Button RunButton;
        InputNUMBERS Code;

        string ProjectName, ProjectPath;
        bool Is9xCode;

        public IDEFrame(string projectName, string projectPath, bool is9xCode)
        {
            try
            {
                // Store project name and project path to local variable.
                ProjectName = projectName;
                ProjectPath = projectPath;
                Is9xCode = is9xCode;
                
                // Create the window.
                Contents = new Canvas(800, 600);
                Title = projectName + " - GoIDE";
                Visible = true;
                Closable = true;
                SetDock(WindowDock.Auto);

                // Initialize the controls.
                SaveButton = new Button(this, 2, 2, 48, 18, "Save") { Clicked = SaveButton_Click, UseSystemStyle = false, BackgroundColour = Color.LightGray };
                RunButton = new Button(this, Convert.ToUInt16(Contents.Width - 42), 2, 40, 18, "Run") { Clicked = RunButton_Click, UseSystemStyle = false, BackgroundColour = Color.LightGray };
                Code = new InputNUMBERS(this, 2, 20, Convert.ToUInt16(Contents.Width - 4), Convert.ToUInt16(Contents.Height - 43), string.Empty) { MultiLine = true };
                Code.Text = File.ReadAllText(projectPath);

                // Paint the window.
                Paint("Loaded");
            }
            catch (Exception ex)
            {
                Dialogue.Show("GoIDE", "Something went wrong.\nPlease try again.\n\n" + ex, null, WindowManager.errorIcon);
            }
        }

        void Paint(string status)
        {
            Contents.Clear(Color.LightGray);
            RenderSystemStyleBorder();
            Contents.DrawImage(Contents.Width - 62, 0, RunImage);
            SaveButton.Render();
            RunButton.Render();
            Code.Render();
            Contents.DrawString(4, Contents.Height - 20, status, Fonts.Font_1x, Color.LighterBlack);
        }

        bool Debugging = false;

        void SaveButton_Click()
        {
            File.WriteAllText(ProjectPath, Code.Text);
            Paint("Saved");
        }

        void RunButton_Click()
        {
            if (!Debugging)
            {
                Debugging = true;

                File.WriteAllText(ProjectPath, Code.Text);
                BetterConsole.Clear();
                BetterConsole.Title = "Terminal - GoIDE";
                WindowManager.AddWindow(new GTerm(false));

                if (!Is9xCode)
                    GoCode.Main(ProjectPath, false);
                else
                    _9xCode.Interpreter.Run(ProjectPath);

                Debugging = false;
            }
        }
    }
}
