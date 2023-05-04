using System;
using System.Collections.Generic;
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

namespace WaCalcter
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<double, double> Quantity_Coefficient = new Dictionary<double, double>() 
        {
            { 5.0, 2.7 },
            { 15.0, 2.4 },
            { 40.0, 2.1 },
            { 70.0, 2.0 },
            { 100.0, 1.9 },
            { 200.0, 1.8 },
            { 500.0, 1.6 },
            { 1000.0, 1.5 },
        };
        List<double> QuantityList = new List<double>() { 5.0, 15.0, 40.0, 70.0, 100.0, 200.0, 500.0, 1000.0 };
        public MainWindow()
        {
            InitializeComponent();
        }
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {
                // throw;
            }
        }
        private double Check_QuantityRange(double quantity)
        {
            double result = 0;
            for (int i = 0; i < QuantityList.Count; i++)
            {
                if (quantity <= 0)
                {
                    Tbx_DayAvg_Check.Text = "0~0";
                    result = 0;
                }
                else if (quantity < QuantityList[0] && quantity > 0)
                {
                    Tbx_DayAvg_Check.Text = "0~5";
                    result = 2.7;
                }
                else if (quantity == QuantityList[i])
                {
                    Tbx_DayAvg_Check.Text = $"{QuantityList[i]}";
                    result = Quantity_Coefficient[QuantityList[i]];
                }
                else if (quantity >= QuantityList[7])
                {
                    Tbx_DayAvg_Check.Text = ">=1000";
                    result = 1.5;
                }
                else if (quantity > QuantityList[i] && quantity < QuantityList[i + 1])
                {
                    try
                    {
                        // 已经A1对应的数值为B1，A2对应的数值为B2，计算A对应的数值B
                        // (A1, B1),(A2, B2), (A,B)
                        // B = B1 - （A1-A）/(A1-A2) * (B1 - B2)
                        double B2 = Quantity_Coefficient[QuantityList[i + 1]];
                        double B1 = Quantity_Coefficient[QuantityList[i]];
                        double A1 = QuantityList[i];
                        double A2 = QuantityList[i + 1];
                        double A = quantity;
                        result = B1 - (A1 - A) / (A1 - A2) * (B1 - B2);
                        Tbx_DayAvg_Check.Text = $"{A1}~{A2}";
                    }
                    catch { continue; }
                }
            }
            return result;
        }
        private void MinMumBt_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseBt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Tbx_Input_Ls_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(Tbx_Input_Ls.Text.Trim(), out double result))
            {
                double Tbx_Input_Ls_Num = double.Parse(Tbx_Input_Ls.Text.Trim());
                // 1L = 0.001m3, 1s = 1/86400d
                double Tbx_Input_md_Num = Tbx_Input_Ls_Num * 0.001 * 86400;
                Tbx_Input_md.Text = Tbx_Input_md_Num.ToString();
                Tbx_Chg_Coefficient.Text = Check_QuantityRange(Tbx_Input_Ls_Num).ToString("0.00");
                Tbx_MaxQuantity.Text = (Tbx_Input_Ls_Num * double.Parse(Tbx_Chg_Coefficient.Text)).ToString("0.0000");
            }
            else
            {
                Tbx_Input_Ls.Text = "";
                Tbx_Input_md.Text = "";
            }
        }

        private void Tbx_Input_md_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(Tbx_Input_md.Text.Trim(), out double result))
            {
                double Tbx_Input_md_Num = double.Parse(Tbx_Input_md.Text.Trim());
                // 1m3 = 1000L, 1d = 86400s
                double Tbx_Input_Ls_Num = Tbx_Input_md_Num * 1000 / 86400;
                Tbx_Input_Ls.Text = Tbx_Input_Ls_Num.ToString();
                Tbx_Chg_Coefficient.Text = Check_QuantityRange(Tbx_Input_Ls_Num).ToString("0.00");
                Tbx_MaxQuantity.Text = (Tbx_Input_Ls_Num * double.Parse(Tbx_Chg_Coefficient.Text)).ToString("0.0000");
            }
            else
            {
                Tbx_Input_Ls.Text = "";
                Tbx_Input_md.Text = "";
            }
        }
    }
}
