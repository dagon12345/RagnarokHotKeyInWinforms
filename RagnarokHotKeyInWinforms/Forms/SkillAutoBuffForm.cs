using _4RTools.Model;
using RagnarokHotKeyInWinforms.Model;
using RagnarokHotKeyInWinforms.Utilities;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace RagnarokHotKeyInWinforms.Forms
{
    public partial class SkillAutoBuffForm : Form, IObserver
    {
        private List<BuffContainer> skillContainers = new List<BuffContainer>();

        public SkillAutoBuffForm(Subject subject)
        {
            this.KeyPreview = true;
            InitializeComponent();
            skillContainers.Add(new BuffContainer(this.ArcherSkillsGP, Buff.GetArcherSkills()));
            skillContainers.Add(new BuffContainer(this.SwordmanSkillGP, Buff.GetSwordmanSkill()));
            skillContainers.Add(new BuffContainer(this.MageSkillGP, Buff.GetMageSkills()));
            skillContainers.Add(new BuffContainer(this.MerchantSkillsGP, Buff.GetMerchantSkills()));
            skillContainers.Add(new BuffContainer(this.ThiefSkillsGP, Buff.GetThiefSkills()));
            skillContainers.Add(new BuffContainer(this.AcolyteSkillsGP, Buff.GetAcolyteSkills()));
            skillContainers.Add(new BuffContainer(this.TKSkillGroupBox, Buff.GetTaekwonSkills()));
            skillContainers.Add(new BuffContainer(this.NinjaSkillsGP, Buff.GetNinjaSkills()));
            skillContainers.Add(new BuffContainer(this.GunsSkillsGP, Buff.GetGunsSkills()));

            //This is where the textbox and picture trigger you can resize the textbox and arrange here.
            new BuffRenderer(skillContainers, toolTip1).doRender();
            subject.Attach(this);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    BuffRenderer.doUpdate(new Dictionary<EffectStatusIDs, Key>(ProfileSingleton.GetCurrent().AutoBuff.buffMapping), this);
                    break;
            }
        }
    }
}
