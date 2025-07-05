using System.ComponentModel;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;
using System;

namespace Infrastructure.Helpers
{
    public class ElevationHelper
    {
        public static bool EnsureElevated()
        {
            if (IsRunAsAdmin())
            {
                MessageBox.Show("Running as Administrator");
                return true;
            }

            MessageBox.Show("Still running as standard user");

            try
            {
                var proc = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    WorkingDirectory = Environment.CurrentDirectory,
                    FileName = Application.ExecutablePath,
                    Verb = "runas"
                };
                Process.Start(proc);
                return false; // Exit current instance
            }
            catch (Win32Exception ex) when (ex.NativeErrorCode == 1223)
            {
                MessageBox.Show("Admin privileges were denied.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }



        private static bool IsRunAsAdmin()
        {
            var id = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }


    }
}
