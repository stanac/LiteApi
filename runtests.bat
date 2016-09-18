nuget restore -PackagesDirectory "nugettools"

cd LiteApi\LiteApi.Tests

dotnet build

..\..\nugettools\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user -target:"C:/Program Files/dotnet/dotnet.exe" -targetargs:"test" -output:"..\..\coverageopencover.xml" -oldstyle -filter:"+[LiteApi]*"

cd ..

cd ..