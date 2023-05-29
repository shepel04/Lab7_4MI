namespace Lab7_Winforms
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelAB = new System.Windows.Forms.Panel();
            this.panelCD = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelAB
            // 
            this.panelAB.Location = new System.Drawing.Point(12, 24);
            this.panelAB.Name = "panelAB";
            this.panelAB.Size = new System.Drawing.Size(450, 355);
            this.panelAB.TabIndex = 0;
            // 
            // panelCD
            // 
            this.panelCD.Location = new System.Drawing.Point(12, 398);
            this.panelCD.Name = "panelCD";
            this.panelCD.Size = new System.Drawing.Size(450, 355);
            this.panelCD.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1334, 568);
            this.Controls.Add(this.panelCD);
            this.Controls.Add(this.panelAB);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelAB;
        private System.Windows.Forms.Panel panelCD;
    }
}

