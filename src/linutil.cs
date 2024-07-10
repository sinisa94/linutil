namespace LinUtil {
    using Terminal.Gui;
    using System;

    public partial class LinUtil {
        
        public LinUtil() {
            SetupMenu();
        }

        private void SetupMenu() {

            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem("_File", new MenuItem[] {
                    new MenuItem("_Setup Bash Prompt", "", () => {
                        MessageBox.Query("Debug", "Setup Bash Prompt selected", "Ok");
                        SetupBashPromptCommand();
                    }),
                    new MenuItem("_Setup Neovim", "", () => {
                        MessageBox.Query("Debug", "Setup Neovim selected", "Ok");
                        SetupNeovimCommand();
                    }),
                    new MenuItem("_Quit", "", () => {
                        QuitCommand();
                    })
                })
            });

            Application.Top.Add(menu);
        }

        private void SetupBashPromptCommand() {
            MessageBox.Query("Debug", "Setup Bash Prompt command invoked", "Ok");
            // Implement the Setup Bash Prompt command logic here
        }

        private void SetupNeovimCommand() {
            MessageBox.Query("Debug", "Setup Neovim command invoked", "Ok");
            // Implement the Setup Neovim command logic here
        }

        private void QuitCommand() {
            MessageBox.Query("Debug", "Quit command invoked", "Ok");
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