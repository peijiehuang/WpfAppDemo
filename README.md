# WPF Prism 快速开发通用模板

这是一个基于 **.NET 8**、**Prism (DryIoc)**、**Material Design in XAML** 和 **SqlSugar** 构建的高级 WPF 通用模板。它旨在为您提供一个生产就绪、颜值极高且易于扩展的企业级桌面应用起点。

---

## 🌟 核心特性

- **现代视觉设计**：基于 Material Design 3 风格，支持**深浅主题一键切换**，具有沉浸式侧边栏和极简主义生产力 UI。
- **模块化架构**：使用 Prism 框架实现 MVVM 模式，支持各组件间的松耦合导航与事件通信。
- **持久化方案**：集成 SqlSugar ORM，内置 SQLite 数据库，支持自动分表、AOP 审计等高级功能。
- **国际化 (i18n)**：预设中英文本地化资源，支持运行时动态切换语言而无需重启应用。
- **配置系统**：集成 `appsettings.json` 外部配置，轻松管理数据库连接字符串及各环境参数。
- **稳健性保障**：内置 Serilog 结构化日志系统及全局异常拦截机制，确保应用 7x24 稳定运行。
- **数据验证**：完美支持 `IDataErrorInfo` 接口，提供实时的 UI 校验反馈。

---

## 🚀 快速上手

### 1. 环境准备
- 安装 [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (建议 v17.8+)
- 安装 [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- 工作负载勾选：`.NET 桌面开发`

### 2. 运行项目
1. 克隆或下载本项目至本地。
2. 使用 Visual Studio 打开 `.sln` 解决方案文件。
3. 还原 NuGet 软件包。
4. 按 `F5` 启动项目。

> [!TIP]
> **默认账号**: `admin`  
> **默认密码**: `admin`  
> (首次运行会自动创建数据库并初始化该管理账号)

---

## 🛠 开发指南

### 1. 如何添加一个新页面 (View & ViewModel)
1. **创建 View**：在 `Views/` 目录下创建 `UserControl`（例如 `SettingsView.xaml`）。
2. **创建 ViewModel**：在 `ViewModels/` 目录下创建关联类 `SettingsViewModel.cs`，继承自 `BindableBase`。
3. **注册页面**：在 `App.xaml.cs` 的 `RegisterTypes` 方法中添加：
   ```csharp
   containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
   ```
4. **添加到菜单**：在 `MainWindowViewModel.cs` 的 `InitializeMenuItems` 中加入新菜单项：
   ```csharp
   MenuItems.Add(new MenuItem { 
       Title = "Settings", 
       Icon = "Cog", 
       NavigationPath = nameof(SettingsView) 
   });
   ```

### 2. 如何添加新服务 (Service)
1. **定义接口**：在 `Services/` 定义接口 `IDataService`。
2. **实现接口**：创建 `DataService` 类。
3. **注入容器**：在 `App.xaml.cs` 的 `RegisterTypes` 中注册单例或瞬态实例：
   ```csharp
   containerRegistry.RegisterSingleton<IDataService, DataService>();
   ```

### 3. 如何增加数据库表
1. **定义模型**：在 `Models/` 目录下创建实体类（例如 `Product.cs`），使用 `[SugarTable]` 标记。
2. **使用 ORM**：在 Service 层注入 `SqlSugarClient` 即可进行 CRUD 操作。本项目已在 `UserService` 提供了标准参考。

---

## 📁 目录结构

- `Common/`：通用转换器、扩展方法及全局异常拦截。
- `Models/`：领域实体、SqlSugar 配置及数据校验逻辑。
- `Resources/`：XAML 样式资源及多语言本地化字典。
- `Services/`：核心业务逻辑服务、主题管理及弹窗服务。
- `ViewModels/`：页面交互逻辑（Prism 核心）。
- `Views/`：XAML 界面定义（Material Design 核心）。
- `appsettings.json`：外部应用配置中心。

---

## 📦 技术栈详情

| 组件 | 说明 |
| :--- | :--- |
| **Prism.DryIoc** | MVVM 框架、依赖注入、区域管理、导航 |
| **MaterialDesignThemes** | Google Material Design 设计风格控件库 |
| **SqlSugarCore** | 极简、高性能的内生国产 ORM 框架 |
| **Serilog** | 高性能结构化诊断日志库 |
| **Microsoft.Extensions.Configuration** | 标准化 JSON 配置文件读取 |

---
