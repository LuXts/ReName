using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using Panuon.UI.Silver;
using ReName.src;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ReName
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowX
    {
        private string[] OldPathStr;
        private string[] NewPathStr;
        private List<DealBase> RuleList = new List<DealBase>();
        private string Path;

        private int max;

        public MainWindow()
        {
            OldPathStr = new String[0];
            DealOldData();
            InitializeComponent();
        }

        private void Show_List()
        {
            ListDataChange(Old_Path_List, OldPathStr);
            ListDataChange(New_Path_list, NewPathStr);
        }

        private void DealOldData()
        {
            NewPathStr = new String[OldPathStr.Length];
            for (int i = 0; i < OldPathStr.Length; i++)
            {
                NewPathStr[i] = Deal(OldPathStr[i], i + 1);
            }
        }

        private void ListDataChange(ListBox inList, String[] inDataList)
        {
            inList.Items.Clear();
            for (int i = 0; i < inDataList.Length; i++)
            {
                DataListItem temp = new DataListItem();
                temp.index = i + 1;
                temp.str = inDataList[i];
                inList.Items.Add(temp);
            }
        }

        private String Deal(String inStr, int n)
        {
            String Temp = inStr;
            for (int i = 0; i < RuleList.Count; i++)
            {
                Temp = RuleList[i].Deal(Temp, n, out _);
            }

            return Temp;
        }

        private void Show_Rule_List()
        {
            Rule_ListBox.Items.Clear();
            for (int i = 0; i < RuleList.Count; i++)
            {
                RuleListItem temp = new RuleListItem();
                string[] t = RuleList[i].GetDescription();
                temp.name = t[0];
                temp.description = t[1];
                Rule_ListBox.Items.Add(temp);
            }
        }

        private void OpenFileButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Title = "请选择文件";
            ofd.Filter = "全部文件(*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                OldPathStr = ofd.SafeFileNames;
                int j = OldPathStr.Length;
                int k;
                for (k = 0; j / 10 != 0; k++)
                {
                    j /= 10;
                }
                ReGlobal.PathCount = k + 1;
                DealOldData();
                Path = ofd.FileName.Replace(ofd.SafeFileName, "");
                max = 0;
                for (int i = 0; i < OldPathStr.Length; i++)
                {
                    if (max < OldPathStr[i].Length)
                    {
                        max = OldPathStr[i].Length;
                    }
                }
            }
            Show_List();
        }

        private void AddRuleButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OldPathStr.Length == 0)
            {
                MessageBoxX.Show("您尚未添加任何文件！", "错误", MessageBoxButton.OK, MessageBoxIcon.Error);
                return;
            }
            EditRule editRule = new EditRule(NewPathStr[0]);
            editRule.deal = new ReplaceDeal();
            editRule.deal.AimLen = 0;
            editRule.deal.AimStr = "";
            editRule.deal.NewStr = "";
            editRule.deal.Len = 0;
            editRule.tab = 0;
            editRule.max = max;

            editRule.OnShow();
            editRule.ShowDialog();
            if (editRule.DialogResult == true)
            {
                RuleList.Add(editRule.deal);
            }
            DealOldData();
            Show_List();
            Show_Rule_List();
        }

        private void DelRuleButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Rule_ListBox.SelectedIndex == -1)
            {
                MessageBoxX.Show("您尚未选择任何规则！", "错误", MessageBoxButton.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                RuleList.Remove(RuleList[Rule_ListBox.SelectedIndex]);
                DealOldData();
                Show_List();
                Show_Rule_List();
            }
        }

        private void EditRuleButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Rule_ListBox.SelectedIndex == -1)
            {
                MessageBoxX.Show("您尚未选择任何规则！", "错误", MessageBoxButton.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                int i = Rule_ListBox.SelectedIndex;
                string tempPath = OldPathStr[0];
                for (int k = 0; k < i; k++)
                {
                    tempPath = RuleList[k].Deal(tempPath, 1, out _);
                }
                DealBase d = RuleList[Rule_ListBox.SelectedIndex];
                EditRule editRule = new EditRule(tempPath);
                editRule.deal = d;
                string temp = d.GetType().ToString();
                if (temp == "ReName.src.ReplaceDeal")
                {
                    editRule.tab = 0;
                }
                else if (temp == "ReName.src.CleanDeal")
                {
                    editRule.tab = 1;
                }
                else if (temp == "ReName.src.AddIndexDeal")
                {
                    editRule.tab = 2;
                }
                else if (temp == "ReName.src.AddDeal")
                {
                    editRule.tab = 3;
                }
                else if (temp == "ReName.src.RemoveDeal")
                {
                    editRule.tab = 4;
                }
                else if (temp == "ReName.src.RegexDeal")
                {
                    editRule.tab = 5;
                }
                editRule.max = max;

                editRule.OnShow();
                editRule.ShowDialog();
                if (editRule.DialogResult == true)
                {
                    RuleList.Remove(d);
                    RuleList.Insert(i, editRule.deal);
                }
                DealOldData();
                Show_List();
                Show_Rule_List();
            }
        }

        private void MoveRuleUpButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Rule_ListBox.SelectedIndex == -1)
            {
                MessageBoxX.Show("您尚未选择任何规则！", "错误", MessageBoxButton.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                int i = Rule_ListBox.SelectedIndex;
                if (i == 0)
                {
                    return;
                }
                DealBase d = RuleList[Rule_ListBox.SelectedIndex];
                RuleList.Remove(d);
                RuleList.Insert(i - 1, d);
                DealOldData();
                Show_List();
                Show_Rule_List();
                Rule_ListBox.SelectedIndex = i - 1;
            }
        }

        private void MoveRuleDownButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Rule_ListBox.SelectedIndex == -1)
            {
                MessageBoxX.Show("您尚未选择任何规则！", "错误", MessageBoxButton.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                int i = Rule_ListBox.SelectedIndex;
                if (i == (RuleList.Count - 1))
                {
                    return;
                }
                DealBase d = RuleList[Rule_ListBox.SelectedIndex];
                RuleList.Remove(d);
                RuleList.Insert(i + 1, d);
                DealOldData();
                Show_List();
                Show_Rule_List();
                Rule_ListBox.SelectedIndex = i + 1;
            }
        }

        private void ApplyButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OldPathStr.Length == 0)
            {
                MessageBoxX.Show("您尚未添加任何文件！", "错误", MessageBoxButton.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (RuleList.Count == 0)
                {
                    MessageBoxX.Show("您尚未添加任何规则！", "警告", MessageBoxButton.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    for (int i = 0; i < NewPathStr.Length; i++)
                    {
                        if (NewPathStr[i] == "")
                        {
                            MessageBoxX.Show("任何输出文件名不能为空！", "错误", MessageBoxButton.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    MessageBoxResult t = MessageBoxX.Show("您是否确认应用所有重命名规则？", "应用确认", MessageBoxButton.OKCancel, MessageBoxIcon.Info, DefaultButton.YesOK);
                    if (t == MessageBoxResult.OK)
                    {
                        Computer rename = new Computer();
                        for (int i = 0; i < OldPathStr.Length; i++)
                        {
                            rename.FileSystem.RenameFile(Path + OldPathStr[i], NewPathStr[i]);
                        }
                        OldPathStr = new string[0];
                        NewPathStr = new string[0];
                        Path = "";
                        ReGlobal.PathCount = 0;
                        max = 0;
                        Show_List();
                        NoticeX.Show("重命名文件成功！", "批量重命名工具", MessageBoxIcon.Info, 3000);
                    }

                }
            }


        }


        private void CloseButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBoxResult t = MessageBoxX.Show("您是否确认退出此程序？", "退出", MessageBoxButton.OKCancel, MessageBoxIcon.Warning, DefaultButton.YesOK);
            if (t == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

    }

    public class DataListItem
    {
        public int index { get; set; }
        public String str { get; set; }
    }

    public class RuleListItem
    {
        public String name { get; set; }
        public String description { get; set; }
    }
}
