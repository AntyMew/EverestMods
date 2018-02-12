# EverestMods
Miscellaneous mods for Celeste using the [Everest](https://github.com/EverestAPI/Everest) mod API

## Building

* Install Everest
* Copy `Celeste.Mod.mm.dll` and the libraries in lib and lib-stripped to some directory
* Change `ReferencePath` in [Everest.targets](Everest.targets#L4) to the absolute path of your Everest libs
  * I use `$(HOME)/.local/lib/everest` as a shared directory. If you don't mind copying the libraries locally, I'd suggest using something like `$(SolutionDir)/lib`.
* Run `msbuild`

## Licensing

This project is licensed under the LGPL v3 License. See [LICENSE](LICENSE) for details.
