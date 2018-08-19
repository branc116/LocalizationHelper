# LocalizationHelper
Super awesome localization helper for helping with resx localization files. IStringLocalizer, IViewLocalizer, ... 
Use this tool to generate Resx files from you'r c# project

Use it like this:


     dotnet .\LocalizationHelper.dll csharProjectFolder ResxOutFolder Languages[] 

E.G.
  
  
    dotnet .\LocalizationHelper.dll MyAwesomeCSharpProject\src MyAwesomeCSharpProject\src\Resources en hr rus pl

After you start you can start filling in the missing localization strings!

     Can you tell me how to translate this? (Press enter to skip)
     (hr, CountdownString)=

This will change CountdownString in all the resx files that need changing.!



Fuck ResX!
