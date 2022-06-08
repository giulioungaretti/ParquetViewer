using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;

using System;

using WinRT; 

namespace LOBapp;

public class BackDrop
{
    readonly Window window;
    WindowsSystemDispatcherQueueHelper m_wsdqHelper;
    IDisposable controller;
    SystemBackdropConfiguration m_configurationSource;

    public BackDrop(Window window)
    {
        this.window = window;
        m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
        m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();
        // Hooking up the policy object
        m_configurationSource = new SystemBackdropConfiguration();

        window.Activated += Window_Activated;
        window.Closed += Window_Closed;
        ((FrameworkElement)window.Content).ActualThemeChanged += Window_ThemeChanged;
        // Initial configuration state.
        m_configurationSource.IsInputActive = true;
        SetConfigurationSourceTheme();
    }


    public void SetBackdrop()
    {
        if (MicaController.IsSupported())
        {
            controller = new MicaController();
            ((MicaController)controller).AddSystemBackdropTarget(window.As<ICompositionSupportsSystemBackdrop>());
            ((MicaController)controller).SetSystemBackdropConfiguration(m_configurationSource);
        }
        else if (DesktopAcrylicController.IsSupported())
        {
            controller = new DesktopAcrylicController();
            ((DesktopAcrylicController)controller).AddSystemBackdropTarget(window.As<ICompositionSupportsSystemBackdrop>());
            ((DesktopAcrylicController)controller).SetSystemBackdropConfiguration(m_configurationSource);
        }
    }

    private void Window_Activated(object sender, WindowActivatedEventArgs args)
    {
        m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        if (controller != null)
        {
            controller.Dispose();
            controller = null;
        }
        window.Activated -= Window_Activated;
        m_configurationSource = null;
    }

    private void Window_ThemeChanged(FrameworkElement sender, object args)
    {
        if (m_configurationSource != null)
        {
            SetConfigurationSourceTheme();
        }
    }

    private void SetConfigurationSourceTheme()
    {
        switch (((FrameworkElement)window.Content).ActualTheme)
        {
            case ElementTheme.Dark: m_configurationSource.Theme = SystemBackdropTheme.Dark; break;
            case ElementTheme.Light: m_configurationSource.Theme = SystemBackdropTheme.Light; break;
            case ElementTheme.Default: m_configurationSource.Theme = SystemBackdropTheme.Default; break;
        }
    }

}




