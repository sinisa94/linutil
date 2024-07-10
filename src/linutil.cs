namespace LinUtil {
    using Terminal.Gui;
    using System;
    using System.Diagnostics;
    using System.IO;

    public partial class LinUtil {
        private View outputView = new View(); // Initialize here

        public LinUtil() {
            SetupMenu();
            SetupOutputView();
        }

        private void SetupMenu() {
            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem("_File", new MenuItem[] {
                    new MenuItem("_Setup Bash Prompt", "", () => {
                        RunBashScript("https://raw.githubusercontent.com/ChrisTitusTech/mybash/main/setup.sh");
                        Application.Refresh(); // Redraw the menu
                    }),
                    new MenuItem("_Setup Neovim", "", () => {
                        RunBashScript("https://raw.githubusercontent.com/ChrisTitusTech/neovim/main/setup.sh");
                        Application.Refresh(); // Redraw the menu
                    }),
                    new MenuItem("_Quit", "", () => {
                        QuitCommand();
                    })
                })
            });

            Application.Top.Add(menu);
        }

        private void SetupOutputView() {
            outputView = new View {
                X = 0,
                Y = 2, // Leave space for the menu and window title
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            Application.Top.Add(outputView);
        }

        private void RunBashScript(string scriptUrl) {
            var tempScriptPath = Path.Combine(Path.GetTempPath(), "temp_script.sh");

            var downloadProcess = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"curl -L {scriptUrl} -o {tempScriptPath} && chmod +x {tempScriptPath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            downloadProcess.OutputDataReceived += (sender, args) => {
                Application.MainLoop.Invoke(() => {
                    if (args.Data != null) {
                        var label = new Label(args.Data) {
                            X = 0,
                            Y = outputView.Subviews.Count // Position below the last added view
                        };
                        outputView.Add(label);
                    }
                });
            };

            downloadProcess.Start();
            downloadProcess.BeginOutputReadLine();
            downloadProcess.WaitForExit();

            var executeProcess = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{tempScriptPath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            executeProcess.OutputDataReceived += (sender, args) => {
                Application.MainLoop.Invoke(() => {
                    if (args.Data != null) {
                        var label = new Label(args.Data) {
                            X = 0,
                            Y = outputView.Subviews.Count // Position below the last added view
                        };
                        outputView.Add(label);
                    }
                });
            };

            executeProcess.Start();
            executeProcess.BeginOutputReadLine();
            executeProcess.WaitForExit();
        }

        private void QuitCommand() {
            Console.Clear(); // Clear the terminal screen
            Application.RequestStop();
        }

        public static void Main() {
            Application.Init();
            
            var top = Application.Top;

            var win = new Window("LinUtil") {
                X = 0,
                Y = 1, // Leave one row for the menu
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            top.Add(win);
            var linUtil = new LinUtil();

            Application.Run();
        }
    }
}