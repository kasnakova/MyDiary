using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace MyDiary.Mobile.Common
{
    public class ReminderManager
    {
        public void AddReminder(string text, DateTime date)
        {
            try
            {
                ToastTemplateType toastType = ToastTemplateType.ToastText02;
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastType);

                XmlNodeList toastTextElement = toastXml.GetElementsByTagName("text");
                toastTextElement[0].AppendChild(toastXml.CreateTextNode(StringResources.Reminder));
                toastTextElement[1].AppendChild(toastXml.CreateTextNode(text));
                string id = text.GetReminderHashCode(date);

                IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
                ((XmlElement)toastNode).SetAttribute("duration", "long");
                ScheduledToastNotification scheduledToast = new ScheduledToastNotification(toastXml, date);
                scheduledToast.Id = id;
                ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledToast);
                LocalSettingsManager.Instance.AddReminder(id, text, date);
            }
            catch(Exception ex)
            {

            }
        }

        public void RemoveReminder(string id)
        {
            IReadOnlyList<ScheduledToastNotification> scheduled =
                ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications();

            foreach (ScheduledToastNotification notify in scheduled)
            {
                if (notify.Id == id)
                {
                    ToastNotificationManager.CreateToastNotifier().RemoveFromSchedule(notify);
                }
            }

            LocalSettingsManager.Instance.RemoveReminder(id);
        }

        public void RemoveAllReminders()
        {
            IReadOnlyList<ScheduledToastNotification> scheduled =
                ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications();

            foreach (ScheduledToastNotification notify in scheduled)
            {
                ToastNotificationManager.CreateToastNotifier().RemoveFromSchedule(notify);
            }

            LocalSettingsManager.Instance.RemoveAllReminders();
        }
    }
}
