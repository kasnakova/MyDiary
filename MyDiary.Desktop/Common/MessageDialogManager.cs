namespace MyDiary.Desktop.Common
{
    using System;
    using System.Windows;

    public class MessageDialogManager
    {
        public static void ShowConfirmationDialog(string title, string content)
        {
            //MessageBox msgDialog = new MessageBox(content, title);
            //msgDialog.Commands.Add(new UICommand(StringResources.Ok));
            //msgDialog.ShowAsync();

            MessageBoxResult result = MessageBox.Show(content, title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.Yes)
            {
                //Application.Current.Shutdown();
            }
        }

        //public static void ShowDialog(string title, string content, Action<IUICommand> confirmationAction)
        //{
        //    MessageDialog msgDialog = new MessageDialog(content, title);
        //    msgDialog.Commands.Add(new UICommand(StringResources.Cancel));
        //    msgDialog.Commands.Add(new UICommand(StringResources.Yes, new UICommandInvokedHandler(confirmationAction)));
        //    msgDialog.ShowAsync();
        //}
    }
}
