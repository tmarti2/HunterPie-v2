﻿using System.Windows;
using HunterPie.Core.Domain.Dialog;
using System.ComponentModel;
using HunterPie.UI.Overlay;
using System;
using HunterPie.Core.Logger;
using HunterPie.Internal;
using HunterPie.UI.Overlay.Widgets.Damage.View;
using System.Windows.Input;
using HunterPie.UI.Overlay.Widgets.Monster.Views;
using HunterPie.Core.Client;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModel;
using HunterPie.Internal.Tray;
using System.Diagnostics;

namespace HunterPie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            Log.Info("Initializing HunterPie GUI");
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {

            NativeDialogResult result = DialogManager.Info("Confirmation", "Are you sure you want to exit HunterPie?", NativeDialogButtons.Accept | NativeDialogButtons.Cancel);  
            
            if (result != NativeDialogResult.Accept)
            {
                e.Cancel = true;
                return;
            }

            base.OnClosing(e);
        }

        private void OnInitialized(object sender, EventArgs e)
        {
            InitializerManager.InitializeGUI();
            InitializeDebugWidgets();
            
            Show();
            SetupTrayIcon();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) && e.KeyboardDevice.IsKeyDown(Key.R))
                App.Restart();
        }

        private void InitializeDebugWidgets()
        {
            if (ClientConfig.Config.Debug.MockBossesWidget)
                WidgetManager.Register(
                    new MonstersView()
                    {
                        DataContext = new MockMonstersViewModel()
                    }
                );

            if (ClientConfig.Config.Debug.MockDamageWidget)
                WidgetManager.Register(
                    new MeterView()
                    {
                        DataContext = new MockMeterViewModel()
                    }
                );
        }

        private void SetupTrayIcon()
        {
            TrayService.AddDoubleClickHandler((_, __) =>
            {
                Show();
                WindowState = WindowState.Normal;
                Focus();
            });

            TrayService.AddItem("Show")
                .Click += (_, __) =>
                {
                    Show();
                    WindowState = WindowState.Normal;
                    Focus();
                };

            TrayService.AddItem("Close")
                .Click += (_, __) => {
                    Close();
                };
        }

        private void OnStartGameClick(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = "steam://run/1446780",
                    UseShellExecute = true
                });
            } catch(Exception err) { Log.Error(err); }
            
        }
    }
}
