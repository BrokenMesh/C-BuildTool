<a name="readme-top"></a>

<div align="center">
<h3 align="center">C-BuildTool</h3>

  <p align="center">
    A very simple portable c buildtool.
    <br />
    <a href="https://github.com/BrokenMesh/C-BuildTool/issues">Report Bug</a>
    ·
    <a href="https://github.com/BrokenMesh/C-BuildTool/issues">Request Feature</a>
  </p>
</div>


<!-- GETTING STARTED -->
## Getting Started

### Build
1. Clone the repo
   ```sh
   git clone https://github.com/BrokenMesh/C-BuildTool.git
   ```
2. Open the Solution
   ```sh
   C-BuildTool.sln
   ```
3. Build Project

<!-- USAGE EXAMPLES -->
## Usage
Move the `C-BuildTool.exe` to the root folder of you project.

Open the terminal and run the `Setup` command to create the required folders and files.
 ```sh
  C-BuildTool.exe Setup
 ```
Now you should have these folders:
(your project)
├── bin                   # Location of the final binary
├── lib                   # Libraries and other dependencies
├── obj                   # Object files
├── res                   # Resources (content will be copied to `/bin`)
├── src                   # Source files
├── CBConfig.json         # Configuration of your project
└── C-BuildTool.exe

Bulld the project by running `Build`.
 ```sh
  C-BuildTool.exe Build
 ```

and clean the object folders with `Clean`.
 ```sh
  C-BuildTool.exe Clean
 ```
<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<!-- CONTACT -->
## Contact

Hicham El-Kord - elkordhicham@gmail.com

Project Link: [https://github.com/BrokenMesh/C-BuildTool](https://github.com/BrokenMesh/C-BuildTool)

<p align="right">(<a href="#readme-top">back to top</a>)</p>
