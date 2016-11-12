If (Test-Path "BuildOutput"){
    Remove-Item "BuildOutput" -recurse
}

New-Item "BuildOutput" -type directory

cd "BuildOutput"

New-Item "netstandard1.6" -type directory

cd ..

cd LiteApi

cd LiteApi

If (Test-Path "bin"){
    Remove-Item "bin" -recurse
}

If (Test-Path "obj"){
    Remove-Item "obj" -recurse
}

dotnet build -c Release -o "..\..\BuildOutput\netstandard1.6" -f "netstandard1.6"

cd ..

cd ..

cmd.exe /c "nuget pack LiteApi.nuspec"