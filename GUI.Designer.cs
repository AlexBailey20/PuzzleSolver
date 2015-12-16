namespace PuzzleSolver
{
    partial class GUI
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
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SolveButton = new System.Windows.Forms.Button();
            this.NotificationBox = new System.Windows.Forms.RichTextBox();
            this.RotationCheck = new System.Windows.Forms.CheckBox();
            this.ReflectionCheck = new System.Windows.Forms.CheckBox();
            this.ComponentsList = new System.Windows.Forms.ListView();
            this.SolutionsList = new System.Windows.Forms.ListView();
            this.Target = new System.Windows.Forms.ListView();
            this.Current = new System.Windows.Forms.ListView();
            this.NextButton = new System.Windows.Forms.Button();
            this.PreviousButton = new System.Windows.Forms.Button();
            this.CurrentCheck = new System.Windows.Forms.CheckBox();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(1374, 40);
            this.MenuStrip.TabIndex = 0;
            this.MenuStrip.Text = "MenuStrip";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.createToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(64, 36);
            this.toolStripMenuItem1.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(174, 38);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(174, 38);
            // 
            // SolveButton
            // 
            this.SolveButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SolveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.SolveButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SolveButton.Location = new System.Drawing.Point(12, 43);
            this.SolveButton.Name = "SolveButton";
            this.SolveButton.Size = new System.Drawing.Size(100, 100);
            this.SolveButton.TabIndex = 2;
            this.SolveButton.Text = "Solve";
            this.SolveButton.UseVisualStyleBackColor = true;
            this.SolveButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // NotificationBox
            // 
            this.NotificationBox.Location = new System.Drawing.Point(13, 612);
            this.NotificationBox.Name = "NotificationBox";
            this.NotificationBox.Size = new System.Drawing.Size(1369, 49);
            this.NotificationBox.TabIndex = 7;
            this.NotificationBox.Text = "";
            // 
            // RotationCheck
            // 
            this.RotationCheck.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.RotationCheck.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.RotationCheck.Location = new System.Drawing.Point(564, 355);
            this.RotationCheck.Name = "RotationCheck";
            this.RotationCheck.Size = new System.Drawing.Size(210, 29);
            this.RotationCheck.TabIndex = 8;
            this.RotationCheck.Text = "Include Rotations";
            this.RotationCheck.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.RotationCheck.UseVisualStyleBackColor = true;
            // 
            // ReflectionCheck
            // 
            this.ReflectionCheck.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ReflectionCheck.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ReflectionCheck.Location = new System.Drawing.Point(564, 355);
            this.ReflectionCheck.Name = "ReflectionCheck";
            this.ReflectionCheck.Size = new System.Drawing.Size(226, 29);
            this.ReflectionCheck.TabIndex = 9;
            this.ReflectionCheck.Text = "Include Reflections";
            this.ReflectionCheck.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ReflectionCheck.UseVisualStyleBackColor = true;
            // 
            // ComponentsList
            // 
            this.ComponentsList.GridLines = true;
            this.ComponentsList.Location = new System.Drawing.Point(12, 149);
            this.ComponentsList.Name = "ComponentsList";
            this.ComponentsList.Size = new System.Drawing.Size(0, 0);
            this.ComponentsList.TabIndex = 13;
            this.ComponentsList.UseCompatibleStateImageBehavior = false;
            this.ComponentsList.View = System.Windows.Forms.View.Tile;
            // 
            // SolutionsList
            // 
            this.SolutionsList.GridLines = true;
            this.SolutionsList.Location = new System.Drawing.Point(12, 380);
            this.SolutionsList.Name = "SolutionsList";
            this.SolutionsList.Size = new System.Drawing.Size(0, 0);
            this.SolutionsList.TabIndex = 11;
            this.SolutionsList.UseCompatibleStateImageBehavior = false;
            this.SolutionsList.View = System.Windows.Forms.View.Tile;
            // 
            // Target
            // 
            this.Target.GridLines = true;
            this.Target.Location = new System.Drawing.Point(0, 0);
            this.Target.Name = "Target";
            this.Target.Size = new System.Drawing.Size(121, 97);
            this.Target.TabIndex = 0;
            this.Target.UseCompatibleStateImageBehavior = false;
            this.Target.View = System.Windows.Forms.View.Tile;
            // 
            // Current
            // 
            this.Current.GridLines = true;
            this.Current.Location = new System.Drawing.Point(0, 0);
            this.Current.Name = "Current";
            this.Current.Size = new System.Drawing.Size(121, 97);
            this.Current.TabIndex = 0;
            this.Current.UseCompatibleStateImageBehavior = false;
            this.Current.View = System.Windows.Forms.View.Tile;
            // 
            // NextButton
            // 
            this.NextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.NextButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.NextButton.Location = new System.Drawing.Point(516, 87);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(100, 100);
            this.NextButton.TabIndex = 14;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // PreviousButton
            // 
            this.PreviousButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.PreviousButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.PreviousButton.Location = new System.Drawing.Point(633, 96);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(100, 100);
            this.PreviousButton.TabIndex = 15;
            this.PreviousButton.Text = "Previous";
            this.PreviousButton.UseVisualStyleBackColor = true;
            this.PreviousButton.Click += new System.EventHandler(this.PreviousButton_Click);
            // 
            // CurrentCheck
            // 
            this.CurrentCheck.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CurrentCheck.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.CurrentCheck.Location = new System.Drawing.Point(557, 283);
            this.CurrentCheck.Name = "CurrentCheck";
            this.CurrentCheck.Size = new System.Drawing.Size(226, 29);
            this.CurrentCheck.TabIndex = 16;
            this.CurrentCheck.Text = "Show Current";
            this.CurrentCheck.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.CurrentCheck.UseVisualStyleBackColor = true;
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1374, 629);
            this.Controls.Add(this.CurrentCheck);
            this.Controls.Add(this.PreviousButton);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.SolutionsList);
            this.Controls.Add(this.ComponentsList);
            this.Controls.Add(this.ReflectionCheck);
            this.Controls.Add(this.RotationCheck);
            this.Controls.Add(this.NotificationBox);
            this.Controls.Add(this.SolveButton);
            this.Controls.Add(this.MenuStrip);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "GUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PuzzleSolver";
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Button SolveButton;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.RichTextBox NotificationBox;
        private System.Windows.Forms.CheckBox RotationCheck;
        private System.Windows.Forms.CheckBox ReflectionCheck;
        private System.Windows.Forms.ListView ComponentsList;
        private System.Windows.Forms.ListView SolutionsList;
        private System.Windows.Forms.ListView Target;
        private System.Windows.Forms.ListView Current;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Button PreviousButton;
        private System.Windows.Forms.CheckBox CurrentCheck;
    }
}