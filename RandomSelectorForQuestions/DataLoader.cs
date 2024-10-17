using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RandomSelectorForQuestions
{
    internal class DataLoader
    {
        // 字典用于存储每个文件夹中的题目
        private Dictionary<string, string[]> problemData;
        private List<string> folders;
        private int problemCount;
        int seeds;
        private string myDataFolderPath;
        Dictionary<string, int> suf_count_problemset;
        
        Dictionary<string, int> used_for_problemset;
        // 构造函数，传入data文件夹路径
        public DataLoader(string dataFolderPath){
            problemData = new Dictionary<string, string[]>();
            folders = new List<string>();
            suf_count_problemset = new Dictionary<string, int>();
            used_for_problemset= new Dictionary<string, int>();
            problemCount = 0;
            LoadData(dataFolderPath);
        }

        // 读取data文件夹下所有子题库中的problems.txt文件
        private void LoadData(string dataFolderPath)
        {
            myDataFolderPath = dataFolderPath;
            folders = Directory.GetDirectories(dataFolderPath).Select(Path.GetFileName).ToList();
            foreach (var folder in folders)
            {
                string filePath = Path.Combine(dataFolderPath, folder, "problems.txt");
                if (File.Exists(filePath))
                {
                    // 读取每个文件的所有行，保存为字符串数组
                    problemData[folder] = File.ReadAllLines(filePath);
                    problemCount += problemData[folder].Length;
                }
            }
            int suf = 0;
            int folder_count = 0;
            foreach (var folder in folders)
            {
                suf += GetProblemCount(folder);
                suf_count_problemset[folder] = suf;
                folder_count++;
            }
            int valid_seed = 0;
            if (File.Exists(Path.Combine(dataFolderPath, "seeds.txt")))
            {
                string[] seed = File.ReadAllLines(Path.Combine(dataFolderPath, "seeds.txt"));
                if (seed.Length == folder_count + 1)
                {
                    seeds = int.Parse(seed[0]);
                    int tot = 1;
                    foreach (var folder in folders)
                    {
                        used_for_problemset[folder] = int.Parse(seed[tot]);
                        tot++;
                    }
                    valid_seed = 1;
                }
            }
            if (valid_seed == 0)
            {
                seeds = new Random().Next(0, 10000);
                foreach (var folder in folders)
                {
                    used_for_problemset[folder] = 0;
                }
            }
        }
        private void WriteData()
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(myDataFolderPath, "seeds.txt")))
            {
                writer.WriteLine(seeds);
                foreach (var folder in folders)
                {
                    writer.WriteLine(used_for_problemset[folder]);
                }
            }
        }
        // 对外接口，获取指定文件夹下指定索引的题目
        private int GetProblemIndex(string folder, int prob)
        {
            int indexl = 0, indexr = 0;
            indexr = suf_count_problemset[folder];
            indexl = indexr - GetProblemCount(folder)+1;
            RandomNumberGenerator.setSeed(seeds);
            Dictionary<int,int> count = new Dictionary<int,int>();
            while (prob != 0)
            {
                int index = RandomNumberGenerator.getNext()% problemCount+1;
                if (count.ContainsKey(index)) continue;
                count[index] = 1;
                if (indexl <= index && index <= indexr)
                {
                    prob--;
                    if (prob == 0)
                    {
                        return index - indexl;
                    }
                }
            }
            return 0;
        }
        private int GetNumberIndex(string folder, int prob)
        {
            int indexl = 0, indexr = 0, ans = 0;
            indexr = suf_count_problemset[folder];
            indexl = indexr - GetProblemCount(folder);
            RandomNumberGenerator.setSeed(seeds);
            Dictionary<int, int> count = new Dictionary<int, int>();
            while (prob != 0)
            {
                int index = RandomNumberGenerator.getNext() % problemCount + 1;
                if (count.ContainsKey(index)) continue;
                count[index] = 1;
                ans++;
                if (indexl <= index && index <= indexr)
                {
                    prob--;
                    if (prob == 0)
                    {
                        return ans;
                    }
                }
            }
            return 0;
        }
        public void ReSet()
        {
            seeds = new Random().Next(0, 10000);
            foreach (var folder in folders)
            {
                used_for_problemset[folder] = 0;
            }
            WriteData();
        }
        public string allfolder;
        public string Get(string folder){
            int index = -1;
            if (folder == "all")
            {
                foreach(var f in folders)
                {
                    if (used_for_problemset[f] == GetProblemCount(f)) ;
                    else
                    {
                        int count = GetNumberIndex(f, used_for_problemset[f] + 1);
                        if(count<index || index == -1)
                        {
                            index = count;
                            folder = f;
                        }
                    }
                }
                if(index==-1) return "两眼空空";
                used_for_problemset[folder]++;
                index = GetProblemIndex(folder, used_for_problemset[folder]);
            }
            else
            {
                if (used_for_problemset[folder]== GetProblemCount(folder))
                {
                    return "两眼空空";
                }
                used_for_problemset[folder]++;
                index = GetProblemIndex(folder, used_for_problemset[folder]);
            }

            WriteData();
            allfolder = folder;
            return problemData[folder][index];
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
