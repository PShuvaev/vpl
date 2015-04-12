using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	/// <summary>
	/// Загрузка dll может производится только из текущей директории.
	/// dll должна содержать единственный класс с одноименным с файлом названием
	/// </summary>
	public class DllManager : Form
	{
		IList<DllRow> dlls;
		ToolStripItemCollection menuItems;

		//TODO: ugly getting tabcontrol, ugly!
		Func<TabControl> getFunPanelTabs;
		Action<DraggableControl> onAddFunctionToWorkspace;

		public DllManager (ToolStripItemCollection menuItems, Func<TabControl> getFunPanelTabs, Action<DraggableControl> onAddFunctionToWorkspace) : base()
		{
			this.onAddFunctionToWorkspace = onAddFunctionToWorkspace;
			this.getFunPanelTabs = getFunPanelTabs;
			Text = "Подключенные модули";
			StartPosition = FormStartPosition.CenterScreen;
			Size = new Size (500, 300);
			
			this.menuItems = menuItems;

			var window = this;
			new FlowLayoutPanel ().With (panel => {
				panel.Dock = DockStyle.Top;
				panel.Parent = window;

				var addDllBtn = new Button {
					Text = "Добавить модуль",
				};
				addDllBtn.Click += (object sender, EventArgs e) => {
					var newLibPath = AskFilePath();
					if(string.IsNullOrEmpty(newLibPath)) return;

					addDll(Path.GetFileNameWithoutExtension(newLibPath));
				};
				addDllBtn.Parent = panel;
			});

		}

		public IList<string> getDlls ()
		{
			return dlls.EmptyIfNull ().Select (x => x.getDllName ()).ToList ();
		}
		
		ToolStripMenuItem NewItem (string name, Action action){
			return new ToolStripMenuItem(name).With(item => {
				item.Click+= (object sender, EventArgs e) => action();
			});
		}

		IList<MethodInfo> GetFunsFromAssembly(string pathToAssembly){
			return Assembly.LoadFrom (pathToAssembly)
				.GetExportedTypes ()
				.Where(x => x.IsClass)
				.SelectMany (x => x.GetMethods ())
				.Where(x => !new []{"Equals", "GetType", "ToString", "GetHashCode"}.Contains(x.Name))
				.ToList();
		}

		VFunCall MakeFunCall(MethodInfo info){
			return new VFunCall (new FunctionCall{
				function = new FunctionDeclaration{
					name = info.DeclaringType.Name + '.' + info.Name,
					argumentsCount = info.GetParameters().Count()
				},
				arguments = info.GetParameters().Select<ParameterInfo, IExpression>(x => null).ToList()
			});
		}

		static string AskFilePath(){
			var dialog = new OpenFileDialog ();
			if (dialog.ShowDialog() == DialogResult.OK && dialog.CheckFileExists)
			{
				return MakeRelativePath(dialog.FileName);
			}
			return null;
		}

		static String MakeRelativePath(String path)
		{
			return new Uri(AppDomain.CurrentDomain.BaseDirectory)
				.MakeRelativeUri(new Uri(path)).ToString();
		}

		public void SetImportDlls(IEnumerable<string> dllNames){
			//TODO: correct menu cleanup
			for (int i = 1; i < menuItems.Count; i++) {
				menuItems.RemoveAt (i);
			}

			dllNames = dllNames ?? Enumerable.Empty<string>();

			foreach(var dllName in dllNames){
				addDll (dllName);
			}
			
			dlls = dllNames.Select (p => new DllRow(menuItems, p)).ToList();
		}

		private void addDll(string dllName){
			var dllRow = new DllRow (menuItems, dllName);
			dlls.Add (dllRow);
			dllRow.Parent = this;

			menuItems.Add(NewItem(dllName, () => {}));

			var funDic = new Dictionary<string, Func<DraggableControl>>();
			foreach(var fun in GetFunsFromAssembly(dllName+".dll")){
				funDic[fun.Name] = () => MakeFunCall(fun);
			}
			getFunPanelTabs().Controls.Add(new TabPage().With(_ => {
				_.Text = dllName;
				new ElementPanel(onAddFunctionToWorkspace, funDic).Parent = _;
			}));
		}

		class DllRow : FlowLayoutPanel {
			Label pathLabel;
			Button selectDllBtn, removeDllBtn;

			public DllRow(ToolStripItemCollection menuItems, string dllPath){
				Dock = DockStyle.Top;
				Height = 30;
				pathLabel = new Label{
					Text = dllPath,
					Dock = DockStyle.Left,
					Width = 300
				};
				Controls.Add(pathLabel);
				pathLabel.BringToFront ();

				removeDllBtn = new Button {
					Text = "Удалить",
					Dock = DockStyle.Left
				};
				removeDllBtn.Click += (object sender, EventArgs e) => {
					ToolStripItem item = menuItems.Cast<ToolStripItem>().SingleOrDefault<ToolStripItem> (x => x.Text == pathLabel.Text);
					menuItems.Remove(item);
				};
				Controls.Add(removeDllBtn);
				removeDllBtn.BringToFront ();

				selectDllBtn = new Button {
					Text = "Изменить путь",
					Dock = DockStyle.Left
				};
				selectDllBtn.Click += (object sender, EventArgs e) => {
					var newLibPath = AskFilePath();
					if(string.IsNullOrEmpty(newLibPath)) return;
					pathLabel.Text = newLibPath;
				};
				Controls.Add(selectDllBtn);
				selectDllBtn.BringToFront ();
			}

			public string getDllName(){
				return Path.GetFileNameWithoutExtension(pathLabel.Text);
			}
		}
	}
}

