This application is to validate zip files uploaded by the user. 

## Table of Contents

- [Introduction](#introduction)
- [Zip File Validations](#file-validations)
- [Prerequisites](#prerequisites)
- [To get started](#to-get-started)
  - [Clone the github project](#clone-the-github-project)
  - [Open the project](#open-the-project)
  - [Load node modules](#load-node-modules)
  - [Run the dotnet application](#run-the-dotnet-application)
- [How to use this application](#how-to-use-this-application)
- [Assumptions](#assumptions)
- [Conclusion](#conclusion)

## Introduction

This application created with .NET 6 API and React frontend. It allow users to validate, save and delete zip files as required. 

## File Validations

zip file uploaded by the user must adhere to the below structure,<br>
`RootFolder`<br>
* `dlls`<br>
Must contain RootFolder.dll file. Can have other files, other filetypes besides .dll are allowed<br>
* `images`<br>
Must contain at least 1 image of filetype .jpg or .png. No other filetypes allowed<br>
* `languages`<br>
Must contain RootFolder_en.xml file. Folder can have only .xml files, but file name must follow the RootFolder_xx.xml naming convention 
where xx is 2 letter language code.	

If upload and validation was successful. user can save the zip file.

And if required, user can delete zip files from the saved location.

## Prerequisites

* Download .NET 6 SDK <br>
https://dotnet.microsoft.com/en-us/download
* Download node.js <br>
https://nodejs.org/en/ <br>
I've downloaded the latest node.js version and during the installtion it will install the npm to your machine.
* Download git <br>
https://git-scm.com/downloads

## To get started

In the project directory, you can run:

### `Clone the github project`

Navigate to your desired folder and run the below command using a terminal.

```
git clone https://github.com/anjalinimz/file-validator-app.git
```
### `Open the project`

You can use any compatible code editing tool to open this project. If you are using Visual Studio Code, navigate to the project root location and execute the below command using a terminal,

```
code .
```
### `Load node modules`

To load node modules navigate to the 'ClientApp' folder inside this project and execute the below command,

```
npm install
```
This command will create a folder named 'node_modules' inside the 'ClientApp' and install all required frontend dependencies to the project.

### `Run the dotnet application`

As all set to run the project, navigate back to thr project root usinf a terminal and execute below command,

```
dotnet run
```

## How to use this application

`Step 1` <br> After executing dotnet run command, if successful, you can access the application using a preferred web browser.

`Step 2` <br> Choose the zip file from your computer or drag and drop the file to the file uploader.

then this application will show a file tree of the uploaded zip to the user.

`Step 3` <br> Click 'Validate' button to validate the zip file. It must adhere to the valid folder structure already explained in the sub-section 'File 'Validation'.

If some of this structure is missing or is incorrect, this application will display relevant validation errors below the zip file tree, so the user can fix their zip.

`Step 4` <br> If the validation was successful, application will enable the 'Save' button. So that, user can store valid zip files inside the 'zips' folder in project directory.

To `delete` zip files stored in 'zips' folder, use API endpoind implemented in this application.

## Assumptions

1. I assumed, if user saved the zip file with the same name multiple times, the old file should be replaced by the new one.

2. I assumed, if the root folder does not include all three sub folders; dlls, images and languages, the application shouldn't display the file tree but should display the relevant errors.

## Conclusion

This application implemented by Anjali Wijesinghe as the soltion for the test task provided by Derivco. It is a scalable solution because it uses solid principles in software engineering. The UI/UX can be improved using more user friendly designs.  

Open for your feedbacks and questions: <br>
anjalinimz@gmail.com

github URL: <br>
https://github.com/anjalinimz 
