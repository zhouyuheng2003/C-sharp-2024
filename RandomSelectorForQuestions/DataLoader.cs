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

        // 构造函数，传入data文件夹路径
        public DataLoader(string dataFolderPath){
            problemData = new Dictionary<string, string[]>();
            folders = new List<string>();
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
                }
            }
        }

        // 对外接口，获取指定文件夹下指定索引的题目
        public string Get(string folder, int index){
            return problemData[folder][index];
        }
        public string[] GetFolders(){
            return folders.ToArray();
        }
        public int GetProblemCount(string folder){
            return problemData[folder].Length;
        }
    }
}
