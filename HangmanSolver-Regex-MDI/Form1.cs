using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HangmanSolver_Regex
{
    public partial class Form1 : Form
    {
        public List<string> loadedWords;
        public List<WordForm> wordForms = new List<WordForm>();
        public Form1()
        {
            InitializeComponent();
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateLists();
            #region OLD
            /*for (int i = 0; i < filterWords.Count; i++)
            {
                if (filterWords[i].Length == charCount)
                {
                    if (Regex.IsMatch(filterWords[i], textBoxRegex.Text.ToLower()))
                    {
                        bool bad = false;
                        for (int x = 0; x < badchars.Count(); x++)
                        {
                            if (filterWords[i].ToLower().Contains(badchars[x].ToString().ToLower())) { bad = true; break; }
                        }
                        if (!bad)
                        {
                            listBoxFiltered.Items.Add(filterWords[i]);

                        }
                    }
                }
            }*/
            #endregion
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadWords();

            WordForm firstForm = new WordForm();
            firstForm.MdiParent = this;
            firstForm.WindowState = FormWindowState.Maximized;
            firstForm.Show();
            wordForms.Add(firstForm);
        }

        private void LoadWords()
        {
            IEnumerable<string> allWords = from tf in Directory.EnumerateFiles(Environment.CurrentDirectory, "*.txt", SearchOption.AllDirectories)
                                           from fs in File.ReadAllLines(tf)
                                           select fs.ToLower();

            loadedWords = allWords.Distinct().ToList();

            Text = $"Hangman Solver w/ Regex ({loadedWords.Count()} words loaded)";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < wordForms.Count; i++)
            {
                wordForms[i].ResetInfo();
            }
            textBoxBadLetters.Clear();
        }

        private void textBoxBadLetters_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                UpdateLists();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WordForm firstForm = new WordForm();
            firstForm.MdiParent = this;
            firstForm.Show();
            wordForms.Add(firstForm);
        }

        public void UpdateLists()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            for (int i = 0; i < wordForms.Count; i++)
            {
                try
                {
                    wordForms[i].UpdateList(textBoxBadLetters.Text.ToLower(), ref loadedWords);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bad regex!\n\n" + ex.ToString());
                    break;
                }
            }
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            MessageBox.Show(
                "Recreate what you know of a full-word using letters and '.'s in the 'Regex for known Letters' box in each word form. (ex 'e..e..ai..e..', where '.' represents letters not yet known.)\n\n" +
                "The formatiing of the regex boxes does matter, but not at all for the bad letters box. Everytime you make a change, you may press update to apply it to all words already in each list. If no" +
                "words are found in a list, it will begin searching using the full list of loaded words.\n\n" +
                "To add words to this program, simply place a .txt file in the same directory or any subdirectory with the programs main executable. Files must be formatted with 1 word, seperated by a line break. (1 word per line)\n\n" +
                "Program created by Jaika★ as a fun little side-project.",
                "Application Help", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
            );
        }

        private void addWordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3_Click(this, new EventArgs());
        }

        private void resetWordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2_Click(this, new EventArgs());
        }

        private void removeAllWordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < wordForms.Count;)
                wordForms[i].Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void reloadWordListsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadedWords.Clear();
            LoadWords();
        }
    }
}
