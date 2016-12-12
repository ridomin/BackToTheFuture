﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace RidoClassicWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            var a = Assembly.GetExecutingAssembly();
            var v = a.GetName().Version;
            labelPath.Text = a.CodeBase;           
            labelVersion.Text = $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision} running on {a.ImageRuntimeVersion}"; 
            labelAppx.Text = GetPackageNameIfAvailable();
        }

        string GetPackageNameIfAvailable()
        {
            string pfn = string.Empty;
            try
            {
                var id = Windows.ApplicationModel.Package.Current.Id;
                var v = id.Version;
                pfn = $"{id.FamilyName} ";
                pfn += $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
            catch (Exception ex)
            {
                pfn = ex.Message;
            }
            return pfn;
        }

        private void TabItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            string result = string.Empty;
            try
            {
                var o = new ClassicCOM.MyClassClass();
                result = o.GetInfo();
            }
            catch(Exception ex)
            {
                result = ex.Message;
            }
            labelCOMInfo.Text = result;

        }

        private void buttonShowToast_Click(object sender, RoutedEventArgs e)
        {
            var o = new ClassicCOM.MyClassClass();
            ShowToast(o.Salute("UWP"));
        }

        private void ShowToast(string msg)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(msg));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode(DateTime.Now.ToString()));
            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
