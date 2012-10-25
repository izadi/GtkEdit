using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GtkEdit
{
	public partial class UndoableTextBuffer
	{
		#region class UndoManager

		protected partial class UndoManager
		{
			#region Events

			public event EventHandler Changed;

			#endregion Events

			#region Fields

			private readonly UndoableTextBuffer textBuffer;
			private int maxUndoLevels;
			private readonly LinkedList<UndoRedoAction> actions;
			private LinkedListNode<UndoRedoAction> currentActionNode;
			private int notUndoableAction;
			private bool unmodifiedNodeSet;
			private LinkedListNode<UndoRedoAction> unmodifiedNode;

			private Stack<List<UndoRedoAction>> pendingActions;

			#endregion Fields

			#region Constructors

			public UndoManager(UndoableTextBuffer textBuffer)
			{
				this.textBuffer = textBuffer;
				textBuffer.UserActionBegun += TextBuffer_UserActionBegun;
				textBuffer.UserActionEnded += TextBuffer_UserActionEnded;
				textBuffer.InsertingText += TextBuffer_InsertingText;
				textBuffer.DeletingRange += TextBuffer_DeletingRange;
				textBuffer.ModifiedChanged += TextBuffer_ModifiedChanged;

				maxUndoLevels = -1;
				actions = new LinkedList<UndoRedoAction>();

				Reset();
			}

			#endregion Constructors

			#region Methods

			protected virtual void OnChanged(EventArgs e)
			{
				if (Changed != null)
					Changed(this, e);
			}

			private void AddAction(UndoRedoAction action)
			{
				if (pendingActions.Count > 0)
				{
					pendingActions.Peek().Add(action);
					return;
				}
				Debug.WriteLine(string.Format("Action: Pos: {0}, Old: {1}, New: {2}", action.Position,
					action.OldText.Length, action.NewText.Length));
				if (currentActionNode == null)
					actions.Clear();
				else
					while (currentActionNode.Next != null)
						actions.Remove(currentActionNode.Next);
				actions.AddLast(action);
				if (maxUndoLevels > 0 && actions.Count > maxUndoLevels)
				{
					if (unmodifiedNodeSet)
						if (unmodifiedNode == actions.First)
							unmodifiedNode = null;
						else if (unmodifiedNode == null)
							unmodifiedNodeSet = false;
					actions.RemoveFirst();
				}
				currentActionNode = actions.Last;
				OnChanged(EventArgs.Empty);
			}

			private void AddAction(int position, string oldText, string newText, int insertMarkPosition,
				int selectionBoundPosition)
			{
				AddAction(new UndoRedoAction(this, position, oldText, newText, insertMarkPosition,
					selectionBoundPosition));
			}

			public void Reset()
			{
				actions.Clear();
				currentActionNode = null;

				notUndoableAction = 0;
				unmodifiedNode = null;
				unmodifiedNodeSet = !textBuffer.Modified;
				pendingActions = new Stack<List<UndoRedoAction>>();
			}

			public void BeginNotUndoableAction()
			{
				notUndoableAction++;
			}

			public void EndNotUndoableAction()
			{
				notUndoableAction = Math.Max(0, notUndoableAction - 1);
			}

			public bool Undo()
			{
				if (pendingActions.Count > 0) return false;
				if (currentActionNode == null) return false;
				currentActionNode.Value.Undo();
				currentActionNode = currentActionNode.Previous;
				if (unmodifiedNodeSet && currentActionNode == unmodifiedNode)
					textBuffer.Modified = false;
				OnChanged(EventArgs.Empty);
				return true;
			}

			public bool Redo()
			{
				if (pendingActions.Count > 0) return false;
				var nextActionNode = currentActionNode == null ? actions.First : currentActionNode.Next;
				if (nextActionNode == null) return false;
				nextActionNode.Value.Redo();
				currentActionNode = nextActionNode;
				if (unmodifiedNodeSet && currentActionNode == unmodifiedNode)
					textBuffer.Modified = false;
				OnChanged(EventArgs.Empty);
				return true;
			}

			#region Event Handlers

			private void TextBuffer_UserActionBegun(object sender, EventArgs e)
			{
				pendingActions.Push(new List<UndoRedoAction>());
			}

			private void TextBuffer_UserActionEnded(object sender, EventArgs e)
			{
				var pending = pendingActions.Pop();
				if (pending.Count == 0) return;
				var sum = pending[0];
				for (int i = 1; i < pending.Count; i++)
					if (!sum.TryMergeWith(pending[i]))
					{
						AddAction(sum);
						sum = pending[i];
					}
				AddAction(sum);
			}

			private void TextBuffer_InsertingText(object o, Gtk.InsertTextArgs args)
			{
				if (notUndoableAction > 0) return;
				AddAction(args.Pos.Offset, String.Empty, args.Text, textBuffer.GetIterAtMark(textBuffer.InsertMark).Offset,
					textBuffer.GetIterAtMark(textBuffer.SelectionBound).Offset);
			}

			private void TextBuffer_DeletingRange(object o, Gtk.DeleteRangeArgs args)
			{
				if (notUndoableAction > 0) return;
				AddAction(args.Start.Offset, textBuffer.GetText(args.Start, args.End, true), String.Empty,
					textBuffer.GetIterAtMark(textBuffer.InsertMark).Offset, textBuffer.GetIterAtMark(textBuffer.SelectionBound).Offset);
			}

			private void TextBuffer_ModifiedChanged(object sender, EventArgs e)
			{
				if (!textBuffer.Modified)
				{
					unmodifiedNode = currentActionNode;
					unmodifiedNodeSet = true;
				}
			}

			#endregion Event Handlers

			#endregion Methods

			#region Properties

			public UndoableTextBuffer TextBuffer
			{
				get { return textBuffer; }
			}

			public int MaxUndoLevels
			{
				get { return maxUndoLevels; }
				set { maxUndoLevels = value; }
			}

			public bool CanUndo
			{
				get { return currentActionNode != null; }
			}

			public bool CanRedo
			{
				get { return currentActionNode != actions.Last; }
			}

			#endregion Properties
		}

		#endregion class UndoManager
	}
}
