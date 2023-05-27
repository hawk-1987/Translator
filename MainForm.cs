using Translator.Analysis;
using Translator.Analysis.Errors;

namespace Translator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void mExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                edtCode.LoadFile(openFileDialog.FileName);
                lstErrors.Items.Clear();
            }
        }

        private void mSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                edtCode.SaveFile(saveFileDialog.FileName);
            }
        }

        private void mPerform_Click(object sender, EventArgs e)
        {
            lstErrors.Items.Clear();
            try
            {
                LexicalAnalyzer.Analyze(edtCode.Text);
                Parser.Parse();
                lstErrors.Items.Add("Программа соответствует грамматике");
                TablesForm tf = new TablesForm();
                tf.ShowDialog();
            }
            catch (Error error)
            {
                lstErrors.Items.Add(error.Message);
            }
        }

        private void mTables_Click(object sender, EventArgs e)
        {
            TablesForm tf = new TablesForm();
            tf.ShowDialog();
        }
    }
}