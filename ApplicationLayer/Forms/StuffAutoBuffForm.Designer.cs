namespace RagnarokHotKeyInWinforms.Forms
{
    partial class StuffAutoBuffForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StuffAutoBuffForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.EtcGP = new System.Windows.Forms.GroupBox();
            this.ScrollBuffsGP = new System.Windows.Forms.GroupBox();
            this.ElementalsGP = new System.Windows.Forms.GroupBox();
            this.BoxesGP = new System.Windows.Forms.GroupBox();
            this.PotionsGP = new System.Windows.Forms.GroupBox();
            this.FoodsGP = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 10;
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 10;
            this.toolTip1.ReshowDelay = 2;
            // 
            // EtcGP
            // 
            resources.ApplyResources(this.EtcGP, "EtcGP");
            this.EtcGP.Name = "EtcGP";
            this.EtcGP.TabStop = false;
            // 
            // ScrollBuffsGP
            // 
            resources.ApplyResources(this.ScrollBuffsGP, "ScrollBuffsGP");
            this.ScrollBuffsGP.Name = "ScrollBuffsGP";
            this.ScrollBuffsGP.TabStop = false;
            // 
            // ElementalsGP
            // 
            resources.ApplyResources(this.ElementalsGP, "ElementalsGP");
            this.ElementalsGP.Name = "ElementalsGP";
            this.ElementalsGP.TabStop = false;
            // 
            // BoxesGP
            // 
            resources.ApplyResources(this.BoxesGP, "BoxesGP");
            this.BoxesGP.Name = "BoxesGP";
            this.BoxesGP.TabStop = false;
            // 
            // PotionsGP
            // 
            resources.ApplyResources(this.PotionsGP, "PotionsGP");
            this.PotionsGP.Name = "PotionsGP";
            this.PotionsGP.TabStop = false;
            // 
            // FoodsGP
            // 
            resources.ApplyResources(this.FoodsGP, "FoodsGP");
            this.FoodsGP.Name = "FoodsGP";
            this.FoodsGP.TabStop = false;
            // 
            // StuffAutoBuffForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EtcGP);
            this.Controls.Add(this.ScrollBuffsGP);
            this.Controls.Add(this.ElementalsGP);
            this.Controls.Add(this.BoxesGP);
            this.Controls.Add(this.PotionsGP);
            this.Controls.Add(this.FoodsGP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StuffAutoBuffForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox EtcGP;
        private System.Windows.Forms.GroupBox ScrollBuffsGP;
        private System.Windows.Forms.GroupBox ElementalsGP;
        private System.Windows.Forms.GroupBox BoxesGP;
        private System.Windows.Forms.GroupBox PotionsGP;
        private System.Windows.Forms.GroupBox FoodsGP;
    }
}