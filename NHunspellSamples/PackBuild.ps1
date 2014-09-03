Stop-Process -Name CSharpConsoleSamples.vshost -ErrorAction Ignore
remove-item CSharpConsoleSamples\bin -Recurse -Force -ErrorAction Ignore
remove-item CSharpConsoleSamples\obj -Recurse -Force -ErrorAction Ignore

Stop-Process -Name VisualBasicConsoleSampels.vshost -ErrorAction Ignore
remove-item VisualBasicConsoleSampels\bin -Recurse -Force -ErrorAction Ignore
remove-item VisualBasicConsoleSampels\obj -Recurse -Force -ErrorAction Ignore

remove-item WebSampleApplication\bin -Recurse -Force -ErrorAction Ignore
remove-item WebSampleApplication\obj -Recurse -Force -ErrorAction Ignore


$packageZipFile = $context.SolutionArtifactsLocation + "\NHunspellSamples." + $context.PackageVersion + ".zip"

Get-ChildItem CSharpConsoleSamples -Recurse | Export-Zip $packageZipFile
Get-ChildItem VisualBasicConsoleSampels -Recurse | Export-Zip $packageZipFile -Append
Get-ChildItem WebSampleApplication -Recurse | Export-Zip $packageZipFile -Append
Get-ChildItem packages -Recurse | Export-Zip $packageZipFile -Append
Export-Zip $packageZipFile -EntryFile NHunspellSamples.sln -Append