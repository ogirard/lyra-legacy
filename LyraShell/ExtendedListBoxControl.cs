using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
	/// <summary>
	/// ExtendedListBox Control class
	/// </summary>
	public class ExtendedListBoxControl : ListBox
	{
		#region Variables, properties and such
		
		/// <summary>
		/// Previous index - Used when we are updating thelist items.
		/// </summary>
		private int _previousIndex = -1;

		/// <summary>
		/// Currently selected item
		/// </summary>
		private int _currentIndex = -1;

		/// <summary>
		/// Is the variable for determining if collapsed or not
		/// </summary>
        private bool isCollapsed = true;
	
		/// <summary>
		///Holds custom WWExtendedListItem objects
		/// </summary>
		private ArrayList itemCache = new ArrayList();

        public delegate void ListItemClickEventHandler(object sender, XLBIEventArgs e);
        public event ListItemClickEventHandler ListItemClick;
	
		#endregion

		public ExtendedListBoxControl()
		{
			// This call is required by the Windows.Forms Form Designer.
		    this.InitializeComponent();
		}

		#region Initialization code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // ExtendedListBoxControl
            // 
            this.DrawMode = DrawMode.OwnerDrawVariable;
            this.ResumeLayout(false);

		}

		#endregion

		#region ArrayList methods

		/// <summary>
		/// Adds an XLBC item to the item cache
		/// </summary>
		/// <param name="xlbi">ExtendedListBoxItem to be added</param>
		public void AddItem(ExtendedListBoxItem xlbi)
		{
		    this.itemCache.Add(xlbi);
			//NOTE Adding a dummy as a placeholder here for the object I'm
			//	going to draw!
			this.Items.Add(" ");
		}

        public void RemoveItem()
        {
            if ((this._currentIndex < 0) || (this._currentIndex >= this.itemCache.Count))
                return;

            this.itemCache.RemoveAt(this._currentIndex);
            //NOTE We have to remove item at correct index!
            this.Items.RemoveAt(this._currentIndex);

            //Now set the List as not having a selected item.
            this._currentIndex = -1;
        }

		#endregion

		#region Owner Drawn overrides
    
		/// <summary>
		/// Called when the item needs to be drawn
		/// </summary>
		/// <param name="e">DrawItemEventArgs</param>
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem(e);

            //If not a valid index just ignore
			if ((e.Index < 0) || (e.Index >= this.itemCache.Count))
				return;

			var xlbi = (ExtendedListBoxItem) this.itemCache[e.Index];

            //Smooth drawing shapes!  Without this shapes are drawn with ragged edges,
            //  especially arcs.
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			//If its the current selection and not collapsed
			//	draw the item as required;
			if ((e.Index == this._currentIndex) && !this.isCollapsed)
				xlbi.DrawExpanded(e);
			else
				xlbi.DrawCollapsed(e);
		}

		/// <summary>
		/// Called when invalidated to find the item bounds.  Since we are only concerned
		///		with the height we only need to set this value and leave the bounds.width
		///		as is!
		/// </summary>
		/// <param name="e">MeasureItemEventArgs</param>
		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			base.OnMeasureItem(e);
	
			if ((e.Index < 0) || (e.Index >= this.itemCache.Count))
				return;

			var xlbi = (ExtendedListBoxItem) this.itemCache[e.Index];
			//If its the current selection and not collapsed
			//	set height appropriately
			if ((e.Index == this._currentIndex) && !this.isCollapsed)
				e.ItemHeight = xlbi.MaxSize;
			else
				e.ItemHeight = xlbi.MinSize;

			e.ItemWidth = this.Width;
		}

		/// <summary>
		/// We use MouseDown instead of SelectedIndexChanged because the SelectedIndexChanged
		///		handler is not called unless we choose an item other than the current one and
		///		since we want to toggle the state we use this handler instead!
		/// </summary>
		/// <remarks>
		/// A special note here......
		/// To have the ListBox update the previous and current items in the list we must remove
		///		and then re-add the items.  If this is not done the MeasureItem handler does not
		///		get called and the ListBox does not get updated.
		/// </remarks>
		/// <param name="e">MouseEventArgs</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

            var hit = false;

		    this._previousIndex = this._currentIndex;
		    this._currentIndex = this.SelectedIndex;

            if ((this._currentIndex >= 0) && (this._currentIndex < this.itemCache.Count))
                hit = ((ExtendedListBoxItem) this.itemCache[this._currentIndex]).HitCheck(new Point(e.X, e.Y));

            //If current index is selected toggle state
			//	else just expand
			if (this._previousIndex == this._currentIndex)
			    this.isCollapsed = !this.isCollapsed;
			else
			{
				if ((this._currentIndex >= 0) && (this._currentIndex < this.itemCache.Count))
				    this.isCollapsed = false;
				else
				    this.isCollapsed = true;
			}

			//Update previous selection
		    this.InvalidateItem(this._previousIndex);
					
			//Update current selection
		    this.InvalidateItem(this._currentIndex);

            if ((this.ListItemClick != null) && (this._currentIndex >= 0)) this.ListItemClick(this, new XLBIEventArgs((ExtendedListBoxItem) this.itemCache[this._currentIndex], hit));
		}

        /// <summary>
        /// Invalidates the item at index
        /// </summary>
        /// <param name="index"></param>
        public void InvalidateItem(int index)
        {
            if ((index < 0) || (index >= this.itemCache.Count))
                return;

            //All we need to do here is make sure we get the correct item index
            //  since it is just a place holder!
            this.Items.RemoveAt(index);
            this.Items.Insert(index, " ");
        }

		#endregion
	}

    /// <summary>
    /// Event Argumemnts for the this class. Maintains a field for selected item.
    /// </summary>
    public class XLBIEventArgs : EventArgs
    {
        public ExtendedListBoxItem entry = null;
        public bool ctrlHit = false;

        public XLBIEventArgs() { }

        public XLBIEventArgs(ExtendedListBoxItem obj, bool hit)
        {
            this.entry = obj;
            this.ctrlHit = hit;
        }
    }
}
