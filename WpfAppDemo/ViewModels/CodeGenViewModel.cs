using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using WpfAppDemo.Services;

namespace WpfAppDemo.ViewModels
{
    public class GeneratedFile : BindableBase
    {
        private string _name = string.Empty;
        private string _path = string.Empty;
        private string _content = string.Empty;

        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public string Path { get => _path; set => SetProperty(ref _path, value); }
        public string Content { get => _content; set => SetProperty(ref _content, value); }
    }

    public class CodeGenViewModel : BindableBase
    {
        private readonly ICodeGenService _codeGenService;
        private string? _selectedTable;
        private string _usageInstructions = string.Empty;
        private string _lastExportPath = string.Empty;
        private const string Namespace = "WpfAppDemo";

        public ObservableCollection<string> Tables { get; } = new ObservableCollection<string>();
        public ObservableCollection<GeneratedFile> GeneratedFiles { get; } = new ObservableCollection<GeneratedFile>();

        public string UsageInstructions
        {
            get => _usageInstructions;
            set => SetProperty(ref _usageInstructions, value);
        }

        public string LastExportPath
        {
            get => _lastExportPath;
            set => SetProperty(ref _lastExportPath, value);
        }

        public string? SelectedTable
        {
            get => _selectedTable;
            set
            {
                if (SetProperty(ref _selectedTable, value))
                {
                    UpdateGeneratedContent();
                    UpdateUsageInstructions();
                    LastExportPath = string.Empty;
                }
            }
        }

        public DelegateCommand SaveToDiskCommand { get; }
        public DelegateCommand OpenExportFolderCommand { get; }
        public DelegateCommand<string> CopyTextCommand { get; }

        public CodeGenViewModel(ICodeGenService codeGenService)
        {
            _codeGenService = codeGenService;
            SaveToDiskCommand = new DelegateCommand(OnSaveToDisk);
            OpenExportFolderCommand = new DelegateCommand(OnOpenExportFolder);
            CopyTextCommand = new DelegateCommand<string>(OnCopyText);
            
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(LoadTables));
        }

        private void OnOpenExportFolder()
        {
            if (string.IsNullOrEmpty(LastExportPath)) return;
            try
            {
                if (Directory.Exists(LastExportPath))
                {
                    System.Diagnostics.Process.Start("explorer.exe", LastExportPath);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"无法打开目录: {ex.Message}");
            }
        }

        private void OnSaveToDisk()
        {
            if (!GeneratedFiles.Any()) return;

            try 
            {
                string projectDir = Directory.GetCurrentDirectory();
                string outputDir = Path.Combine(projectDir, "GeneratedCode", SelectedTable ?? "Unknown");

                if (Directory.Exists(outputDir)) Directory.Delete(outputDir, true);
                Directory.CreateDirectory(outputDir);

                foreach (var file in GeneratedFiles)
                {
                    string filePath = Path.Combine(outputDir, file.Path.Replace("/", "\\"));
                    string? dirPath = Path.GetDirectoryName(filePath);
                    if (dirPath != null) Directory.CreateDirectory(dirPath); 
                    
                    File.WriteAllText(filePath, file.Content, Encoding.UTF8);
                }

                LastExportPath = outputDir;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"保存失败: {ex.Message}", "错误");
            }
        }

        private void OnLoadTables()
        {
            try
            {
                var tables = _codeGenService.GetTableNames();
                Tables.Clear();
                foreach (var table in tables) Tables.Add(table);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "CodeGen: Failed to load tables");
            }
        }

        private void LoadTables()
        {
            OnLoadTables();
        }

        private void UpdateUsageInstructions()
        {
            if (string.IsNullOrEmpty(SelectedTable))
            {
                UsageInstructions = "请先选择一个数据库表以生成集成说明。";
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("# 集成步骤指引");
            sb.AppendLine();
            sb.AppendLine("## 1. 放置文件");
            sb.AppendLine($"点击上方按钮将代码导出到 `GeneratedCode` 目录，然后将文件夹内的内容复制到项目对应的 `Models`, `Services`, `ViewModels`, `Views` 文件夹中。");
            sb.AppendLine();
            sb.AppendLine("## 2. 注册服务与页面 (修改 App.xaml.cs)");
            sb.AppendLine("在 `RegisterTypes` 方法中添加以下代码：");
            sb.AppendLine("```csharp");
            sb.AppendLine($"// 注册 Service");
            sb.AppendLine($"containerRegistry.RegisterSingleton<I{SelectedTable}Service, {SelectedTable}Service>();");
            sb.AppendLine();
            sb.AppendLine($"// 注册导航视图");
            sb.AppendLine($"containerRegistry.RegisterForNavigation<{SelectedTable}ListView>();");
            sb.AppendLine($"containerRegistry.RegisterForNavigation<{SelectedTable}EditView>();");
            sb.AppendLine("```");
            sb.AppendLine();
            sb.AppendLine("## 3. 添加菜单项 (修改 MainWindowViewModel.cs)");
            sb.AppendLine("在 `InitializeMenuItems` 方法中添加：");
            sb.AppendLine("```csharp");
            sb.AppendLine($"// 建议先在 Language.zh-CN.xaml 中添加资源键");
            sb.AppendLine($"MenuItems.Add(new MenuItem(\"Menu_{SelectedTable}\", \"Database\", \"{SelectedTable}ListView\"));");
            sb.AppendLine("```");
            sb.AppendLine();
            sb.AppendLine("## 4. 数据库提示");
            sb.AppendLine($"- 此代码根据现有表结构生成。请确保数据库中已存在 `{SelectedTable}` 表。");

            UsageInstructions = sb.ToString();
        }

        private void UpdateGeneratedContent()
        {
            if (string.IsNullOrEmpty(SelectedTable))
            {
                GeneratedFiles.Clear();
                return;
            }

            try
            {
                var columns = _codeGenService.GetColumnInfos(SelectedTable);
                string camelTable = char.ToLower(SelectedTable[0]) + SelectedTable.Substring(1);

                GeneratedFiles.Clear();

                var fileConfigs = new List<(string Name, string Path, string Template)>
                {
                    ("Model", $"Models/{SelectedTable}.cs", "Model.txt"),
                    ("IService", $"Services/I{SelectedTable}Service.cs", "IService.txt"),
                    ("Service", $"Services/{SelectedTable}Service.cs", "Service.txt"),
                    ("List VM", $"ViewModels/{SelectedTable}ListViewModel.cs", "ListViewModel.txt"),
                    ("Edit VM", $"ViewModels/{SelectedTable}EditViewModel.cs", "EditViewModel.txt"),
                    ("List View", $"Views/{SelectedTable}ListView.xaml", "ListView.txt"),
                    ("List View CS", $"Views/{SelectedTable}ListView.xaml.cs", "ListViewCs.txt"),
                    ("Edit View", $"Views/{SelectedTable}EditView.xaml", "EditView.txt"),
                    ("Edit View CS", $"Views/{SelectedTable}EditView.xaml.cs", "EditViewCs.txt")
                };

                foreach (var f in fileConfigs)
                {
                    GeneratedFiles.Add(new GeneratedFile {
                        Name = f.Name,
                        Path = f.Path,
                        Content = ReplaceTemplate(f.Template, SelectedTable, camelTable, columns)
                    });
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "CodeGen: Failed to generate content");
            }
        }

        private void OnCopyText(string text)
        {
            if (!string.IsNullOrEmpty(text)) System.Windows.Clipboard.SetText(text);
        }

        private string ReplaceTemplate(string fileName, string tableName, string camelTableName, List<SqlSugar.DbColumnInfo> columns)
        {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", fileName);
            if (!File.Exists(templatePath)) templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", fileName);
            if (!File.Exists(templatePath)) return $"Template not found: {fileName}";

            string content = File.ReadAllText(templatePath);
            content = content.Replace("{TableName}", tableName)
                             .Replace("{camelTableName}", camelTableName)
                             .Replace("{Namespace}", Namespace);

            if (fileName == "Model.txt")
            {
                var props = new StringBuilder();
                foreach (var col in columns)
                {
                    if (col.IsPrimarykey) 
                    {
                        props.AppendLine("        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]");
                    }
                    string type = MapType(col.DataType);
                    string init = type == "string" ? " = string.Empty;" : "";
                    props.AppendLine($"        public {type} {col.DbColumnName} {{ get; set; }}{init}");
                }
                content = content.Replace("{Properties}", props.ToString());
            }
            else if (fileName == "Service.txt")
            {
                string searchField = "Id"; 
                if (columns.Count >= 2)
                {
                    searchField = columns[1].DbColumnName;
                }
                else if (columns.Count >= 1)
                {
                    searchField = columns[0].DbColumnName;
                }

                var targetCol = columns.FirstOrDefault(c => c.DbColumnName == searchField);
                string searchLogic;
                if (targetCol != null && MapType(targetCol.DataType) == "string")
                {
                    searchLogic = $"it.{searchField}.Contains(keyword!)";
                }
                else
                {
                    searchLogic = $"it.{searchField}.ToString().Contains(keyword!)";
                }

                content = content.Replace("{SearchLogic}", searchLogic);
            }
            else if (fileName == "ListView.txt")
            {
                var cols = new StringBuilder();
                foreach (var col in columns)
                {
                    cols.AppendLine($"                <DataGridTextColumn Header=\"{col.DbColumnName}\" Binding=\"{{Binding {col.DbColumnName}}}\" Width=\"Auto\">");
                    cols.AppendLine("                    <DataGridTextColumn.ElementStyle>");
                    cols.AppendLine("                        <Style TargetType=\"TextBlock\">");
                    cols.AppendLine("                            <Setter Property=\"HorizontalAlignment\" Value=\"Center\"/>");
                    cols.AppendLine("                            <Setter Property=\"VerticalAlignment\" Value=\"Center\"/>");
                    cols.AppendLine("                        </Style>");
                    cols.AppendLine("                    </DataGridTextColumn.ElementStyle>");
                    cols.AppendLine("                </DataGridTextColumn>");
                }
                content = content.Replace("{Columns}", cols.ToString());
            }
            else if (fileName == "EditView.txt")
            {
                var controls = new StringBuilder();
                foreach (var col in columns)
                {
                    if (col.IsIdentity || col.IsPrimarykey) continue;
                    
                    controls.AppendLine($"                <TextBox materialDesign:HintAssist.Hint=\"{col.DbColumnName}\" Text=\"{{Binding {col.DbColumnName}}}\" Margin=\"0,8\" Style=\"{{StaticResource MaterialDesignFloatingHintTextBox}}\"/>");
                }
                content = content.Replace("{EditControls}", controls.ToString());
            }

            return content;
        }

        private string MapType(string dbType)
        {
            dbType = dbType.ToLower();
            if (dbType.Contains("int") || dbType.Contains("integer")) return "int";
            if (dbType.Contains("varchar") || dbType.Contains("text") || dbType.Contains("string")) return "string";
            if (dbType.Contains("datetime") || dbType.Contains("date")) return "DateTime";
            if (dbType.Contains("bit") || dbType.Contains("bool")) return "bool";
            return "string";
        }
    }
}
