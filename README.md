# SvcService

服务管理

## 配置

程序的包含如下主要配置，位于`application.yaml`中:

```yaml
Dbms: mysql # 活动的connectionstring配置
MYSQL_IP: localhost # mysql的主机，需要开放3306
MYSQL_UID: mcsdbg # mysql用户名
MYSQL_PWD: MyWceQc-cFgPynao # mysql密码
LISTER_PORT: 80 # 工作端口
```

**可以直接使用环境变量来覆盖这些配置。**

`appsettings.json`是基本配置文件，一般无需修改。

## 构建

使用docker完成构建。

```bash
docker build -t <img>:<tag> .
```

## 数据库

建库脚本位于`svc-service.sql`, 可以使用`dotnet ef migrations script -o svc-service.sql`来导出建库脚本。**然而，建库是自动完成的。**

数据库名称为`svcservice`，可以在`appsettings.json`的连接字符串里修改

### ServiceEntity

字段和长度/精度说明如下：

|字段|长度/精度|备注|
|:---:|:---:|:---:|
|Id|32||
|Repo|128||
|VersionMajor/Minor|16||
|VersionPatch|32||
|Res|decimal(16,4)|任一资源|

### InterfaceEntity

字段和长度/精度说明如下：

|字段|长度/精度|备注|
|:---:|:---:|:---:|
|ServiceId|32|FK|
|IdSuffix|32||
|Path|64||
|OutputSize|50||

## 目录结构

```text
├─📂Data EF Core实体模型定义
├─📂Migrations 迁移, EFCore自动生成
├─📂Properties
│   └─🗒️launchSettings.json 启动配置│
├─📂TestData 接口测试数据json
│
├─🗒️application.yaml 主要配置文件
├─🗒️appsettings.json 基本配置文件
├─🗒️appsettings.Development.json 基本配置文件(dev)
├─🗒️Dockerfile (自动生成)
├─🗒️Program.cs 程序入口（最小API），包含**请求处理、Bean定义和依赖注入**
├─🗒️README.md 本文件
├─🗒️SvcService.csproj 项目文件(自动生成)，包含**依赖项**
├─🗒️SvcService.sln 解决方案文件(自动生成)
└─🗒️svc-service.sql 建库脚本
```