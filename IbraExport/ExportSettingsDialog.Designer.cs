﻿namespace IbraExport
{
    partial class ExportSettingsDialog
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
            this.elementSizeCheckBox = new System.Windows.Forms.CheckBox();
            this.elementSizeTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.polynomialDegreeTextBox = new System.Windows.Forms.TextBox();
            this.polynomialDegreeCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // elementSizeCheckBox
            // 
            this.elementSizeCheckBox.AutoSize = true;
            this.elementSizeCheckBox.Location = new System.Drawing.Point(6, 26);
            this.elementSizeCheckBox.Name = "elementSizeCheckBox";
            this.elementSizeCheckBox.Size = new System.Drawing.Size(113, 17);
            this.elementSizeCheckBox.TabIndex = 0;
            this.elementSizeCheckBox.Text = "Max. Element Size";
            this.elementSizeCheckBox.UseVisualStyleBackColor = true;
            this.elementSizeCheckBox.CheckedChanged += new System.EventHandler(this.ElementSizeCheckBox_CheckedChanged);
            // 
            // elementSizeTextBox
            // 
            this.elementSizeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.elementSizeTextBox.Enabled = false;
            this.elementSizeTextBox.Location = new System.Drawing.Point(152, 24);
            this.elementSizeTextBox.Name = "elementSizeTextBox";
            this.elementSizeTextBox.Size = new System.Drawing.Size(150, 20);
            this.elementSizeTextBox.TabIndex = 1;
            this.elementSizeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ElementSizeTextBox_Validating);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.polynomialDegreeCheckBox);
            this.groupBox1.Controls.Add(this.elementSizeCheckBox);
            this.groupBox1.Controls.Add(this.polynomialDegreeTextBox);
            this.groupBox1.Controls.Add(this.elementSizeTextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(308, 82);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Refinement";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(245, 100);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(164, 100);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // polynomialDegreeTextBox
            // 
            this.polynomialDegreeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.polynomialDegreeTextBox.Enabled = false;
            this.polynomialDegreeTextBox.Location = new System.Drawing.Point(152, 50);
            this.polynomialDegreeTextBox.Name = "polynomialDegreeTextBox";
            this.polynomialDegreeTextBox.Size = new System.Drawing.Size(150, 20);
            this.polynomialDegreeTextBox.TabIndex = 1;
            this.polynomialDegreeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.PolynomialDegreeTextBox_Validating);
            // 
            // polynomialDegreeCheckBox
            // 
            this.polynomialDegreeCheckBox.AutoSize = true;
            this.polynomialDegreeCheckBox.Location = new System.Drawing.Point(6, 52);
            this.polynomialDegreeCheckBox.Name = "polynomialDegreeCheckBox";
            this.polynomialDegreeCheckBox.Size = new System.Drawing.Size(137, 17);
            this.polynomialDegreeCheckBox.TabIndex = 0;
            this.polynomialDegreeCheckBox.Text = "Min. Polynomial Degree";
            this.polynomialDegreeCheckBox.UseVisualStyleBackColor = true;
            this.polynomialDegreeCheckBox.CheckedChanged += new System.EventHandler(this.PolynomialDegreeCheckBox_CheckedChanged);
            // 
            // ExportSettingsDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(332, 135);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportSettingsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "IBRA Export Settings";
            this.Load += new System.EventHandler(this.ExportSettingsDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox elementSizeCheckBox;
        private System.Windows.Forms.TextBox elementSizeTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.CheckBox polynomialDegreeCheckBox;
        private System.Windows.Forms.TextBox polynomialDegreeTextBox;
    }
}