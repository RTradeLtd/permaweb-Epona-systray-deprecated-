# Epona
> Simplifying sharing and managing IPFS file 

Epona is a system try tool for managing [IPFS](https://github.com/ipfs/ipfs) on Windows. The goal is to make sharing files on IPFS as simple and intuitive as possible.

![](ipfssytemtray/IPFSSystemTray/Resources/AppIcon.png)

## Installation

Windows:

Run `Epona.exe` as administator to start Epona and setup the installation.

## Requirements

Windows 10 is required to make full use of Epona's functionalities (such as Cloud Storage Provider integration).

## Development setup

Microsoft Visual Studio for C# is required to open and build the project.

## Release History

* 1.5.2
    * Added additional checks for deleted shared files
    * Proper cleanup after forced shutdown
* 1.5.1
    * Fixed bug with handling invalid hashes
* 1.5.0
    * Added support for mounting IPFS locally
* 1.4.0
    * Added symlink for sharing folders
    * Added watcher for changes on shared files/folders
* 1.3.0
    * Added folder support for shares
* 1.2.0
    * Multiple files support
* 1.1.0
    * Fixed context menu setup for sharing and unsharing files
* 1.0.0
    * First release

## Contributing

1. Fork it (<https://github.com/Epona-app/Epona>)
2. Create your feature branch (`git checkout -b feature/myFeature`)
3. Commit your changes (`git commit -am 'Add some new feature'`)
4. Push to the branch (`git push origin feature/myFeature`)
5. Create a new Pull Request
