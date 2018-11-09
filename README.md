# PatronKiosk
To avoid single use devices, this overlay software allows you to lock down a computers functions to set software/websites whilst giving users a clean interface to select desired function

This project needs to cover several facets of IT, from the chosen hardware through to the desired endpoint software. It is important to remember that this does not replace any of your existing software. This project is just creating an interface for patrons to select between bespoke software solutions you already utilise - e.g. Self-Checkout, OPAC, Computer Reservation, etc.

We do not hope to provide hardware guidence on your system. The below recomendations are just based on our current implementation and your choice of hardware/brand should simply reflect the function that hardware is performing and not the specific brand or model.

# Hardware

Computer/PC - We make use of the same device across all fixed computers. From Staff PC's through public access devices and now Patron Kiosks. This means that we only need to have one device type in cold storage for hot swaps (issue replacements) and provides staff with one device to acclimatise and learn. For this, we are running HP G series all-in-one devices, bringing the computer and screen into one visually/aesthetically appealing package that minimises the cable usage and port access (for security reasons). (We sterotypically budget $1,650 for this device)

Thermal Printer - With so many patrons still reliant on paper, the most efficient printing method is retail style receipts to print patron information and chekcouts. For this we use EPSON series thermal printers connected via USB to the computer. (We sterotypically budget $500 for this device)

Barcode Scanner - Our patron cards still make use of barcodes and all assets are tagged with barcode for redundancies sake. With a transition to digital and our mobile app, many of our patrons are using digital cards meaning our readers need to be able to also read from screens. For this we make use of two type of barcode scanner based on the station - Opticon and Zebra - connected via USB to the computer. (We sterotypically budget $300 for this device)

RFID Pad - With all of our assets on RFID we make use of these pads to speed of transactions and simplify the end result for paterons. Two big factors in choosing our devices were the reading distance and the peripheral field. Our chosen device actually allows us to check out up to 20 books (at a distance of 40cm) and is shielded so that the device only reads assets place directly above the device. For this we make use of the FEIG Shielded Antenna and Reader connected via USB to the computer. (We sterotypically budget $650 for this device)

# Software

Self-Checkout System - At present we are making use of FE's software at a cost of $2,000 up fornt per licence and $200 per annum per licence. This is a bespoke piece of software installed and configured on the computer, which we then use Windows 10 limiting to lock down access specifically to this software.

OPAC - We are presently using SirsiDynix Enterprise as our catalogue system. As this is web-based we use the Windows 10 limiting function to lock down to Mozilla Firefox and then restrict website access to this sub-domain.

Computer Reservation - We are presently using MyReservation as our public PC booking system. As this is web-based we use the Windows 10 limiting function to lock down to Mozilla Firefox and then restrict website access to this sub-domain.
