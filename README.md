# CodeKata
The Application process a text file (.txt).
You can add the file with the following ways: 
-Drag and drop the file into the drop zone or click in it to select the file 
-Write the path to the file in the input component 
-Click in the "Browse.." button to navigate into the explorer and select the file. 
Each line in the input file will start with a command and there are two possible commands: 
-The first command is Driver, which will register a new Driver in the app. Example: Driver Dan
-The second command is Trip, which will record a trip attributed to a driver. The line will be space delimited with the following fields: the command (Trip), driver name, start time, stop time, miles driven. Times will be given in the format of hours: minutes. You'll use a 24-hour clock and will assume that drivers never drive past midnight (the start time will always be before the end time) and the trips that have a speed of less than 5 mph or greater than 100 mph are discarded. Example: Trip Dan 07:15 07:45 17.3 .
Once time that the file is selected you can upload, remove or see the details of the file in the buttons of the file preview window or in the file input. If you upload the file the application will display a table with the information of the drivers, total miles and hours driven, and the average speed, and if you click the button "Get Report" you´ll generate a report containing each driver with total miles driven and average speed. The output file is sorted by most miles driven to least.
