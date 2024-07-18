using Avalonia.Controls;
using Avalonia.Interactivity;
using DialogHostAvalonia;
using MsBox.Avalonia;
using System.Net;
using System.Threading.Tasks;
using MsBox.Avalonia.Enums;
using System.Diagnostics;
using System;
using System.Threading;

namespace AvaloniaUos;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public static bool IsValidIPAddress(string ipAddress)
    {
        return IPAddress.TryParse(ipAddress, out _);
    }

    public static void ExecuteCommandAsync(string command)
    {
        var process = new Process();

        process.StartInfo.FileName = "bash";

        process.StartInfo.UseShellExecute = false;

        process.StartInfo.RedirectStandardInput = true;

        process.StartInfo.RedirectStandardOutput = false;
        process.StartInfo.RedirectStandardError = false;

        process.StartInfo.CreateNoWindow = true;

        process.Start();
        process.StandardInput.WriteLine(command);

        Thread.Sleep(TimeSpan.FromSeconds(5));

        process.Kill();
        process.StandardInput.WriteLine(command);

        process.WaitForExitAsync().Wait();
    }

    private async void ConnectButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!IsValidIPAddress(AddressText.Text))
        {
            var box = MessageBoxManager.GetMessageBoxStandard("输入格式错误", "主机地址格式错误");
            await box.ShowAsPopupAsync(this);
            return;
        }

        var userHomeDir = Environment.GetEnvironmentVariable("HOME") ??
                          Environment.GetEnvironmentVariable("USERPROFILE");

        var driveParam = userHomeDir is null || DriveCheck.IsChecked != true ? "/drives" : $"/drive:home,{userHomeDir}";
        var command =
            $"xfreerdp /v:{AddressText.Text} /u:{UsernameText.Text} /p:{PasswordText.Text} /app:\"||RemoteDesktopAppService\" {driveParam}";

        ExecuteCommandAsync(command);
    }
}