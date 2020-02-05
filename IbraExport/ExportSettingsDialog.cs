using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IbraExport
{
    public partial class ExportSettingsDialog : Form
    {
        public ExportSettingsDialog()
        {
            InitializeComponent();
        }

        private void ExportSettingsDialog_Load(object sender, EventArgs e)
        {
            elementSizeCheckBox.Checked = Info.Instance.Settings.GetBool("EnableMaxElementSize", false);
            elementSizeTextBox.Text = Info.Instance.Settings.GetDouble("MaxElementSize", 10.0).ToString();

            polynomialDegreeCheckBox.Checked = Info.Instance.Settings.GetBool("EnableMinPolynomialDegree", false);
            polynomialDegreeTextBox.Text = Info.Instance.Settings.GetInteger("MinPolynomialDegree", 1).ToString();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Info.Instance.Settings.SetBool("EnableMaxElementSize", elementSizeCheckBox.Checked);
            Info.Instance.Settings.SetDouble("MaxElementSize", double.Parse(elementSizeTextBox.Text));

            Info.Instance.Settings.SetBool("EnableMinPolynomialDegree", polynomialDegreeCheckBox.Checked);
            Info.Instance.Settings.SetInteger("MinPolynomialDegree", int.Parse(polynomialDegreeTextBox.Text));
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
    }
}
