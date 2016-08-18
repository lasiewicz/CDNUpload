namespace Uploader
{
    partial class Uploader
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
            this.RunJob = new System.Windows.Forms.Button();
            this.jobtextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // RunJob
            // 
            this.RunJob.Location = new System.Drawing.Point(99, 91);
            this.RunJob.Name = "RunJob";
            this.RunJob.Size = new System.Drawing.Size(75, 23);
            this.RunJob.TabIndex = 0;
            this.RunJob.Text = "test";
            this.RunJob.UseVisualStyleBackColor = true;
            this.RunJob.Click += new System.EventHandler(this.button1_Click);
            // 
            // jobtextbox
            // 
            this.jobtextbox.Location = new System.Drawing.Point(88, 147);
            this.jobtextbox.Name = "jobtextbox";
            this.jobtextbox.Size = new System.Drawing.Size(100, 20);
            this.jobtextbox.TabIndex = 1;
            this.jobtextbox.Text = "100037";
            this.jobtextbox.TextChanged += new System.EventHandler(this.jobtextbox_TextChanged);
            // 
            // Uploader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.jobtextbox);
            this.Controls.Add(this.RunJob);
            this.Name = "Uploader";
            this.Text = "Uploader";
            this.Load += new System.EventHandler(this.Uploader_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RunJob;
        private System.Windows.Forms.TextBox jobtextbox;
    }
}

