using System;

namespace GtkEdit
{
	#region class UndoableTextBuffer

	public partial class UndoableTextBuffer: Gtk.TextBuffer
	{
		#region Events

		public event Gtk.InsertTextHandler InsertingText;
		public event Gtk.DeleteRangeHandler DeletingRange;
		public event EventHandler UndoRedoChanged;
		public event EventHandler SelectionChanged;

		#endregion Events

		#region Fields

		protected UndoManager undoManager;

		#endregion Fields

		#region Constructors

		public UndoableTextBuffer(Gtk.TextTagTable table) :
			base(table)
		{
			undoManager = new UndoManager(this);
			undoManager.Changed += UndoManager_Changed;
		}

		#endregion Constructors

		#region Methods

		protected virtual void OnInsertingText(Gtk.TextIter pos, string text)
		{
			if (InsertingText != null)
			{
				var args = new Gtk.InsertTextArgs { Args = new object[] { pos, text, text.Length, } };
				InsertingText(this, args);
			}
		}

		protected override void OnInsertText(Gtk.TextIter pos, string text)
		{
			OnInsertingText(pos, text);
			base.OnInsertText(pos, text);
		}

		protected virtual void OnDeletingRange(Gtk.TextIter start, Gtk.TextIter end)
		{
			if (DeletingRange != null)
			{
				var args = new Gtk.DeleteRangeArgs { Args = new object[] { start, end, } };
				DeletingRange(this, args);
			}
		}

		protected override void OnDeleteRange(Gtk.TextIter start, Gtk.TextIter end)
		{
			OnDeletingRange(start, end);
			base.OnDeleteRange(start, end);
		}

		protected virtual void OnUndoRedoChanged(EventArgs e)
		{
			if (UndoRedoChanged != null)
				UndoRedoChanged(this, e);
		}

		protected override void OnMarkSet(Gtk.TextIter location, Gtk.TextMark mark)
		{
			base.OnMarkSet(location, mark);
			if (mark == InsertMark || mark == SelectionBound)
				OnSelectionChanged(EventArgs.Empty);
		}

		protected virtual void OnSelectionChanged(EventArgs e)
		{
			if (SelectionChanged != null)
				SelectionChanged(this, e);
		}

		public bool Undo()
		{
			return undoManager.Undo();
		}

		public bool Redo()
		{
			return undoManager.Redo();
		}

		#region Event Handlers

		private void UndoManager_Changed(object sender, EventArgs e)
		{
			OnUndoRedoChanged(EventArgs.Empty);
		}

		#endregion Event Handlers

		#endregion Methods

		#region Properties

		public int MaxUndoLevels
		{
			get { return undoManager.MaxUndoLevels; }
			set { undoManager.MaxUndoLevels = value; }
		}

		public bool CanUndo
		{
			get { return undoManager.CanUndo; }
		}

		public bool CanRedo
		{
			get { return undoManager.CanRedo; }
		}

		#endregion Properties
	}

	#endregion class UndoableTextBuffer
}
