using RagnarokHotKeyInWinforms.Utilities;
using System.Collections.Generic;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Windows.Input;

namespace ApplicationLayer.Models.RagnarokModels
{
    public class BuffRenderer
    {
        private readonly int BUFFS_PER_ROW = 5;
        private readonly int DISTANCE_BETWEEN_CONTAINERS = 10;
        private readonly int DISTANCE_BETWEEN_ROWS = 30;

        private List<BuffContainer> _containers;
        private ToolTip _toolTip;

        public BuffRenderer(List<BuffContainer> containers, ToolTip toolTip)
        {
            this._containers = containers;
            this._toolTip = toolTip;
        }


        /// <summary>
        /// Settings to display the icon at the left side and the textbox in the rightside
        /// </summary>
        public void doRender()
        {
            for (int i = 0; i < _containers.Count; i++)
            {
                BuffContainer bk = _containers[i];
                Point lastLocation = new Point(bk.container.Location.X, 20);
                int colCount = 0;

                if (i > 0)
                {
                    //If not first container to be rendered, get last container height and append 70
                    bk.container.Location = new Point(_containers[i - 1].container.Location.X, _containers[i - 1].container.Location.Y + _containers[i - 1].container.Height + DISTANCE_BETWEEN_CONTAINERS);
                }
                foreach (Buff skill in bk.skills)
                {
                    PictureBox pb = new PictureBox();
                    TextBox textBox = new TextBox();

                    pb.Image = skill.icon;
                    pb.BackgroundImageLayout = ImageLayout.Center;
                    pb.Location = new Point(lastLocation.X + (colCount * 100), lastLocation.Y);
                    pb.Name = "pbox" + ((int)skill.effectStatusID);
                    pb.Size = new Size(26, 26);
                    _toolTip.SetToolTip(pb, skill.name);

                    textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                    textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                    textBox.TextChanged += new EventHandler(onTextChange);
                    textBox.Size = new Size(55, 20);
                    textBox.Tag = ((int)skill.effectStatusID);

                    // Change this:
                    textBox.Name = skill.effectStatusID.ToString(); // Ensure exact match with enum
                    textBox.Location = new Point(pb.Location.X + 35, pb.Location.Y + 3);

                    bk.container.Controls.Add(textBox);
                    bk.container.Controls.Add(pb);

                    colCount++;

                    if (colCount == BUFFS_PER_ROW)
                    {
                        colCount = 0;
                        lastLocation = new Point(bk.container.Location.X, lastLocation.Y + DISTANCE_BETWEEN_ROWS);
                    }
                }

            }
        }

        private void onTextChange(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;

                if (!string.IsNullOrWhiteSpace(txtBox.Text))
                {
                    // Parse Key from TextBox value
                    if (!Enum.TryParse(txtBox.Text, out Key key))
                    {
                        Console.WriteLine($"Error: Invalid key format '{txtBox.Text}'");
                        return;
                    }

                    // Try to convert TextBox Name directly to EffectStatusIDs
                    if (!Enum.TryParse(txtBox.Name, out EffectStatusIDs statusID))
                    {
                        Console.WriteLine($"Error: TextBox name '{txtBox.Name}' does not match expected EffectStatusIDs.");
                        return;
                    }

                    Console.WriteLine($"Successfully updated: {statusID} -> {key}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in onTextChange: {ex.Message}");
            }
        }
    }
}
