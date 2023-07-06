using Microsoft.Toolkit.Uwp.Notifications;

namespace Nvupd.Core.Notifications;

public static class UpdateAvailable
{
    public static void Show()
    {
        new ToastContentBuilder()
            .AddText("An update for your GPU is available")
            .Show();
    }
}