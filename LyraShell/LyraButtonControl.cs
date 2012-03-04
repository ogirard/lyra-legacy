using System.ComponentModel;
using Infragistics.Win.Misc;

namespace Lyra2.LyraShell
{
	/// <summary>
	/// Summary description for LyraButtonControl.
	/// </summary>
	public class LyraButtonControl : UltraButton
    {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public LyraButtonControl() : base()
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Component Designer generated code
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.SuspendLayout();
            this.ResumeLayout(false);

		}
		
		#endregion
	}
}
