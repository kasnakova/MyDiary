namespace MyDiary.Mobile.Common
{
    using System;
    using Windows.UI.Popups;

    public class MessageDialogManager
    {
        public static void ShowConfirmationDialog(string title, string content)
        {
            MessageDialog msgDialog = new MessageDialog(content, title);
            msgDialog.Commands.Add(new UICommand(StringResources.Ok));
            msgDialog.ShowAsync();
        }

        public static void ShowDialog(string title, string content, Action<IUICommand> confirmationAction)
        {
            MessageDialog msgDialog = new MessageDialog(content, title);
            msgDialog.Commands.Add(new UICommand(StringResources.Cancel));
            msgDialog.Commands.Add(new UICommand(StringResources.Yes, new UICommandInvokedHandler(confirmationAction)));
            msgDialog.ShowAsync();
        }
    }
}
