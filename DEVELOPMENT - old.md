# 开发者文档

本文档旨在介绍本项目的开发事宜，统一开发行为，并减缓代码 “腐烂” 的速度。

> [!NOTE]
>
> 此文档在不断编写中......
>
> 如果对于编码中的一些细节感到不确定，可以参考项目中已有代码的实现。

## 先决条件

项目使用的 IDE 是 Visual Studio 2026（或以后版本），SDK 是 .NET 10（您应安装相应的组件）。

您第一次打开解决方案时，默认的启动项目可能是 `SWTools.Core`，您需要右键 `SWTools.WPF` 并将其设置为启动项目。

> `SWTools.Core` 的生成目标是 “类库”，这意味着它不能被启动。如果您需要单独测试 `SWTools.Core`，请修改其生成目标。

### 代码格式

项目采用统一的代码格式，这些格式在 `.editorconfig` 中定义，您的 IDE 应该能正确识别它。

## 项目结构、托管 API

本项目项目遵循 MVVM（Model-ViewModel-View）架构，并拆分成三个子项目：

- **SWTools.Core** *（生成类型：类库）*

  这是核心代码，即 “Model” 部分，是程序的后端。

- **SWTools.ViewModel** *（生成类型：类库）*

  实现交互逻辑，构建前端和后端的桥梁，为前端 XAML 提供绑定。除了一些简单的窗口，每个 SWTools.WPF 中的窗口在这里都有对应的 ViewModel。这些对应的 ViewModel 作为窗口的 DataContext 使用。

  此项目引用 SWTools.Core。

- **SWTools.WPF** *（生成类型：Windows 应用程序）*

  负责实现前端，即 “View” 部分，包含少量交互逻辑代码。应该把复杂的交互逻辑放在 SWTools.ViewModel 中实现，然后由 SWTools.WPF 调用。

  此项目引用 SWTools.Core、SWTools.ViewModel。

> [!TIP]
>
> 为了防止 UI 线程被阻塞（这会使得程序看上去卡住了），需要一定时间完成的操作（例如下载）请在 SWTools.WPF 中以异步调用这些方法。
>
> 具体来说，需要搭配使用 C# 的 `async` `await` 语法。

每个子项目可能建立子文件夹以进一步管理文件。每个子文件夹代表一级命名空间。例如，`SWTools.Core/API` 对应的命名空间是 `SWTools.Core.API`。

> [!NOTE]
>
> 这些子项目层层依赖。比如 SWTools.Core 不能调用 SWTools.ViewModel、SWTools.WPF。
>
> 这样做实现了模块之间的解耦，有利于把 bug 控制在最小范围内；也有利于单元测试，比如单独测试 Core。注意：由于 Core 的生成类型为 “类库”，不能直接启动 Core。应该先修改其项目的生成类型为 “控制台应用程序”，再进行测试。发布前记得改回来。

例如，在 SWTools.WPF 中可以通过 `Core.Constants.Version` 来访问 SWTools.Core 项目下的内容。不推荐在文件中写 `using SWTools.Core` 直接引用命名空间。

### 托管的 API

除此之外，还有部分信息作为 API 并托管在仓库内。这样设计是为了提高更新的灵活性，即通过修改 API 实现功能更新，而不是发布新的发行版。下面列出托管的API：

- `api/latest_info`：包含了关于仓库最新动态的信息。包括最新的发行版、预发行版等信息。
- `api/pub_accounts`：最新的公共账户池。

这些文件没有后缀名，按照 JSON 文件格式编写。

## 版本管理、开发-发布流程

本项目使用 [语义化版本](https://semver.org/lang/zh-CN/) 来为发行版编号。下面举例说明：

- `1.0.0-alpha.1`：Alpha 阶段，即内部测试阶段。（不常用）
- `1.0.0-beta.1`：Beta 阶段，即公开测试阶段。此阶段的目的是发现程序中潜在的 bug，并补充一些功能。
- `1.0.0-rc.1`：预发布阶段，不会添加新的功能。此阶段的目的是尽可能发现程序的 bug。（不常用）
- `1.0.0`：正式版本，已经迭代稳定。

前缀 `x.y.z` 递增的逻辑如下：

- `1.0.1`：发现 bug 或其他不完善的地方，做微小修订。
- `1.1.0`：增加新的功能。
- `2.0.0`：发生重大技术变动，或者不能支持旧版本（例如 api 不兼容）

### 发布前事项

发布前需要修改程序的版本信息。该信息存储在以下两个地方：

- `SWTools.Core.Constants.Version` 字段。此信息将在程序内供各处逻辑使用。

  ```csharp
  /* 版本 */
  public static readonly SemVersion Version = SemVersion.Parse("2.0.0-beta.3");
  ```

- `SWTools.WPF` 项目的 `.csproj` 文件。可以在 Visual Studio 中双击项目打开。此信息将在生成的 `SWTools.WPF.exe` 的详细信息中展示。（即程序集信息）

  ```xaml
  <!--版本-->
  <Version>2.0.0-beta.3</Version>
  ```

同时还要修改 `api/latest_info` 中最新版本信息：

```json
{
    ...
    "PreRelease": "2.0.0-beta.3",
    ...
}
```

### 构建发布包

发布前请确认，`SWTools.Core` 的 “输出类型” 已设为 “类库”。然后按照以下步骤构建发布包：

- 把解决方案配置改为 “Release”。

- 执行 “重新生成解决方案”（`Ctrl+Alt+F7`）。
- 删除生成目录（如 `\bin\Release\net10.0-windows7.0\`）下的所有 `.pdb` 文件。这些文件是调试符号，不应包含在发行版中。
- 添加必要的 `LICENSE.txt`、`THIRD-PARTY-NOTICE.md`（位于项目根目录下）。
- 将剩余文件压缩，然后即可在 Github 上发布。

## SWTools.Core

此项目负责后端主要逻辑。

### 子命名空间

- `Core.Helper`：包含了一些辅助方法，在整个项目各处都有使用。
- `Core.API`：封装所有的 API。每个 API 都至少有一个 `Request()` 方法用于请求 API，并返回一个 `API.xxx.Response` 类型的回复包。
- `Core.Cache`：封装所有的缓存逻辑。

### 静态访问点

一些后端功能以静态访问点的形式提供。整个项目统一使用这些访问点。

- 日志器：`Core.LogManager.Log`

- 应用配置：`Core.ConfigManager.Config`

- （用于下载的）账户池管理器：`Core.AccountsManager`

- 常量：`Core.Constants`


> [!NOTE]
>
> 对于程序中需要应用的常量，例如目录、网址等，请考虑：
>
> - 此常量是否可能会在多处用到？
> - 此常量是否可能在未来的版本中修改？
>
> 如果满足任一条件，请将此量放在 `Core.Constants` 内。

### 核心逻辑

物品的解析、下载核心逻辑集中在 `Core.Item` 和 `Core.ItemList` 中。以下是部分核心方法：

- **单物品下载**：`Core.Item.Download()`
- 单物品解析：`Core.Item.Parse()`
- **批量解析**：`Core.ItemList.ParseAll()`

`Core.Item` 表示一个创意工坊物品，存储了物品的相关信息，并提供相关方法。

`Core.ItemList` 继承自 `List<Item>`，在原有基础上提供了更多功能，比如按 `ItemId` 检索物品、JSON 读取和存储等。如果需要存储多个物品的信息，请优先考虑使用 `ItemList` 而不是 `List<Item>`。

### 创意工坊物品唯一标识符

`Core.Item.ItemId` 作为创意工坊物品的唯一标识符使用。这就是说：

- 比较两个物品是否相同时，请比较 `Item.ItemId` 而不是 `Item` 本身。
- 查询物品时，应该用 `ItemList.Find(Item.ItemId)`。
- 其他需要区分两个物品的情况，也尽量使用 `Item.ItemId`。

## SWTools.WPF

此项目负责实现前端。

### 画刷资源

在 `App.xaml` 中定义了几个广泛使用的画刷，供调用。

如果以后要添加某些多处使用的画刷，请考虑在这里定义。

### 自定义的控件、窗口

- **SWTools.WPF.Controls.IconButton**

  一个带图标的按钮控件。用 `Text` 属性来指定按钮文本，用 `Icon` 属性来指定其图标。**SWTools.WPF.Controls.IconButtonAccent** 是颜色加深的强调变种。

- **SWTools.WPF.MsgBox**

  自定义的对话框窗口。