namespace GtkEdit
{
	#region class ClipboardManager

	public class ClipboardManager
	{
		#region Fields

		private readonly Gtk.Clipboard clipboard;

		#endregion Fields

		#region Constructors

		public ClipboardManager(Gtk.Clipboard clipboard)
		{
			this.clipboard = clipboard;
		}

		public ClipboardManager() :
			this(GetDefaultClipboard())
		{
		}

		#endregion Constructors

		#region Static Methods

		public static Gtk.Clipboard GetDefaultClipboard()
		{
			return Gtk.Clipboard.Get(Gdk.Atom.Intern("CLIPBOARD", false));
		}

		#endregion Static Methods

		#region Methods

		public void Cut(Gtk.TextBuffer textBuffer)
		{
			textBuffer.CutClipboard(clipboard, true);
		}

		public void Copy(Gtk.TextBuffer textBuffer)
		{
			textBuffer.CopyClipboard(clipboard);
		}

		public void Paste(Gtk.TextBuffer textBuffer)
		{
			textBuffer.PasteClipboard(clipboard);
		}

		public void Delete(Gtk.TextBuffer textBuffer)
		{
			textBuffer.DeleteSelection(true, true);
		}

		public bool CanCut(Gtk.TextBuffer textBuffer)
		{
			Gtk.TextIter start, end;
			textBuffer.GetSelectionBounds(out start, out end);
			return start.Compare(end) != 0;
		}

		public bool CanCopy(Gtk.TextBuffer textBuffer)
		{
			Gtk.TextIter start, end;
			textBuffer.GetSelectionBounds(out start, out end);
			return start.Compare(end) != 0;
		}

		public bool CanPaste(Gtk.TextBuffer textBuffer)
		{
			return clipboard.WaitIsTextAvailable();
		}

		public bool CanDelete(Gtk.TextBuffer textBuffer)
		{
			Gtk.TextIter start, end;
			textBuffer.GetSelectionBounds(out start, out end);
			return start.Compare(end) != 0;
		}

		#endregion Methods
	}

	#endregion class ClipboardManager
}
