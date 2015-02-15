using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Genbox.Extensibility.Vsix
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    // This attribute registers a tool window exposed by this package.
    [ProvideToolWindow(typeof(GenboxToolWindow))]
    [Guid(GuidList.GenboxPackage)]
    public sealed class GenboxPackage : Package
    {
        /// <summary>
        /// This function is called when the user clicks the menu item that shows the 
        /// tool window. See the Initialize method to see how the menu item is associated to 
        /// this function using the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindowPane window = this.FindToolWindow(typeof(GenboxToolWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }
            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation

        #region Package Members
        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                var menuItem = new MenuCommand(GeneratePassword, new CommandID(GuidList.GenboxCmdSet, (int)PkgCmdIDList.CommandIdPassword));
                mcs.AddCommand(menuItem);
                //// Create the command for the tool window
                //CommandID toolwndCommandID = new CommandID(GuidList.GenboxCmdSet, (int)PkgCmdIDList.cmdidGenbox);
                //MenuCommand menuToolWin = new MenuCommand(ShowToolWindow, toolwndCommandID);
                //mcs.AddCommand( menuToolWin );
            }
        }
        #endregion

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void GeneratePassword(object sender, EventArgs e)
        {
            var dte = (DTE)GetService(typeof(DTE));
            var commands = dte.Commands;
            if (dte.ActiveDocument != null)
            {
                var selection = dte.ActiveDocument.Selection as TextSelection;
                if (selection != null)
                {
                    selection.Insert("This is a test");
                }
            }
        }
    }
}