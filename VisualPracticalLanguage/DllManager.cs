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
	public class DllManager : Form
	{
		IList<DllRow> dlls;
		ToolStripItemCollection menuItems;

		public DllManager (IEnumerable<string> pathList, ToolStripItemCollection menuItems, Func<TabControl> getFunPanelTabs, Action<DraggableControl> onAddFunctionToWorkspace) : base()
		{
			Text = "Подключенные модули";
			StartPosition = FormStartPosition.CenterScreen;
			Size = new Size (500, 300);
			
			this.menuItems = menuItems;
			pathList = pathList ?? Enumerable.Empty<string>();


			foreach(var path in pathList){
				menuItems.Add(NewItem(path, () => {}));
			}

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

					var dllRow = new DllRow (menuItems, newLibPath);
					dlls.Add (dllRow);
					dllRow.Parent = window;

					menuItems.Add(NewItem(newLibPath, () => {}));

					var funDic = new Dictionary<string, Func<DraggableControl>>();
					foreach(var fun in GetFunsFromAssembly(newLibPath)){
						funDic[fun.Name] = () => MakeFunCall(fun);
					}
					getFunPanelTabs().Controls.Add(new TabPage().With(_ => {
						_.Text = newLibPath;
						new ElementPanel(onAddFunctionToWorkspace, funDic).Parent = _;
					}));
				};
				addDllBtn.Parent = panel;
			});

			dlls = pathList.EmptyIfNull ().Select (p => new DllRow(menuItems, p)).ToList();
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
				.ToList();
		}

		VFunCall MakeFunCall(MethodInfo info){
			return new VFunCall (new FunctionCall{
				function = new FunctionDeclaration{
					name = info.Name,
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

		public static String MakeRelativePath(String path)
		{
			return new Uri(AppDomain.CurrentDomain.BaseDirectory)
				.MakeRelativeUri(new Uri(path)).ToString();
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
		}
	}

}

