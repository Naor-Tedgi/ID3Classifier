using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ID3Project
{
    class Tree
    {
        private DataTable table;
        private Dictionary<string, List<string>> artibute;
        private KeyValuePair<string, string[]> targetArtibute;

        /// <summary>
        /// Root of tree
        /// </summary>
        public TreeNode root { get; set; }
        public Tree(DataTable table, Dictionary<string, List<string>> artibute, KeyValuePair<string, string[]> targetArtibute)
        {
            this.table = table;
            this.artibute = artibute;
            this.targetArtibute = targetArtibute;
        }


        /// <summary>
        /// creating the xml file acording to a chaning ot the tree node
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ToXMLString(string path)
        {
            string s = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + ':' + root.XmlConverter();

            string[] s_t = s.Split(':');

            using (XmlWriter writer = XmlWriter.Create(path + @"\tree.xml"))
            {
                writer.WriteStartDocument(true);
                foreach (string item in s_t)
                {

                    bool flag = false;
                    if (item == "<?xml version=\"1.0\" encoding=\"utf-8\" ?>")
                        continue;

                    if (item == "<Children>")
                    {
                        writer.WriteStartElement("Children");
                        flag = true;
                    }

                    if (item == "</Children>" || item == "</Node>")
                    {
                        writer.WriteEndElement();
                        flag = true;
                    }


                    if (!flag & item != "</Node>" & item != "")
                    {
                        string[] ll = item.Split('~');

                        writer.WriteStartElement("Node");
                        writer.WriteAttributeString("Name", ll[0]);
                        writer.WriteAttributeString("Value", ll[1]);
                        writer.WriteAttributeString("Instances", ll[2]);
                        if (ll.Length == 4)
                            writer.WriteEndElement();


                    }



                }

            }


            return s;
        }


        /// <summary>
        /// building the tree from the  id3 Algorithem
        /// </summary>
        public void BuildingProc()
        {
            root = new TreeNode(table);
            root.BuildingTree(artibute, targetArtibute);

        }



    }

    /// <summary>
    /// Represents a single node in an ID3 tree
    /// </summary>
    class TreeNode
    {


        /// <summary>
        /// Holds the instances relevant to the node
        /// </summary>
        public DataTable data { get; set; }

        /// <summary>
        /// child nodes of this node
        /// </summary>
        public List<TreeNode> children;
        string type;
        /// <summary>
        /// Name of attribute to split the children by
        /// </summary>
        public string SplitByAttributeName;

        /// <summary>
        /// Value of current node attribute
        /// </summary>
        public string NodeAttributeValue;

        private Dictionary<string, List<string>> artibute;
        private KeyValuePair<string, string[]> targetArtibute;
        private int instance;
        private int splitNum;

        public void BuildingTree(Dictionary<string, List<string>> artibute, KeyValuePair<string, string[]> targetArtibute)
        {
            this.artibute = new Dictionary<string, List<string>>(artibute);
            this.targetArtibute = new KeyValuePair<string, string[]>(targetArtibute.Key, targetArtibute.Value);
            this.BuiledTree(data);
        }





        public TreeNode(DataTable dataSet, string _NodeAttributeValue, string _SplitByAttributeName, string type, int _instance)
        {
            this.NodeAttributeValue = _NodeAttributeValue;
            this.SplitByAttributeName = _SplitByAttributeName;
            children = new List<TreeNode>();
            this.type = type;
            this.data = dataSet;
            instance = _instance;

        }
        public TreeNode(DataTable dataSet)
        {
            data = dataSet;
            children = new List<TreeNode>();
            NodeAttributeValue = "root";
            splitNum = 1;
        }

        /// <summary>
        /// bulding an chning format of node to cunsruct a easy format for the creation of xml file
        /// </summary>
        /// <returns></returns>
        public string XmlConverter()
        {
            string s = "";
            List<string> leafs = new List<string>(targetArtibute.Value);

            if (children.Count > 0)
            {
                s += NodeXMLine(NodeAttributeValue, SplitByAttributeName, instance, 1) + ':';

                s += "<Children>" + ':';

                foreach (TreeNode item in children)
                {
                    //  if (!leafs.Contains(item.SplitByAttributeName))
                    s += item.XmlConverter();

                }

                s += "</Children>" + ':';

                s += "</Node>" + ':';

            }
            else
                s = NodeXMLine(NodeAttributeValue, SplitByAttributeName, instance, 0);




            return s;

        }

        /// <summary>
        /// create a string from a node for the xml convertor
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Val"></param>
        /// <param name="instance"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private string NodeXMLine(string Name, string Val, int instance, int i)
        {

            if (i == 1)
                return Name + "~" + Val + "~" + instance;
            else return Name + "~" + Val + "~" + instance + "~" + "fixed :";



        }
        private void BuiledTree(DataTable currTable)
        {

            if (artibute.Count == 0)
                return;

            List<DataTable> split1 = new List<DataTable>();
            #region Step1
            string s;
            DataView view = new DataView(currTable);
            DataRow[] dt;
            foreach (string item in artibute.Keys)
            {

                DataTable t = new DataTable();
                t.Columns.Add("Distinct", typeof(double));

                foreach (string Artibute in targetArtibute.Value)
                {
                    t.Columns.Add(Artibute + " L", typeof(int));
                    t.Columns.Add(Artibute + " R", typeof(int));

                }
                t.Columns.Add("Total L", typeof(int));
                t.Columns.Add("Total R", typeof(int));

                DataRow dr = t.NewRow();
                DataTable distinctValues = view.ToTable(true, item);



                int TotalLeft, TotalRight, i;
                i = TotalLeft = TotalRight = 0;
                foreach (DataRow row in distinctValues.Rows)
                {
                    dr[0] = row[item];
                    i++;
                    foreach (string targets in targetArtibute.Value)
                    {

                        s = item + "  <=  " + row[item] + " AND class = '" + targets + "'";
                        dt = currTable.Select(s);
                        TotalLeft += dt.Length;
                        dr[i] = dt.Length;
                        i++;
                        s = item + "  >  " + row[item] + " AND class = '" + targets + "'";
                        dt = currTable.Select(s);
                        TotalRight += dt.Length;
                        dr[i] = dt.Length;
                        i++;

                    }
                    dr[i] = TotalLeft;
                    i++;
                    dr[i] = TotalRight;


                    t.Rows.Add(dr);
                    dr = t.NewRow();
                    i = TotalRight = TotalLeft = 0;

                }




                split1.Add(t);







            }
            #endregion

            findRoot(split1);

            Console.WriteLine();

        }
        private void findRoot(List<DataTable> split1)
        {


            int i = 0;
            int TotalL, TotalR;
            TotalL = TotalR = 0;
            decimal currGiniSplit = 1;

            KeyValuePair<int, string> minVal = new KeyValuePair<int, string>(9, "9");
            foreach (DataTable item in split1)
            {

                foreach (DataRow Row in item.Rows)
                {
                    int totalL, totalR;
                    decimal giniL, giniR, giniSplit;
                    giniL = giniR = 1;
                    giniSplit = 0;
                    string s = string.Join(";", Row.ItemArray.Select(x => x.ToString()));
                    string[] vals = s.Split(';');
                    decimal[] numericVal = new decimal[vals.Length];
                    for (int j = 0; j < vals.Length; j++)
                    {
                        numericVal[j] = Decimal.Parse(vals[j]);
                    }
                    totalL = (int)numericVal[numericVal.Length - 2];
                    totalR = (int)numericVal[numericVal.Length - 1];

                    if (numericVal[numericVal.Length - 2] != 0)
                        for (int L = 1; L < numericVal.Length - 2; L = L + 2)
                        {

                            numericVal[L] = numericVal[L] / numericVal[numericVal.Length - 2];
                            giniL = giniL - (numericVal[L] * numericVal[L]);
                        }

                    if (numericVal[numericVal.Length - 1] != 0)
                        for (int R = 2; R < numericVal.Length - 2; R = R + 2)
                        {
                            numericVal[R] = numericVal[R] / numericVal[numericVal.Length - 1];
                            giniR = giniR - (numericVal[R] * numericVal[R]);

                        }

                    decimal sum = numericVal[numericVal.Length - 1] + numericVal[numericVal.Length - 2];
                    giniSplit = (numericVal[numericVal.Length - 2] / sum) * giniL + (numericVal[numericVal.Length - 1] / sum) * giniR;


                    if (giniSplit < 0)
                    {
                        Console.WriteLine();
                    }



                    if (giniSplit < currGiniSplit)
                    {
                        minVal = new KeyValuePair<int, string>(i, vals[0]);
                        currGiniSplit = giniSplit;
                        TotalL = totalL;
                        TotalR = totalR;

                    }


                }
                i++;
            }





            string Roots = artibute.Keys.ToArray()[minVal.Key];
            string rootvAL = minVal.Value;


            if (rootvAL == "5")
            {
                Console.WriteLine();
            }


            if (this.NodeAttributeValue == "root")
            {
                this.instance = TotalR + TotalL;

            }
            this.SplitByAttributeName = Roots + " = { " + rootvAL + " }";


            DataView view = new DataView(this.data);
            DataTable distinctValues = view.ToTable(true, "class");
            if (distinctValues.Rows.Count == 1)
            {
                SplitByAttributeName = distinctValues.Rows[0].ItemArray[0].ToString();
                return;
            }
            else if (artibute.Count == 0)
            {

                getMajority();
            }

            artibute.Remove(Roots);


            DataRow[] Left = data.Select(Roots + " <= " + rootvAL);

            if (Left.Length > 0)
            {

                DataTable left_Table = Left.CopyToDataTable();

                view = new DataView(left_Table);
                distinctValues = view.ToTable(true, "class");
                if (distinctValues.Rows.Count == 1)
                {
                    children.Add(new TreeNode(left_Table, Roots + " <= " + rootvAL, distinctValues.Rows[0].ItemArray[0].ToString(), "left", TotalL));
                }
                else if (artibute.Count == 0)
                {

                    TreeNode t = new TreeNode(left_Table, Roots + " <= " + rootvAL, SplitByAttributeName, "left", TotalL);
                    t.getMajority();
                    children.Add(t);
                }
                else
                    children.Add(new TreeNode(left_Table, Roots + " <= " + rootvAL, Roots, "left", TotalL));

            }

            DataRow[] Right = data.Select(Roots + " > " + rootvAL);
            if (Right.Length > 0)
            {
                DataTable right_Table = Right.CopyToDataTable();
                view = new DataView(right_Table);
                distinctValues = view.ToTable(true, "class");
                if (distinctValues.Rows.Count == 1)
                {
                    children.Add(new TreeNode(right_Table, Roots + " > " + rootvAL, distinctValues.Rows[0].ItemArray[0].ToString(), "Right", TotalR));

                }
                else if (artibute.Count == 0)
                {
                    TreeNode t = new TreeNode(right_Table, Roots + " > " + rootvAL, SplitByAttributeName, "Right", TotalR);
                    t.getMajority();
                    children.Add(t);

                }
                else
                    children.Add(new TreeNode(right_Table, Roots + " > " + rootvAL, Roots, "Right", TotalR));

            }




            foreach (TreeNode item in children)
            {
                item.BuildingTree(artibute, targetArtibute);
            }







        }
        /// <summary>
        /// dicide in a case ther isnt any more artinute about whicth property this nide clasifide
        /// </summary>
        public void getMajority()
        {
            Dictionary<string, int> temp = new Dictionary<string, int>();
            for (int z = 0; z < this.data.Rows.Count; z++)
            {
                if (!temp.ContainsKey(this.data.Rows[z].ItemArray[this.data.Rows[z].ItemArray.Length - 1].ToString()))
                    temp.Add(this.data.Rows[z].ItemArray[this.data.Rows[z].ItemArray.Length - 1].ToString(), 1);
                else
                    temp[this.data.Rows[z].ItemArray[this.data.Rows[z].ItemArray.Length - 1].ToString()]++;


            }

            List<string> vals = new List<string>(temp.Keys);
            var max = (from l in vals
                       from d in temp
                       where d.Key == l
                       orderby d.Value
                       select new
                       {
                           d.Value
                       }).LastOrDefault();
            foreach (KeyValuePair<string, int> item in temp)
            {
                if (item.Value == max.Value)
                {
                    SplitByAttributeName = item.Key;

                }
            }
        }





    }
}
