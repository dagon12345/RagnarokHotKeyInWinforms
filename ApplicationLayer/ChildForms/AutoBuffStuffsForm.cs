using ApplicationLayer.Designer;
using ApplicationLayer.Interface;
using ApplicationLayer.Models.RagnarokModels;
using Domain.Constants;
using Domain.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ApplicationLayer.ChildForms
{
    public partial class AutoBuffStuffsForm : Form
    {
        public string email;
        private readonly IBaseTableService _baseTableService;
        private readonly IUserSettingService _userSettingService;
        private List<BuffContainer> stuffBuffContainers = new List<BuffContainer>();
        public AutoBuffStuffsForm(IUserSettingService userSettingService, IBaseTableService baseTableService)
        {
            InitializeComponent();
            //Centralize color
            DesignerService.ApplyDarkBlueTheme(this);
            _userSettingService = userSettingService;
            _baseTableService = baseTableService;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _ = LoadAsync(); // Fire and forget safely
        }
        private async Task LoadAsync()
        {
            try
            {
                await RetrieveStuffAutobuffForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        //Get the reference code of the user
        private async Task<UserSettings> ReturnToggleKey()
        {
            var getBaseTable = await _baseTableService.SearchUser(email);
            var toggleStateValue = await _userSettingService.SelectUserPreference(getBaseTable.ReferenceCode);
            return toggleStateValue;
        }
        private async Task RetrieveStuffAutobuffForm()
        {

            //Load the stuff containers
            stuffBuffContainers.Add(new BuffContainer(this.PotionsGP, Buff.GetPotionsBuffs()));
            stuffBuffContainers.Add(new BuffContainer(this.ElementalsGP, Buff.GetElementalsBuffs()));
            stuffBuffContainers.Add(new BuffContainer(this.BoxesGP, Buff.GetBoxesBuffs()));
            stuffBuffContainers.Add(new BuffContainer(this.FoodsGP, Buff.GetFoodBuffs()));
            stuffBuffContainers.Add(new BuffContainer(this.ScrollBuffsGP, Buff.GetScrollBuffs()));
            stuffBuffContainers.Add(new BuffContainer(this.EtcGP, Buff.GetETCBuffs()));


            //trigger the containers and textboxes doRender()
            new BuffRenderer(stuffBuffContainers, toolTipAutoBuff).doRender();

            // Retrieve the setting
            var userToggleState = await ReturnToggleKey();
            var jsonObject = JsonSerializer.Deserialize<AutoBuff>(userToggleState.Autobuff);

            // Return the dictionary
            var autoBuffClones = new Dictionary<EffectStatusIdEnum, Key>(jsonObject.buffMapping);
            // Assign key values to corresponding textboxes
            foreach (KeyValuePair<EffectStatusIdEnum, Key> config in autoBuffClones)
            {
                bool found = false;

                foreach (BuffContainer container in stuffBuffContainers) // Iterate over all containers
                {
                    Control[] foundControls = container.container.Controls.Find(config.Key.ToString(), true);
                    if (foundControls.Length > 0 && foundControls[0] is TextBox textBox)
                    {
                        textBox.Text = config.Value.ToString(); // Set the assigned key
                        found = true;

                        break; // Stop searching once found
                    }
                }

                if (!found)
                {
                    Console.WriteLine($"Textbox for '{config.Key}' not found in any group!");
                }

            }
            // Attach event handlers to textboxes across all GroupBoxes
            foreach (BuffContainer container in stuffBuffContainers)
            {
                foreach (Control c in container.container.Controls)
                {
                    if (c is TextBox textBox)
                    {
                        textBox.TextChanged += onTextChange;
                    }
                }
            }
        }
        //Updating when text changed.
        private async void onTextChange(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                if (!string.IsNullOrWhiteSpace(txtBox.Text))
                {
                    if (!Enum.TryParse(txtBox.Name, out EffectStatusIdEnum statusID))
                    {
                        Console.WriteLine($"Invalid EffectStatusID from TextBox name: {txtBox.Name}");
                        return;
                    }

                    if (!Enum.TryParse(txtBox.Text, out Key key))
                    {
                        Console.WriteLine($"Invalid Key input: {txtBox.Text}");
                        return;
                    }

                    var userToggleState = await ReturnToggleKey();
                    var jsonObject = JsonSerializer.Deserialize<AutoBuff>(userToggleState.Autobuff);

                    jsonObject.AddKeyToBuff(statusID, key);

                    var updatedJson = JsonSerializer.Serialize(jsonObject);
                    userToggleState.Autobuff = updatedJson;


                    await _userSettingService.SaveChangesAsync(userToggleState);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in onTextChange: {ex.Message}");
            }
        }
    }
}
