# PATRONKIOSK
To avoid single use devices, this overlay software allows you to lock down a computers functions to set software/websites whilst giving users a clean interface to select desired function

This project needs to cover several facets of IT, from the chosen hardware through to the desired endpoint software. It is important to remember that this does not replace any of your existing software. This project is just creating an interface for patrons to select between bespoke software solutions you already utilise - e.g. Self-Checkout, OPAC, Computer Reservation, etc.

We do not hope to provide hardware guidence on your system. The below recomendations are just based on our current implementation and your choice of hardware/brand should simply reflect the function that hardware is performing and not the specific brand or model.

# HARDWARE

Computer/PC - We make use of the same device across all fixed computers. From Staff PC's through public access devices and now Patron Kiosks. This means that we only need to have one device type in cold storage for hot swaps (issue replacements) and provides staff with one device to acclimatise and learn. For this, we are running HP G series all-in-one devices, bringing the computer and screen into one visually/aesthetically appealing package that minimises the cable usage and port access (for security reasons). (We sterotypically budget $1,650 for this device)

Thermal Printer - With so many patrons still reliant on paper, the most efficient printing method is retail style receipts to print patron information and chekcouts. For this we use EPSON series thermal printers connected via USB to the computer. (We sterotypically budget $500 for this device)

Barcode Scanner - Our patron cards still make use of barcodes and all assets are tagged with barcode for redundancies sake. With a transition to digital and our mobile app, many of our patrons are using digital cards meaning our readers need to be able to also read from screens. For this we make use of two type of barcode scanner based on the station - Opticon and Zebra - connected via USB to the computer. (We sterotypically budget $300 for this device)

RFID Pad - With all of our assets on RFID we make use of these pads to speed of transactions and simplify the end result for paterons. Two big factors in choosing our devices were the reading distance and the peripheral field. Our chosen device actually allows us to check out up to 20 books (at a distance of 40cm) and is shielded so that the device only reads assets place directly above the device. For this we make use of the FEIG Shielded Antenna and Reader connected via USB to the computer. (We sterotypically budget $650 for this device)

# SOFTWARE

Self-Checkout System - At present we are making use of FE's software at a cost of $2,000 up fornt per licence and $200 per annum per licence. This is a bespoke piece of software installed and configured on the computer, which we then use Windows 10 limiting to lock down access specifically to this software.

OPAC - We are presently using SirsiDynix Enterprise as our catalogue system. As this is web-based we use the Windows 10 limiting function to lock down to Google Chrome - for its full screen 'kiosk' mode - and then restrict website access to this sub-domain.

Computer Reservation - We are presently using MyReservation as our public PC booking system. As this is web-based we use the Windows 10 limiting function to lock down to Internet Explorer - for its full screen 'kiosk' mode and software dependancies - and then restrict website access to this sub-domain.

# GENERAL INFO

### PRE-REQUISITE SOFTWARE
•	Microsoft Visual C++ 2015-2019 Redistributable (x86) - 14.22.27821 (must be x86 not x64) - NECESSARY<br>
•	Auto Hotkey (optional)<br>
•	Deep freeze, image restoring software as it is a public facing device (optional)<br>
•	Software you wish to use under this overlay (e.g. self-checkout software) – this application has an embedded browser included using the cefsharp Chromium browser package<br>

### HARDWARE
•	Windows 10 PC – not yet tested with other Windows versions (7, 8.1, 11, etc)<br>
•	Anything needed for underlying software – e.g. self-check peripherals, etc.<br>
•	Keyboard and mouse are optional – on-screen keyboard is available<br>

### GROUP POLICY RECOMMENDATIONS
To lockdown the PC/profile even more (non-domain PCs can apply these locally):<br>
•	Disable control panel, settings, run<br>
•	Disable start menu options (logout, information displayed, etc)<br>
•	Disable touch controls like edge-swipe that can be used to show task bar over full-screen apps<br>

### WINDOWS PROFILE SETUP
•	Remove all desktop icons including recycle bin<br>
•	Remove all pinned start menu apps<br>
•	Hide taskbar buttons and options<br>
•	Clear anything easily accessible when profile is loading – most crucial things should be disabled using group policy and anything opened should be covered on startup of other apps, but this is to prevent anyone from opening 100 explorer windows or something<br>

### AUTO LOGIN
It is suggested that you setup the PC to automatically login to the profile you have setup to run the kiosk software. This can be done by editing/creating registry files. The exact method can be found with a quick search online and is slightly different with a domain profile compared to a local one.

### FILES
##### CONFIG FILE
The config file (config.xml) is in XML format to help to show which properties apply to each object.<br>

Please see example config file for how to use fields mentioned below.<br>

>themeColour: set the default of anything that doesn’t receive a colour – mostly just the idle timeout dialogue box<br>
>resourceFolder: the path of the folder that the images and resource files are stored in locally (can contain spaces safely)<br>
>mainBGImage: the file name of your background image<br>
>screensaverImgs: takes multiple <string> properties with the screensaver image file names<br>
>numOfButtons: the number of buttons on the main menu of the overlay<br>
>buttonWidth and buttonHeight: dimensions of the buttons in pixels – the app will attempt to size the buttons to this size, but will resize smaller if the buttons won’t fit on screen<br>
>mainRows: the number of rows the buttons will be placed on – this will attempt to place an even amount of buttons on each row where possible<br>
>mainPaddingTop Right Bottom and Left: the padding/margin of the screen on each side of the buttons – this will resize the buttons if set too high<br>
>idleTimeout: the amount of idle (no input) time in a function (button press destination – e.g. browser, self-check app, etc.) for the application to display the timeout message to then return to the main screen<br>
>timeoutTime: the amount of time that the timeout dialogue confirmation counts down from before actually timing out<br>
>timeoutText: the text to display in the timeout dialogue box<br>
>timeoutWidth and Height: the size of the timeout dialogue box<br>
>timeoutFontSize: size of the font on the dialogue box<br>
><br>
>appButtonsConfig: container for all button objects (can have multiple ButtonConfigs within it)<br>
>ButtonConfig: container for button data<br>
>backgroundImageName: the file name of the button image<br>
>buttonText: the text on the button – currently only one line of text in the middle of the button<br>
>fontSize: the font size of the button text<br>
>textColor: the colour of the text on the button<br>
>option: container for the function of the button<br>
>appProcName: the process name of the full screen application to switch to when the button is pressed, or set as embeddedBrowser to navigate to the embedded browser in within the application<br>
>arguments: just used for the URL to navigate to in the embedded browser for now – experimenting with other uses such as simulated clicks to focus windows or elements in an application, etc.<br>
>homeButtonConfig: container for the home button that takes users back to the main menu<br>
>backgroundImageName: file name of the image for the home button<br>
>buttonText: text for the home button<br>
>fontSize: font size for the home button text<br>
>buttonWidth and Height: dimensions of the home button<br>
>buttonPaddingVertical & horizontal: padding of the home button from screen edges relative to the corner it is placed in<br>
>buttonPosition: the corner to place the home button in (TOPRIGHT, TOPLEFT, BOTTOMRIGHT, BOTTOMLEFT)<br>


##### RESOURCE FOLDER
The resource folder just contains the images referenced in the config file.<br>
If syncing to a server (see in a below section) make sure to only keep the necessary files in the template folder and keep the file sizes small enough to not cause problems.


##### HOTKEY DISABLING
If you have a keyboard attached to the kiosk, or plan to allow input of any kind, this can introduce ways to close/bypass/open things for unintended use. To safeguard against this, I recommend a simple Auto Hotkey script (.ahk file) which allows you to toggle on and off the use of these keys if needed:

>F1 & A::<br>
>Toggle := !Toggle<br>
><br>
>If !Toggle<br><br>
>LAlt::return<br>
>LCtrl::return<br>
>RAlt::returnRCtrl::return<br>
>LWin::return<br>
>;etc. for any other keys to disabled<br>

Replace [F1 & A] with your choice of hotkey to toggle the use of the keys below it (as long as it doesn’t contain any of the keys you have disabled).

Auto Hotkey will need to be installed on each device you want it to run on and the above script placed in a Windows startup folder for it to run on login to the profile you’re using.

##### OPTIONAL METHOD TO SYNC TO A SERVER
If deploying a lot of kiosk devices, it can be hard to keep them consistent if you need/want to update them frequently.<br>
We have seen frequent updates with short term campaigns at branches and across the organization where we wanted extra buttons linking to campaign web pages and corresponding screen saver images. We have also needed to quickly change templates due to changes in hardware or location. This can easily be achieved by changing the template that the device is set to sync to.<br>
The simplest way to do this is using batch a batch (.bat) file in the startup folder – which runs at login. All you need is the script, a folder in a server share that the profile used on the kiosk has access to – to store the templates, and a text file to manage the ‘registered kiosks’ in the same share.<br>
The script below checks runs on login. It checks the share for the ‘registeredkiosks’ text file which returns the template to copy. It then robocopies the files from the template in the share to the local resource files on the device. It then starts the kiosk application with the template applied.<br>

>ECHO off<br>
><br>
>SET configpath=\\SERVER\SHARE\FOLDERPATH OF REGISTEDKIOSKS.TXT\<br>
>SET configfilename=registeredkiosks.txt<br>
>SET templatepath=\\SERVER\SHARE\FOLDERPATH OF TEMPLATE FOLDERS\<br>
><br>
>SET /A k=0<br>
>SET found[0]=zero<br>
>setlocal EnableDelayedExpansion<br>
>FOR /F "tokens=*" %%i IN (%configpath%%configfilename%) DO (<br>
>	ECHO %%i | FINDSTR /C:"%COMPUTERNAME%" >nul & IF NOT ERRORLEVEL 1 (<br>
>		setlocal EnableDelayedExpansion<br>
>		FOR /f "tokens=2 delims=:" %%a in ("%%i") DO (<br>
>			SET /A k+=1<br>
>   			SET found[!k!]=%%a<br>
>  		)<br>
>	)<br>
>)<br>
><br>
>SET template=%templatepath%!found[1]!<br>
><br>
>robocopy %template% "C:\FOLDER PATH OF RESOURCE FILES" /mir /R:0 /W:0<br>
><br>
>cd “C:\FOLDER PATH OF MULTIAPPKIOSK.EXE”<br>
>start MultiAppKiosk.exe<br>

Note: the folder paths at the top of the script and the 2 folder paths in the 2nd and 3rd last lines of the script.<br>
The format of the registeredkiosks text file is as follows:
>PCNAME1:TEMPLATEFOLDERNAME1<br>
>PCNAME2:TEMPLATEFOLDERNAME1<br>
>PCNAME3:TEMPLATEFOLDERNAME2<br>
>PCNAME4:TEMPLATEFOLDERNAME2<br>

e.g.<br>
>KIOSK1:Checkout<br>
>KIOSK2:Checkout<br>
>Catalogue1:OPAC-Only<br>
>Catalogue2:OPAC-Only<br>
>etc.<br>

You will need to create an entry each time you setup a new device.
