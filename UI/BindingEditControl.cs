using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using DevExpress.CodeRush.PlugInCore;

namespace CR_XkeysEngine
{
	public class BindingEditControl : DXCoreUserControl
	{
		#region private fields ...
		private CR_XkeysEngine.XkeysEditControl edCustom;
		private System.ComponentModel.Container components = null;
		#endregion

		#region BindingEditControl
		public BindingEditControl()
		{
			InitializeComponent();
		}
		#endregion

		#region Dispose
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
			base.Dispose(disposing);
		}
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      this.edCustom = new CR_XkeysEngine.XkeysEditControl();
      this.SuspendLayout();
      // 
      // edCustom
      // 
      this.edCustom.AltKeyDown = false;
      this.edCustom.CtrlKeyDown = false;
      this.edCustom.Dock = System.Windows.Forms.DockStyle.Fill;
      this.edCustom.Location = new System.Drawing.Point(0, 0);
      this.edCustom.Name = "edCustom";
      this.edCustom.ShiftKeyDown = false;
      this.edCustom.Size = new System.Drawing.Size(268, 173);
      this.edCustom.TabIndex = 1;
      this.edCustom.CustomInputChanged += new CR_XkeysEngine.CustomInputChangedEventHandler(this.edCustom_InputChanged);
      // 
      // BindingEditControl
      // 
      this.Controls.Add(this.edCustom);
      this.Name = "BindingEditControl";
      this.Size = new System.Drawing.Size(268, 173);
      this.ResumeLayout(false);

		}
		#endregion
		
		// private methods...
		#region LayoutControls()
		private void LayoutControls()
		{
			Size = new Size(269, 80);
			edCustom.Size = Size;
			
			Point location = new Point(0, 0);
			edCustom.Location = location;

			ShowCustomInputControl();
		}
		#endregion

		// public methods ...
		#region ShowKeysControl
		public void ShowCustomInputControl()
		{
      edCustom.Visible = true;
		}
		#endregion

		
		private void edCustom_InputChanged(object sender, CR_XkeysEngine.CustomInputChangedEventArgs ea)
		{
			CustomInputChanged(sender, ea);
		}

		public event CustomInputChangedEventHandler CustomInputChanged;

		// public properties...
		#region CustomInputControl
		public XkeysEditControl CustomInputControl
		{
			get
			{
				return edCustom;
			}
		}
		#endregion
	}
}
