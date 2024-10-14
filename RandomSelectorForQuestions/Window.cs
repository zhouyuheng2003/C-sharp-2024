using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RandomSelectorForQuestions
{
    public partial class Window : Form
    {
        private DataLoader dataLoader;
        private Dictionary<string, string[]> problemData;
        private ComboBox comboBoxFolders;
        private Button buttonNext, buttonLocation;
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
            comboBoxFolders.Location = new System.Drawing.Point(75, 22);
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
            richTextBoxQuestion.Size = new System.Drawing.Size(520, 280);
            richTextBoxQuestion.BackColor = System.Drawing.Color.White;
            richTextBoxQuestion.ForeColor = System.Drawing.Color.Black;
            richTextBoxQuestion.Font = new System.Drawing.Font("Microsoft YaHei", 12);
            //richTextBoxQuestion.ReadOnly = true; // 只读
            this.Controls.Add(richTextBoxQuestion);

            Label labelTitle = new Label();
            labelTitle.Text = "题目";
            labelTitle.Location = new System.Drawing.Point(20, 20);
            labelTitle.Size = new System.Drawing.Size(100, 30);
            labelTitle.Font = new System.Drawing.Font("Microsoft YaHei", 12, System.Drawing.FontStyle.Bold);
            labelTitle.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(labelTitle);

            // 加载题库数据
            dataPath = @"../../data";
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
            SetText(richTextBoxQuestion, question);
            
            
           
        }
        public void SetText(RichTextBox rtb, string question)
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
            rtb.BackColor = Color.White;
            rtb.ForeColor = Color.Blue;
            rtb.SelectionColor = Color.White;
            rtb.Font = new Font("楷体", 16);
            if(picPath != "")
            {
                Image myImage = Image.FromFile(picPath);

                IDataObject data = new DataObject();
                data.SetData(myImage);
                Clipboard.SetDataObject(data, false);
                rtb.SelectionStart = rtb.Text.Length;
                rtb.Paste();
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
