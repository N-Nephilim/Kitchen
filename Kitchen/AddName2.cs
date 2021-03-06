﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Kitchen
{
    public partial class AddName2 : Form
    {
        int i = 1;
        string ingr = "";
        string count = "";
        string type = "";
        string name = "";
        string description = "";
        string imageDirection = "";
        string category = "";
        int rowNumber = 0;
        bool conceived = false;
        string pathToFile = Form1.pathToFile;
        public AddName2(string Ingridients, string Count, string Type, string Name, string Description, int rowN, string ImageDirection, string Category)
        {
            InitializeComponent();
            ingr = Ingridients;
            count = Count;
            type = Type;
            name = Name;
            description = Description;
            rowNumber = rowN;
            imageDirection = ImageDirection;
            category = Category;
            ddescription.ScrollBars = ScrollBars.Both;
            pictureBox1.ImageLocation = imageDirection;
            this.Text = name + " - редактирование";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Ты точно хочешь удалить этот рецепт?", "Удалить рецепт?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                RecipeList RL = new RecipeList
                {
                    Name = name,
                    Description = description
                };
                List<RecipeList> objects = new List<RecipeList>();
                int a = 0;
                using (FileStream fs = new FileStream(pathToFile + "Recipe.dat", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    fs.Position = 0;
                    while (fs.Position < fs.Length)
                    {
                        if (a == rowNumber)
                        {
                            objects.Add((RecipeList)formatter.Deserialize(fs));
                            objects.Remove(objects[a]);
                        }
                        else
                        {
                            objects.Add((RecipeList)formatter.Deserialize(fs));
                        }
                        a++;
                    }
                }
                File.WriteAllText(pathToFile + "Recipe.dat", string.Empty);
                using (FileStream fs = new FileStream(pathToFile + "Recipe.dat", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    for (int i = 0; i < a - 1; i++)
                    {
                        formatter.Serialize(fs, objects[i]);
                    }
                }
                Form2.deleted = true;
                conceived = true;
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            i++;
            ddescription.Text = ddescription.Text + "\r\n\r\n" + i + ". ";
            сщгте.Text = Convert.ToString(i);
            ddescription.Focus();
            SendKeys.Send("{RIGHT}");
        }

        private void AddName2_Shown(object sender, EventArgs e)
        {
            ddescription.Text = description;
            nname.Text = name;

            int count = ddescription.Text.Split('.').Length - 1;
            i = count;
            сщгте.Text = Convert.ToString(i);
        }

        private void сщгте_TextChanged(object sender, EventArgs e)
        {
            try
            {
                i = Convert.ToInt16(сщгте.Text);
            }
            catch
            {
                MessageBox.Show("Сюда можно вписывать только цифры");
                сщгте.Text = Convert.ToString(i);
            }
        }

        private void saveExit_Click(object sender, EventArgs e)
        {
            if (nname.Text == "")
            {
                MessageBox.Show("Впишите название рецепта");
            }
            else if (ddescription.Text == "" || ddescription.Text == "1. " || ddescription.Text == "1. \r\n")
            {
                MessageBox.Show("Впишите описание рецепта");
            }
            else
            {
                bool exist = false;
                using (Stream fs = File.Open(Form1.pathToFile + "Recipe.dat", FileMode.OpenOrCreate))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    var objectss = new List<RecipeList>();
                    fs.Position = 0;
                    while (fs.Position < fs.Length)
                    {
                        objectss.Add((RecipeList)formatter.Deserialize(fs));
                        if ((objectss[objectss.Count - 1].Name == nname.Text) && (rowNumber != objectss.Count - 1))
                        {
                            exist = true;
                            break;
                        }
                    }
                }
                if (exist == false)
                {
                    RecipeList RL = new RecipeList
                    {
                        Name = nname.Text,
                        Ingridients = ingr,
                        Description = ddescription.Text,
                        Type = type,
                        Count = count,
                        ImageDirection = imageDirection,
                        Category = category
                    };
                    var objects = new List<RecipeList>();
                    int a = 0;
                    using (FileStream fs = new FileStream(pathToFile + "Recipe.dat", FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        fs.Position = 0;

                        while (fs.Position < fs.Length)
                        {
                            if (a == rowNumber)
                            {
                                objects.Add(RL);
                                objects.Add((RecipeList)formatter.Deserialize(fs));
                                a++;
                                objects.Remove(objects[a]);
                            }
                            else
                            {
                                objects.Add((RecipeList)formatter.Deserialize(fs));
                                a++;
                            }
                        }
                    }
                    System.IO.File.WriteAllText(pathToFile + "Recipe.dat", string.Empty);
                    using (FileStream fs = new FileStream(pathToFile + "Recipe.dat", FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        for (int i = 0; i < a; i++)
                        {
                            formatter.Serialize(fs, objects[i]);
                        }
                    }
                    conceived = true;
                    this.Close();
                }
                else
                    MessageBox.Show("Рецепт с таким названием уже существует");
            }
        }

        private void ChooseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                imageDirection = fileDialog.FileName;
                pictureBox1.ImageLocation = imageDirection;
            }
        }

        private void AddName2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (conceived == false)
            {
                DialogResult dialogResult = MessageBox.Show("Хочешь отменить изменения?", "Отменить изменения?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void AddName2_FormClosed(object sender, FormClosedEventArgs e)
        {
            SendKeys.Send("{ENTER}");
        }
    }
}
