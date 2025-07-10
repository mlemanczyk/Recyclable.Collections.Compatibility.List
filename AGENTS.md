All documentation, branch names, variable and method names, and comments must use American English. Pull request titles and commit messages must also be in English.
Do not create additional AGENTS.md files.
Do not use the `--no-build` parameter when running the application or tests, as it leads to missing libraries.
All tests should use xUnit with the FluentAssertions package.
Information about available classes and methods from the standard List and List<T> operations is documented in README.md. Additional compatibility methods are distributed across NuGet packages.
Before adding, modifying, or removing methods or classes, review the README files from related repositories (links will be added later).
Projects and tests must target .NET 8 by default and also support .NET 9, .NET 7, and .NET 6. Use the latest available C# language features when possible.
