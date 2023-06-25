FFXIV Mitigation Timeline
=========================

This project is designed to allow you to customize a mitigation plan for encounters in Final Fantasy 14. 

The site is currently in an alpha state and deployed to https://ffxiv-cooldown-planner.azurewebsites.net/. 

## Additional Feature Ideas
- Support for p9s, p12s1, and p12s2
- Showing unmitigated damage, % mit, and damage given the applied mitigation
- Letting you hide abilities you aren't interested in
- Taking into account ability charges and other resources (aetherflow, oath gauge, etc.)

## Development Environment

This website is built on asp.net Blazor Server. Follow [these instructions](https://dotnet.microsoft.com/en-us/learn/aspnet/blazor-tutorial/install) for installing the .net framework. Main development is done through [Visual Studio 2022](https://visualstudio.microsoft.com/vs/), but there is decent built-in integration with [VS Code](https://code.visualstudio.com/) for an editor. 

### Building the Site

- In Visual Studio, right click the project and Build.
- Alternately, run `dotnet build` in the project's directory.

### Running the Site Locally

- In Visual Studio, click the "https" button near the time. 
- Alternately, (and I haven't tested this since swapping to Blazor Server), run `dotnet watch` in the project's directory.
	- As you make changes, many changes can be hot-reloaded and automatically display. You may occassionally make "rude edits" and need to rebuild the app. Pay attention to the console window as you save edits. 

### Running Tests

![](https://i.kym-cdn.com/entries/icons/mobile/000/030/710/dd0.jpg)

### Deployment

Deployment is done through Visual Studio publish profiles.

1. Right click the project and select Publish.
2. Follow the prompts to deploy using the `ffxiv-cooldown-planner Web Deploy.pubxml` file

## Design/Implementation

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
