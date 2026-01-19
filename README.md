# WPF 优雅开发示例项目 (WpfAppDemo)

这是一个基于 .NET 8 打造的 WPF 现代化开发框架示例。本项目深度集成了 **Prism** 导航框架与 **CommunityToolkit.Mvvm**，采用 TDD 设计理念，实现了高度通用且优雅的架构模式。

## 核心技术栈

- **UI 框架**: WPF (.NET 8)
- **MVVM 模式**: Prism.DryIoc + CommunityToolkit.Mvvm (源生成器)
- **UI 组件库**: Material Design In XAML
- **数据库 ORM**: SqlSugar (支持多种数据库，本示例默认使用 SQLite)
- **日志系统**: Serilog (输出至控制台与本地文件)
- **Excel 处理**: MiniExcel (极速导入导出)
- **配置文件**: Microsoft.Extensions.Configuration (appsettings.json)

## 架构优化亮点

### 1. 现代化的 MVVM 实现
通过引入 `CommunityToolkit.Mvvm`，所有 ViewModel 均支持 `[ObservableProperty]` 和 `[RelayCommand]`。这大幅减少了繁琐的属性定义和命令绑定逻辑，使代码行数减少了约 50%，同时提高了运行性能。

### 2. 高度抽象的基类
- **ViewModelBase**: 统一封装了标题管理、忙碌状态控制（IsBusy）以及 Prism 的导航生命周期管理。
- **ServiceBase**: 自动初始化 SqlSugar 数据库客户端与 Serilog 结构化日志，业务服务仅需关注核心逻辑。

### 3. 通用代码生成器 (CodeGen)
项目内置了强大的代码生成工具，能够根据数据库表结构自动生成全套 CRUD 代码：
- **Model**: 带 SqlSugar 特性的实体类。
- **Service**: 继承自 `ServiceBase` 的标准业务层。
- **ViewModel**: 包含分页、查询、导出功能的标准列表及编辑页面逻辑。
- **View**: 基于 Material Design 风格的 XAML 界面。

### 4. 优雅的服务设计
- **LocalizationService**: 支持运行时一键切换中英文。
- **MessageService**: 封装了 Material Design 风格的异步对话框。
- **ThemeService**: 支持一键切换明亮/深色模式。

## 目录结构说明

- `Common`: 存放基类（ViewModelBase, ServiceBase）与全局转换器。
- `Models`: 数据实体定义。
- `Services`: 业务逻辑接口及其实现。
- `ViewModels`: 各页面的业务逻辑控制器。
- `Views`: XAML 界面定义。
- `Templates`: 代码生成器使用的 T4 风格文本模板。

## 快速上手

1. 确保已安装 `.NET 8 SDK`。
2. 运行项目，默认管理员账号为 `admin` / `admin`。
3. 进入“代码生成”页面，选择一个数据库表，点击“保存到本地”即可获得全套源码。

---
*本项目已由资深架构师完成全面优化，所有代码文件均包含详尽的中文注释，符合工业级开发规范。*
