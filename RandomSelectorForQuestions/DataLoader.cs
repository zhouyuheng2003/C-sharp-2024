using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RandomSelectorForQuestions
{
    internal class DataLoader
    {
        // 字典用于存储每个文件夹中的题目
        private Dictionary<string, string[]> problemData;
        private List<string> folders;
        private int problemCount;

        // 构造函数，传入data文件夹路径
        public DataLoader(string dataFolderPath){
            problemData = new Dictionary<string, string[]>();
            folders = new List<string>();
            problemCount = 0;
            LoadData(dataFolderPath);
        }

        // 读取data文件夹下所有子题库中的problems.txt文件
        private void LoadData(string dataFolderPath){
            folders = Directory.GetDirectories(dataFolderPath).Select(Path.GetFileName).ToList();
            foreach (var folder in folders){
                string filePath = Path.Combine(dataFolderPath, folder, "problems.txt");
                if (File.Exists(filePath)){
                    // 读取每个文件的所有行，保存为字符串数组
                    problemData[folder] = File.ReadAllLines(filePath);
                    problemCount += problemData[folder].Length;
                }
            }
        }

        // 对外接口，获取指定文件夹下指定索引的题目
        public string Get(string folder, int index){
            if (folder == "all")
            {
                string selectedFolder = null;
                foreach (var f in folders)
                {
                    int siz = GetProblemCount(f);
                    selectedFolder = f;
                    if (index < siz) break;
                    index -= siz;
                }
                return problemData[selectedFolder][index];
            }
            else return problemData[folder][index];
        }
        public string[] GetFolders(){
            return folders.ToArray();
        }
        public int GetProblemCount(string folder){
            if (folder == "all") return problemCount;
            else return problemData[folder].Length;
        }
    }
}
