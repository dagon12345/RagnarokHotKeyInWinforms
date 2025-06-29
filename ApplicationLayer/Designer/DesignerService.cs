using System.Drawing;
using System.Windows.Forms;

namespace ApplicationLayer.Designer
{
    public static class DesignerService
    {
        public static void ApplyDarkBlueTheme(Control container)
        {
            container.BackColor = Color.FromArgb(23, 32, 42); // Deep navy
            foreach (Control ctrl in container.Controls)
            {
                ctrl.ForeColor = Color.White;

                if (ctrl is TextBox tb)
                {
                    tb.BackColor = Color.FromArgb(33, 47, 61);
                    tb.ForeColor = Color.White;
                    tb.BorderStyle = BorderStyle.FixedSingle;
                }

                if (ctrl is Button btn)
                {
                    btn.BackColor = Color.FromArgb(41, 128, 185);
                    btn.ForeColor = Color.White;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Font = new Font("Segoe UI", 8, FontStyle.Bold);
                }

                if (ctrl is Label lbl)
                {
                    lbl.Font = new Font("Segoe UI", 8, FontStyle.Bold);
                }

                // Recursively apply to nested controls
                if (ctrl.HasChildren)
                {
                    ApplyDarkBlueTheme(ctrl);
                }
            }
        }
    }
}
