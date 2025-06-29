using System.Collections.Generic;
using System.Windows.Forms;

namespace ApplicationLayer.Models.RagnarokModels
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
