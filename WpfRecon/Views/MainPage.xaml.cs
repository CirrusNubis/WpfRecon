﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using WpfRecon.Models;
using WpfRecon.ViewModels;

namespace WpfRecon
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        MainPageVM MPVM = new MainPageVM();
        public MainPage()
        {
            InitializeComponent();
           
        }

      
        //TODO: Create an error to report an incorrect IP Address to the homePage

        public bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Scan_Click(object sender, RoutedEventArgs e)
        {
            pbStatus.IsIndeterminate = true;

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;

            Output.Text = (MPVM.DisplayOutput(IpAddress.Text));
            if (State.SuccessfulPing)
                pbStatus.Visibility = Visibility.Visible;

            MPVM.ScanComplete += ScanCompleteHandler;

//            MPVM.LoadNmapScanInBackgroundAsync(() => {});


            worker.RunWorkerAsync();

           
            
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            MPVM.LoadNmapScanInBackground(() => { });
        }

        private void ScanCompleteHandler(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() => {
                pbStatus.Visibility = Visibility.Hidden;
                Output.Text += "\nNMap Scan Completed, Check Results Pages for details.";
            });
            
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {

            // View The Home page  
            NavigationService.Navigate(new Uri("Views/MainPage.xaml", UriKind.Relative));
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {

            // View The About page 
            NavigationService.Navigate(new Uri("Views/About.xaml", UriKind.Relative));
        }

        private void Results_Click(object sender, RoutedEventArgs e)
        {

            
            // View The Results page 
            NavigationService.Navigate(new Uri("Views/Results.xaml", UriKind.Relative));
            
        }


        //private void Run_PBBar(object sender, EventArgs e)
        //{
        //    BackgroundWorker worker = new BackgroundWorker();
        //    worker.WorkerReportsProgress = true;
        //    worker.DoWork += worker_DoWork;
        //    worker.ProgressChanged += worker_ProgressChanged;

        //    worker.RunWorkerAsync();
        //}




        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
        }
    }

}
