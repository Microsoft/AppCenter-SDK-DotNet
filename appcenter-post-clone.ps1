echo "Executing post clone script in $pwd"

$key = "VSTSAC"
$value = "https://msmobilecenter.pkgs.visualstudio.com/_packaging/AppCenter/nuget/v3/index.json"
dotnet nuget remove source "$key"
dotnet nuget add source "$value" -n "$key" -u "mobilecenter" -p "$env:NUGET_PASSWORD"
