using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3Project
{
    class ID3Classifier
    {
        Tree _tree;
        string _path;
        DataTable table;
        Dictionary<string, List<string>> artibute;
        KeyValuePair<string, string[]> targetArtibute;

        /// <summary>
        /// Create a new ID3Classifier.
        /// Creates the tree according to the train set, using the Gini Index as the splitting function.
        /// </summary>
        /// <param name="dataPath">Path to location of data files.</param>
        public ID3Classifier(string dataPath)
        {
            _path = dataPath;
            artibute = new Dictionary<string, List<string>>();
            targetArtibute = new KeyValuePair<string, string[]>();

            #region Fill Table

            using (StreamReader sr = File.OpenText(dataPath + @"\Structure.txt"))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    string[] t = s.Split(' ');

                    if (t[1] == "class")
                    {
                        targetArtibute = new KeyValuePair<string, string[]>(t[1], t[2].Split(','));
                        targetArtibute.Value[0] = targetArtibute.Value[0].Substring(1);
                        targetArtibute.Value[targetArtibute.Value.Length - 1] = targetArtibute.Value[targetArtibute.Value.Length - 1].Remove(targetArtibute.Value[targetArtibute.Value.Length - 1].Length - 1);
                        break;
                    }


                    artibute.Add(t[1], new List<string>());
                }


            }


            using (table = new DataTable())
            {

                foreach (string item in artibute.Keys)
                {
                    table.Columns.Add(item, typeof(double));

                }
            }
            table.Columns.Add(targetArtibute.Key, typeof(string));



            using (StreamReader sr = File.OpenText(dataPath + @"\train.txt"))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    string[] t = s.Split(',');


                    DataRow Dr = table.NewRow();
                    for (int i = 0; i < t.Length; i++)
                    {
                        if (isNumaric(t[i]))
                            Dr[i] = Double.Parse(t[i]);
                        else
                            Dr[i] = t[i];
                    }

                    table.Rows.Add(Dr);
                }

            }
            #endregion

            _tree = new Tree(table, artibute, targetArtibute);



        }

        /// <summary>
        /// Classifies the test set according to the ID3 tree.
        /// </summary>
        public void Classify()
        {

            string s;
            List<string> artib_t = new List<string>(artibute.Keys.ToArray());
            List<string> Result = new List<string>();
            List<string> classRes = new List<string>(targetArtibute.Value);
            using (StreamReader sr = File.OpenText(_path + @"\test.txt"))
            {

                TreeNode t = _tree.root;
                while ((s = sr.ReadLine()) != null)
                {

                    bool flagsAttribute = false;
                    string[] tempVarsFromFile = s.Split(',');
                    while (!flagsAttribute)
                    {
                        if (Result.Count == 42)
                            Console.WriteLine();

                        string splitByAttributeName = t.SplitByAttributeName;

                        if (classRes.Contains(splitByAttributeName))
                        {
                            Result.Add(splitByAttributeName);
                            flagsAttribute = true;
                        }
                        else
                        {
                            string[] split = splitByAttributeName.Split('=');
                            string artibue = split[0].Remove(split[0].Length - 1);
                            split = splitByAttributeName.Split('{');
                            string num = split[1].Remove(split[1].Length - 2).Substring(1);
                            int indexVal = artib_t.IndexOf(artibue);
                            double number = Double.Parse(num);
                            double testVal = Double.Parse(tempVarsFromFile[indexVal]);
                            if (testVal > number)
                            {
                                t = t.children[1];
                            }
                            else
                                t = t.children[0];
                        }

                    }

                    t = _tree.root;
                }






            }

            string sul = "";
            int ind = 0;
            foreach (string item in Result)
            {
                sul += ind + " " + item + Environment.NewLine;
                ind++;
            }
            System.IO.File.WriteAllText(_path + @"\output.txt", sul);


        }

        /// <summary>
        /// Prints the ID3 tree structure in an XML format to a file.
        /// </summary>                                            
        public void CreateXMLTree()
        {
            _tree.BuildingProc();
            _tree.ToXMLString(_path);
        }



        /// <summary>
        /// its porpuse to check if an artibute is in numaric type
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool isNumaric(string s)
        {
            foreach (char item in s)
            {
                if (!char.IsNumber(item) && item != '.')
                    return false;
            }
            return true;
        }


    }
}
