using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using TenchoNihonGo.Utils;
// Conversion mode (dwCMode)


namespace TenchoNihonGo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnToHiragana_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CFELanguage vLanguage = new CFELanguage();
                vLanguage.Open();

                string strInput = editInput.Text;
                string strOutput = vLanguage.GetJMorphResult(CFELanguage.FELANG_REQ_REV, CFELanguage.FELANG_CMODE_HIRAGANAOUT, strInput.Length, strInput, IntPtr.Zero);

                editOutput.Text = strOutput;
                vLanguage.Close();
            }
            catch
            {

            }
        }

        private void btnToKanji_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CFELanguage vLanguage = new CFELanguage();
                vLanguage.Open();

                string strOutput = editOutput.Text;
                string strInput = vLanguage.GetJMorphResult(CFELanguage.FELANG_REQ_CONV, CFELanguage.FELANG_CMODE_HIRAGANAOUT, strOutput.Length, strOutput, IntPtr.Zero);

                editInput.Text = strInput;

                vLanguage.Close();
            }
            catch
            {

            }
        }

        private void btnToKatakana_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CFELanguage vLanguage = new CFELanguage();
                vLanguage.Open();

                string strInput = editInput.Text;
                string strOutput = vLanguage.GetJMorphResult(CFELanguage.FELANG_REQ_REV, CFELanguage.FELANG_CMODE_KATAKANAOUT, strInput.Length, strInput, IntPtr.Zero);

                editOutput.Text = strOutput;
                vLanguage.Close();
            }
            catch
            {

            }
        }

        private void btnRomanToKana_Click(object sender, RoutedEventArgs e)
        {
            

            try
            {
                CFELanguage vLanguage = new CFELanguage();
                vLanguage.Open();

                string strInput = "";
                string strOutput = editOutput.Text;
                
                strInput = vLanguage.GetJMorphResult(CFELanguage.FELANG_REQ_REV, CFELanguage.FELANG_CMODE_PRECONV, strOutput.Length, strOutput, IntPtr.Zero);

                editInput.Text = strInput;
                vLanguage.Close();
            }
            catch
            {

            }

        }

        private void btnToRoman_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CFELanguage vLanguage = new CFELanguage();
                vLanguage.Open();

                string strInput = editInput.Text;
                string strOutput = vLanguage.GetJMorphResult(CFELanguage.FELANG_REQ_REV, CFELanguage.FELANG_CMODE_KATAKANAOUT, strInput.Length, strInput, IntPtr.Zero);

                strOutput = CLSRoman.GetRomaji(strOutput);


                editOutput.Text = strOutput;
                vLanguage.Close();
            }
            catch
            {

            }
        }

        private void btnToXieyin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CFELanguage vLanguage = new CFELanguage();
                vLanguage.Open();

                string strInput = editInput.Text;
                string strOutput = vLanguage.GetJMorphResult(CFELanguage.FELANG_REQ_REV, CFELanguage.FELANG_CMODE_KATAKANAOUT, strInput.Length, strInput, IntPtr.Zero);

                strOutput = CLSRoman.GetXieyin(strOutput);


                editOutput.Text = strOutput;
                vLanguage.Close();
            }
            catch
            {

            }
        }
    }
}