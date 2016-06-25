using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace mz.betainteractive.utilities.module.BackgroundFeatures {
    public class OnExecuteDialog {
        public delegate void OnExecuteEventHandler();
        public delegate void OnPostExecuteEventHandler();
        
        private LoadingWindow loadingWindow;
        private BackgroundWorker background;
        public event OnExecuteEventHandler OnExecute;
        public event OnPostExecuteEventHandler OnPostExecute;

        public OnExecuteDialog(string title, string message) {

            background = new BackgroundWorker();
            loadingWindow = new LoadingWindow();

            background.DoWork += delegate(object sender, DoWorkEventArgs e) {
                fireOnExecute();
            };

            background.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e) {
                loadingWindow.Dispose();
                fireOnPostExecute();
            };


            loadingWindow.SetTitle(title);
            loadingWindow.LabelTextProgress.Text = message;            
        }

        public void StartExecute() {
            background.RunWorkerAsync();
            loadingWindow.ShowDialog();
        }

        private void fireOnPostExecute() {
            if (OnPostExecute != null) {
                OnPostExecute();
            }
        }

        private void fireOnExecute() {
            if (OnExecute != null) {
                OnExecute();
            }
        }

        public class OnPostExecuteEventArgs : EventArgs {            
            private Exception OcurredException;
            private bool FoundErrors;

            public OnPostExecuteEventArgs() {
                
            }

            
        }
                
    }
}
