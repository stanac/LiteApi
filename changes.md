2.0.0 (more info at [blog post](liteapi.net/blog/2018-05-20/release-2_0_0))
- Dropped support for netstandard < 2.0
- Attributes are no longer in LiteApi.Attributes namespace, they are in LiteApi namespace
- Fixed XML documentation issue where IntelliSense didn't show documentation
- Json.NET dependency updated to 11.0.2
- Discovery of controllers, actions and parameters
- `Action<LiteApi.LiteApiOptions>` override for middleware registration 

1.0.1
- Nuget package fix

1.0.0
- Stable release 

0.9.1
- Nuget package fix

0.9.0
- Documentation and website
- Global filters
- Configuration to replace global API root route (replace "api" in URLs with custom string)
- Support for both .NET Standard 1.6 and 2.0

0.8.0
-  Option to return raw JSON content from string and to set response code
-  Extensibility point to enable replacing any internal service in the middleware
-  Force HTTPS attribute in global config
-  Support action parameters from request header

0.7.4
- Fixes
  - Fix .nuspec

0.7.3
- Changes
  - Update Json.NET dependency to 10.0.2
  - Rename RestfulLinksAttribute to RestfulAttribute
  - Rename FromUrlAttribute to FromQueryAttribute

0.7.2
- Fixes
  - Route parameters are not suppose to be case sensitive
  - When initialization fails it's not possible to see what happen on deceloper exception page

0.7.1
- Fixes in
  - Route segment binder
  - Collection query binder
  - Action overloading

0.7.0
- Support for action parameters from IServiceProvider (dependency injection on action level)
- Support for file uploading (streams)
- Support for file download (streams and byte arrays)

0.6.0 
- Fixed issues with overloading actions with nullable arguments and generics
- Added support for restful links (RestfulLinksAttribute on controller level)

0.5.1 
- Add validation for missing authorization policies

0.5.0 
- New way of authorization

0.4.0 
- Updated route mapping and support for route segment parameters (check wiki)

0.3.0 
- Fixed and refactored AuthorizeFilterAttribute

0.2.0 
- Support for URL parameters of types List, Array, IEnumerable, Dictionary and IDictionary

0.1.1 
- Fix nuget package

0.1.0 
- Initial release
