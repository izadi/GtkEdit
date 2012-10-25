namespace GtkEdit
{
	public partial class UndoableTextBuffer
	{
		protected partial class UndoManager
		{
			#region class UndoRedoAction

			private sealed class UndoRedoAction
			{
				#region Fields

				private readonly UndoManager undoManager;
				private readonly int insertMarkPosition;
				private readonly int selectionBoundPosition;

				#endregion Fields

				#region Constructors

				public UndoRedoAction(UndoManager undoManager, int position, string oldText, string newText,
					int insertMarkPosition, int selectionBoundPosition)
				{
					this.undoManager = undoManager;
					Position = position;
					OldText = oldText;
					NewText = newText;
					this.insertMarkPosition = insertMarkPosition;
					this.selectionBoundPosition = selectionBoundPosition;
				}

				#endregion Constructors

				#region Methods

				public bool TryMergeWith(UndoRedoAction nextAction)
				{
					if (Position == nextAction.Position)
					{
						if (NewText.Length >= nextAction.OldText.Length)
						{
							if (!NewText.StartsWith(nextAction.OldText)) return false;
							NewText = nextAction.NewText + NewText.Substring(nextAction.OldText.Length);
						}
						else
						{
							if (!nextAction.OldText.StartsWith(NewText)) return false;
							OldText = OldText + nextAction.OldText.Substring(NewText.Length);
							NewText = nextAction.NewText;
						}
						return true;
					}
					int nextEnd = nextAction.Position + nextAction.OldText.Length;
					if (Position == nextEnd)
					{
						Position = nextAction.Position;
						OldText = nextAction.OldText + OldText;
						NewText = nextAction.NewText + NewText;
						return true;
					}
					int end = Position + NewText.Length;
					if (end == nextAction.Position)
					{
						OldText += nextAction.OldText;
						NewText += nextAction.NewText;
						return true;
					}
					if (end == nextEnd)
					{
						if (NewText.Length >= nextAction.OldText.Length)
						{
							if (!NewText.EndsWith(nextAction.OldText)) return false;
							NewText = NewText.Substring(0, NewText.Length - nextAction.OldText.Length) + nextAction.NewText;
						}
						else
						{
							if (!nextAction.OldText.EndsWith(NewText)) return false;
							Position = nextAction.Position;
							OldText = nextAction.OldText.Substring(0, nextAction.OldText.Length - NewText.Length) + OldText;
							NewText = nextAction.NewText;
						}
						return true;
					}
					return false;
				}

				private void Replace(int position, int length, string newText)
				{
					undoManager.BeginNotUndoableAction();
					undoManager.textBuffer.BeginUserAction();
					var start = undoManager.textBuffer.GetIterAtOffset(position);
					var end = undoManager.textBuffer.GetIterAtOffset(position + length);
					undoManager.textBuffer.Delete(ref start, ref end);
					start = undoManager.textBuffer.GetIterAtOffset(position);
					undoManager.textBuffer.Insert(ref start, newText);
					undoManager.textBuffer.EndUserAction();
					undoManager.EndNotUndoableAction();
				}

				private void SetInsertMarkAndSelectionBound()
				{
					undoManager.textBuffer.MoveMark(undoManager.textBuffer.InsertMark,
						undoManager.textBuffer.GetIterAtOffset(insertMarkPosition));
					undoManager.textBuffer.MoveMark(undoManager.textBuffer.SelectionBound,
						undoManager.textBuffer.GetIterAtOffset(selectionBoundPosition));
				}

				public void Undo()
				{
					Replace(Position, NewText.Length, OldText);
					SetInsertMarkAndSelectionBound();
				}

				public void Redo()
				{
					Replace(Position, OldText.Length, NewText);
				}

				#endregion Methods

				#region Properties

				public int Position { get; private set; }

				public string OldText { get; private set; }

				public string NewText { get; private set; }

				#endregion Properties
			}

			#endregion class UndoRedoAction
		}
	}
}
