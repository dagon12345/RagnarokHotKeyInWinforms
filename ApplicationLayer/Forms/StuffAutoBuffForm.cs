using _4RTools.Model;
using RagnarokHotKeyInWinforms.Model;
using RagnarokHotKeyInWinforms.Utilities;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RagnarokHotKeyInWinforms.Forms
{
    public partial class StuffAutoBuffForm : Form, IObserver
    {
        private List<BuffContainer> stuffContainers = new List<BuffContainer>();
        public StuffAutoBuffForm(Subject subject)
        {
            InitializeComponent();
            stuffContainers.Add(new BuffContainer(this.PotionsGP, Buff.GetPotionsBuffs()));
            stuffContainers.Add(new BuffContainer(this.ElementalsGP, Buff.GetElementalsBuffs()));
            stuffContainers.Add(new BuffContainer(this.BoxesGP, Buff.GetBoxesBuffs()));
            stuffContainers.Add(new BuffContainer(this.FoodsGP, Buff.GetFoodBuffs()));
            stuffContainers.Add(new BuffContainer(this.ScrollBuffsGP, Buff.GetScrollBuffs()));
            stuffContainers.Add(new BuffContainer(this.EtcGP, Buff.GetETCBuffs()));

            //trigger the containers and textboxes doRender()
            new BuffRenderer(stuffContainers, toolTip1).doRender();

            subject.Attach(this);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    BuffRenderer.doUpdate(new Dictionary<EffectStatusIDs, Key>(ProfileSingleton.GetCurrent().AutoBuff.buffMapping), this);
                    break;
                case MessageCode.TURN_OFF:
                    ProfileSingleton.GetCurrent().AutoBuff.Stop();
                    break;
                case MessageCode.TURN_ON:
                    ProfileSingleton.GetCurrent().AutoBuff.Start();
                    break;
            }
        }
    }
}
