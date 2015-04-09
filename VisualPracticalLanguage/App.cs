using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Sprache;
using System.Collections.Generic;
using VisualPracticalLanguage.Interface;
using System.Linq;

namespace VisualPracticalLanguage
{
	public class MForm : Form {
		public Panel workPanel;
		private Panel groupPanel;
		DllManager dllWindows;

		TabControl funPanelTabs;

		private INamespace currentNamespace;
		private string currentFile;

		public MForm() {
			Size = new Size (800, 600);
			//WindowState = FormWindowState.Maximized;

			new MenuStrip ().With (_ => {
				_.Parent = this;
				_.Dock = DockStyle.Top;

				Func<string, Action, ToolStripMenuItem> newItem = (name, action) =>
					new ToolStripMenuItem(name).With(item => {
						item.Click+= (object sender, EventArgs e) => action();
					});

				_.Items.Add(new ToolStripMenuItem("&File").With(__ => {
					__.DropDownItems.Add(newItem("&New", NewWorkspace));
					__.DropDownItems.Add(newItem("&Open", OpenWorkspace));
					__.DropDownItems.Add(newItem("&Save", SaveToCurrentFile));
					__.DropDownItems.Add(newItem("Save &as", SaveToNewFile));
					__.DropDownItems.Add(new ToolStripMenuItem("E&xit"));
				}));
				_.Items.Add(new ToolStripMenuItem("&Build").With(__ => {
					__.DropDownItems.Add(new ToolStripMenuItem("&Make"));
					__.DropDownItems.Add(new ToolStripMenuItem("&Execute"));
				}));
				_.Items.Add(new ToolStripMenuItem("&Libraries").With(__ => {
					__.DropDownItems.Add(newItem("&Добавить/удалить", EditDlls));
					dllWindows = new DllManager (new List<string>{"sprache.dll", "robots.dll"}, __.DropDownItems, () => funPanelTabs, OnAddFunctionToWorkspace);
				}));
			});

			groupPanel = new Panel ();
			groupPanel.Parent = this;
			groupPanel.Dock = DockStyle.Fill;

			// http://stackoverflow.com/questions/154543/panel-dock-fill-ignoring-other-panel-dock-setting
			groupPanel.BringToFront ();

			
			NewWorkspace ();

			funPanelTabs = new TabControl {
				Width = 180,
				Dock = DockStyle.Right,
				BackColor = Color.Red
			};
			funPanelTabs.Parent = groupPanel;


			funPanelTabs.TabPages.Add (new TabPage().With (_ => {
				_.Text = "standart";
				_.Controls.Add(new ElementPanel (OnAddFunctionToWorkspace, StandartFuns.Funs));
			}));

			new Trasher (workPanel, element => {
				(element as IFunctionDefinition).With(_ => {
					currentNamespace.functions.Remove(_);
				});
			});

			CenterToScreen();
		}

		void OpenWorkspace(){
			var dialog = new OpenFileDialog ();
			if (dialog.ShowDialog() == DialogResult.OK && dialog.CheckFileExists)
			{
				currentFile = dialog.FileName;
				var src = File.ReadAllText (currentFile);
				currentNamespace = VplSharpParser.NamespaceP.Parse (src);
				if (!currentNamespace.functions.Empty()) {
					workPanel.Controls.Add (new VFunction(currentNamespace.functions.First()));
				}
				Text = currentNamespace.namespaceName;
			}
		}

		void NewWorkspace(){
			(workPanel = new WorkspacePanel ()).Parent = groupPanel;
			currentNamespace = new Namespace {
				namespaceName = "New", 
				functions = new List<IFunctionDefinition>()
			};
			Text = currentNamespace.namespaceName;
		}

		void SaveToNewFile(){
			// TODO: устанавливать .cs расширение
			var dialog = new SaveFileDialog ();
			if (dialog.ShowDialog() == DialogResult.OK && dialog.CheckPathExists)
			{
				currentFile = dialog.FileName;
				SaveToCurrentFile ();
			}
		}

		void SaveToCurrentFile(){
			if (currentFile == null)
				SaveToNewFile ();
			var output = new StringWriter ();
			new Generator (output).Generate (currentNamespace);
			File.WriteAllText (currentFile, output.ToString());
		}

		void EditDlls(){
			dllWindows.ShowDialog ();
		}

		void OnAddFunctionToWorkspace(DraggableControl element){
			workPanel.Controls.Add (element);
			(element as IFunctionDefinition).With (_ => {
				currentNamespace.functions.Add (_);
			});
		}
	}

	public static class App
	{
		public static MForm Form;

		static public void Main()
		{
			Application.Run(Form = new MForm());
		}

	}
}

