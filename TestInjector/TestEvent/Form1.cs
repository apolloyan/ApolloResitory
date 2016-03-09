using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestEvent
{
    public partial class Form1 : Form
    {
        delegate void FeedBack(string info);
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //await Task.Delay(10000).ConfigureAwait(false);
            await TestAsync();
            //MessageBox.Show(Thread.CurrentContext.ContextID.ToString());
            ShowInfo("ok", new FeedBack(OutputInfo));

            FeedBack f1 = new FeedBack(OutputInfo);
            Test test = new Test();
            test.teststr = SomeType.str;
            Test test1 = test.Clone();
            test1.teststr = "world";
            FeedBack feedchain = null;
            feedchain = (FeedBack)Delegate.Combine(feedchain, f1);
            ShowInfo(test.teststr,feedchain);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(TaskScheduler.Current.Id.ToString());
        }

        private async Task TestAsync()
        {
            await Task.Run(() => { MessageBox.Show(Thread.CurrentContext.ContextID.ToString()); }).ConfigureAwait(false);
            await Task.Delay(1000);
            await Task.Run(() => { MessageBox.Show(Thread.CurrentContext.ContextID.ToString()); }).ConfigureAwait(false);
        }

        private static void ShowInfo(string info, FeedBack fed)
        {
            if (!string.IsNullOrEmpty(info)) {
                fed(info);
            }
        }

        private static void OutputInfo(string info)
        {
            MessageBox.Show(info + " is ok!");
        }
    }

    public class Test
    {
        public Test Clone()
        {
            return (Test)MemberwiseClone();
        }

        public string teststr { get; set; }

    }

    public static class TestEx
    {
        public static  string indexof(this Test test)
        {
            return "kkk";
        }
    }

    internal class SomeType
    {
        public static string str = "0";
        static SomeType()
        {
            str = "9";
        }
    }
}
