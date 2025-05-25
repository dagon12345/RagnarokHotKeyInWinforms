using System.Collections.Generic;
using System.Windows.Forms;
using RagnarokHotKeyInWinforms.Model;

namespace _4RTools.Model
{
    public class BuffContainer
    {
        public GroupBox container { get; set; }
        public List<Buff> skills { get; set; }

        public BuffContainer(GroupBox p, List<Buff> skills)
        {
            this.skills = skills;
            this.container = p;
        }
    }
}
