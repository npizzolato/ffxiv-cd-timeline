FFXIV Mitigation Timeline
=========================

This project is designed to allow you to customize a mitigation plan for encounters in Final Fantasy 14. 

This site is currently under active development and is not yet at an MVP stage. My initial goals are:

1. Get this to a working state for a single fight.
2. Clean up the presentation so it looks halfway decent.
2. Figure out a deployment story and get it live.
3. Add remaining fights for current content. 
4. Add additional features. 

## Development Environment

This website is built on asp.net Blazor. Follow [these instructions](https://dotnet.microsoft.com/en-us/learn/aspnet/blazor-tutorial/install) for installing the .net framework. There is decent built-in integration with [VS Code](https://code.visualstudio.com/) for an editor. 

### Building the Site

1. Run `dotnet build` in the project's directory.

### Running the Site Locally

1. Run `dotnet watch` in the project's directory. 
2. As you make changes, many changes can be hot-reloaded and automatically display. You may occassionally make "rude edits" and need to rebuild the app. Pay attention to the console window as you save edits. 

### Running Tests

lol

I'll get around to writing tests at some point.
