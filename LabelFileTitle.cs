using System;

namespace GtkEdit
{
	#region class LabelFileTitle

	public class LabelFileTitle: Gtk.Label
	{
		#region Fields

		private readonly FileBuffer fileBuffer;

		#endregion Fields

		#region Constructors

		public LabelFileTitle(FileBuffer fileBuffer) :
			base(fileBuffer.Title)
		{
			this.fileBuffer = fileBuffer;
			fileBuffer.TitleChanged += FileBuffer_TitleChanged;

			UpdateText();
		}

		#endregion Constructors

		#region Methods

		private void UpdateText()
		{
			Text = fileBuffer.Title;
		}

		#region Event Handlers

		private void FileBuffer_TitleChanged(object sender, EventArgs e)
		{
			UpdateText();
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

	#endregion class LabelFileTitle
}
