using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace File3dmInterrogator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ToolStripMenuItem_FileOpen_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog()
            {
                InitialDirectory = "c:\\",
                Filter = "3dm files (*.3dm)|*.3dm|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                Debug.WriteLine(openFileDialog1.FileName);

                try
                {
                    
                    var doc = Rhino.FileIO.File3dm.Read(openFileDialog1.FileName);

                    var report = new StringBuilder();
                    report.AppendLine(openFileDialog1.FileName);
                    
                    report.AppendLine("Rhino Version " + doc.ApplicationName);
                    report.AppendLine("File Version " + doc.Revision.ToString());
                    report.AppendLine(doc.Objects.Count.ToString() + " Objects in this file.");
                    report.AppendLine(doc.PlugInData.Count.ToString() + " PlugInData elements in this file.");
                 
                    var objDictionary = new Dictionary<string, int>();
                    
                    foreach (var obj in doc.Objects)
                    {
                        Debug.WriteLine(obj.Geometry.ObjectType.ToString());
                        Debug.WriteLine(obj.GetType().ToString());

                        var id = obj.GetType().GetProperty("Id").GetValue(obj, null) as Guid?;

                        Debug.WriteLine(id.ToString());

                        if (objDictionary.ContainsKey(obj.Geometry.ObjectType.ToString()))
                            objDictionary[obj.Geometry.ObjectType.ToString()] = objDictionary[obj.Geometry.ObjectType.ToString()]+1;
                        else
                            objDictionary.Add(obj.Geometry.ObjectType.ToString(), 1);

                    }

                    foreach (var kvp in objDictionary)
                    {
                        string line = kvp.Key + "\t" + kvp.Value.ToString();
                        Debug.WriteLine(line);
                        report.AppendLine(line);
                    }

                    textBox1.Text = report.ToString();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }
}
