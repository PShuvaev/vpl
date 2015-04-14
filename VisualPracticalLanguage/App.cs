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
		DllManager dllManager;

		TabControl funPanelTabs;
		ElementPanel createdFunsPanel;

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
					__.DropDownItems.Add(newItem("&Execute", Execute));
				}));
				_.Items.Add(new ToolStripMenuItem("&Libraries").With(__ => {
					__.DropDownItems.Add(newItem("&Добавить/удалить", EditDlls));
					dllManager = new DllManager (__.DropDownItems, () => funPanelTabs, OnAddFunctionToWorkspace);
				}));
			});

			groupPanel = new Panel ();
			groupPanel.Parent = this;
			groupPanel.Dock = DockStyle.Fill;

			// http://stackoverflow.com/questions/154543/panel-dock-fill-ignoring-other-panel-dock-setting
			groupPanel.BringToFront ();

			funPanelTabs = new TabControl {
				Width = Const.ELEMENT_PANEL_WIDTH,
				Dock = DockStyle.Right,
				BackColor = Color.Red
			};
			funPanelTabs.Parent = groupPanel;
			
			funPanelTabs.TabPages.Add (new TabPage().With (_ => {
				_.Text = "Базовые";
				_.Controls.Add(new ElementPanel (OnAddFunctionToWorkspace, StandartFuns.Funs));
			}));

			var createdFuns = new Dictionary<string, Func<DraggableControl>> ();
			funPanelTabs.TabPages.Add (new TabPage ().With (_ => {
				_.Text = "Созданные";
				_.Controls.Add (createdFunsPanel = new ElementPanel (OnAddFunctionToWorkspace, createdFuns));
			}));

			
			NewWorkspace ();

			new Trasher (workPanel, element => {
				(element as IFunctionDefinition).With(_ => {
					//удаление функции из текущего неймспейса
					currentNamespace.functions.Remove(_);

					//удаление кнопки создания функции с панели кастомных функций
					createdFunsPanel.Controls.Cast<Control>()
						.SingleOrDefault(x => x.Text == _.name)
							.With(btn => {btn.Parent = null;});
				});
				(element as IVariableRefsHolder).With(holder => {
					foreach(var vRef in holder.refs){
						vRef.Destroy();
					}
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

				foreach (var c in workPanel.Controls) {
					if (c is VFunction)
						workPanel.Controls.Remove ((Control)c);
				}

				CleanCustomFunPanel ();

				int funPosX = 10;
				currentNamespace.functions = currentNamespace.functions.EmptyIfNull ()
					.Select (f => (IFunctionDefinition)new VFunction (f).With (_ => {
						_.Location = new Point(funPosX, 10);
						_.Parent = workPanel;
						funPosX += _.Width + 10;
						createdFunsPanel.AddFunBtn(_);
					})).ToList();

				Text = currentNamespace.namespaceName;
				dllManager.SetImportDlls (currentNamespace.importedDlls);
			}
		}
		
		/// <summary>
		/// Удаление кнопок создания функции с панели кастомных функций
		/// </summary>
		void CleanCustomFunPanel(){
			createdFunsPanel.Controls.Cast<Control> ()
				.Select (x => (x as Button).With (btn => {
					btn.Parent = null;
				})).DoAll ();
		}

		static int newNamespaceNameNumber = 0;
		void NewWorkspace(){
			CleanCustomFunPanel ();

			(workPanel = new WorkspacePanel ()).Parent = groupPanel;
			currentNamespace = new Namespace {
				namespaceName = "New" + newNamespaceNameNumber++, 
				functions = new List<IFunctionDefinition>{
					new VFunction(new FunctionDefinition{
						name = Const.MainFunName,
						isReturnVoid = true
					}).With(_ => {
						workPanel.Controls.Add (_);
						createdFunsPanel.AddFunBtn(_);
					})
				}
			};
			Text = currentNamespace.namespaceName;
			dllManager.SetImportDlls (currentNamespace.importedDlls);
		}

		void SaveToNewFile(){
			// TODO: устанавливать .cs расширение
			var dialog = new SaveFileDialog ();
			if (dialog.ShowDialog() == DialogResult.OK && dialog.CheckPathExists)
			{
				currentFile = dialog.FileName;
				currentNamespace.namespaceName = Path.GetFileNameWithoutExtension (dialog.FileName);
				SaveToCurrentFile ();
			}
		}

		void SaveToCurrentFile(){
			if (currentFile == null)
				SaveToNewFile ();
			var output = new StringWriter ();

			currentNamespace.importedDlls = dllManager.getDlls ();
			new Generator (output).Generate (currentNamespace);
			File.WriteAllText (currentFile, output.ToString());
		}

		void EditDlls(){
			dllManager.ShowDialog ();
		}

		void OnAddFunctionToWorkspace(DraggableControl element){
			workPanel.Controls.Add (element);
			(element as IFunctionDefinition).With (_ => {
				currentNamespace.functions.Add (_);
				createdFunsPanel.AddFunBtn(_);
			});
		}

		void Execute(){
			currentNamespace.importedDlls = dllManager.getDlls ();
			new BinGenerator (currentNamespace).Execute();
		}
	}

	public static class App
	{
		public static MForm Form;

		[STAThread]
		static public void Main()
		{
			Application.Run(Form = new MForm());
		}
	}
}

