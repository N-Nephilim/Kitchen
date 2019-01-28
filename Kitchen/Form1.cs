﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace Kitchen
{
    public partial class Form1 : Form
    {
        string ingrTrue = "";
        int i;//Количество CheckBox'ов
        CheckBox box; //Обьявляю для того, чтобы можно было использовать чекбоксы ВЕЗДЕЕЕЕЕЕЕЕЕЕЕ
        static public string pathToFile = "";
        bool findTextChanged = false;
        public Form1()
        {
            InitializeComponent();
            MessageBox.Show("Ctrl+Shift+V " + " Shift+Tab");
            this.Cursor = Cursors.WaitCursor;
            #region 1-st TAB
            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // string o = System.AppDomain.CurrentDomain.BaseDirectory;
            //string pathToFile = @o + "\";
            //РАБОТАЕТ ТОЛЬКО ПОСЛЕ ИНСТАЛЯТОРА(должно)
            pathToFile = @"C:\Users\valer\OneDrive\Desktop\Programming\Kitchen\";
            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            FileStream fs = new FileStream(pathToFile + "Ingridients.txt", FileMode.OpenOrCreate);
            fs.Close();
            #region Interface setting
            FindRecepts.TabStop = false;
            FindRecepts.FlatStyle = FlatStyle.Flat;
            FindRecepts.FlatAppearance.BorderSize = 0;
            BrowseRecepts.TabStop = false;
            BrowseRecepts.FlatStyle = FlatStyle.Flat;
            BrowseRecepts.FlatAppearance.BorderSize = 0;
            #endregion 
            string[] ingridients = (File.ReadAllLines(pathToFile + @"Ingridients.txt", Encoding.UTF8));
            List<string> firstLetters = new List<string>();
            List<string> allIngridients = new List<string>();
            foreach (string a in ingridients)
            {
                allIngridients.Add(a.ToString());
                if (firstLetters.Contains(a.Substring(0, 1)))
                { }
                else
                {
                    firstLetters.Add(a.Substring(0, 1));
                }
            }
            firstLetters.Sort();
            allIngridients.Sort();
            foreach (string a in firstLetters)
            {
                comboBox1.Items.Add(a);
            }
            foreach (string a in allIngridients)
            {
                comboBoxSearch.Items.Add(a);
            }
            //metroTabControl1.SelectTab(0);
            #endregion
            this.Cursor = Cursors.Default;
        }

        #region 1-st TAB entrails
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
        }


        private void comboBoxSearch_SelectedIndexChanged_1(object sender, EventArgs e)
        {
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
        }

        private void comboBoxSearch_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            int n = dataGridView1.Rows.Add();
            dataGridView1.Rows[n].Cells["colIngridients"].Value = comboBoxSearch.SelectedItem.ToString();
            comboBoxSearch.Items.Remove(comboBoxSearch.SelectedItem);
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string[] ingrid = (File.ReadAllLines(pathToFile + @"Ingridients.txt", Encoding.UTF8));
            int startLocation = 75;
            foreach (CheckBox chbox in this.Controls.OfType<CheckBox>())
            {
                chbox.Visible = false;
                chbox.Enabled = false;
            }
            List<string> ingridients = new List<string>();
            foreach (string a in ingrid)
            {
                if (a.Substring(0, 1) == comboBox1.Text)
                {
                    ingridients.Add(a);
                }
            }
            for (i = 0; i < ingridients.Count; i++)
            {
                bool repeat = false;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[1].Value.ToString() == ingridients[i])
                    {
                        repeat = true;
                    }
                }
                if (repeat == false)
                {
                    box = new CheckBox(); //Create new checkBox
                    box.Tag = i;//CheckBox (Tag 0-..)
                    box.TabIndex = 8 + i;//Последовательность "выбора" через TAB
                    box.Text = ingridients[i];
                    box.AutoSize = true;
                    box.Location = new Point(2, startLocation);
                    startLocation += 25;
                    this.Controls.Add(box);
                    box.BringToFront();
                }
            }
            //Увеличивает окно, если чекбоксов слишком много
            if (startLocation > 220)
            {
                this.Size = new Size(596, startLocation + 50);
            }
        }

        private void metroTabPage1_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (CheckBox chbox in this.Controls.OfType<CheckBox>())
            {
                if (chbox.Checked == true)
                {
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells["colIngridients"].Value = chbox.Text;
                    comboBoxSearch.Items.Remove(chbox.Text);
                    chbox.Checked = false;
                    chbox.Visible = false;
                    chbox.Enabled = false;
                }
            }
        }

        private void FindRecepts_Click_1(object sender, EventArgs e)
        {
            ingrTrue = "";
            string ingrTrueInt = "";
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                ingrTrue += row.Cells[1].Value.ToString() + " ";
            }
            string[] ingridients = (File.ReadAllLines(pathToFile + @"Ingridients.txt", Encoding.UTF8));
            uint n = 0;
            foreach (string ing in ingridients)
            {
                n++;
                if (ingrTrue.Contains(ing))
                    ingrTrueInt += n + " ";
            }
            Find find = new Find(ingrTrueInt);
            find.StartPosition = FormStartPosition.Manual;
            find.Location = this.Location;
            find.ShowDialog();
        }

        private void BrowseRecepts_Click_1(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.StartPosition = FormStartPosition.Manual;
            f2.Location = this.Location;
            Hide();
            Form2.saveMyIng = ingrTrue;
            f2.ShowDialog();
            Close();//Закрывает ПЕРВУЮ форму
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    comboBoxSearch.Items.Add(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                    comboBoxSearch.Sorted = true;
                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                }
            }
            catch
            {
            }
        }
        #endregion

        #region 2-nd TAB
        private void Reload_Click(object sender, EventArgs e)
        {
           // try
            {
                dataGridViewSQL.Rows.Clear();
                BinaryFormatter formatter = new BinaryFormatter();
                var objectsBackup = new List<RecipeList>();
                using (Stream fs = File.Open(@"C:\RecipeBackup\" + "RecipeBackup.dat", FileMode.OpenOrCreate))
                {
                    int a = -1;
                    fs.Position = 0;
                    while (fs.Position < fs.Length)
                    {
                        a++;
                        objectsBackup.Add((RecipeList)formatter.Deserialize(fs));
                        int n = dataGridViewSQL.Rows.Add();
                        dataGridViewSQL.Rows[n].Cells["colIngredients"].Value = objectsBackup[a].Ingridients;
                        dataGridViewSQL.Rows[n].Cells["colName"].Value = objectsBackup[a].Name;
                        dataGridViewSQL.Rows[n].Cells["colNumber"].Value = a+1;
                    }
                }
            }
           // catch
            {
            }
        }
        private void findText_TextChanged(object sender, EventArgs e)
        {
            if (label1 != null)
            {
                label1.Text = null;
            }
            if (findText.Text == "")
            {
                label1.Text = "Найти по названию";
            }
            findTextChanged = true;
        }

        private void Find_Click(object sender, EventArgs e)
        {
            if (dataGridViewSQL.RowCount != 0)
            {
                if (findText.Text != "")
                {
                    bool contains = false;
                    this.Cursor = Cursors.WaitCursor;

                    if (dataGridViewSQL.CurrentRow.Index == dataGridViewSQL.RowCount - 1)
                    {
                        Reload.PerformClick();
                        SendKeys.Send("{TAB}");
                        SendKeys.Send("{TAB}");
                        SendKeys.Send("{TAB}");
                        SendKeys.Send("{TAB}");
                        SendKeys.Send("{TAB}");
                    }
                    int i = 0;
                    if (findTextChanged)
                    {
                        i = 0;
                        findTextChanged = false;
                    }
                    else
                    {
                        i = dataGridViewSQL.CurrentRow.Index;
                    }
                    for (i = i + 1 ; i < dataGridViewSQL.RowCount; i++)
                    {
                        string name = dataGridViewSQL.Rows[i].Cells[2].Value.ToString();
                        int cursor;
                        for (cursor = 0; cursor + findText.Text.Length <= name.Length; cursor++)
                        {
                            if (name.Substring(cursor, findText.Text.Length).Equals(findText.Text, StringComparison.InvariantCultureIgnoreCase))
                            {
                                contains = true;

                                Reload.PerformClick();
                                dataGridViewSQL.ClearSelection();
                                SendKeys.Send("{TAB}");
                                SendKeys.Send("{TAB}");
                                SendKeys.Send("{TAB}");
                                SendKeys.Send("{TAB}");

                                for (int n = 0; n < i; n++)
                                {
                                    SendKeys.Send("{Enter}");
                                }
                                break;
                            }
                        }
                        if (contains == true)
                        {
                            break;
                        }
                    }
                    if (contains == false)
                    {
                        MessageBox.Show("Слово: '" + findText.Text + "' НЕ найдено!");
                        Reload.PerformClick();
                        SendKeys.Send("{TAB}");
                        SendKeys.Send("{TAB}");
                        SendKeys.Send("{TAB}");
                        SendKeys.Send("{TAB}");
                    }
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void dataGridViewSQL_CellClick(object sender, DataGridViewCellEventArgs e)
        {
                        this.Cursor = Cursors.WaitCursor;
            //try
            {
                if (e.ColumnIndex == 0)
                {
                    DialogResult dialogResult = MessageBox.Show("Удалить этот игредиент НАВСЕГДА?", "Мы удаляем?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string name = dataGridViewSQL.Rows[e.RowIndex].Cells[2].Value.ToString();
                        string ingridients = dataGridViewSQL.Rows[e.RowIndex].Cells[3].Value.ToString();
                        dataGridViewSQL.Rows.RemoveAt(e.RowIndex);

                        BinaryFormatter formatter = new BinaryFormatter();
                        var objectsBackup = new List<RecipeList>();
                        using (Stream fs = File.Open(@"C:\RecipeBackup\" + "RecipeBackup.dat", FileMode.OpenOrCreate))
                        {
                            fs.Position = 0;
                            while (fs.Position < fs.Length)
                            {
                                objectsBackup.Add((RecipeList)formatter.Deserialize(fs));
                                if (objectsBackup[objectsBackup.Count - 1].Name == name && objectsBackup[objectsBackup.Count - 1].Ingridients == ingridients)
                                {
                                    objectsBackup.RemoveAt(objectsBackup.Count - 1);
                                }
                            }
                        }
                        File.WriteAllText(@"C:\RecipeBackup\RecipeBackup.dat", String.Empty);
                        foreach (RecipeList recipe in objectsBackup)
                        {
                            RecipeList.Serialization2(recipe.Name, recipe.Description, recipe.Ingridients, recipe.Count, recipe.Type, "");
                        }
                    }
                }
            }
            //catch
            {
            }
                        this.Cursor = Cursors.Default;
        }
            #endregion

            private void Form1_Shown(object sender, EventArgs e)
        {
            dataGridViewSQL.RowHeadersWidth = 20;
            dataGridViewSQL.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            System.Timers.Timer timer = new System.Timers.Timer(1000 * 60 * 5);
            timer.AutoReset = true; // the key is here so it repeats
            timer.Elapsed += timer_elapsed;
            timer.Start();
        }

        private void timer_elapsed(object sender, ElapsedEventArgs e)
        {
            NotifyIcon notifyIcon = new NotifyIcon();
            notifyIcon.Visible = true;
            notifyIcon.BalloonTipTitle = "Ээээй";
            notifyIcon.BalloonTipText = "Есть тут кто?";
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.ShowBalloonTip(5000);
        }

        private void metroTabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}