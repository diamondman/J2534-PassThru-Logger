When building the installer, please put the relevant VC_redist.x86.exe file and .net
framework runtime installer into this folder.
Make sure that the uninstall registry key is correct in the panda_install.nsi file.

Here is a list of the VC runtime downloads: https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads
An list of the registry keys has been maintained here: https://stackoverflow.com/a/34209692/627525

.net 4.0 is used because that is the last version that supports XP.
The .net 4.0 web installer can be downloaded here: https://www.microsoft.com/en-us/download/confirmation.aspx?id=17851

Copy runtimeinfo.nsh.sample to runtimeinfo.nsh and edit it for your version of Visual Studio.