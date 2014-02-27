namespace RAWPrinter
{
    partial class Form1
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
            this.btSelectFileAndPrinter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btSelectFileAndPrinter
            // 
            this.btSelectFileAndPrinter.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSelectFileAndPrinter.Location = new System.Drawing.Point(12, 12);
            this.btSelectFileAndPrinter.Name = "btSelectFileAndPrinter";
            this.btSelectFileAndPrinter.Size = new System.Drawing.Size(286, 117);
            this.btSelectFileAndPrinter.TabIndex = 6;
            this.btSelectFileAndPrinter.Text = "Print EPL File";
            this.btSelectFileAndPrinter.UseVisualStyleBackColor = true;
            this.btSelectFileAndPrinter.Click += new System.EventHandler(this.btSelectFileAndPrinter_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 148);
            this.Controls.Add(this.btSelectFileAndPrinter);
            this.Name = "Form1";
            this.Text = "RAW EPL Printer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btSelectFileAndPrinter;
    }
}

