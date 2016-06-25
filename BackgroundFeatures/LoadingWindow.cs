using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mz.betainteractive.utilities.module.BackgroundFeatures {
    public partial class LoadingWindow : Form {
        public LoadingWindow() {
            InitializeComponent();
        }

        public void SetTitle(string title) {
            this.Text = title;
        }

        public void SetLoadingText(string text) {
            this.LabelTextProgress.Text = text;
        }

        public void UpdatePicture() {
            Console.WriteLine("update pic");
            this.pictureBox1.Update();
        }
    }
}
