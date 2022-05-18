Authors: Haydn Thurman, Grange Simpson

10/17/2021
Created Spreadsheet GUI.
Added Content textbox.
Added cellName/cellValue labels.
Successfully implemented Set button to display current value on selected cell.
Added tool strip, but have not implemented yet.
Will work on aesthetics of GUI.

10/18/2021
Created File tool strip menu item
Implemented New and Close
Have not implemented open and save
Fixed GUI so when a value is changed it also displays on the spreadsheet cell.

10/19/2021
Succesfully implemented open and save buttons.
Working on implementing help button.
Fixed error that caues formula errors to stay as formula errors in contents.
Throws error message box for invalid formulas and circular dependencies.

10/20/21
Succesfully implemented open button to show either only sprd files or all files.
Save now saves into the current directory.
Added a help button in the file menu.
Fixed a circular dependency error when changing contents of a invalid formula.
Added check to prevent overwrites.
Added a feature that allows the user to change the color of the spreadsheet.

10/21/21
Fixed error of not having .gitignore which was affecting pushing and pulling between
partners.

10/22/21
Successfully added error message for dealing with situation where user is about to 
close the spreadsheet without saving. Used base code for showing message from online sources
since the red x close button is very particular.
If the user wishes to close without saving, spreadsheet will just close.
If the user wishes to save before closing, the spreadsheet will stay up then close 
immediately after being saved.

Spreadsheet Help:
Click the Top Left Corner "File" drop down menu to:
New- Create a new Spreadsheet.
Close- Close the current Spreadsheet.
Save- Save the current Spreadsheet.
Open- Open an existing Spreadsheet.

Click the "Colors" drop down menu to:
Change spreadsheet border color to any of
the desired colors in the menu.

Below the file menu, from left to right:
Cell Name
Box containing cell contents
Button to set cell contents and calculate value.
Value of the selected cell.

If closing without saving, continue button can sometimes
become stuck. Closing the warning will continue
operation and close spreadsheet without saving changes.