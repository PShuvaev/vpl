using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Sprache;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    public class MForm : Form
    {
        private static int newNamespaceNameNumber;
        private ElementPanel createdFunsPanel;
        private string currentFile;
        private INamespace currentNamespace;
        private DllManager dllManager;
        public Panel workPanel;
        private readonly TabControl funPanelTabs;
        private readonly Panel groupPanel;

        public MForm()
        {
            Size = new Size(800, 600);
            //WindowState = FormWindowState.Maximized;

            new MenuStrip().With(_ =>
            {
                _.Parent = this;
                _.Dock = DockStyle.Top;

                Func<string, Action, ToolStripMenuItem> newItem = (name, action) =>
                    new ToolStripMenuItem(name).With(item => { item.Click += (sender, e) => action(); });

                _.Items.Add(new ToolStripMenuItem("Файл").With(__ =>
                {
                    __.DropDownItems.Add(newItem("Новый проект", NewWorkspace));
                    __.DropDownItems.Add(newItem("Открыть", OpenWorkspace));
                    __.DropDownItems.Add(newItem("Сохранить", () => SaveToCurrentFile()));
                    __.DropDownItems.Add(newItem("Сохранить как", () => SaveToNewFile()));
                    __.DropDownItems.Add(new ToolStripMenuItem("Выход"));
                }));
                _.Items.Add(new ToolStripMenuItem("Сборка").With(__ =>
                {
                    __.DropDownItems.Add(newItem("Собрать", Build));
                    __.DropDownItems.Add(newItem("Выполнить", Execute));
                }));
                _.Items.Add(new ToolStripMenuItem("Библиотеки").With(__ =>
                {
                    __.DropDownItems.Add(newItem("Добавить/удалить", EditDlls));
                    dllManager = new DllManager(__.DropDownItems, () => funPanelTabs, OnAddFunctionToWorkspace);
                }));
                _.Items.Add(new ToolStripMenuItem("О программе").With(__ =>
                {
                    __.Click += (sender, e) =>
                    {
                        MessageBox.Show("VPL.NET версия 1.0\r\n" +
                                        "Copyright @ 2015 Pavel Shuvaev\r\n" +
                                        "Email: ipshuvaev@gmail.com", "О программе", MessageBoxButtons.OK);
                    };
                }));
            });

            groupPanel = new Panel {Parent = this, Dock = DockStyle.Fill};

            // http://stackoverflow.com/questions/154543/panel-dock-fill-ignoring-other-panel-dock-setting
            groupPanel.BringToFront();

            funPanelTabs = new TabControl
            {
                Width = Const.ELEMENT_PANEL_WIDTH,
                Dock = DockStyle.Right,
                BackColor = Color.Red
            };
            funPanelTabs.Parent = groupPanel;

            funPanelTabs.TabPages.Add(new TabPage().With(_ =>
            {
                _.Text = "Базовые";
                _.Controls.Add(new ElementPanel(OnAddFunctionToWorkspace, StandartFuns.Funs));
            }));

            var createdFuns = new Dictionary<string, Func<DraggableControl>>();
            funPanelTabs.TabPages.Add(new TabPage().With(_ =>
            {
                _.Text = "Созданные";
                _.Controls.Add(createdFunsPanel = new ElementPanel(OnAddFunctionToWorkspace, createdFuns));
            }));


            NewWorkspace();

            new Trasher(workPanel, element =>
            {
                (element as IFunctionDefinition).With(_ =>
                {
                    //удаление функции из текущего неймспейса
                    currentNamespace.functions.Remove(_);

                    //удаление кнопки создания функции с панели кастомных функций
                    createdFunsPanel.Controls.Cast<Control>()
                        .SingleOrDefault(x => x.Text == _.name)
                        .With(btn => { btn.Parent = null; });
                });
                (element as IVariableRefsHolder).With(holder =>
                {
                    foreach (var vRef in holder.refs)
                    {
                        vRef.Destroy();
                    }
                });
            });

            CenterToScreen();
        }

        private void OpenWorkspace()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Проектные файлы (*.cs)|*.cs"
            };
            if (dialog.ShowDialog() != DialogResult.OK || !dialog.CheckFileExists) return;
            currentFile = dialog.FileName;
            var src = File.ReadAllText(currentFile);
            currentNamespace = VplSharpParser.NamespaceP.Parse(src);

            foreach (var c in workPanel.Controls.OfType<VFunction>())
            {
                workPanel.Controls.Remove(c);
            }

            CleanCustomFunPanel();
            CleanWorkspacePanel();

            var funPosX = 10;
            currentNamespace.functions = currentNamespace.functions.EmptyIfNull()
                .Select(f => (IFunctionDefinition) new VFunction(f).With(_ =>
                {
                    _.Location = new Point(funPosX, 10);
                    _.Parent = workPanel;
                    funPosX += _.Width + 10;
                    createdFunsPanel.AddFunBtn(_);
                })).ToList();

            Text = currentNamespace.namespaceName;
            dllManager.SetImportDlls(currentNamespace.importedDlls);
        }

        /// <summary>
        ///     Удаление кнопок создания функции с панели кастомных функций
        /// </summary>
        private void CleanCustomFunPanel()
        {
            createdFunsPanel.Controls.Cast<Control>()
                .Select(x => (x as Button).With(btn => { btn.Parent = null; })).DoAll();
        }

        private void CleanWorkspacePanel()
        {

            if (workPanel != null) workPanel.Parent = null;
            (workPanel = new WorkspacePanel()).Parent = groupPanel;
        }

        private void NewWorkspace()
        {
            CleanCustomFunPanel();
            CleanWorkspacePanel();

            currentNamespace = new Namespace
            {
                namespaceName = "New" + newNamespaceNameNumber++,
                functions = new List<IFunctionDefinition>
                {
                    new VFunction(new FunctionDefinition
                    {
                        name = Const.MainFunName,
                        isReturnVoid = true
                    }).With(_ =>
                    {
                        workPanel.Controls.Add(_);
                        createdFunsPanel.AddFunBtn(_);
                    })
                }
            };
            Text = currentNamespace.namespaceName;
            dllManager.SetImportDlls(currentNamespace.importedDlls);
        }

        private bool SaveToNewFile()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Проектные файлы (*.cs)|*.cs"
            };
            if (dialog.ShowDialog() == DialogResult.OK && dialog.CheckPathExists)
            {
                currentFile = dialog.FileName;
                currentNamespace.namespaceName = Path.GetFileNameWithoutExtension(dialog.FileName);
                SaveToCurrentFile();
                return true;
            }
            return false;
        }

        private bool SaveToCurrentFile()
        {
            if (currentFile != null || currentFile == null && SaveToNewFile())
            {
                var output = new StringWriter();

                currentNamespace.importedDlls = dllManager.getDlls();
                new Generator(output).Generate(currentNamespace);
                File.WriteAllText(currentFile, output.ToString());
                return true;
            }
            return false;
        }

        private void EditDlls()
        {
            dllManager.ShowDialog();
        }

        private void OnAddFunctionToWorkspace(DraggableControl element)
        {
            workPanel.Controls.Add(element);
            (element as IFunctionDefinition).With(_ =>
            {
                currentNamespace.functions.Add(_);
                createdFunsPanel.AddFunBtn(_);
            });
        }

        private void Build()
        {
            currentNamespace.importedDlls = dllManager.getDlls();
            new BinGenerator(currentNamespace).Build();
        }

        private void Execute()
        {
            currentNamespace.importedDlls = dllManager.getDlls();
            new BinGenerator(currentNamespace).Execute();
        }
    }

    public static class App
    {
        public static MForm Form;

        [STAThread]
        public static void Main()
        {
            try
            {
                Application.Run(Form = new MForm());
            }
            catch (Exception e)
            {
                File.AppendAllText("ErrorLog.txt", "Error: " + e.Message + "\r\n" + e.StackTrace);
            }
        }
    }
}