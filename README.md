This is based on this YouTube tutorial by Teddy Smith
"ASP.NET Core Web API .NET 8 2024"
https://www.youtube.com/watch?v=qBTe6uHJS_Y&list=PL82C6-O4XrHfrGOCPmKmwTO7M0avXyQKc&ab_channel=TeddySmith


Angular frontend generate by using "ng new" command. 
The YouTube tutorial has a react one, I might add my own react frontend later.

<h1><u>development flow</u></h1>
Dev branch (local and dev environment) --> main  (QA environment) --> prod branch (production)

1. Make a copy of the dev branch for local development (e.g. HPT-local)
2. Always remember to pull the latest from dev branch and merge with your local branch.
3. After your changes is tested and working on your machine then push to the dev branch.
4. Coordinate with dev lead, PM, and QA manager for when you can push to main branch (QA). <b>Same goes for prodcution push.</b>