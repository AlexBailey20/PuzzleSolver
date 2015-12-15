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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SolveButton = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.NotificationBox = new System.Windows.Forms.RichTextBox();
            this.RotationCheck = new System.Windows.Forms.CheckBox();
            this.ReflectionCheck = new System.Windows.Forms.CheckBox();
            this.ComponentsList = new System.Windows.Forms.ListView();
            this.SolutionsList = new System.Windows.Forms.ListView();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1394, 40);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
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
            this.SolveButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SolveButton.Location = new System.Drawing.Point(12, 43);
            this.SolveButton.Name = "SolveButton";
            this.SolveButton.Size = new System.Drawing.Size(100, 100);
            this.SolveButton.TabIndex = 2;
            this.SolveButton.Text = "Solve";
            this.SolveButton.UseVisualStyleBackColor = true;
            this.SolveButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.PauseButton.Location = new System.Drawing.Point(118, 43);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(100, 100);
            this.PauseButton.TabIndex = 5;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            // 
            // StopButton
            // 
            this.StopButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.StopButton.Location = new System.Drawing.Point(224, 43);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(100, 100);
            this.StopButton.TabIndex = 6;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
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
            this.RotationCheck.AutoSize = true;
            this.RotationCheck.Location = new System.Drawing.Point(363, 80);
            this.RotationCheck.Name = "RotationCheck";
            this.RotationCheck.Size = new System.Drawing.Size(210, 29);
            this.RotationCheck.TabIndex = 8;
            this.RotationCheck.Text = "Include Rotations";
            this.RotationCheck.UseVisualStyleBackColor = true;
            // 
            // ReflectionCheck
            // 
            this.ReflectionCheck.AutoSize = true;
            this.ReflectionCheck.Location = new System.Drawing.Point(596, 80);
            this.ReflectionCheck.Name = "ReflectionCheck";
            this.ReflectionCheck.Size = new System.Drawing.Size(226, 29);
            this.ReflectionCheck.TabIndex = 9;
            this.ReflectionCheck.Text = "Include Reflections";
            this.ReflectionCheck.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.ComponentsList.Location = new System.Drawing.Point(12, 149);
            this.ComponentsList.Name = "ComponentsList";
            this.ComponentsList.Size = new System.Drawing.Size(1370, 225);
            this.ComponentsList.TabIndex = 10;
            this.ComponentsList.UseCompatibleStateImageBehavior = false;
            // 
            // listView2
            // 
            this.SolutionsList.Location = new System.Drawing.Point(12, 380);
            this.SolutionsList.Name = "SolutionsList";
            this.SolutionsList.Size = new System.Drawing.Size(1370, 225);
            this.SolutionsList.TabIndex = 11;
            this.SolutionsList.UseCompatibleStateImageBehavior = false;
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1394, 672);
            this.Controls.Add(this.SolutionsList);
            this.Controls.Add(this.ComponentsList);
            this.Controls.Add(this.ReflectionCheck);
            this.Controls.Add(this.RotationCheck);
            this.Controls.Add(this.NotificationBox);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.PauseButton);
            this.Controls.Add(this.SolveButton);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimizeBox = false;
            this.Name = "GUI";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PuzzleSolver";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Button SolveButton;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.Button PauseButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.RichTextBox NotificationBox;
        private System.Windows.Forms.CheckBox RotationCheck;
        private System.Windows.Forms.CheckBox ReflectionCheck;
        private System.Windows.Forms.ListView ComponentsList;
        private System.Windows.Forms.ListView SolutionsList;
    }
}