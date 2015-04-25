using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    /// <summary>
    ///     Загрузка dll может производится только из текущей директории.
    ///     dll должна содержать единственный класс с одноименным с файлом названием
    /// </summary>
    public class DllManager : Form
    {
        //TODO: ugly getting tabcontrol, ugly!
        private readonly Func<TabControl> getFunPanelTabs;
        private readonly ToolStripItemCollection menuItems;
        private readonly Action<DraggableControl> onAddFunctionToWorkspace;

        public DllManager(ToolStripItemCollection menuItems, Func<TabControl> getFunPanelTabs,
            Action<DraggableControl> onAddFunctionToWorkspace)
        {
            this.onAddFunctionToWorkspace = onAddFunctionToWorkspace;
            this.getFunPanelTabs = getFunPanelTabs;
            Text = "Подключенные модули";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(250, 300);

            this.menuItems = menuItems;

            var window = this;
            new FlowLayoutPanel().With(panel =>
            {
                panel.Dock = DockStyle.Top;
                panel.Parent = window;

                var addDllBtn = new Button
                {
                    Text = "Добавить библиотеку",
                    AutoSize = true
                };
                addDllBtn.Click += (object sender, EventArgs e) =>
                {
                    var newLibPath = AskFilePath();
                    if (string.IsNullOrEmpty(newLibPath)) return;

                    addDll(Path.GetFileNameWithoutExtension(newLibPath));
                };
                addDllBtn.Parent = panel;
            });
        }

        public IList<string> getDlls()
        {
            return
                Controls.Cast<Control>()
                    .Select(x => x as DllRow)
                    .Where(x => x != null)
                    .Select(x => x.getDllName())
                    .ToList();
        }

        private ToolStripMenuItem NewItem(string name, Action action)
        {
            return new ToolStripMenuItem(name).With(item => { item.Click += (object sender, EventArgs e) => action(); });
        }

        private IList<MethodInfo> GetFunsFromAssembly(string pathToAssembly)
        {
            return Assembly.LoadFrom(pathToAssembly)
                .GetExportedTypes()
                .Where(x => x.IsAbstract && x.IsSealed) // hack to check static class
                .SelectMany(x => x.GetMethods())
                .Where(x => x.IsStatic)
                .Where(x => !new[] {"Equals", "GetType", "ToString", "GetHashCode"}.Contains(x.Name))
                .ToList();
        }

        private VFunCall MakeFunCall(MethodInfo info)
        {
            return new VFunCall(new FunctionCall
            {
                function = new FunctionDeclaration
                {
                    name = info.DeclaringType.Name + '.' + info.Name,
                    argumentsCount = info.GetParameters().Count()
                },
                arguments = info.GetParameters().Select<ParameterInfo, IExpression>(x => null).ToList()
            });
        }

        private static string AskFilePath()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Библиотеки (*.dll)|*.dll"
            };
            if (dialog.ShowDialog() == DialogResult.OK && dialog.CheckFileExists)
            {
                return MakeRelativePath(dialog.FileName);
            }
            return null;
        }

        private static String MakeRelativePath(String path)
        {
            return new Uri(AppDomain.CurrentDomain.BaseDirectory)
                .MakeRelativeUri(new Uri(path)).ToString();
        }

        public void SetImportDlls(IEnumerable<string> dllNames)
        {
            //TODO: correct menu cleanup
            for (var i = 1; i < menuItems.Count; i++)
            {
                menuItems.RemoveAt(i);
            }

            dllNames = dllNames ?? Enumerable.Empty<string>();

            foreach (var dllName in dllNames)
            {
                addDll(dllName);
            }

            dllNames.Select(p => new DllRow(menuItems, getFunPanelTabs(), p)).DoAll();
        }

        private void addDll(string dllName)
        {
            var dllRow = new DllRow(menuItems, getFunPanelTabs(), dllName);
            dllRow.Parent = this;

            menuItems.Add(NewItem(dllName, () => { }));

            var funDic = new Dictionary<string, Func<DraggableControl>>();
            foreach (var fun in GetFunsFromAssembly(dllName + ".dll"))
            {
                funDic[fun.Name] = () => MakeFunCall(fun);
            }
            getFunPanelTabs().Controls.Add(new TabPage().With(_ =>
            {
                _.Text = dllName;
                new ElementPanel(onAddFunctionToWorkspace, funDic).Parent = _;
            }));
        }

        private class DllRow : TableLayoutPanel
        {
            private readonly Label pathLabel;
            private readonly Button removeDllBtn;

            public DllRow(ToolStripItemCollection menuItems, TabControl funPanelTabs, string dllPath)
            {
                Dock = DockStyle.Top;
                BackColor = Color.LightGreen;
                Height = 30;
                ColumnCount = 2;

                pathLabel = new Label
                {
                    Text = dllPath,
                    AutoSize = true,
                    Padding = new Padding(7)
                };

                removeDllBtn = new Button
                {
                    Text = "Удалить",
                    AutoSize = true
                };
                removeDllBtn.Click += (object sender, EventArgs e) =>
                {
                    menuItems.Cast<ToolStripItem>()
                        .SingleOrDefault(x => x.Text == pathLabel.Text)
                        .With(menuItems.Remove);
                    var tabs = funPanelTabs.Controls;
                    tabs.Cast<TabPage>().SingleOrDefault(x => x.Text == pathLabel.Text).With(tabs.Remove);
                    Parent = null;
                };
                Controls.Add(removeDllBtn);
                Controls.Add(pathLabel);
            }

            public string getDllName()
            {
                return Path.GetFileNameWithoutExtension(pathLabel.Text);
            }
        }
    }
}