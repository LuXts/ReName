using System;
using System.Text.RegularExpressions;

namespace ReName.src
{
    public abstract class DealBase
    {
        protected String aimStr;
        protected String newStr;
        protected int aimLen;
        protected int len;

        public string AimStr { get => aimStr; set => aimStr = value; }
        public string NewStr { get => newStr; set => newStr = value; }
        public int AimLen { get => aimLen; set => aimLen = value; }
        public int Len { get => len; set => len = value; }

        public abstract String Deal(String inStr, int index, out bool output);

        public abstract String[] GetDescription();
    }

    public class ReplaceDeal : DealBase
    {
        override public String Deal(String inStr, int index, out bool output)
        {
            if (this.aimStr.Length == 0)
            {
                output = false;
                return inStr;
            }
            else
            {
                String temp = inStr.Replace(this.aimStr, this.newStr);
                if (temp == inStr)
                {
                    output = false;
                    return inStr;
                }
                else
                {
                    output = true;
                    return temp;
                }
            }
        }

        override public String[] GetDescription()
        {
            string[] myself = new string[2];
            myself[0] = "替换";
            myself[1] = "把" + this.aimStr + "替换为" + this.newStr;
            return myself;
        }
    }

    public class CleanDeal : DealBase
    {
        override public String Deal(String inStr, int index, out bool output)
        {
            output = true;
            return "";
        }

        override public String[] GetDescription()
        {
            string[] myself = new string[2];
            myself[0] = "清除";
            myself[1] = "清除全部字符";
            return myself;
        }
    }

    public class AddIndexDeal : DealBase
    {
        override public String Deal(String inStr, int index, out bool output)
        {
            output = true;
            if (aimLen > inStr.Length)
            {
                return inStr + index.ToString().PadLeft(ReGlobal.PathCount, '0');
            }
            else if (aimLen < 0)
            {
                int temp = aimLen;
                if (-temp >= inStr.Length)
                {
                    return index.ToString().PadLeft(ReGlobal.PathCount, '0') + inStr;
                }
                return inStr.Insert(inStr.Length + temp + 1,
                                    index.ToString().PadLeft(ReGlobal.PathCount, '0'));
            }
            else
            {
                return inStr.Insert(this.aimLen, index.ToString().PadLeft(ReGlobal.PathCount, '0'));
            }
        }

        override public String[] GetDescription()
        {
            string[] myself = new string[2];
            myself[0] = "添加序号";
            myself[1] = "在第" + this.aimLen.ToString() + "位字符后添加序号。";
            return myself;
        }
    }

    public class AddDeal : DealBase
    {
        override public String Deal(String inStr, int index, out bool output)
        {
            output = true;
            if (aimLen > inStr.Length)
            {
                return inStr + newStr;
            }
            else if (aimLen < 0)
            {
                int temp = aimLen;
                if (-temp >= inStr.Length)
                {
                    return newStr + inStr;
                }
                return inStr.Insert(inStr.Length + temp + 1, newStr);
            }
            else
            {
                return inStr.Insert(aimLen, newStr);
            }
        }

        override public String[] GetDescription()
        {
            string[] myself = new string[2];
            myself[0] = "添加字符";
            myself[1] = "添加" + this.newStr + "到第" + this.aimLen.ToString() + "个位置后。";
            return myself;
        }
    }

    public class RemoveDeal : DealBase
    {
        override public String Deal(String inStr, int index, out bool output)
        {
            output = true;
            if (aimLen >= inStr.Length)
            {
                return inStr;
            }
            else if (aimLen >= 0 && aimLen + Len > inStr.Length)
            {
                return inStr.Remove(aimLen);
            }
            else if (aimLen < 0)
            {
                int temp = aimLen;
                if (-temp >= inStr.Length)
                {
                    temp = -inStr.Length;
                }
                // TODO
                if (inStr.Length + temp + Len > inStr.Length)
                {
                    return inStr.Remove(inStr.Length + temp);
                }
                return inStr.Remove(inStr.Length + temp, Len);
            }
            else
            {
                return inStr.Remove(aimLen, Len);
            }
        }

        override public String[] GetDescription()
        {
            string[] myself = new string[2];
            myself[0] = "删除字符";
            myself[1] = "从第" + this.aimLen.ToString() +
                        "个字符开始删除" + this.Len.ToString() + "个字符。";
            return myself;
        }
    }

    public class RegexDeal : DealBase
    {
        override public String Deal(String inStr, int index, out bool output)
        {
            Regex regex;
            output = false;
            try
            {
                regex = new Regex(aimStr);
                output = regex.IsMatch(inStr);
                if (output)
                {
                    return regex.Replace(inStr, newStr);
                }
            }
            catch (System.ArgumentException e)
            {
            }
            return inStr;
        }

        override public String[] GetDescription()
        {
            string[] myself = new string[2];
            myself[0] = "正则替换";
            myself[1] = "将" + aimStr + "替换为" + newStr;
            return myself;
        }
    }
}
