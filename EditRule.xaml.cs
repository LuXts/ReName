using Panuon.UI.Silver;
using ReName.src;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ReName
{
    /// <summary>
    /// EditRule.xaml 的交互逻辑
    /// </summary>
    public partial class EditRule : WindowX
    {
        public DealBase deal = null;

        private string OldStr;

        public int tab;

        public int max;

        public int min;

        public EditRule()
        {
            InitializeComponent();
        }

        public EditRule(string OldStr)
        {
            this.OldStr = OldStr;
            InitializeComponent();
        }

        public void OnShow()
        {
            AddIndexAimLenSlider.Maximum = max;
            AddIndexAimLenSlider.Minimum = -max;
            AddAimLenSlider.Maximum = max;
            AddAimLenSlider.Minimum = -max;
            RemoveAimLenSlider.Maximum = max;
            RemoveAimLenSlider.Minimum = -max;
            RemoveLenSlider.Maximum = max;
            RemoveLenSlider.Minimum = 0;

            SetAimLenSlider();
            SetLenSlider();
            SetAimLenEdit();
            SetLenEdit();
            SetAimStr();
            SetNewStr();

            OldStrTextBox.Text = OldStr;
            NewStrTextBox.Text = OldStr;
            RuleTab.SelectedIndex = tab;
            MyDeal();
        }

        private void SetAimLenSlider()
        {
            int v = deal.AimLen;
            if (v < -max)
            {
                v = -max;
            }
            else if (v > max)
            {
                v = max;
            }
            AddIndexAimLenSlider.Value = v;
            AddAimLenSlider.Value = v;
            RemoveAimLenSlider.Value = v;
        }

        private void SetLenSlider()
        {
            int v = deal.Len;
            if (v > max)
            {
                v = max;
            }
            RemoveLenSlider.Value = v;
        }

        private void SetAimLenEdit()
        {
            int v = deal.AimLen;
            AddIndexAimLenEdit.Text = v.ToString();
            AddAimLenEdit.Text = v.ToString();
            RemoveAimLenEdit.Text = v.ToString();
        }

        private void SetLenEdit()
        {
            int v = deal.Len;
            RemoveLenEdit.Text = v.ToString();
        }

        private void SetAimStr()
        {
            ReplaccAimStrEdit.Text = deal.AimStr;
            RegexAimStrEdit.Text = deal.AimStr;
        }

        private void SetNewStr()
        {
            ReplaceNewStrEdit.Text = deal.NewStr;
            AddNewStrEdit.Text = deal.NewStr;
            RegexNewStrEdit.Text = deal.NewStr;
        }

        private void MyDeal()
        {
            string temp;
            switch (tab)
            {
                case 5:
                    bool mark = true;
                    temp = deal.Deal(OldStrTextBox.Text, 1, out mark);
                    if (mark)
                    {
                        NewStrTextBox.Text = temp;
                    }
                    else
                    {
                        NewStrTextBox.Text = "该旧文件名中未能匹配到此正则表达式。";
                    }
                    break;
                default:
                    temp = deal.Deal(OldStrTextBox.Text, 1, out _);
                    NewStrTextBox.Text = temp;
                    break;
            }
        }

        private void ChangeData()
        {
            DealBase temp = null;
            switch (tab)
            {
                case 0:
                    temp = new ReplaceDeal();
                    break;
                case 1:
                    temp = new CleanDeal();
                    break;
                case 2:
                    temp = new AddIndexDeal();
                    break;
                case 3:
                    temp = new AddDeal();
                    break;
                case 4:
                    temp = new RemoveDeal();
                    break;
                case 5:
                    temp = new RegexDeal();
                    break;
            }
            temp.AimLen = deal.AimLen;
            temp.Len = deal.Len;
            temp.AimStr = deal.AimStr;
            temp.NewStr = deal.NewStr;
            deal = temp;
        }
        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            sender.GetType().ToString();
            TextBox temp = (TextBox)sender;
            Regex re = new Regex("^-?[1-9]?[0-9]*$");
            e.Handled = !re.IsMatch(temp.Text + e.Text);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tab = RuleTab.SelectedIndex;
            ChangeData();
            MyDeal();
        }

        private void NewStrEdit_PreviewTextInput(object sender, TextChangedEventArgs e)
        {
            if (sender.GetType().ToString() == "System.Windows.Controls.TextBox")
            {
                TextBox textBox = (TextBox)sender;
                deal.NewStr = textBox.Text;
                MyDeal();
            }
        }

        private void AimStrEdit_PreviewTextInput(object sender, TextChangedEventArgs e)
        {
            if (sender.GetType().ToString() == "System.Windows.Controls.TextBox")
            {
                TextBox textBox = (TextBox)sender;
                deal.AimStr = textBox.Text;
                MyDeal();
            }
        }

        private void AimLenEdit_PreviewTextInput(object sender, TextChangedEventArgs e)
        {
            if (sender.GetType().ToString() == "System.Windows.Controls.TextBox")
            {
                TextBox textBox = (TextBox)sender;
                if (textBox.Text != "-" && textBox.Text != "")
                {
                    deal.AimLen = int.Parse(textBox.Text);
                }
                else
                {
                    deal.AimLen = 0;
                }
                MyDeal();
                SetAimLenSlider();
            }
        }

        private void LenEdit_PreviewTextInput(object sender, TextChangedEventArgs e)
        {
            if (sender.GetType().ToString() == "System.Windows.Controls.TextBox")
            {
                TextBox textBox = (TextBox)sender;
                if (textBox.Text != "-" && textBox.Text != "")
                {
                    deal.Len = int.Parse(textBox.Text);
                }
                else
                {
                    deal.Len = 0;
                }
                MyDeal();
                SetLenSlider();
            }
        }

        private void AimLenSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender.GetType().ToString() == "System.Windows.Controls.Slider")
            {
                Slider slider = (Slider)sender;
                deal.AimLen = (int)slider.Value;
                MyDeal();
                SetAimLenEdit();
            }
        }

        private void LenSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender.GetType().ToString() == "System.Windows.Controls.Slider")
            {
                Slider slider = (Slider)sender;
                deal.Len = (int)slider.Value;
                MyDeal();
                SetLenEdit();
            }
        }

        private void ApplyButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
