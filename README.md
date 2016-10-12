# dotnet-script

Run C# scripts from the .NET CLI.

[![Nuget](http://img.shields.io/nuget/v/Dotnet.Script.svg?maxAge=3600)](https://www.nuget.org/packages/Dotnet.Script/)

## Prerequisites

> What do I need to install? 

Nothing - everything is self contained from the `project.json` level. Just make sure you have .NET Core installed and `dotnet` available in your PATH.

## Usage

1> Create a `project.json` file with your dependencies and reference `Dotnet.Script` as a `tool`:

```
{
  "dependencies": {
    "Automapper": "5.1.1",
    "Newtonsoft.Json": "9.0.1"
  },

  "frameworks": {
    "netcoreapp1.0": {
    }
  },
  "tools": {
    "Dotnet.Script": {
      "version": "0.2.0-beta",
      "imports": [
        "portable-net45+win8",
        "dnxcore50"
      ]
    }
  }
}
```

In the above case we will pull in `Automapper` and `Newtonsoft.Json` from nuget into our script.

2> Run `dotnet restore`

3> Now, create a C# script beside the `project.json`. You can use any types from the packages you listed in your dependencies. You can also use anything that is part of [Microsoft.NETCore.App](https://www.nuget.org/packages/Microsoft.NETCore.App/). Your script will essentially be a `netcoreapp1.0` app.

For example:

```csharp
using Newtonsoft.Json;
using AutoMapper;

Console.WriteLine("hello!");

var test = new { hi = "i'm json!" };
Console.WriteLine(JsonConvert.SerializeObject(test));

Console.WriteLine(typeof(MapperConfiguration));
```

4> You can now execute your script using `dotnet script foo.csx`. 

> CSX script could also be located elsewhere and referenced by absolute path - what's important is that the `project.json` with its dependencies is located next to the script file, and that restore was run beforehand.

This should produce the following output:

```shell
λ dotnet script foo.csx
hello!
{"hi":"i'm json!"}
AutoMapper.MapperConfiguration
```

## Debugging

`dotnet-script` supports debugging scripts. To debug a script using Visual Studio Code, create a folder `.vscode` next to your script and put the following `launch.json` file inside:

```
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": ".NET Script Debug",
            "type": "coreclr",
            "request": "launch",
            "program": "<path-to>\\dotnet-script.dll",
            "args": ["${workspaceRoot}\\<name of your script>.csx","-d"],
            "cwd": "${workspaceRoot}",
            "externalConsole": false,
            "stopAtEntry": false,
            "internalConsoleOptions": "openOnSessionStart"
        },
        {
            "name": ".NET Core Attach",
            "type": "coreclr",
            "request": "attach",
            "processId": "${command.pickProcess}"
        }
    ]
}
```

You can now set breakpoints inside your CSX file and launch the debugger using F5.

![](http://i.imgur.com/YzBkVil.png)

## Advanced usage

### Referencing local script from a script

You can also reference a script from a script - this is achieved via the `#load` directive.

Imagine having the following 2 CSX files side by side - `bar.csx` and `foo.csx`:

```csharp
Console.WriteLine("Hello from bar.csx");
```

```csharp
#load "bar.csx"
Console.WriteLine("Hello from foo.csx");
```

Running `dotnet script foo.csx` will produce:

```shell
Hello from bar.csx
Hello from foo.csx
```

### Referencing an HTTP-based script from a script

Even better, `Dotnet.Script` supports loading CSX references over HTTP too. You could now modify the `foo.csx` accordingly:

```csharp
#load "https://gist.githubusercontent.com/filipw/9a79bb00e4905dfb1f48757a3ff12314/raw/adbfe5fade49c1b35e871c49491e17e6675dd43c/foo.csx"
#load "bar.csx"

Console.WriteLine("Hello from foo.csx");
```

In this case, the first dependency is loaded as `string` and parsed from an HTTP source - in this case a [gist](https://gist.githubusercontent.com/filipw/9a79bb00e4905dfb1f48757a3ff12314/raw/adbfe5fade49c1b35e871c49491e17e6675dd43c/foo.csx) I set up beforehand.

Running `dotnet script foo.csx` now, will produce:

```shell
Hello from a gist
Hello from bar.csx
Hello from foo.csx
```

### Passing arguments to scripts

You can pass arguments to the script the following way:

```
dotnet script foo.csx -a arg1 -a arg2 -a arg3
```

Then you can access the arguments in the script context using a global `ScriptArgs` collection:

```
foreach (var arg in ScriptArgs)
{
    Console.WriteLine(arg);
}
```

## Issues and problems

![](http://lh6.ggpht.com/-z_BeRqTrtJE/T2sLYAo-WmI/AAAAAAAAAck/0Co6XilSmNU/WorksOnMyMachine_thumb%25255B4%25255D.png?imgmax=800)

![](http://i110.photobucket.com/albums/n86/MCRfreek92/i-have-no-idea-what-im-doing-dog.jpg)

Due to [this .NET CLI bug](https://github.com/dotnet/cli/issues/4198) in order to debug the cloned solution, comment out the `buildOptions > outputName` property in `project.json`.

## Credits

Special thanks to [Bernhard Richter](https://twitter.com/bernhardrichter?lang=en) for his help with .NET Core debugging.

## License

[MIT](https://github.com/filipw/dotnet-script/blob/master/LICENSE)
