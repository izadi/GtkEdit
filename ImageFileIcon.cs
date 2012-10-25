namespace GtkEdit
{
	#region class ImageFileIcon

	public class ImageFileIcon: Gtk.Image
	{
		#region Fields

		private readonly FileBuffer fileBuffer;

		#endregion Fields

		#region Constructors

		public ImageFileIcon(FileBuffer fileBuffer) :
			base(Gtk.Stock.File, Gtk.IconSize.Menu)
		{
			this.fileBuffer = fileBuffer;
		}

		#endregion Constructors

		#region Properties

		public FileBuffer FileBuffer
		{
			get { return fileBuffer; }
		}

		#endregion Properties
	}

	#endregion class ImageFileIcon
}
