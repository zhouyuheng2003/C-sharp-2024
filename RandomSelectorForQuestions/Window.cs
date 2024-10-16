using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

//using MathNet.Numerics;
using System.Drawing.Imaging;
using Aspose.TeX.Features;
using System.Text.RegularExpressions;

namespace RandomSelectorForQuestions
{
    public partial class Window : Form
    {
        private DataLoader dataLoader;
        private Dictionary<string, string[]> problemData;
        private ComboBox comboBoxFolders;
        private Button buttonNext, buttonLocation, buttonReset, buttonSmall, buttonBig;
        private RichTextBox richTextBoxQuestion; // 替换为 RichTextBox
        private Random random;
        private string dataPath;

        public Window()
        {
            InitializeComponent();
            random = new Random();
            RandomNumberGenerator.init();
        }

        private void Window_Load(object sender, EventArgs e)
        {
            // 界面初始化
            this.Text = "抽题 App";
            this.Size = new System.Drawing.Size(600, 400);

            // 初始化 comboBoxFolders
            comboBoxFolders = new ComboBox();
            comboBoxFolders.Location = new System.Drawing.Point(80, 22);
            comboBoxFolders.Size = new System.Drawing.Size(250, 50);
            comboBoxFolders.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(comboBoxFolders);

            // 初始化 buttonNext
            buttonNext = new Button();
            buttonNext.Text = "下一题";
            buttonNext.Location = new System.Drawing.Point(440, 21);
            buttonNext.Width = 60;
            buttonNext.Click += ButtonNext_Click;
            this.Controls.Add(buttonNext);

            // 初始化 buttonReset
            buttonReset = new Button();
            buttonReset.Text = "重置";
            buttonReset.Location = new System.Drawing.Point(500, 21);
            buttonReset.Width = 57;
            buttonReset.Click += ButtonReset_Click;
            this.Controls.Add(buttonReset);

            // 初始化 buttonLocation
            buttonLocation = new Button();
            buttonLocation.Text = "切换题库目录";
            buttonLocation.Location = new System.Drawing.Point(340, 21);
            buttonLocation.Click += ButtonLocation_Click;
            buttonLocation.Width = 100;
            this.Controls.Add(buttonLocation);

            // 初始化 richTextBoxQuestion
            richTextBoxQuestion = new RichTextBox();
            richTextBoxQuestion.Location = new System.Drawing.Point(25, 60);
            richTextBoxQuestion.Size = new System.Drawing.Size(530, 280);
            richTextBoxQuestion.ScrollBars = RichTextBoxScrollBars.Vertical;
            richTextBoxQuestion.BackColor = System.Drawing.Color.White;
            richTextBoxQuestion.ForeColor = System.Drawing.Color.Red;
            richTextBoxQuestion.Font = new System.Drawing.Font("楷体", 16);
            richTextBoxQuestion.Text = "欢迎使用抽题APP！请点击下一题开始作答。";
            this.Controls.Add(richTextBoxQuestion);

            //Label labelTitle = new Label();
            //labelTitle.Text = "题目";
            //labelTitle.Location = new System.Drawing.Point(20, 20);
            //labelTitle.Size = new System.Drawing.Size(100, 30);
            //labelTitle.Font = new System.Drawing.Font("Microsoft YaHei", 12, System.Drawing.FontStyle.Bold);
            //labelTitle.ForeColor = System.Drawing.Color.Black;
            //labelTitle.BackColor = Color.Transparent;
            //this.Controls.Add(labelTitle);

            // 初始化 buttonLocation
            buttonBig = new Button();
            buttonBig.BackgroundImage = Image.FromFile(@"./src/+.png");
            buttonBig.BackgroundImageLayout = ImageLayout.Stretch;
            buttonBig.Location = new System.Drawing.Point(25, 21);
            buttonBig.Size = new System.Drawing.Size(24, 24); // 设置按钮大小（与图片大小一致）
            buttonBig.FlatStyle = FlatStyle.Flat; // 设置按钮样式，使其无边框
            buttonBig.FlatAppearance.BorderSize = 0; // 去掉按钮边框
            buttonBig.Click += ButtonBig_Click;
            this.Controls.Add(buttonBig);

            buttonSmall = new Button();
            buttonSmall.BackgroundImage = Image.FromFile(@"./src/-.png"); // 设置背景图片
            buttonSmall.BackgroundImageLayout = ImageLayout.Stretch; // 设置图片填充方式
            buttonSmall.Location = new System.Drawing.Point(50, 21);
            buttonSmall.Size = new System.Drawing.Size(24, 24); // 设置按钮大小（与图片大小一致）
            buttonSmall.FlatStyle = FlatStyle.Flat; // 设置按钮样式，使其无边框
            buttonSmall.FlatAppearance.BorderSize = 0; // 去掉按钮边框
            buttonSmall.Click += ButtonSmall_Click;
            this.Controls.Add(buttonSmall);

            // 加载题库数据
            dataPath = @"../../data";
            this.BackgroundImage = Image.FromFile("./src/bg.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            dataLoader = new DataLoader(dataPath);
            LoadComboBoxData();
        }

        // 加载题库下拉框数据
        private void LoadComboBoxData()
        {
            comboBoxFolders.Items.Clear();
            comboBoxFolders.Items.Add("题库：all");
            foreach (var folder in dataLoader.GetFolders())
            {
                Console.WriteLine(folder);
                comboBoxFolders.Items.Add("题库：" + folder);
            }
            if (comboBoxFolders.Items.Count > 0)
            {
                comboBoxFolders.SelectedIndex = 0; // 默认选择第一个题库
            }
        }
        

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            string selectedFolder = comboBoxFolders.SelectedItem.ToString().Replace("题库：", "");
            Console.WriteLine(selectedFolder);
            int problemCount = dataLoader.GetProblemCount(selectedFolder);//获取题库的题目数量

            // 这行代码之后要改成老虎的Rand
            //int randomIndex = random.Next(0, problemCount);

            string question = dataLoader.Get(selectedFolder);
            if(question == "两眼空空")
            {
                richTextBoxQuestion.ForeColor = Color.Red;
                richTextBoxQuestion.Font = new Font("楷体", 16); ;
                richTextBoxQuestion.Text = "当前题库已全部抽完，请更换题库或重置。";
            }
            else SetText(richTextBoxQuestion, question);
            

        }
        private void ButtonReset_Click(object sender, EventArgs e)
        {
            dataLoader.ReSet();
        }
        private void ButtonBig_Click(object sender, EventArgs e)
        {
            float currentSize = richTextBoxQuestion.Font.Size;
            richTextBoxQuestion.Font = new Font(richTextBoxQuestion.Font.FontFamily, currentSize + 2);
        }
        private void ButtonSmall_Click(object sender, EventArgs e)
        {
            float currentSize = richTextBoxQuestion.Font.Size;
            richTextBoxQuestion.Font = new Font(richTextBoxQuestion.Font.FontFamily, currentSize - 2);
        }

        int _counter = 0;
        public void SetText(RichTextBox rtb, string question)
        {
            if (question.Contains("$"))
            {


                rtb.Text = "" + Environment.NewLine;


                MathRendererOptions options = new PngMathRendererOptions() { Resolution = 150 };

                // 指定序言。
                options.Preamble = @"\usepackage{amsmath}
                  \usepackage{amsfonts}
                  \usepackage{amssymb}
                  \usepackage{color}
                  ";

                // 指定比例因子 300%。
                options.Scale = 3000;
                options.TextColor = System.Drawing.Color.Black;
                options.BackgroundColor = System.Drawing.Color.White;
                options.LogStream = new MemoryStream();
                options.ShowTerminal = true;
                System.Drawing.SizeF size = new System.Drawing.SizeF();
                Stream stream;

                _counter += 1;

                string selectedFolder = comboBoxFolders.SelectedItem.ToString().Replace("题库：", "");
                String save_path = Path.Combine(dataPath, selectedFolder, @"math-formula" + _counter.ToString() + ".png");
                using (stream = File.Open(save_path, FileMode.Create))
                {
                    MathRenderer.Render(question, stream, options, out size);
                }
                stream.Close();

                Console.WriteLine(_counter.ToString());

                Image formulaImage = Image.FromFile(save_path);
                IDataObject _data = new DataObject();
                _data.SetData(formulaImage);
                Clipboard.SetDataObject(_data, false);

                //rtb.SelectionStart = rtb.Text.Length;

                rtb.Paste();
                Clipboard.Clear();

            } else
            {
                string picPath = "";
                if (question.Contains("[IMAGE"))
                {
                    question = question.Replace("[IMAGE:", "");
                    foreach (var ch in question)
                    {
                        if (ch == ']') break;
                        else picPath += ch;
                    }
                    question = question.Replace(picPath + "]", "");
                    string selectedFolder = comboBoxFolders.SelectedItem.ToString().Replace("题库：", "");
                    picPath = Path.GetFullPath(Path.Combine(dataPath, selectedFolder, picPath));
                    Console.WriteLine(picPath);
                }
                rtb.Text = "题目：" + question + Environment.NewLine;
                rtb.ForeColor = Color.Blue;
                rtb.SelectionColor = Color.White;
                rtb.Font = new Font("楷体", 16);
                if (picPath != "")
                {
                    Image myImage = Image.FromFile(picPath);

                    IDataObject data = new DataObject();
                    data.SetData(myImage);
                    Clipboard.SetDataObject(data, false);
                    rtb.SelectionStart = rtb.Text.Length;
                    rtb.Paste();
                }
            }
        }


        private void ButtonLocation_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "请选择一个目录";
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
            {
                string selectedPath = folderBrowserDialog.SelectedPath;
                dataLoader = new DataLoader(selectedPath);
                dataPath = selectedPath;
            }
            LoadComboBoxData();
        }
    }
}
