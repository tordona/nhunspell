Write-Host "  Stopping vshost processes"
Stop-Process -Name CSharpConsoleSamples.vshost -ErrorAction Ignore
Stop-Process -Name VisualBasicConsoleSampels.vshost -ErrorAction Ignore


Write-Host "  Removing bin and obj directories"
remove-item CSharpConsoleSamples\bin -Recurse -Force -ErrorAction Ignore
remove-item CSharpConsoleSamples\obj -Recurse -Force -ErrorAction Ignore

remove-item VisualBasicConsoleSampels\bin -Recurse -Force -ErrorAction Ignore
remove-item VisualBasicConsoleSampels\obj -Recurse -Force -ErrorAction Ignore

remove-item WebSampleApplication\bin -Recurse -Force -ErrorAction Ignore
remove-item WebSampleApplication\obj -Recurse -Force -ErrorAction Ignore


Write-Host "  Zipping solution"
$packageZipFile = $context.SolutionArtifactsLocation + "\NHunspellSamples." + $context.PackageVersion + ".zip"
Get-ChildItem CSharpConsoleSamples -Recurse | Export-Zip $packageZipFile | Out-Null
Get-ChildItem VisualBasicConsoleSampels -Recurse | Export-Zip $packageZipFile -Append | Out-Null
Get-ChildItem WebSampleApplication -Recurse | Export-Zip $packageZipFile -Append | Out-Null
Get-ChildItem packages -Recurse | Export-Zip $packageZipFile -Append | Out-Null
Export-Zip $packageZipFile -EntryFile NHunspellSamples.sln -Append | Out-Null