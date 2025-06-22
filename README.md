Documentation of Ragnarok

Flow of the program in order.
Auto Patcher - check the website etc for updates
Client Updater Form - get the server example supported_servers.json 
LocalServerManager - get the address inside the supported_servers.json
Client -  To scan each addredd saved in to supported_servers.json
Container - Open when done initiating
AHK form - Where we can work with the checkboxes trigger for the hotkey and mouse spammers.
ProfileForm - Is to create a new profile: This is where we integrate our database for syncing.

NOTE: Class ATKDefMode.cs is for attacking configuration

Updates:
May 01, 2025 (Done)
Progress: Creation of AutoBuffForm intended for Stuff;
Next: SkillAutoBuffForm intended for skills automation;

June 01, 2025
- Integration of database;
- Registering of user using google sign in;
- Refactor the code and written constant values (google constants).

June 07, 2025
-Successfully figured out how to serialize and deserialize data from .Json and store it into database.
-Data retrieval success.
-Cleaning of source code.
-Transitioned into one form (frm_Main).

June 22, 2025
-Successfully created the new Macro Switch form. This is all database integrated;
-Removed the secret keys and secret appsettings.json for privacy purposes.
-Removed the additional unused form;
-Created a new model named Macro Switch instead of just Macro;
-Finished the attack defend functionality;
-Finished all the function needed.
