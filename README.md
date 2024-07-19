# 打包命令
```bash
dotnet restore -r linux-x64
dotnet msbuild AvaloniaUos.csproj /t:CreateDebUOS /p:RuntimeIdentifier=linux-x64 /p:Configuration=Release /p:SelfContained=true
```