using System.Collections.Generic;
using System.Collections.ObjectModel;
using MyDiary.Desktop.ViewModels;
using System;
namespace MyDiary.Desktop.Common
{
    public static class Extensions
    {
        public static void AddRange<T>(this ObservableCollection<NoteViewModel> collection,
           IEnumerable<NoteViewModel> items)
        {
            foreach (var item in items)
            {
                if(item.HasPassword)
                {
                    item.NoteText = StringResources.PasswordNeeded;
                }

                collection.Add(item);
            }
        }

        public static void AddRange<T>(this ObservableCollection<T> collection,
           IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        public static string GetReminderHashCode(this string str, DateTime date)
        {
            return Math.Abs((str + date).GetHashCode()) + "";
        }
    }
}
