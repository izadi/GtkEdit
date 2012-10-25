using System;
using System.IO;

namespace GtkEdit
{
	#region class FileBuffer

	public class FileBuffer: UndoableTextBuffer
	{
		#region Events

		public event EventHandler TitleChanged;

		#endregion Events

		#region Constants

		public const string NewFileTitle = "Untitled";
		private const string ModifiedPrefix = "*";

		#endregion Constants

		#region Constructors

		public FileBuffer(Gtk.TextTagTable table) :
			base(table)
		{
			UpdateTitle();
		}

		#endregion Constructors

		#region Methods

		protected override void OnModifiedChanged()
		{
			base.OnModifiedChanged();
			UpdateTitle();
		}

		protected virtual void OnTitleChanged(EventArgs e)
		{
			if (TitleChanged != null)
				TitleChanged(this, e);
		}

		private void UpdateTitle()
		{
			string newTitle = FileInfo == null ? NewFileTitle : FileInfo.Name;
			if (Modified)
				newTitle = ModifiedPrefix + newTitle;
			if (Title != newTitle)
			{
				Title = newTitle;
				OnTitleChanged(EventArgs.Empty);
			}
		}

		public void Load(string fileName)
		{
			undoManager.Reset();
			undoManager.BeginNotUndoableAction();
			FileInfo = new FileInfo(fileName);
			if (FileInfo.Exists)
				using (var sr = new StreamReader(FileInfo.FullName))
					Text = sr.ReadToEnd();
			else
				Text = String.Empty;
			Modified = false;
			undoManager.EndNotUndoableAction();
		}

		public void Reload()
		{
			if (FileInfo == null)
				throw new InvalidOperationException();
			if (!FileInfo.Exists)
				throw new FileNotFoundException();
			Load(FileInfo.FullName);
		}

		public void Save()
		{
			if (FileInfo == null)
				throw new InvalidOperationException();
			using (var sw = new StreamWriter(FileInfo.FullName))
				sw.Write(Text);
			Modified = false;
		}

		public void SaveAs(string fileName)
		{
			FileInfo = new FileInfo(fileName);
			bool modified = Modified;
			Save();
			if (!modified)
				UpdateTitle();
		}

		#endregion Methods

		#region Properties

		public FileInfo FileInfo { get; private set; }

		public string Title { get; private set; }

		#endregion Properties
	}

	#endregion class FileBuffer
}
