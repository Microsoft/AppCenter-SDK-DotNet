#!/usr/bin/env bash

# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

echo "Executing post clone script in $pwd"

$nugetFileName = "NuGet.config"
 $contentValue = "<?xml version=""1.0"" encoding=""utf-8""?>`n" +
 "<configuration>`n" +
 "  <packageSources>`n" +
 "    <add key=""VSTSAC"" value=""https://msmobilecenter.pkgs.visualstudio.com/_packaging/AppCenter/nuget/v3/index.json"" />`n" +
 "  </packageSources>`n" +
 "  <activePackageSource>`n" +
 "    <add key=""All"" value=""(Aggregate source)"" />`n" +
 "  </activePackageSource>`n" +
 "  <packageSourceCredentials>`n" +
 "    <VSTSAC>`n" +
 "      <add key=""Username"" value=""mobilecenter"" />`n" +
 "      <add key=""ClearTextPassword"" value=""$env:NUGET_PASSWORD"" />`n" +
 "    </VSTSAC>`n" +
 "  </packageSourceCredentials>`n" +
 "</configuration>`n"

 if (Test-Path $nugetFileName) {
   Remove-Item $nugetFileName
 }
 New-Item -Path . -Name $nugetFileName -Value $contentValue 
