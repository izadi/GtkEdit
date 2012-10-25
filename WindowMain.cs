using System;
using System.Diagnostics;

namespace GtkEdit
{
	#region class WindowMain

	public class WindowMain: Gtk.Window
    {
        #region Fields

		#region GUI

		// WindowMain
		private Gtk.AccelGroup accelGroupMain;
		private Gtk.ActionGroup actionGroupMain;
        private Gtk.VBox vBoxMain;

        // vBoxMain
        private Gtk.MenuBar menuBarMain;
        private Gtk.Toolbar toolbarMain;
        private FilesNotebook filesNotebookFiles;
        private Gtk.Statusbar statusbarMain;

        // menuBarMain
        private Gtk.MenuItem menuItemFile;
        private Gtk.MenuItem menuItemEdit;
        private Gtk.MenuItem menuItemHelp;

        // menuItemFile
        private Gtk.Menu menuFile;

		// menuFile
		private Gtk.ImageMenuItem imageMenuItemFileNew;
		private Gtk.ImageMenuItem imageMenuItemFileOpen;
		private Gtk.ImageMenuItem imageMenuItemFileSave;
		private Gtk.ImageMenuItem imageMenuItemFileSaveAs;
		private Gtk.ImageMenuItem imageMenuItemFileClose;
		private Gtk.SeparatorMenuItem separatorMenuItemFile1;
		private Gtk.ImageMenuItem imageMenuItemFileQuit;

        // menuItemEdit
        private Gtk.Menu menuEdit;

		// menuEdit
		private Gtk.ImageMenuItem imageMenuItemEditUndo;
		private Gtk.ImageMenuItem imageMenuItemEditRedo;
		private Gtk.SeparatorMenuItem separatorMenuItemEdit1;
		private Gtk.ImageMenuItem imageMenuItemEditCut;
		private Gtk.ImageMenuItem imageMenuItemEditCopy;
		private Gtk.ImageMenuItem imageMenuItemEditPaste;
		private Gtk.ImageMenuItem imageMenuItemEditDelete;

		// menuItemHelp
        private Gtk.Menu menuHelp;

		// menuHelp
		private Gtk.ImageMenuItem imageMenuItemHelpAbout;

		// toolbarMain
        private Gtk.ToolButton toolButtonNew;
        private Gtk.ToolButton toolButtonOpen;
        private Gtk.ToolButton toolButtonSave;
		private Gtk.ToolButton toolButtonClose;
        private Gtk.SeparatorToolItem separatorToolItem1;
        private Gtk.ToolButton toolButtonUndo;
        private Gtk.ToolButton toolButtonRedo;
		private Gtk.SeparatorToolItem separatorToolItem2;
		private Gtk.ToolButton toolButtonCut;
		private Gtk.ToolButton toolButtonCopy;
		private Gtk.ToolButton toolButtonPaste;
		private Gtk.SeparatorToolItem separatorToolItem3;
		private Gtk.ToolButton toolButtonQuit;

		// statusbarMain

		#endregion GUI

		private readonly ClipboardManager clipboardManager;

		#endregion Fields

		#region Constructors

		public WindowMain() :
			base(Gtk.WindowType.Toplevel)
        {
			ConstructWindowMain();

			clipboardManager = new ClipboardManager();

			UpdateTitle();
			UpdateActions();
        }

        #endregion Constructors

        #region Methods

		#region GUI

		private void ConstructAccelGroupMain()
		{
			accelGroupMain = new Gtk.AccelGroup();
			AddAccelGroup(accelGroupMain);
		}

		private void ConstructActionGroupMain()
		{
			actionGroupMain = new Gtk.ActionGroup("Main");
			var actionEntriesMain = new[]
			{
				// File menu
				new Gtk.ActionEntry("File.New", Gtk.Stock.New, ActionMain_File_New_Activated),
				new Gtk.ActionEntry("File.Open", Gtk.Stock.Open, ActionMain_File_Open_Activated),
				new Gtk.ActionEntry("File.Save", Gtk.Stock.Save, ActionMain_File_Save_Activated),
				new Gtk.ActionEntry("File.SaveAs", Gtk.Stock.SaveAs, ActionMain_File_SaveAs_Activated),
				new Gtk.ActionEntry("File.Close", Gtk.Stock.Close, ActionMain_File_Close_Activated),
				new Gtk.ActionEntry("File.Quit", Gtk.Stock.Quit, ActionMain_File_Quit_Activated),
				// Edit menu
				new Gtk.ActionEntry("Edit.Undo", Gtk.Stock.Undo, ActionMain_Edit_Undo_Activated),
				new Gtk.ActionEntry("Edit.Redo", Gtk.Stock.Redo, ActionMain_Edit_Redo_Activated),
				new Gtk.ActionEntry("Edit.Cut", Gtk.Stock.Cut, ActionMain_Edit_Cut_Activated),
				new Gtk.ActionEntry("Edit.Copy", Gtk.Stock.Copy, ActionMain_Edit_Copy_Activated),
				new Gtk.ActionEntry("Edit.Paste", Gtk.Stock.Paste, ActionMain_Edit_Paste_Activated),
				new Gtk.ActionEntry("Edit.Delete", Gtk.Stock.Delete, ActionMain_Edit_Delete_Activated),
				// Help menu
				new Gtk.ActionEntry("Help.About", Gtk.Stock.About, ActionMain_Help_About_Activated),
			};
			actionGroupMain.Add(actionEntriesMain);
		}

		private void ConstructImageMenuItemFileNew()
		{
			Debug.WriteLine("ConstructImageMenuItemFileNew");
			imageMenuItemFileNew = new Gtk.ImageMenuItem(Gtk.Stock.New, accelGroupMain)
			{
				Name = "imageMenuItemFileNew",
			};
			actionGroupMain["File.New"].ConnectProxy(imageMenuItemFileNew);
		}

		private void ConstructImageMenuItemFileOpen()
		{
			Debug.WriteLine("ConstructImageMenuItemFileOpen");
			imageMenuItemFileOpen = new Gtk.ImageMenuItem(Gtk.Stock.Open, accelGroupMain)
			{
				Name = "imageMenuItemFileOpen",
			};
			actionGroupMain["File.Open"].ConnectProxy(imageMenuItemFileOpen);
		}

		private void ConstructImageMenuItemFileSave()
		{
			Debug.WriteLine("ConstructImageMenuItemFileSave");
			imageMenuItemFileSave = new Gtk.ImageMenuItem(Gtk.Stock.Save, accelGroupMain)
			{
				Name = "imageMenuItemFileSave",
			};
			actionGroupMain["File.Save"].ConnectProxy(imageMenuItemFileSave);
		}

		private void ConstructImageMenuItemFileSaveAs()
		{
			Debug.WriteLine("ConstructImageMenuItemFileSaveAs");
			imageMenuItemFileSaveAs = new Gtk.ImageMenuItem(Gtk.Stock.SaveAs, accelGroupMain)
			{
				Name = "imageMenuItemFileSaveAs",
			};
			actionGroupMain["File.SaveAs"].ConnectProxy(imageMenuItemFileSaveAs);
		}

		private void ConstructImageMenuItemFileClose()
		{
			Debug.WriteLine("ConstructImageMenuItemFileClose");
			imageMenuItemFileClose = new Gtk.ImageMenuItem(Gtk.Stock.Close, accelGroupMain)
			{
				Name = "imageMenuItemFileClose",
			};
			actionGroupMain["File.Close"].ConnectProxy(imageMenuItemFileClose);
		}

		private void ConstructSeparatorMenuItemFile1()
		{
			Debug.WriteLine("ConstructSeparatorMenuItemFile1");
			separatorMenuItemFile1 = new Gtk.SeparatorMenuItem
			{
				Visible = true,
			};
		}

		private void ConstructImageMenuItemFileQuit()
		{
			Debug.WriteLine("ConstructImageMenuItemFileQuit");
			imageMenuItemFileQuit = new Gtk.ImageMenuItem(Gtk.Stock.Quit, accelGroupMain)
			{
				Name = "imageMenuItemFileQuit",
			};
			actionGroupMain["File.Quit"].ConnectProxy(imageMenuItemFileQuit);
		}
		
		private void ConstructMenuFile()
        {
			Debug.WriteLine("ConstructMenuFile");
			ConstructImageMenuItemFileNew();
			ConstructImageMenuItemFileOpen();
			ConstructImageMenuItemFileSave();
			ConstructImageMenuItemFileSaveAs();
			ConstructImageMenuItemFileClose();
			ConstructSeparatorMenuItemFile1();
			ConstructImageMenuItemFileQuit();

			menuFile = new Gtk.Menu
			{
				Name = "menuFile",
				Visible = true,
			};

			menuFile.Add(imageMenuItemFileNew);
			menuFile.Add(imageMenuItemFileOpen);
			menuFile.Add(imageMenuItemFileSave);
			menuFile.Add(imageMenuItemFileSaveAs);
			menuFile.Add(imageMenuItemFileClose);
			menuFile.Add(separatorMenuItemFile1);
			menuFile.Add(imageMenuItemFileQuit);
		}

        private void ConstructMenuItemFile()
        {
			Debug.WriteLine("ConstructMenuItemFile");
			ConstructMenuFile();

			menuItemFile = new Gtk.MenuItem("File")
			{
				Name = "menuItemFile",
				Visible = true,
				Submenu = menuFile,
			};        }

		private void ConstrcutImageMenuItemEditUndo()
		{
			Debug.WriteLine("ConstrcutImageMenuItemEditUndo");
			imageMenuItemEditUndo = new Gtk.ImageMenuItem(Gtk.Stock.Undo, accelGroupMain)
			{
				Name = "imageMenuItemEditUndo",
			};
			actionGroupMain["Edit.Undo"].ConnectProxy(imageMenuItemEditUndo);
		}

		private void ConstrcutImageMenuItemEditRedo()
		{
			Debug.WriteLine("ConstrcutImageMenuItemEditRedo");
			imageMenuItemEditRedo = new Gtk.ImageMenuItem(Gtk.Stock.Redo, accelGroupMain)
			{
				Name = "imageMenuItemEditRedo",
			};
			actionGroupMain["Edit.Redo"].ConnectProxy(imageMenuItemEditRedo);
		}

		private void ConstructSeparatorMenuItemEdit1()
		{
			Debug.WriteLine("ConstructSeparatorMenuItemEdit1");
			separatorMenuItemEdit1 = new Gtk.SeparatorMenuItem
			{
				Visible = true,
			};
		}

		private void ConstrcutImageMenuItemEditCut()
		{
			Debug.WriteLine("ConstrcutImageMenuItemEditCut");
			imageMenuItemEditCut = new Gtk.ImageMenuItem(Gtk.Stock.Cut, accelGroupMain)
			{
				Name = "imageMenuItemEditCut",
			};
			actionGroupMain["Edit.Cut"].ConnectProxy(imageMenuItemEditCut);
		}

		private void ConstrcutImageMenuItemEditCopy()
		{
			Debug.WriteLine("ConstrcutImageMenuItemEditCopy");
			imageMenuItemEditCopy = new Gtk.ImageMenuItem(Gtk.Stock.Copy, accelGroupMain)
			{
				Name = "imageMenuItemEditCopy",
			};
			actionGroupMain["Edit.Copy"].ConnectProxy(imageMenuItemEditCopy);
		}

		private void ConstrcutImageMenuItemEditPaste()
		{
			Debug.WriteLine("ConstrcutImageMenuItemEditPaste");
			imageMenuItemEditPaste = new Gtk.ImageMenuItem(Gtk.Stock.Paste, accelGroupMain)
			{
				Name = "imageMenuItemEditPaste",
			};
			actionGroupMain["Edit.Paste"].ConnectProxy(imageMenuItemEditPaste);
		}

		private void ConstrcutImageMenuItemEditDelete()
		{
			Debug.WriteLine("ConstrcutImageMenuItemEditDelete");
			imageMenuItemEditDelete = new Gtk.ImageMenuItem(Gtk.Stock.Delete, accelGroupMain)
			{
				Name = "imageMenuItemEditDelete",
			};
			actionGroupMain["Edit.Delete"].ConnectProxy(imageMenuItemEditDelete);
		}

        private void ConstructMenuEdit()
        {
			Debug.WriteLine("ConstructMenuEdit");
			ConstrcutImageMenuItemEditUndo();
			ConstrcutImageMenuItemEditRedo();
			ConstructSeparatorMenuItemEdit1();
			ConstrcutImageMenuItemEditCut();
			ConstrcutImageMenuItemEditCopy();
			ConstrcutImageMenuItemEditPaste();
			ConstrcutImageMenuItemEditDelete();

			menuEdit = new Gtk.Menu
			{
				Name = "menuEdit",
				Visible = true,
			};

        	menuEdit.Add(imageMenuItemEditUndo);
			menuEdit.Add(imageMenuItemEditRedo);
			menuEdit.Add(separatorMenuItemEdit1);
			menuEdit.Add(imageMenuItemEditCut);
			menuEdit.Add(imageMenuItemEditCopy);
			menuEdit.Add(imageMenuItemEditPaste);
			menuEdit.Add(imageMenuItemEditDelete);
		}

        private void ConstructMenuItemEdit()
        {
			Debug.WriteLine("ConstructMenuItemEdit");
			ConstructMenuEdit();

			menuItemEdit = new Gtk.MenuItem("Edit")
			{
				Name = "menuItemEdit",
				Visible = true,
				Submenu = menuEdit,
			};        }

		private void ConstructImageMenuItemHelpAbout()
		{
			Debug.WriteLine("ConstructImageMenuItemHelpAbout");
			imageMenuItemHelpAbout = new Gtk.ImageMenuItem
			{
				Name = "imageMenuItemHelpAbout",
			};
			actionGroupMain["Help.About"].ConnectProxy(imageMenuItemHelpAbout);
		}

		private void ConstructMenuHelp()
        {
			Debug.WriteLine("ConstructMenuHelp");
			ConstructImageMenuItemHelpAbout();

			menuHelp = new Gtk.Menu
			{
				Name = "menuHelp",
				Visible = true,
			};

			menuHelp.Add(imageMenuItemHelpAbout);
        }

        private void ConstructMenuItemHelp()
        {
			Debug.WriteLine("ConstructMenuItemHelp");
			ConstructMenuHelp();

			menuItemHelp = new Gtk.MenuItem("Help")
			{
				Name = "menuItemHelp",
				Visible = true,
				Submenu = menuHelp,
			};        }

        private void ConstructMenuBarMain()
        {
			Debug.WriteLine("ConstructMenuBarMain");
			ConstructMenuItemFile();
            ConstructMenuItemEdit();
            ConstructMenuItemHelp();

			menuBarMain = new Gtk.MenuBar
			{
				Name = "menuBarMain",
				Visible = true,
			};

        	menuBarMain.Add(menuItemFile);
            menuBarMain.Add(menuItemEdit);
            menuBarMain.Add(menuItemHelp);
        }

        private void ConstructToolButtonNew()
        {
			Debug.WriteLine("ConstructToolButtonNew");
			toolButtonNew = new Gtk.ToolButton(null, null)
			{
				Name = "toolButtonNew",
			};
        	actionGroupMain["File.New"].ConnectProxy(toolButtonNew);
        }

        private void ConstructToolButtonOpen()
        {
			Debug.WriteLine("ConstructToolButtonOpen");
			toolButtonOpen = new Gtk.ToolButton(null, null)
			{
				Name = "toolButtonOpen",
			};
        	actionGroupMain["File.Open"].ConnectProxy(toolButtonOpen);
        }

        private void ConstructToolButtonSave()
        {
			Debug.WriteLine("ConstructToolButtonSave");
			toolButtonSave = new Gtk.ToolButton(null, null)
			{
				Name = "toolButtonSave",
			};
        	actionGroupMain["File.Save"].ConnectProxy(toolButtonSave);
        }

		private void ConstructToolButtonClose()
		{
			Debug.WriteLine("ConstructToolButtonClose");
			toolButtonClose = new Gtk.ToolButton(null, null)
			{
				Name = "toolButtonClose",
			};
			actionGroupMain["File.Close"].ConnectProxy(toolButtonClose);
		}

		private void ConstructSeparatorToolItem1()
		{
			Debug.WriteLine("ConstructSeparatorToolItem1");
			separatorToolItem1 = new Gtk.SeparatorToolItem
			{
				Name = "separatorToolItem1",
				Visible = true,
			};
		}

		private void ConstructToolButtonUndo()
		{
			Debug.WriteLine("ConstructToolButtonUndo");
			toolButtonUndo = new Gtk.ToolButton(null, null)
			{
				Name = "toolButtonUndo",
			};
			actionGroupMain["Edit.Undo"].ConnectProxy(toolButtonUndo);
		}

		private void ConstructToolButtonRedo()
		{
			Debug.WriteLine("ConstructToolButtonRedo");
			toolButtonRedo = new Gtk.ToolButton(null, null)
			{
				Name = "toolButtonRedo",
			};
			actionGroupMain["Edit.Redo"].ConnectProxy(toolButtonRedo);
		}

		private void ConstructSeparatorToolItem2()
		{
			Debug.WriteLine("ConstructSeparatorToolItem2");
			separatorToolItem2 = new Gtk.SeparatorToolItem
			{
				Name = "separatorToolItem2",
				Visible = true,
			};
		}

		private void ConstructToolButtonCut()
		{
			Debug.WriteLine("ConstructToolButtonCut");
			toolButtonCut = new Gtk.ToolButton(null, null)
			{
				Name = "toolButtonCut",
			};
			actionGroupMain["Edit.Cut"].ConnectProxy(toolButtonCut);
		}

		private void ConstructToolButtonCopy()
		{
			Debug.WriteLine("ConstructToolButtonCopy");
			toolButtonCopy = new Gtk.ToolButton(null, null)
			{
				Name = "toolButtonCopy",
			};
			actionGroupMain["Edit.Copy"].ConnectProxy(toolButtonCopy);
		}

		private void ConstructToolButtonPaste()
		{
			Debug.WriteLine("ConstructToolButtonPaste");
			toolButtonPaste = new Gtk.ToolButton(null, null)
			{
				Name = "toolButtonPaste",
			};
			actionGroupMain["Edit.Paste"].ConnectProxy(toolButtonPaste);
		}

		private void ConstructSeparatorToolItem3()
		{
			Debug.WriteLine("ConstructSeparatorToolItem3");
			separatorToolItem3 = new Gtk.SeparatorToolItem
			{
				Name = "separatorToolItem3",
				Visible = true,
			};
		}

		private void ConstructToolButtonQuit()
		{
			Debug.WriteLine("ConstructToolButtonQuit");
			toolButtonQuit = new Gtk.ToolButton(null, null)
			{
				Name = "toolButtonQuit",
			};
			actionGroupMain["File.Quit"].ConnectProxy(toolButtonQuit);
		}

		private void ConstructToolbarMain()
        {
			Debug.WriteLine("ConstructToolbarMain");
			ConstructToolButtonNew();
            ConstructToolButtonOpen();
			ConstructToolButtonSave();
			ConstructToolButtonClose();
			ConstructSeparatorToolItem1();
			ConstructToolButtonUndo();
			ConstructToolButtonRedo();
			ConstructSeparatorToolItem2();
			ConstructToolButtonCut();
			ConstructToolButtonCopy();
			ConstructToolButtonPaste();
			ConstructSeparatorToolItem3();
			ConstructToolButtonQuit();

			toolbarMain = new Gtk.Toolbar
			{
				Name = "toolbarMain",
				Visible = true,
				Tooltips = true,
			};

			toolbarMain.Add(toolButtonNew);
            toolbarMain.Add(toolButtonOpen);
			toolbarMain.Add(toolButtonSave);
			toolbarMain.Add(toolButtonClose);
			toolbarMain.Add(separatorToolItem1);
			toolbarMain.Add(toolButtonUndo);
			toolbarMain.Add(toolButtonRedo);
			toolbarMain.Add(separatorToolItem2);
			toolbarMain.Add(toolButtonCut);
			toolbarMain.Add(toolButtonCopy);
			toolbarMain.Add(toolButtonPaste);
			toolbarMain.Add(separatorToolItem3);
			toolbarMain.Add(toolButtonQuit);
		}

        private void ConstructFilesNotebookFiles()
        {
			Debug.WriteLine("ConstructFilesNotebookFiles");
			filesNotebookFiles = new FilesNotebook
			{
				Name = "filesNotebookFiles",
				Visible = true,
				BorderWidth = 1,
			};

        	filesNotebookFiles.CurrentFileBufferChanged += FilesNotebookFiles_CurrentOpenFileChanged;
			filesNotebookFiles.FileBufferChanged += FilesNotebookFiles_OpenFileChanged;
			filesNotebookFiles.FileBufferModifiedChanged += FilesNotebookFiles_OpenFileModifiedChanged;
			filesNotebookFiles.FileBufferTitleChanged += FilesNotebookFiles_CurrentOpenFileTitleChanged;
			filesNotebookFiles.FileBufferSelectionChanged += FilesNotebookFiles_OpenFileSelectionChanged;
			filesNotebookFiles.FileBufferClosing += FilesNotebookFiles_OpenFileClosing;
		}

        private void ConstructStatusbarMain()
        {
			Debug.WriteLine("ConstructStatusbarMain");
			statusbarMain = new Gtk.Statusbar
			{
				Name = "statusbarMain",
				Visible = true,
			};
        }

        private void ConstructVBoxMain()
        {
			Debug.WriteLine("ConstructVBoxMain");
			ConstructMenuBarMain();
            ConstructToolbarMain();
            ConstructFilesNotebookFiles();
            ConstructStatusbarMain();

			vBoxMain = new Gtk.VBox(false, 0)
			{
				Name = "vBoxMain",
				Visible = true,
			};

        	vBoxMain.PackStart(menuBarMain, false, true, 0);
            vBoxMain.PackStart(toolbarMain, false, true, 0);
            vBoxMain.PackStart(filesNotebookFiles, true, true, 0);
            vBoxMain.PackEnd(statusbarMain, false, false, 0);
        }

        private void ConstructWindowMain()
        {
			Debug.WriteLine("ConstructWindowMain");
			ConstructAccelGroupMain();
			ConstructActionGroupMain();
            ConstructVBoxMain();

            Name = "WindowMain";
            SetPosition(Gtk.WindowPosition.Center);
            IconName = Gtk.Stock.Edit;
            int w, h;
            GetSize(out w, out h);
            Resize(Math.Max(w, 700), Math.Max(h, 500));
			HideOnDelete();

			DeleteEvent += WindowMain_DeleteEvent;

            Add(vBoxMain);
		}

		#endregion GUI

		private void UpdateTitle()
		{
			Title = "Gtk# Simple Editor";
			if (filesNotebookFiles.CurrentFileBuffer != null)
				Title += " - " + filesNotebookFiles.CurrentFileBuffer.Title;
		}

		private void UpdateActions()
		{
			actionGroupMain["File.Save"].Sensitive = filesNotebookFiles.CurrentFileBuffer != null &&
				filesNotebookFiles.CurrentFileBuffer.Modified;
			actionGroupMain["File.SaveAs"].Sensitive = filesNotebookFiles.CurrentFileBuffer != null;
			actionGroupMain["File.Close"].Sensitive = filesNotebookFiles.CurrentFileBuffer != null;

			actionGroupMain["Edit.Undo"].Sensitive = filesNotebookFiles.CurrentFileBuffer != null &&
				filesNotebookFiles.CurrentFileBuffer.CanUndo;
			actionGroupMain["Edit.Redo"].Sensitive = filesNotebookFiles.CurrentFileBuffer != null &&
				filesNotebookFiles.CurrentFileBuffer.CanRedo;
			actionGroupMain["Edit.Cut"].Sensitive = filesNotebookFiles.CurrentFileBuffer != null &&
				clipboardManager.CanCut(filesNotebookFiles.CurrentFileBuffer);
			actionGroupMain["Edit.Copy"].Sensitive = filesNotebookFiles.CurrentFileBuffer != null &&
				clipboardManager.CanCopy(filesNotebookFiles.CurrentFileBuffer);
			actionGroupMain["Edit.Paste"].Sensitive = filesNotebookFiles.CurrentFileBuffer != null &&
				clipboardManager.CanPaste(filesNotebookFiles.CurrentFileBuffer);
			actionGroupMain["Edit.Delete"].Sensitive = filesNotebookFiles.CurrentFileBuffer != null &&
				clipboardManager.CanDelete(filesNotebookFiles.CurrentFileBuffer);
		}

		private bool CloseAll()
		{
			bool result = filesNotebookFiles.CloseAll();
			UpdateActions();
			return result;
		}

		private void ActionMain_File_New()
		{
			filesNotebookFiles.New();
			UpdateActions();
		}

		private void ActionMain_File_Open()
		{
			using (var fcd = new Gtk.FileChooserDialog("Open", this, Gtk.FileChooserAction.Open))
			{
				fcd.AddButton("gtk-cancel", Gtk.ResponseType.Cancel);
				fcd.AddButton("gtk-open", Gtk.ResponseType.Ok).GrabDefault();
				if (fcd.Run() == (int) Gtk.ResponseType.Ok)
				{
					filesNotebookFiles.Open(fcd.Filename);
					UpdateActions();
				}
				fcd.Destroy();
			}
		}

		private void ActionMain_File_Save()
		{
			if (filesNotebookFiles == null || filesNotebookFiles.CurrentFileBuffer == null)
				throw new InvalidOperationException();
			if (filesNotebookFiles.CurrentFileBuffer.FileInfo != null)
			{
				filesNotebookFiles.CurrentFileBuffer.Save();
				UpdateActions();
			}
			else
				ActionMain_File_SaveAs();
		}

		private void ActionMain_File_SaveAs()
		{
			if (filesNotebookFiles == null || filesNotebookFiles.CurrentFileBuffer == null)
				throw new InvalidOperationException();
			using (var fcd = new Gtk.FileChooserDialog("Save", this, Gtk.FileChooserAction.Save))
			{
				fcd.AddButton("gtk-cancel", Gtk.ResponseType.Cancel);
				fcd.AddButton("gtk-save", Gtk.ResponseType.Ok).GrabDefault();
				if (fcd.Run() == (int) Gtk.ResponseType.Ok)
				{
					filesNotebookFiles.CurrentFileBuffer.SaveAs(fcd.Filename);
					UpdateActions();
				}
				fcd.Destroy();
			}
		}

		private void ActionMain_File_Close()
		{
			filesNotebookFiles.CloseCurrent();
			UpdateActions();
		}

		private void ActionMain_File_Quit()
		{
			if (CloseAll())
				Destroy();
		}

		private void ActionMain_Edit_Undo()
		{
			if (filesNotebookFiles.CurrentFileBuffer != null && filesNotebookFiles.CurrentFileBuffer.Undo())
				UpdateActions();
		}

		private void ActionMain_Edit_Redo()
		{
			if (filesNotebookFiles.CurrentFileBuffer != null && filesNotebookFiles.CurrentFileBuffer.Redo())
				UpdateActions();
		}

		private void ActionMain_Edit_Cut()
		{
			if (filesNotebookFiles.CurrentFileBuffer != null)
			{
				clipboardManager.Cut(filesNotebookFiles.CurrentFileBuffer);
				UpdateActions();
			}
		}

		private void ActionMain_Edit_Copy()
		{
			if (filesNotebookFiles.CurrentFileBuffer != null)
			{
				clipboardManager.Copy(filesNotebookFiles.CurrentFileBuffer);
				UpdateActions();
			}
		}

		private void ActionMain_Edit_Paste()
		{
			if (filesNotebookFiles.CurrentFileBuffer != null)
			{
				clipboardManager.Paste(filesNotebookFiles.CurrentFileBuffer);
				UpdateActions();
			}
		}

		private void ActionMain_Edit_Delete()
		{
			if (filesNotebookFiles.CurrentFileBuffer != null)
			{
				clipboardManager.Delete(filesNotebookFiles.CurrentFileBuffer);
				UpdateActions();
			}
		}

		private void ActionMain_Help_About()
		{
		}

		#region Event Handlers

		private void WindowMain_DeleteEvent(object o, Gtk.DeleteEventArgs args)
		{
			args.RetVal = !CloseAll();
		}

		private void ActionMain_File_New_Activated(object sender, EventArgs e)
		{
			ActionMain_File_New();
		}

		private void ActionMain_File_Open_Activated(object sender, EventArgs e)
		{
			ActionMain_File_Open();
		}

		private void ActionMain_File_Save_Activated(object sender, EventArgs e)
		{
			ActionMain_File_Save();
		}

		private void ActionMain_File_SaveAs_Activated(object sender, EventArgs e)
		{
			ActionMain_File_SaveAs();
		}

		private void ActionMain_File_Close_Activated(object sender, EventArgs e)
		{
			ActionMain_File_Close();
		}

		private void ActionMain_File_Quit_Activated(object sender, EventArgs e)
		{
			ActionMain_File_Quit();
		}

		private void ActionMain_Edit_Undo_Activated(object sender, EventArgs e)
		{
			ActionMain_Edit_Undo();
		}

		private void ActionMain_Edit_Redo_Activated(object sender, EventArgs e)
		{
			ActionMain_Edit_Redo();
		}

		private void ActionMain_Edit_Cut_Activated(object sender, EventArgs e)
		{
			ActionMain_Edit_Cut();
		}

		private void ActionMain_Edit_Copy_Activated(object sender, EventArgs e)
		{
			ActionMain_Edit_Copy();
		}

		private void ActionMain_Edit_Paste_Activated(object sender, EventArgs e)
		{
			ActionMain_Edit_Paste();
		}

		private void ActionMain_Edit_Delete_Activated(object sender, EventArgs e)
		{
			ActionMain_Edit_Delete();
		}

		private void ActionMain_Help_About_Activated(object sender, EventArgs e)
		{
			ActionMain_Help_About();
		}

		private void FilesNotebookFiles_CurrentOpenFileChanged(object sender, EventArgs e)
		{
			UpdateTitle();
			UpdateActions();
		}

		private void FilesNotebookFiles_OpenFileChanged(object sender, FilesNotebook.FileBufferEventArgs e)
		{
			UpdateActions();
		}

		private void FilesNotebookFiles_OpenFileModifiedChanged(object sender, FilesNotebook.FileBufferEventArgs e)
		{
			UpdateActions();
		}

		private void FilesNotebookFiles_CurrentOpenFileTitleChanged(object sender, FilesNotebook.FileBufferEventArgs e)
		{
			if (e.FileBuffer == filesNotebookFiles.CurrentFileBuffer)
				UpdateTitle();
		}

		private void FilesNotebookFiles_OpenFileSelectionChanged(object sender, FilesNotebook.FileBufferEventArgs e)
		{
			if (e.FileBuffer == filesNotebookFiles.CurrentFileBuffer)
				UpdateActions();
		}

		private void FilesNotebookFiles_OpenFileClosing(object sender, FilesNotebook.FileBufferCancelEventArgs e)
		{
			if (e.FileBuffer.Modified)
			{
				string fileName = e.FileBuffer.FileInfo == null ? FileBuffer.NewFileTitle : e.FileBuffer.FileInfo.FullName;
				using (var md = new Gtk.MessageDialog(this, Gtk.DialogFlags.Modal, Gtk.MessageType.Question,
					Gtk.ButtonsType.YesNo, "File '" + fileName + "' is modified but not saved! Do you want to close it anyway?"))
				{
					int res = md.Run();
					if (res == (int) Gtk.ResponseType.No)
						e.Cancel = true;
					md.Destroy();
				}
			}
		}

		#endregion Event Handlers

		#endregion Methods
	}

	#endregion class WindowMain
}
