using System.CodeDom;
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
                    tb.ForeColor = Color.LightGray;
                    tb.BorderStyle = BorderStyle.None;
                    tb.Font = new Font("Segoe UI", 11, FontStyle.Bold);
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
                    lbl.ForeColor = Color.LightGray;
                    lbl.Font = new Font("Segoe UI", 8, FontStyle.Bold);

                }

                if(ctrl is NumericUpDown nu)
                {
                    nu.BorderStyle = BorderStyle.None;
                    nu.BackColor = Color.FromArgb(23, 32, 42); // Deep navy
                    nu.ForeColor = Color.White;
                    nu.Controls[0].BackColor = Color.FromArgb(33, 97, 140); // Up/Down button background
                    nu.Controls[0].Paint += (s, e) => { }; // Optional: suppress default painting
                }
                if (ctrl is TabControl tab) { 
                    tab.BackColor = Color.FromArgb(23, 32, 42); // Deep navy
                    tab.Font = new Font("Segoe UI", 8, FontStyle.Regular);
                }
                if(ctrl is ComboBox cmb)
                {
                   cmb.DrawMode = DrawMode.OwnerDrawFixed;
                   cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                   cmb.FlatStyle = FlatStyle.Flat;
                   cmb.BackColor = Color.FromArgb(23, 32, 42); // Deep navy
                   cmb.ForeColor = Color.White;
                }
                if (ctrl is CheckBox chk)
                {
                    chk.ForeColor = Color.LightGray;
                    chk.Font = new Font("Segoe UI", 8, FontStyle.Bold);
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
