﻿using System.Threading;

namespace BulkSMSSender2._0
{
    public partial class MainPage : ContentPage
    {
        public static MainPage? ins {  get; private set; }

        public readonly PhoneConnection phoneConnection;

        public MainPage()
        {
            InitializeComponent();

            ins ??= this;

            phoneConnection = new(connectedPhonesLabel);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await phoneConnection.StartAsync();
        } 
        
        private void SendSingleSMS(object sender, EventArgs e)
        {
            SMSSending sending = new();
            sending.SendSingleAsync(numberEntry.Text, "Test");
        }
    }
}
