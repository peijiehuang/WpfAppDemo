# WPF Prism 快速开发通用模板

这是一个基于 **.NET 8**、**Prism (DryIoc)**、**Material Design in XAML** 和 **SqlSugar** 构建的高级 WPF 通用模板。它旨在为您提供一个生产就绪、颜值极高且内置 **代码生成器** 的企业级桌面应用起点。

---

## 🌟 核心特性

- **🚀 生产力利器：内置代码生成器**
  - **基于数据库驱动**：选定现有数据库表，一键生成 Model、Service、ViewModel 和 View。
  - **全套 CRUD 逻辑**：自动生成包含查询、新增、编辑、删除、搜索在内的完整业务闭环。
  - **一键本地导出**：支持代码实时预览及物理文件导出，并提供详细的集成指引与目录结构保持。
- **🎨 现代视觉设计**
  - **极简卡片式布局**：列表页采用统一卡片容器，标题与数据展示无缝衔接。
  - **完全居中对齐**：表格数据经过精准样式控制，实现水平与垂直全方位居中。
  - **交互增强**：支持无边框登录窗口自由拖拽，平滑的 Material Design 动效。
- **💬 现代化消息系统**
  - **MD 对话框**：弃用原生 MessageBox，采用基于 `DialogHost` 的 Material Design 风格对话框。
  - **异步等待支持**：全局 `IMessageService` 支持 Task 异步调用，交互流畅不卡顿。
- **🌍 深度国际化 (i18n)**
  - **动态切换**：支持中英文运行时实时切换，连弹窗标题和提示信息也完美适配。
  - **零崩溃保护**：增强型 `ResourceStringConverter`，即使缺失资源键也能优雅降级显示。
- **🏗 稳健架构**
  - **模块化导航**：基于 Prism 区域管理器实现松耦合页面跳转。
  - **数据持久化**：集成 SqlSugar ORM，内置 SQLite，支持搜索关键字自动分析与多字段模糊查询。

---

## ⚡ 快速开发流程 (使用生成器)

1. **连接数据库**：在 `appsettings.json` 中配置您的 SQLite 或其他数据库连接。
2. **启用生成器**：确保 `appsettings.json` 中的 `"EnableCodeGen": true`。
3. **选择表**：在应用侧边栏进入“代码生成器”，选择目标表。
4. **导出与集成**：
   - 点击“导出所有代码文件到本地”。
   - 点击“打开文件夹”获取源文件。
   - 按照页面上的 **“使用说明”** 页签，复制几行注册代码至 `App.xaml.cs`。
5. **即刻运行**：重启应用，全新的业务模块即可投入使用。

---

## 🛠 技术栈

| 组件 | 说明 |
| :--- | :--- |
| **Prism.DryIoc** | MVVM 框架、依赖注入、区域管理、导航系统 |
| **MaterialDesignThemes** | 顶级的 Material Design 设计风格 UI 控件库 |
| **SqlSugarCore** | 极简、高性能的 ORM 框架，支持代码生成与 AOP |
| **Serilog** | 结构化诊断日志库，支持文件与控制台多输出 |
| **Microsoft.Extensions.Configuration** | 标准化 JSON 配置文件管理系统 |

---

## 🚀 快速上手

### 1. 环境准备
- 安装 [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (建议 v17.8+)
- 安装 [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### 2. 运行项目
1. 下载本项目，使用 Visual Studio 打开 `WpfAppDemo.sln`。
2. 还原 NuGet 包并按 `F5` 启动。
3. **默认账号/密码**: `admin` / `admin`。

---

## 📂 目录结构简述

- `CodeGen/`：代码生成器核心逻辑与服务。
- `Templates/`：代码生成器使用的各种 `.txt` 源码模板。
- `Resources/`：集中管理的主题样式与多语言字典。
- `Services/`：包含 MessageService (MD风格弹窗)、ThemeService 等通用服务。
- `ViewModels/` & `Views/`：遵循 Prism 命名约定的视图模型与 XAML 界面。

---