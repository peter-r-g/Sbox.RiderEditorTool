# Rider Editor Support
The `CodeEditor.Rider.cs` file adds Rider as a supported code editor within S&box. This means you can open the S&box solution and code files from within S&box with no extra effort!

## Media
Showcase - https://youtu.be/kgqC8B6rpoo

## Installation
### 1. Installing `CodeEditor.Rider.cs`
Due to tool addons not referencing the base tools ([issue here](https://github.com/Facepunch/sbox-issues/issues/2047)), CodeEditor.Rider.cs will need to be added to the base tools addon manually. The recommended location is `tools/code/CodeEditors/CodeEditor.Rider.cs` although it can technically be put anywhere in the `tools/code` directory.

### 2. Allow S&box to detect Rider as a default editor (Optional)
You can have Rider be detected as a default if you modify `tools/code/CodeEditors/CodeEditor.cs` at the `GetDefault` method. At the end you can add the following code:

```cs
var rider = new CodeEditors.Rider();
if ( rider.IsInstalled() )
    return typeof( CodeEditors.Rider ).Name;
```

Note when doing this optional step, if in the future this file gets changed you will need to re-do this.
