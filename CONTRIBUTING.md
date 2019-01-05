[README](./README.md)

[Code of Conduct](./CODE_OF_CONDUCT.md)

Hi there! Thanks for taking the time to contribute. Just to be clear, this repo
is meant to host *my* solutions to Code Jam problems. Thus, I won't accept pull
requests for solutions from others, even if they're better. However, I'll
consider bug fixes, improved docs, and even optimizations that don't change the
basic idea of a solution. I'll also consider changes to the CLI app that don't
change or add to the solution algorithms.

# Creating good issues

Report issues on GitHub. The guidelines in this section are based on [Atom's
contributor
guidelines.](https://github.com/atom/atom/blob/master/CONTRIBUTING.md).

## Bugs

A good bug report has:

* A clear, descriptive title
* Exact steps to reproduce the issue
  * If you're not sure, describe what you were doing before the issue appeared
* Examples demonstrating the steps
* What happened, what you *expected* to happen, and why you expected it
* Screenshots/GIFs

## Enhancements

You can also suggest an enhancement as an issue. Make sure to include:

* A clear, descriptive title
* A detailed description of the enhancement
* Examples to demonstrate the proposal
* The current behavior, the behavior you want, and why
* Screenshots/GIFs
* Why the enhancement would be useful

# Development

In the `CodeJam` directory, there is a Visual Studio solution.

The `CodeJam` project contains code for the CLI and the Code Jam solutions. It
can be built for both Debug and Release targets.

The `CodeJam.Tests` project contains tests for the Code Jam solutions and the
CLI. The tests can be ran using Visual Studio's built-in test runner. The
`CodeJam` project must be built for the Debug target before all the tests will
pass.

## Creating a demo GIF to put in the README

Ensure that your Debug build of the `CodeJam` project is up-to-date.

Make sure that [Terminalizer](https://github.com/faressoft/terminalizer) is
installed.

```batchfile
npm install -g terminalizer
```

In the root of the repository, run

```batchfile
terminalizer record demo-gif-recording --config config\demo-gif-config
```

Terminalizer starts up PowerShell. Next, type the following lines:

```powershell
.\CodeJam --problem coin-jam
1
6 3
.\CodeJam
.\CodeJam -v
exit
```

Now, Terminalizer's stored a recording of your PowerShell session in
`demo-gif-recording.yml`. You can check out this recording by running
`terminalizer play demo-gif-recording`.

Next, render the recording into a GIF.

```batchfile
terminalizer render demo-gif-recording --output demo.gif
```

## Release

Releases should be made using GitHub's Release feature. Release tags should
start with a `v` followed by a version number `major.minor.patch`. Use
[semantic versioning](https://semver.org/spec/v2.0.0.html).

A ZIP file with the following should be shipped with each release:

* A Release build of the `CodeJam` project (`CodeJam.exe`)
* Other necessary DLLs, like `FSharp.Core.dll` and `System.ValueTuple.dll`
* A file with licenses for the included dependencies (`LICENSES.md`)

In the release notes, there should instructions to install the [.NET 4
redistributable](https://www.microsoft.com/en-us/download/details.aspx?id=17718).

---

**On behavior:** Be nice to each other, be willing to accept critique.
