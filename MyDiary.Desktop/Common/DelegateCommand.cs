using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MyDiary.Desktop.Common
{
    public class DelegateCommand : ICommand
    {
        private bool hasParameter = false;
        public DelegateCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public DelegateCommand(Action<object> execute, Func<bool> canExecute = null)
        {
            this.executeWithParameter = execute;
            this.canExecute = canExecute;
            this.hasParameter = true;
        }

        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null)
            {
                return true;
            }

            return this.canExecute();
        }

        public event EventHandler CanExecuteChanged;

        private Func<bool> canExecute;
        private Action execute;
        private Action<object> executeWithParameter;

        public void Execute(object parameter)
        {
            if (this.hasParameter)
            {
                this.executeWithParameter(parameter);
            }
            else
            {
                this.execute();
            }
            
        }
    }
}
