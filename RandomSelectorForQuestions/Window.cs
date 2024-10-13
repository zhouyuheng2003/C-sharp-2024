using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace RandomSelectorForQuestions
{
    public partial class Window : Form
    {
        private DataLoader dataLoader;
        private Dictionary<string, string[]> problemData;
        private ComboBox comboBoxFolders;
        private Button buttonNext;
        private Label labelQuestion;
        private Random random;

        public Window()
        {
            InitializeComponent();
            random = new Random();
        }

        private void Window_Load(object sender, EventArgs e)
        {
            // 界面初始化
            this.Text = "抽题 App";
            this.Size = new System.Drawing.Size(400, 300);

            // 初始化 ComboBox
            comboBoxFolders = new ComboBox();
            comboBoxFolders.Location = new System.Drawing.Point(50, 30);
            comboBoxFolders.Size = new System.Drawing.Size(300, 30);
            comboBoxFolders.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(comboBoxFolders);

            // 初始化 Button
            buttonNext = new Button();
            buttonNext.Text = "下一题";
            buttonNext.Location = new System.Drawing.Point(150, 80);
            buttonNext.Click += ButtonNext_Click;
            this.Controls.Add(buttonNext);

            // 初始化 Label
            labelQuestion = new Label();
            labelQuestion.Location = new System.Drawing.Point(50, 130);
            labelQuestion.Size = new System.Drawing.Size(300, 100);
            labelQuestion.AutoSize = true; // 自动调整大小
            this.Controls.Add(labelQuestion);

            // 加载题库数据
            dataLoader = new DataLoader(@"..\..\data");
            LoadComboBoxData();
        }

        // 加载题库下拉框数据
        private void LoadComboBoxData()
        {
            foreach (var folder in dataLoader.GetFolders())
            {
                Console.WriteLine(folder);
                comboBoxFolders.Items.Add(folder);
            }
            if (comboBoxFolders.Items.Count > 0)
            {
                comboBoxFolders.SelectedIndex = 0; // 默认选择第一个题库
            }
        }
        // 显示下一题
        private void ButtonNext_Click(object sender, EventArgs e)
        {
            string selectedFolder = comboBoxFolders.SelectedItem.ToString();
            Console.WriteLine(selectedFolder);
            int problemCount = dataLoader.GetProblemCount(selectedFolder);
            int randomIndex = random.Next(0, problemCount);
            string question = dataLoader.Get(selectedFolder, randomIndex);
            labelQuestion.Text = question; // 更新 Label 的文本
            
        }
    }
}
