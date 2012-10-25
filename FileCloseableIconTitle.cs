using System;

namespace GtkEdit
{
	#region class FileCloseableIconTitle

	public class FileCloseableIconTitle: Gtk.HBox
	{
		#region Events

		public event EventHandler CloseClicked;

		#endregion Events

		#region Fields

		private readonly FileBuffer fileBuffer;
		private readonly FileIconTitle fileIconTitle;
		private readonly Gtk.Image imageClose;
		private readonly Gtk.Button buttonClose;

		#endregion Fields

		#region Constructors

		public FileCloseableIconTitle(FileBuffer fileBuffer)
			: base(false, 0)
		{
			this.fileBuffer = fileBuffer;

			fileIconTitle = new FileIconTitle(fileBuffer) { Visible = true };

			imageClose = new Gtk.Image(Gtk.Stock.Close, Gtk.IconSize.Menu) { Visible = true };

			buttonClose = new Gtk.Button(imageClose) { Visible = true, Relief = Gtk.ReliefStyle.None };
			buttonClose.Clicked += ButtonClose_Clicked;

			PackStart(fileIconTitle, false, true, 0);
			PackStart(buttonClose, false, true, 0);
		}

		#endregion Constructors

		#region Methods

		protected virtual void OnCloseClicked(EventArgs e)
		{
			if (CloseClicked != null)
				CloseClicked(this, e);
		}

		#region Event Handlers

		private void ButtonClose_Clicked(object sender, EventArgs e)
		{
			OnCloseClicked(EventArgs.Empty);
		}

		#endregion Event Handlers

		#endregion Methods

		#region Properties

		public FileBuffer FileBuffer
		{
			get { return fileBuffer; }
		}

		#endregion Properties
	}

	#endregion class FileCloseableIconTitle
}
