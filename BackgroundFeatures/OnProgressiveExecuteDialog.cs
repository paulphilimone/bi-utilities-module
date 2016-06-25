using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace mz.betainteractive.utilities.BackgroundFeatures {
    public class OnProgressiveExecuteDialog {

        public delegate void OnProgressiveExecuteEventHandler();
        public delegate void OnProgressiveExecutingEventHandler(OnExecutingEventArgs eventArgs);
        public delegate void OnProgressivePostExecuteEventHandler(OnPostExecuteEventArgs eventArgs);

        private ProgressWindow progressWindow;
                
        public event OnProgressiveExecutingEventHandler OnExecuting;
        public event OnProgressivePostExecuteEventHandler OnPostExecute;

        private int MaxPercentage;

        public OnProgressiveExecuteDialog(string title, int maxPercentage) {
            this.progressWindow = new ProgressWindow();
            this.progressWindow.Text = title;
            this.MaxPercentage = maxPercentage;
        }

        public void StartExecute() {
            // Run Application with ProgressBar
            try {

                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(fireOnExecuting), progressWindow);
                this.progressWindow.ShowDialog();

            } catch (Exception ex) {
                fireOnPostExecute(ex);
            } finally {
                fireOnPostExecute();
            }
        }

        private void fireOnExecuting(object status) {
            IProgressCallback callback = status as IProgressCallback;

            // Init Progressbar                
            callback.Begin(0, MaxPercentage);

            if (OnExecuting != null) {
                OnExecutingEventArgs eventArg = new OnExecutingEventArgs(callback);
                OnExecuting(eventArg);
            }

            // End Progressbar
            callback.End();
        }

        private void fireOnPostExecute() {
            if (OnPostExecute != null) {
                OnPostExecuteEventArgs e = new OnPostExecuteEventArgs(null, false);
                OnPostExecute(e);
            }
        }

        private void fireOnPostExecute(Exception ex) {
            if (OnPostExecute != null) {
                OnPostExecuteEventArgs e = new OnPostExecuteEventArgs(ex, true);
                OnPostExecute(e);
            }
        }

        public class OnPostExecuteEventArgs : EventArgs {
            public Exception OcurredException;
            public bool FoundErrors;

            public OnPostExecuteEventArgs(Exception ex, bool errors) {
                this.OcurredException = ex;
                this.FoundErrors = errors;
            }
        }

        public class OnExecutingEventArgs : EventArgs {
            private IProgressCallback callback;
            public IProgressCallback ProgressCallback { get { return callback; } }

            public OnExecutingEventArgs(IProgressCallback callback) {
                this.callback = callback;
            }

        }

    }
}
