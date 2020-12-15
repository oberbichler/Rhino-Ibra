using Rhino.Geometry;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace IbraTools
{
    public partial class ExportSettingsDialog : Form
    {
        public ExportSettingsDialog()
        {
            InitializeComponent();
        }

        private void ElementSizeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            elementSizeTextBox.Enabled = elementSizeCheckBox.Checked;
        }

        private void ElementSizeTextBox_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = !double.TryParse(elementSizeTextBox.Text, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out var _);
        }

        private void PolynomialDegreeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            polynomialDegreeTextBox.Enabled = polynomialDegreeCheckBox.Checked;
        }

        private void PolynomialDegreeTextBox_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = !(int.TryParse(elementSizeTextBox.Text, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out var value) && value > 0);
        }

        public void LoadData(GeometryBase geometry)
        {
            elementSizeCheckBox.Checked = geometry.GetUserString("Ibra.MaxElementSize") != null;
            elementSizeTextBox.Text = geometry.GetUserString("Ibra.MaxElementSize");

            polynomialDegreeCheckBox.Checked = geometry.GetUserString("Ibra.MinPolynomialDegree") != null;
            polynomialDegreeTextBox.Text = geometry.GetUserString("Ibra.MinPolynomialDegree");
        }

        public void SaveData(GeometryBase geometry)
        {
            if (elementSizeCheckBox.Checked)
                geometry.SetUserString("Ibra.MaxElementSize", elementSizeTextBox.Text);
            else
                geometry.UserDictionary.Remove("Ibra.MaxElementSize");

            if (polynomialDegreeCheckBox.Checked)
                geometry.SetUserString("Ibra.MinPolynomialDegree", elementSizeTextBox.Text);
            else
                geometry.UserDictionary.Remove("Ibra.MinPolynomialDegree");
        }
    }
}
