/////// ekeleze ///////
// I hate xrc2 code. // 
///////////////////////

////////// xrc2 //////////
// I hate ekeleze code. //
//////////////////////////

using Cosmos.HAL;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using System;
using System.Collections.Generic;
using Sys = Cosmos.System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using GoOS.Themes;
using GoOS.Commands;
using Console = BetterConsole;
using ConsoleColor = PrismAPI.Graphics.Color;
using static GoOS.Core;
using System.Threading;
using Cosmos.System.Network.IPv4.UDP.DNS;
using PrismAPI.Graphics;
using IL2CPU.API.Attribs;
using PrismAPI.Hardware.GPU;
using GoOS.GUI;
using GoOS.GUI.Apps;
using GoOS.Networking;
using LibDotNetParser.CILApi;
using TcpClient = Cosmos.System.Network.IPv4.TCP.TcpClient;

// Goplex Studios - GoOS
// Copyright (C) 2022  Owen2k6

namespace GoOS
{
    public class Kernel : Sys.Kernel
    {
        public static Dictionary<string, string> InstalledPrograms = new Dictionary<string, string>() { };

        public static bool isGCIenabled = false;

        //Vars for OS
        public static string version = "1.5";
        public static string BuildType = "Beta";
        public static string olddir = @"0:\";

        public static string Notepadtextsavething = "";
        public static string NotepadFileToSaveNameThing = "";

        public static Sys.FileSystem.CosmosVFS FS;

        public static string username = null;
        public static string computername = null;

        public static string cutStatus = "Disabled";
        public static Color DesktopColour = Color.ClassicBlue;

        [ManifestResourceStream(ResourceName = "GoOS.Resources.GoOS_Intro.bmp")]
        public static byte[] rawBootLogo;

        protected override void BeforeRun()
        {
            if (Cosmos.Core.CPU.GetAmountOfRAM() < 150)
            {
                System.Console.ForegroundColor = System.ConsoleColor.Red;
                System.Console.WriteLine();
                System.Console.Write("GoOS - Not enough ram to boot GoOS. Please increase the amount of RAM of your VM");
                System.Console.Write("GoOS - Or if you are running this on real hardware (you shouldn't), buy more RAM");

                while (true);
            }

            WindowManager.Canvas = Display.GetDisplay(800, 600); //TODO: Not have this hard coded >:^(
            Console.Init(800, 600);

            var loadingDialogue = new LoadingDialogue("GoOS is starting\nPlease wait...");
            WindowManager.AddWindow(loadingDialogue);

            ThemeManager.SetTheme(Theme.Fallback);
            log(ThemeManager.WindowText, "GoOS - Starting GoOS...");
            try
            {
                FS = new Sys.FileSystem.CosmosVFS();
                Sys.FileSystem.VFS.VFSManager.RegisterVFS(FS);
                FS.Initialize(true);
                var total_space = FS.GetTotalSize(@"0:\");
            }
            catch
            {
                log(ThemeManager.ErrorText, "GoOS - Failed to initialize filesystem.\n");
                log(ThemeManager.ErrorText,
                    "GoOS - GoOS Needs a HDD installed to save user settings, application data and more.\n");
                log(ThemeManager.ErrorText, "GoOS - Please verify that your hard disk is plugged in correctly.");
                while (true)
                {
                }
            }

            if (!File.Exists(@"0:\content\sys\setup.gms"))
            {
                Console.ConsoleMode = true;
                WindowManager.AddWindow(new GTerm());
                Console.WriteLine("First boot... This may take awhile...");
                OOBE.Launch();
            }
            //
            // try
            // {
            //     if (!File.Exists(@"0:\content\sys\desktopcolour.gms"))
            //     {
            //         File.Create(@"0:\content\sys\desktopcolour.gms");
            //         File.WriteAllText(@"0:\content\sys\desktopcolour.gms", "C=ClassicBlue");
            //         DesktopColour = Color.ClassicBlue;
            //     }
            //     if (File.Exists(@"0:\content\sys\desktopcolour.gms"))
            //     {
            //         foreach (string line in File.ReadAllLines(@"0:\content\sys\desktopcolour.gms"))
            //         {
            //             if (line.StartsWith("C="))
            //             {
            //                 if (line.Replace("C=", "").Equals("ClassicBlue"))
            //                 {
            //                     DesktopColour = Color.ClassicBlue;
            //                     break;
            //                 }
            //                 else if (line.Replace("C=", "").Equals("UbuntuPurple"))
            //                 {
            //                     DesktopColour = Color.UbuntuPurple;
            //                     break;
            //                 }
            //                 else if (line.Replace("C=", "").Equals("SuperOrange"))
            //                 {
            //                     DesktopColour = Color.SuperOrange;
            //                     break;
            //                 }
            //                 else if (line.Replace("C=", "").Equals("Red"))
            //                 {
            //                     DesktopColour = Color.Red;
            //                     break;
            //                 }
            //                 else if (line.Replace("C=", "").Equals("Green"))
            //                 {
            //                     DesktopColour = Color.Green;
            //                     break;
            //                 }
            //                 else if (line.Replace("C=", "").Equals("Blue"))
            //                 {
            //                     DesktopColour = Color.Blue;
            //                     break;
            //                 }
            //                 else if (line.Replace("C=", "").Equals("Yellow"))
            //                 {
            //                     DesktopColour = Color.Yellow;
            //                     break;
            //                 }
            //                 else if (line.Replace("C=", "").Equals("Purple"))
            //                 {
            //                     DesktopColour = Color.Magenta;
            //                     break;
            //                 }
            //                 else if (line.Replace("C=", "").Equals("Cyan"))
            //                 {
            //                     DesktopColour = Color.Cyan;
            //                     break;
            //                 }
            //                 else if (line.Replace("C=", "").Equals("White"))
            //                 {
            //                     DesktopColour = Color.White;
            //                     break;
            //                 }
            //
            //                 else if (line.Replace("C=", "").Equals("Black"))
            //                 {
            //                     DesktopColour = Color.Black;
            //                     break;
            //                 }
            //                 else
            //                 {
            //                     DesktopColour = Color.ClassicBlue;
            //                     break;
            //                 }
            //             }
            //         }
            //     }
            // }
            // catch (Exception)
            // {
            //     DesktopColour = Color.ClassicBlue;
            //     //Dialogue.Show("Error", "GoOS - Failed to load desktop colour",
            //         //null);
            // }

            if (!Directory.Exists(@"0:\content\GCI\"))
            {
                try
                {
                    Directory.CreateDirectory(@"0:\content\GCI\");
                }
                catch (Exception)
                {
                    if (File.Exists(@"0:\content\sys\GCI.gms"))
                    {
                        File.Delete(@"0:\content\sys\GCI.gms");
                    }
                }
            }

            try
            {
                var systemsetup = File.ReadAllLines(@"0:\content\sys\user.gms");
                foreach (string line in systemsetup)
                {
                    if (line.StartsWith("username: "))
                    {
                        username = line.Replace("username: ", "");
                    }

                    if (line.StartsWith("computername: "))
                    {
                        computername = line.Replace("computername: ", "");
                    }
                }

                foreach (string line in File.ReadAllLines(@"0:\content\sys\theme.gms"))
                {
                    if (line.StartsWith("ThemeFile = "))
                    {
                        ThemeManager.SetTheme(line.Split("ThemeFile = ")[1]);
                    }
                }

                //byte videoMode = File.ReadAllBytes(@"0:\content\sys\resolution.gms")[0];
                //Console.Init(ControlPanel.videoModes[videoMode].Item2.Width, ControlPanel.videoModes[videoMode].Item2.Height);
            }
            catch
            {
                log(ThemeManager.Other1, "GoOS - Failed to load settings, continuing with default values...");
            }

            if (username == null || username == "")
            {
                username = "user";
            }

            if (computername == null || computername == "")
            {
                computername = "GoOS";
            }

            using (var xClient = new DHCPClient())
            {
                /** Send a DHCP Discover packet **/
                //This will automatically set the IP config after DHCP response
                xClient.SendDiscoverPacket();
                log(ConsoleColor.Blue, NetworkConfiguration.CurrentAddress.ToString());
            }

            loadingDialogue.Closing = true;
            WindowManager.Canvas = Display.GetDisplay(1600, 900);
            WindowManager.AddWindow(new Taskbar());
            WindowManager.AddWindow(new Desktop());
            WindowManager.AddWindow(new Welcome());

            Console.Clear();

            Canvas cv = Image.FromBitmap(rawBootLogo, false);
            Console.Canvas.DrawImage(0, 0, cv, false);
            Console.SetCursorPosition(0, 13);

            Directory.SetCurrentDirectory(@"0:\");
        }

        public static string currentdirfix = string.Empty;

        public static void DrawPrompt()
        {
            textcolour(ThemeManager.WindowText);
            string currentdir = Directory.GetCurrentDirectory() + @"\";
            currentdirfix = @"0:\";
            if (currentdir.Contains(@"0:\\"))
            {
                currentdirfix = currentdir.Replace(@"0:\\", @"0:\");
            }
            else if (currentdir.Contains(@"0:\\\"))
            {
                currentdirfix = currentdir.Replace(@"0:\\\", @"0:\");
            }

            write($"{username}");
            textcolour(ThemeManager.Other1);
            write("@");
            textcolour(ThemeManager.WindowText);
            write($"{computername} ");
            textcolour(ThemeManager.WindowBorder);
            write(currentdirfix);
            textcolour(ThemeManager.Default);
        }

        protected override void Run()
        {
            isGCIenabled = File.Exists(@"0:\content\sys\GCI.gms");

            if (isGCIenabled)
            {
                GoCodeInstaller.CheckForInstalledPrograms();
            }

            if (cutStatus == "FULL")
            {
            }

            if (cutStatus == "Single")
            {
            }

            DrawPrompt();

            // Commands section

            string[] args = Console.ReadLine().Trim().Split(' ');

            uint TotalRamUINT = Cosmos.Core.CPU.GetAmountOfRAM();
            int TotalRam = Convert.ToInt32(TotalRamUINT);

            switch (args[0])
            {
                case "gui":
                    Console.ConsoleMode = false;
                    WindowManager.Canvas = Display.GetDisplay(1280, 720);
                    WindowManager.AddWindow(new Taskbar());
                    WindowManager.AddWindow(new Desktop());
                    WindowManager.AddWindow(new Welcome());
                    break;
                case "exit":
                    Console.Visible = false;
                    break;
                case "ping":
                    Ping.Run();
                    break;
                case "install":
                    if (args.Length < 2)
                    {
                        log(ThemeManager.ErrorText, "Missing arguments!");
                        break;
                    }

                    if (args.Length > 2)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments!");
                        break;
                    }

                    GoCodeInstaller.Install(args[1]);
                    break;
                case "uninstall":
                    if (args.Length < 2)
                    {
                        log(ThemeManager.ErrorText, "Missing arguments!");
                        break;
                    }

                    if (args.Length > 2)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments!");
                        break;
                    }

                    GoCodeInstaller.Uninstall(args[1]);
                    break;
                case "movefile":
                    if (args.Length < 3)
                    {
                        log(ThemeManager.ErrorText, "Missing arguments!");
                        break;
                    }

                    if (args.Length > 3)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments!");
                        break;
                    }

                    try
                    {
                        ExtendedFilesystem.MoveFile(args[1], args[2]);
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine("Error whilst trying to move file: " + e);
                        break;
                    }

                    break;
                case "copyfile":
                    if (args.Length < 3)
                    {
                        log(ThemeManager.ErrorText, "Missing arguments!");
                        break;
                    }

                    if (args.Length > 3)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments!");
                        break;
                    }

                    try
                    {
                        ExtendedFilesystem.CopyFile(args[1], args[2]);
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine("Error whilst trying to copy file: " + e);
                        break;
                    }

                    break;
                case "help":
                    if (args.Length > 1)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                    }

                    Commands.Help.Main();
                    break;
                case "go":
                    if (args.Length < 2)
                    {
                        log(ThemeManager.ErrorText, "Insufficient arguments");
                        break;
                    }

                    switch (args[1])
                    {
                        case "repo":
                            log(Color.Minty, "GoOS - Application Repositorys");
                            log(Color.GoogleYellow, "-a apps.goos.owen2k6.com");
                            log(Color.GoogleYellow, "-b dev.apps.goos.owen2k6.com");
                            break;
                        case "type":
                            log(Color.Minty, "GoOS - Application Types");
                            log(Color.GoogleYellow, "-g Goexe");
                            log(Color.GoogleYellow, "-9 9xCode");
                            break;
                        case "install":
                            if (args.Length != 5)
                            {
                                log(ThemeManager.ErrorText, "X: go install -<repo> <appname> -<type>");
                                break;
                            }
                            String filetoget = args[3];
                            try
                            {
                                var dnsClient = new DnsClient();
                                var tcpClient = new TcpClient();
                                string repo;
                                string type;
                                //temporary use of local repository list
                                if (args[2] == "-a")
                                {
                                    repo = "apps.goos.owen2k6.com";
                                    log(Color.Red, repo);
                                }
                                else if (args[2] == "-b")
                                {
                                    repo = "dev.apps.goos.owen2k6.com";
                                    log(Color.Red, repo);
                                }
                                else
                                {
                                    log(ThemeManager.ErrorText, "Unknown repository");
                                    break;
                                }

                                if (args[4] == "-g")
                                {
                                    type = "goexe";
                                    log(Color.Red, type);
                                }
                                else if (args[4] == "-9")
                                {
                                    type = "9x";
                                    log(Color.Red, type);
                                }
                                else
                                {
                                    log(ThemeManager.ErrorText, "Unknown application type");
                                    break;
                                }
                                log(Color.Red, repo+"/"+filetoget+"."+type);

                                dnsClient.Connect(DNSConfig.DNSNameservers[0]);
                                dnsClient.SendAsk(repo);
                                Address address = dnsClient.Receive();
                                log(Color.Red, address.ToString());
                                dnsClient.Close();

                                tcpClient.Connect(address, 80);

                                string httpget = "GET /" + filetoget + "."+type+" HTTP/1.1\r\n" +
                                                 "User-Agent: GoOS\r\n" +
                                                 "Accept: */*\r\n" +
                                                 "Accept-Encoding: identity\r\n" +
                                                 "Host: "+repo+"\r\n" +
                                                 "Connection: Keep-Alive\r\n\r\n";

                                tcpClient.Send(Encoding.ASCII.GetBytes(httpget));

                                var ep = new EndPoint(Address.Zero, 0);
                                var data = tcpClient.Receive(ref ep);
                                tcpClient.Close();

                                string httpresponse = Encoding.ASCII.GetString(data);

                                string[] responseParts =
                                    httpresponse.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);

                                if (responseParts.Length == 2)
                                {
                                    string headers = responseParts[0];
                                    string content = responseParts[1];
                                    //Console.WriteLine(content);

                                    File.Create(@"0:\" + filetoget + "."+type);
                                    File.WriteAllText(@"0:\" + filetoget + "."+type, content);
                                    log(Color.Green, "Downloaded " + filetoget + "."+type);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }

                            break;
                        default:
                            log(ThemeManager.ErrorText, "Unknown request.");
                            break;
                    }

                    break;
                case "run":
                    if (args.Length > 2)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                        break;
                    }

                    if (args.Length == 1)
                    {
                        log(ThemeManager.ErrorText, "Missing arguments");
                        break;
                    }

                    Commands.Run.Main(args[1]);
                    break;
                case "mkdir":
                    if (args.Length > 2)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                        break;
                    }

                    if (args.Length == 1)
                    {
                        log(ThemeManager.ErrorText, "Missing arguments");
                        break;
                    }

                    Commands.Make.MakeDirectory(args[1]);
                    break;
                case "mkfile":
                    if (args.Length > 2)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                        break;
                    }

                    if (args.Length == 1)
                    {
                        log(ThemeManager.ErrorText, "Missing arguments");
                        break;
                    }

                    Commands.Make.MakeFile(args[1]);
                    break;
                case "deldir":
                    if (args.Length > 2)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                        break;
                    }

                    if (args.Length == 1)
                    {
                        log(ThemeManager.ErrorText, "Missing arguments");
                        break;
                    }

                    Commands.Delete.DeleteDirectory(args[1]);
                    break;
                case "delfile":
                    if (args.Length > 2)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                        break;
                    }

                    if (args.Length == 1)
                    {
                        log(ThemeManager.ErrorText, "Missing arguments");
                        break;
                    }

                    Commands.Delete.DeleteFile(args[1]);
                    break;
                case "del":
                    if (args.Length > 2)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                        break;
                    }

                    if (args.Length == 1)
                    {
                        log(ThemeManager.ErrorText, "Missing arguments");
                        break;
                    }

                    Delete.UniveralDelete(args[1]);
                    break;
                case "cd":
                    if (args.Length > 2)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                        break;
                    }

                    if (args.Length == 1)
                    {
                        log(ThemeManager.ErrorText, "Missing arguments");
                        break;
                    }

                    Commands.Cd.Run(args[1]);
                    break;
                case "cd..":
                    if (args.Length > 1)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                        break;
                    }

                    // do it the ChaOS way, it works so dont touch else you're gay
                    try
                    {
                        Directory.SetCurrentDirectory(Directory.GetCurrentDirectory().TrimEnd('\\')
                            .Remove(Directory.GetCurrentDirectory().LastIndexOf('\\') + 1));
                        Directory.SetCurrentDirectory(Directory.GetCurrentDirectory()
                            .Remove(Directory.GetCurrentDirectory().Length - 1));
                    }
                    catch
                    {
                    }

                    if (!Directory.GetCurrentDirectory().StartsWith(@"0:\"))
                    {
                        Directory.SetCurrentDirectory(@"0:\"); // Directory error correction
                    }

                    break;
                case "cdr":
                    if (args.Length > 1)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                        break;
                    }

                    string roota = @"0:\";
                    Directory.SetCurrentDirectory(roota);
                    break;
                case "dir":
                    if (args.Length > 1)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                        break;
                    }

                    Commands.Dir.Run();
                    break;
                case "ls":
                    if (args.Length > 1)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                        break;
                    }

                    Commands.Dir.Run();
                    break;
                case "notepad":
                    if (TotalRam < 1000)
                    {
                        log(ThemeManager.ErrorText, "This program has been disabled due to low ram.");
                        break;
                    }

                    if (args.Length > 2)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                        break;
                    }

                    if (args.Length == 1)
                    {
                        log(ThemeManager.ErrorText, "Missing arguments");
                        break;
                    }

                    if (args[1].EndsWith(".gms"))
                    {
                        log(ThemeManager.ErrorText,
                            "Files that end with .gms cannot be opened. they are protected files.");
                        break;
                    }

                    textcolour(ThemeManager.Default);
                    var editor = new TextEditor(Util.Paths.JoinPaths(currentdirfix, args[1]));
                    editor.Start();
                    break;
                case "settings":
                    ControlPanel.Launch();
                    break;
                case "vm":
                    if (args.Length > 2)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                        break;
                    }

                    if (args.Length == 1)
                    {
                        log(ThemeManager.ErrorText, "Missing arguments");
                        break;
                    }

                    VM.Run(args[1]);
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "settheme":
                    if (args.Length > 2)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                        break;
                    }

                    if (args.Length == 1)
                    {
                        log(ThemeManager.ErrorText, "Missing arguments");
                        break;
                    }

                    ThemeManager.SetTheme(@"0:\content\themes\" + args[1]);
                    break;
                case "systest":
                    systest.run();
                    break;
                case "whoami":
                    log(ThemeManager.ErrorText, "Showing Internet Information");
                    log(ThemeManager.ErrorText, NetworkConfiguration.CurrentAddress.ToString());
                    break;
                case "lr":
                    if (args[1] == "get")
                    {
                        string app = new GoOS.Util.localRepo().GetFile(args[2]);
                        log(ThemeManager.WindowText, app);
                    }
                    else
                    {
                        log(ThemeManager.ErrorText, "Unknown order.");
                    }

                    break;
                case "mode":
                    if (args.Length > 3)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                        break;
                    }

                    if (args.Length == 1)
                    {
                        log(ThemeManager.ErrorText, "Missing arguments");
                        break;
                    }

                    Console.Init(Convert.ToUInt16(args[1]), Convert.ToUInt16(args[2]));
                    break;
                case "dtest":
                    Dialogue.Show("Message", "Hello world!!!");
                    break;
                case "dotnet":
                    var fl = new DotNetFile(Directory.GetCurrentDirectory() + args[1]);
                    DotNetClr.DotNetClr clr = new DotNetClr.DotNetClr(fl, @"0:\framework");
                    clr.Start();
                    break;
                case "9xcode":
                    if (args.Length > 2)
                    {
                        log(ThemeManager.ErrorText, "Too many arguments");
                        break;
                    }

                    if (args.Length == 1)
                    {
                        log(ThemeManager.ErrorText, "Missing arguments");
                        break;
                    }

                    _9xCode.Interpreter.Run(Directory.GetCurrentDirectory() + args[1]);
                    break;
                default:
                    if (isGCIenabled)
                    {
                        GoCodeInstaller.CheckForInstalledPrograms();
                    }

                    if (InstalledPrograms.ContainsKey(args[0]))
                    {
                        string rootass = @"0:\";

                        string currentDIRRRRRR = Directory.GetCurrentDirectory();

                        Directory.SetCurrentDirectory(rootass);

                        InstalledPrograms.TryGetValue(args[0], out string locat);

                        string TrueLocat = locat;

                        if (locat.Contains(@"0:\"))
                        {
                            TrueLocat = TrueLocat.Replace(@"0:\", "");
                        }

                        Commands.Run.Main(TrueLocat);

                        Directory.SetCurrentDirectory(currentDIRRRRRR);
                        break;
                    }

                    Console.WriteLine("Invalid command.");
                    break;
            }
        }

        public static List OW;
    }
}