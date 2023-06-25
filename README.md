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

![](https://i.kym-cdn.com/entries/icons/mobile/000/030/710/dd0.jpg)

Surely I'll get around to writing tests at some point... lol

### Job Ability Icons

Job ability icons are sourced from the official [job guides](https://na.finalfantasyxiv.com/jobguide/battle/).

Process:

1. Load a page for a job and View Page Source
2. Copy entire source into a sublime text file
3. Find all entries matching `tooltip=".+"><img src=".+" width` and copy the results into a new tab. Ensure regex support is on.
4. Find/Replace All, finding all `tooltip="(.+)"><img src="(.+)" width` with `$1\n$2\n`. Ensure regex support is on.
5. For each ability, copy the URI into the job data file.

More manual process for jobs with few abilities:

1. Right click an icon and Inspect
2. Expand the `div` to see the `img`, copy the address and paste it into the job data file.
