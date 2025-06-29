using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Infrastructure.Utilities
{
    public class FormUtilities
    {
        public static void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;
                Keys thisk = (Keys)Enum.Parse(typeof(Keys), e.KeyCode.ToString());

                switch (thisk)
                {
                    case Keys.Escape:
                    case Keys.Back:
                        textBox.Text = Keys.None.ToString();
                        break;
                    default:
                        textBox.Text = e.KeyCode.ToString();
                        break;
                }
                textBox.Parent.Focus();
                e.Handled = true;
            }
            catch
            {

                throw;
            }
        }
        public static bool IsValidKey(Keys key)
        {
            return (key != Keys.Back && key != Keys.Escape && key != Keys.None);
        }
        public static void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        public static IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                  .Concat(controls)
                                  .Where(c => c.GetType() == type);
        }
        private static void resetForm(Control control)
        {
            IEnumerable<Control> texts = GetAll(control, typeof(TextBox));
            IEnumerable<Control> checks = GetAll(control, typeof(CheckBox));
            IEnumerable<Control> combos = GetAll(control, typeof(ComboBox));

            foreach (Control c in texts)
            {
                TextBox textBox = (TextBox)c;
                textBox.Text = Keys.None.ToString();
            }
            foreach (Control c in checks)
            {
                CheckBox checkBox = (CheckBox)c;
                checkBox.Checked = false;
            }

            foreach (Control c in combos)
            {
                ComboBox comboBox = (ComboBox)c;
                if (comboBox.Items.Count > 0)
                    comboBox.SelectedIndex = 0;
            }
        }
        public static void ResetForm(Form form)
        {
            resetForm(form);
        }

        public static void ResetForm(Control form)
        {
            resetForm(form);
        }

        public static void ResetForm(Panel panel)
        {
            resetForm(panel);
        }

        public static void ResetForm(GroupBox group)
        {
            resetForm(group);
        }
    }
}
