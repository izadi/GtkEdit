namespace GtkEdit
{
	#region class FileIconTitle

	public class FileIconTitle: Gtk.HBox
	{
		#region Fields

		private readonly FileBuffer fileBuffer;
		private readonly ImageFileIcon icon;
		private readonly LabelFileTitle title;

		#endregion Fields

		#region Constructors

		public FileIconTitle(FileBuffer fileBuffer): base(false, 0)
		{
			this.fileBuffer = fileBuffer;

			icon = new ImageFileIcon(fileBuffer) { Visible = true };

			title = new LabelFileTitle(fileBuffer) { Visible = true };

			PackStart(icon, false, true, 0);
			PackStart(title, false, true, 0);
		}

		#endregion Constructors

		#region Properties

		public FileBuffer FileBuffer
		{
			get { return fileBuffer; }
		}

		#endregion Properties
	}

	#endregion class FileIconTitle
}
