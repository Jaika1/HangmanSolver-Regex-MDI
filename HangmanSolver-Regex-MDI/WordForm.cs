using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HangmanSolver_Regex
{
    public partial class WordForm : Form
    {
        public WordForm()
        {
            InitializeComponent();
        }

        private void WordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            (MdiParent as Form1).wordForms.Remove(this);
        }

        private void textBoxRegex_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                (MdiParent as Form1).UpdateLists();
            }
        }

        public void ResetInfo()
        {
            textBoxRegex.Clear();
            listBoxFiltered.Items.Clear();
        }

        public void UpdateList(string badChars, ref List<string> words)
        {
            List<string> listBoxContents = new List<string>();
            for (int i = 0; i < listBoxFiltered.Items.Count; i++)
                listBoxContents.Add(listBoxFiltered.Items[i] as string);
            List<string> filterWords = (listBoxFiltered.Items.Count <= 0) ? words.ToList() : listBoxContents;
            int charCount = textBoxRegex.Text.Count();
            listBoxFiltered.Items.Clear();

            listBoxFiltered.Items.AddRange((from w in filterWords
                                            where w.Length == charCount
                                            where Regex.IsMatch(w, textBoxRegex.Text.ToLower())
                                            where Regex.IsMatch(w, (badChars.Trim().Count() > 0) ? "^[^" + badChars + "]*$" : ".*")
                                            where w.Count(c => textBoxRegex.Text.Replace(".", "").ToCharArray().Contains(c)) <= textBoxRegex.Text.Replace(".", "").Count()
                                            select w).ToArray());
        }
    }
}
