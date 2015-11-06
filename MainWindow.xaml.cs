using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OneBigBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileSystem fs;
        public MainWindow()
        {
            InitializeComponent();
            fs = new FileSystem();
            Block b = new Block("memory10", 10,fs);
            Block b2 = new Block("memory5", 5, fs);
            fs.add(b2);
            fs.open(b);
            

            File f1 = (File)fs.writeFile("test", new byte[] { 1, 2, 3, 4, 5 });
            Console.WriteLine(String.Join(", ", b.data));
            File f2 = (File)fs.writeFile("tet2", new byte[] { 11,12,13,14,15 });
            Console.WriteLine(String.Join(", ", b.data));
            fs.delete(f1);
            fs.writeFile("test3", new byte[] { 21, 22, 23 });
            fs.writeFile("test4", new byte[] { 201, 202, 203, 204, 205, 206 });
            Console.WriteLine(String.Join(", ", b.data));


            refresh();
        }

        void refresh()
        {
            var oList = new System.Collections.ObjectModel.ObservableCollection<byte>(fs.blockPool.First().data);
            listView.DataContext = oList;

            Binding binding = new Binding();
            listView.SetBinding(ListBox.ItemsSourceProperty, binding);

            oList = new System.Collections.ObjectModel.ObservableCollection<byte>(fs.blockPool.Last().data);
            listView2.DataContext = oList;

            Binding binding2 = new Binding();
            listView2.SetBinding(ListBox.ItemsSourceProperty, binding2);




            var oList2 = new System.Collections.ObjectModel.ObservableCollection<File>(fs.ToC);
            listView3.DataContext = oList2;

            Binding binding3 = new Binding();
            listView3.SetBinding(ListBox.ItemsSourceProperty, binding3);
        }
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                microasm(textBox.Text);
                textBox.Clear();
            }
        }

        void microasm(string cmd)
        {
            textBlock.Text += ">" +cmd + Environment.NewLine;
            string[] c = cmd.Split(' ');
            if (c[0] == "w")
            {
                string[] ss = c.Skip(2).ToArray();
                byte[] arg = new byte[ss.Length];
                for (int i = 0; i < ss.Length; i++)
                {
                    arg[i] = byte.Parse(ss[i]);
                }
                fs.writeFile(c[1], arg);
              
            }
           else if (c[0] == "del")
            {
                fs.delete(fs.GetFile(c[1]));
               
            }
            else if (c[0] == "read")
            {
                textBlock.Text += String.Join(", ", fs.readFileBytes(fs.GetFile(c[1]))) + Environment.NewLine;

            }
            else if (c[0] == "space")
            {
                fs.blockPool.ToList()
                    .ForEach(
                        x =>
                            textBlock.Text +=
                                x.ToString() + " " + (x.length - x.usedSpace) + "/" + x.length + Environment.NewLine);
            }
            refresh();


        }
    }
}
