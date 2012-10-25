using System;
using System.IO;
using System.ComponentModel;

namespace GtkEdit
{
	#region class FilesNotebook

	public class FilesNotebook: Gtk.Notebook
	{
		#region Classes

		#region class FileBufferEventArgs

		public class FileBufferEventArgs: EventArgs
		{
			#region Fields

			private readonly FileBuffer fileBuffer;

			#endregion Fields

			#region Constructors

			public FileBufferEventArgs(FileBuffer fileBuffer)
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

		#endregion class FileBufferEventArgs

		#region class FileBufferCancelEventArgs

		public class FileBufferCancelEventArgs: CancelEventArgs
		{
			#region Fields

			private readonly FileBuffer fileBuffer;

			#endregion Fields

			#region Constructors

			public FileBufferCancelEventArgs(FileBuffer fileBuffer) :
				base(false)
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

		#endregion class FileBufferCancelEventArgs

		#endregion Classes

		#region Events

		public event EventHandler CurrentFileBufferChanged;
		public event EventHandler<FileBufferEventArgs> FileBufferAdded;
		public event EventHandler<FileBufferEventArgs> FileBufferChanged;
		public event EventHandler<FileBufferEventArgs> FileBufferModifiedChanged;
		public event EventHandler<FileBufferEventArgs> FileBufferTitleChanged;
		public event EventHandler<FileBufferEventArgs> FileBufferSelectionChanged;
		public event EventHandler<FileBufferCancelEventArgs> FileBufferClosing;
		public event EventHandler<FileBufferEventArgs> FileBufferClosed;

		#endregion Events

		#region Constructors

		public FilesNotebook()
		{
			EnablePopup = true;
			Scrollable = true;
		}

		#endregion Constructors

		#region Methods

		protected override void OnSwitchPage(Gtk.NotebookPage page, uint pageNum)
		{
			base.OnSwitchPage(page, pageNum);
			OnCurrentFileBufferChanged(EventArgs.Empty);
		}

		protected virtual void OnCurrentFileBufferChanged(EventArgs e)
		{
			if (CurrentFileBufferChanged != null)
				CurrentFileBufferChanged(this, e);
		}

		protected virtual void OnFileBufferAdded(FileBufferEventArgs e)
		{
			if (FileBufferAdded != null)
				FileBufferAdded(this, e);
		}

		protected virtual void OnFileBufferChanged(FileBufferEventArgs e)
		{
			if (FileBufferChanged != null)
				FileBufferChanged(this, e);
		}

		protected virtual void OnFileBufferModifiedChanged(FileBufferEventArgs e)
		{
			if (FileBufferModifiedChanged != null)
				FileBufferModifiedChanged(this, e);
		}

		protected virtual void OnFileBufferTitleChanged(FileBufferEventArgs e)
		{
			if (FileBufferTitleChanged != null)
				FileBufferTitleChanged(this, e);
		}

		protected virtual void OnFileBufferSelectionChanged(FileBufferEventArgs e)
		{
			if (FileBufferSelectionChanged != null)
				FileBufferSelectionChanged(this, e);
		}

		protected virtual void OnFileBufferClosing(FileBufferCancelEventArgs e)
		{
			if (FileBufferClosing != null)
				FileBufferClosing(this, e);
		}

		protected virtual void OnFileBufferClosed(FileBufferEventArgs e)
		{
			if (FileBufferClosed != null)
				FileBufferClosed(this, e);
		}

		public FileBuffer GetNthFileBuffer(int n)
		{
			return ((Gtk.TextView) ((Gtk.ScrolledWindow) GetNthPage(n)).Child).Buffer as FileBuffer;
		}

		private void AddFileBuffer(FileBuffer fileBuffer)
		{
			fileBuffer.Changed += FileBuffer_Changed;
			fileBuffer.ModifiedChanged += FileBuffer_ModifiedChanged;
			fileBuffer.TitleChanged += FileBuffer_TitleChanged;
			fileBuffer.SelectionChanged += OpenFile_SelectionChanged;

			var textView = new Gtk.TextView(fileBuffer) { Visible = true, WrapMode = Gtk.WrapMode.WordChar };

			var scrolledWindow = new Gtk.ScrolledWindow { Visible = true };
			scrolledWindow.Add(textView);

			var fileCloseableIconTitle = new FileCloseableIconTitle(fileBuffer) { Visible = true };
			fileCloseableIconTitle.CloseClicked += FileCloseableIconTitle_CloseClicked;

			var fileIconTitleMenuLabel = new FileIconTitle(fileBuffer) { Visible = true };

			AppendPageMenu(scrolledWindow, fileCloseableIconTitle, fileIconTitleMenuLabel);

			OnFileBufferAdded(new FileBufferEventArgs(fileBuffer));
		}

		private void RemoveFileBuffer(int index)
		{
			RemovePage(index);
		}

		private void RemoveFileBuffer(FileBuffer fileBuffer)
		{
			for (int i = 0; i < NPages; i++)
				if (GetNthFileBuffer(i) == fileBuffer)
				{
					RemoveFileBuffer(i);
					break;
				}
		}

		public FileBuffer New(bool focus)
		{
			var result = new FileBuffer(new Gtk.TextTagTable());
			AddFileBuffer(result);
			if (focus)
				Page = NPages - 1;
			return result;
		}

		public FileBuffer New()
		{
			return New(true);
		}

		public FileBuffer Open(string fileName, bool focus)
		{
			var fileInfo = new FileInfo(fileName);
			for (int i = 0; i < NPages; i++)
			{
				var fileBuffer = GetNthFileBuffer(i);
				if (fileBuffer.FileInfo != null && fileBuffer.FileInfo.FullName == fileInfo.FullName)
				{
					if (focus)
						Page = i;
					return fileBuffer;
				}
			}
			var result = New(focus);
			result.Load(fileName);
			return result;
		}

		public FileBuffer Open(string fileName)
		{
			return Open(fileName, true);
		}

		public bool Close(FileBuffer fileBuffer)
		{
			var e = new FileBufferCancelEventArgs(fileBuffer);
			OnFileBufferClosing(e);
			if (e.Cancel)
				return false;
			RemoveFileBuffer(fileBuffer);
			OnFileBufferClosed(new FileBufferEventArgs(fileBuffer));
			if (CurrentFileBuffer == null)
				OnCurrentFileBufferChanged(EventArgs.Empty);
			return true;
		}

		public bool CloseCurrent()
		{
			return CurrentFileBuffer == null || Close(CurrentFileBuffer);
		}

		public bool CloseAll()
		{
			while (NPages > 0)
				if (!Close(GetNthFileBuffer(0)))
					return false;
			return true;
		}

		public void Focus(FileBuffer fileBuffer)
		{
			for (int i = 0; i < NPages; i++)
				if (GetNthFileBuffer(i) == fileBuffer)
				{
					Page = i;
					break;
				}
		}

		#region Event Handlers

		private void FileBuffer_Changed(object sender, EventArgs e)
		{
			OnFileBufferChanged(new FileBufferEventArgs(sender as FileBuffer));
		}

		private void FileBuffer_ModifiedChanged(object sender, EventArgs e)
		{
			OnFileBufferModifiedChanged(new FileBufferEventArgs(sender as FileBuffer));
		}

		private void FileBuffer_TitleChanged(object sender, EventArgs e)
		{
			OnFileBufferTitleChanged(new FileBufferEventArgs(sender as FileBuffer));
		}

		private void OpenFile_SelectionChanged(object sender, EventArgs e)
		{
			OnFileBufferSelectionChanged(new FileBufferEventArgs(sender as FileBuffer));
		}

		private void FileCloseableIconTitle_CloseClicked(object sender, EventArgs e)
		{
			Close(((FileCloseableIconTitle) sender).FileBuffer);
		}

		#endregion Event Handlers

		#endregion Methods

		#region Properties

		public FileBuffer CurrentFileBuffer
		{
			get { return Page < 0 ? null : GetNthFileBuffer(Page); }
		}

		#endregion Properties
	}

	#endregion class FilesNotebook
}
