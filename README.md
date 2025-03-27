
# Datacute.LightweightTracing

A lightweight tracing library for .NET.

The library was created in order to provide a simple way to trace the behaviour of Incremental Source Generators.

## Installation

You can install the `Datacute.LightweightTracing` package via NuGet Package Manager or the .NET CLI.

### Using the .NET CLI

Run the following command in your terminal:

```bash
dotnet add package Datacute.LightweightTracing --version 1.0.0
```

### Using the NuGet Package Manager in Visual Studio

1. Open your project in Visual Studio.
2. Right-click on your project in the **Solution Explorer** and select **Manage NuGet Packages**.
3. Search for `Datacute.LightweightTracing` in the **Browse** tab.
4. Select the package and click **Install**.

## Usage

Once installed, you can start using the library in your .NET project.

### Adding trace events

Here is how the library can be used with an Incremental Source Generator:

```csharp
using Datacute.LightweightTracing;

...

public void Initialize(IncrementalGeneratorInitializationContext context)
{
    LightweightTrace.Add(TrackingNames.Generator_Initialize);
    ...
}

private void Action(SourceProductionContext sourceProductionContext, ...
{    
    LightweightTrace.Add(TrackingNames.Generator_Action);
    ...
}            
```
The `TrackingNames` constants are not included in the library (see below for an example).
The `Add` method takes an `int` as the event id, and you can use any integer value you like.

For an incremental source generator, you could include other calls to `Add` in the various value providers' `Select` methods.

### Getting the trace

Call `GetTrace` to write the trace details to a string builder. A dictionary of event id names can be passed.

For example, when generating your source file, you could end the file like so:

```csharp
private void AppendDiagnosticLogs(StringBuilder sb)
{
    sb.AppendLine();
    sb.AppendLine("/* Diagnostic Log");

    LightweightTrace.GetTrace(sb, TrackingNames.TracingNames);

    sb.AppendLine("*/");
}
```

Here's the `TrackingNames` class that I've used, with values relevant to incremental source generators:

```csharp
public static class TrackingNames
{
    public const int Generator_Initialize = 0;
    public const int AttributeContext_Transform = 1;
    public const int AnalyzerConfigOptionsDescription_Select = 2;
    public const int CompilationDescription_Select = 3;
    public const int ParseOptionsDescription_Select = 4;
    public const int AdditionalTextDescription_Select = 5;
    public const int MetadataReferenceDescription_Select = 6;
    public const int Generator_Action = 7;

    public static readonly Dictionary<int, string> TracingNames = new()
    {
        { Generator_Initialize, nameof(Generator_Initialize) },
        { AttributeContext_Transform, nameof(AttributeContext_Transform) },
        { AnalyzerConfigOptionsDescription_Select, nameof(AnalyzerConfigOptionsDescription_Select) },
        { CompilationDescription_Select, nameof(CompilationDescription_Select) },
        { ParseOptionsDescription_Select, nameof(ParseOptionsDescription_Select) },
        { AdditionalTextDescription_Select, nameof(AdditionalTextDescription_Select) },
        { MetadataReferenceDescription_Select, nameof(MetadataReferenceDescription_Select) },
        { Generator_Action, nameof(Generator_Action) },
    };
}
```

An example of the generated log is:

```csharp
/* Diagnostic Log
2025-03-27T08:49:59.9743437Z [000] Generator_Initialize
2025-03-27T08:50:00.0292375Z [001] AttributeContext_Transform
2025-03-27T08:50:00.0343270Z [001] AttributeContext_Transform
2025-03-27T08:50:00.0345618Z [001] AttributeContext_Transform
... many more lines not shown ...
2025-03-27T08:50:00.0383238Z [001] AttributeContext_Transform
2025-03-27T08:50:00.0411632Z [001] AttributeContext_Transform
2025-03-27T08:50:00.0486961Z [002] AnalyzerConfigOptionsDescription_Select
2025-03-27T08:50:00.0666194Z [003] CompilationDescription_Select
2025-03-27T08:50:00.0887126Z [004] ParseOptionsDescription_Select
2025-03-27T08:50:00.1105587Z [005] AdditionalTextDescription_Select
2025-03-27T08:50:00.1128166Z [005] AdditionalTextDescription_Select
2025-03-27T08:50:00.1440210Z [006] MetadataReferenceDescription_Select
2025-03-27T08:50:00.1455885Z [006] MetadataReferenceDescription_Select
2025-03-27T08:50:00.1455975Z [006] MetadataReferenceDescription_Select
2025-03-27T08:50:00.1455982Z [006] MetadataReferenceDescription_Select
... many more lines not shown ...
2025-03-27T08:50:00.1456775Z [006] MetadataReferenceDescription_Select
2025-03-27T08:50:00.1456784Z [006] MetadataReferenceDescription_Select
2025-03-27T08:50:00.1648541Z [007] Generator_Action
2025-03-27T08:50:00.1684447Z [007] Generator_Action
2025-03-27T08:50:00.1685249Z [007] Generator_Action
2025-03-27T08:50:00.1685576Z [007] Generator_Action
... many more lines not shown ...
2025-03-27T08:50:00.1716251Z [007] Generator_Action
2025-03-27T08:50:00.1717164Z [007] Generator_Action
*/
```

## Changing the behaviour

### Disabling/Enabling and Capacity

Tracing is enabled by default, but there are methods provided to control it,
and the capacity of the circular buffer can be set when enabling tracing.
(Repeated enabling with the same capacity will leave the buffer intact.)

```csharp
LightweightTrace.EnableTracing();
LightweightTrace.EnableTracing(100); // ensures capacity, the default is 1024 events
LightweightTrace.Clear();
LightweightTrace.DisableTracing();~~~~
```

## The examples don't work!

This package doesn't solve the problems of adding dependencies to the source generators.

The good news is that the code to implement this lightweight tracing library is very small,
so feel free to just go ahead and copy it (or parts of it) into your project or solution.

## License

This project is licensed under the [MIT License](LICENSE).
