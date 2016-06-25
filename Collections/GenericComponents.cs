using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mz.betainteractive.utilities.module.Collections {
    public class ObjectItem {
        public Object Value;
        private String name;

        public ObjectItem(Object o, String name) {
            this.Value = o;
            this.name = name;
        }

        public override string ToString() {
            return this.name;
        }
    }

    public class TreeNodeItem<T> : TreeNode {
        public T Value;

        public TreeNodeItem(T value) {
            this.Value = value;
            this.Text = value.ToString();
        }

        public TreeNodeItem(T value, int imageIndex, int selectedImageIndex) {
            this.Value = value;
            this.Text = value.ToString();
            this.ImageIndex = imageIndex;
            this.SelectedImageIndex = selectedImageIndex;
        }

        public override string ToString() {
            return this.Value.ToString();
        }
    }

    public class ListViewObjectItem<T> : ListViewItem {
        public T Value;

        public ListViewObjectItem(T value) {
            this.Value = value;
            this.Text = value.ToString();
        }

        public ListViewObjectItem(T value, int imageIndex) {
            this.Value = value;
            this.Text = value.ToString();
            this.ImageIndex = imageIndex;
        }

        /*
        public void UpdateColumns() {
            this.SubItems.Clear();
            this.Text = Value.GetDoor().ToString();
            this.SubItems.Add(Value.Name);
            this.SubItems.Add(Value.Connected ? "ON" : "OFF");
        }
        */

        public override string ToString() {
            return this.Value.ToString();
        }
    }

    public class ContextMenuStripGeneric<T> : ContextMenuStrip {
        public T Value;

        public ContextMenuStripGeneric() {

        }
        public ContextMenuStripGeneric(T value) {
            this.Value = value;
        }
    }

    public class ToolStripMenuItemGeneric<T> : ToolStripMenuItem {
        public T Value;
        public ToolStripMenuItemGeneric(T value) {
            this.Value = value;
            this.Text = value.ToString();
        }
    }

    public class ListViewGenericItem<T> : ListViewItem {
        public T Value;
        public bool ReadOnly;

        public delegate void SetSubItemsHandler();

        public ListViewGenericItem(T value) {
            this.Value = value;
        }

        public ListViewGenericItem(T value, int imageIndex)
            : this(value) {
            this.Text = value.ToString();
            this.ImageIndex = imageIndex;
        }

        public virtual event SetSubItemsHandler SetItems;
    }

    public class DataGridViewGenericRow<T> : DataGridViewRow {
        public T Value;
        public DataGridViewGenericRow(T value) {
            this.Value = value;
        }
    }

    public class DataGridViewCheckBoxColumnGeneric<T> : DataGridViewCheckBoxColumn {
        public T Value;

        public DataGridViewCheckBoxColumnGeneric(T value) {
            this.Value = value;
        }

    }

    public class DataGridViewTextBoxCellGeneric<T> : DataGridViewTextBoxCell {
        public T GenericValue;

        public DataGridViewTextBoxCellGeneric() { }

        public DataGridViewTextBoxCellGeneric(T value) {
            this.GenericValue = value;
        }

        public void SetTextUsingValue() {
            this.Value = GenericValue != null ? GenericValue.ToString() : "";
        }
        /*
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts) {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);


            int x = cellBounds.X;
            int y = cellBounds.Y;
            int w = cellBounds.Width;
            int h = cellBounds.Height;

            Rectangle rect = new Rectangle(x+w-20, y, 20, 20);


            graphics.DrawRectangle(Pens.Black, rect);
        }
        */
    }

    public class DataGridViewTextBoxColumnGeneric<T> : DataGridViewTextBoxColumn {
        public T Value;



        public DataGridViewTextBoxColumnGeneric(T value) {
            this.Value = value;
        }

    }

    public class DataGridViewComboBoxCellGeneric<T> : DataGridViewComboBoxCell {
        public T Value;

        public DataGridViewComboBoxCellGeneric() {
        }

        public DataGridViewComboBoxCellGeneric(T value) {
            this.Value = value;
        }

    }

    public class DataGridViewCheckBoxCellGeneric<T> : DataGridViewCheckBoxCell {
        public T GenericValue;
        public object DefaultValue { get; set; }

        public DataGridViewCheckBoxCellGeneric() {
        }

        public DataGridViewCheckBoxCellGeneric(object defaultValue) {
            this.DefaultValue = defaultValue;
        }

        public DataGridViewCheckBoxCellGeneric(T value, object defaultValue) {
            this.GenericValue = value;
            this.DefaultValue = defaultValue;
        }

    }

    public class EqualityComparerObjectAtt<T> : IEqualityComparer<T> {

        #region IEqualityComparer<T> Members

        public bool Equals(T x, T y) {
            return x.Equals(y);
        }

        public int GetHashCode(T obj) {
            return 0;// obj.GetHashCode();
        }

        #endregion

        #region IEqualityComparer<T> Members

        bool IEqualityComparer<T>.Equals(T x, T y) {
            return x.Equals(y);
        }

        int IEqualityComparer<T>.GetHashCode(T obj) {
            return 0;// obj.GetHashCode();
        }

        #endregion
    }
}
