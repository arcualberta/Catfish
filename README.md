# Catfish
Catfish is an open-source, multilingual data repository solution developed based on the Portlan Commons Data Model (PCDM). Currently, it is implemented on top of the Pirahna CMS (.NET 4.6 version), thus Catfish can be used as a hybrid solution with CMS and data repository functionality. Catfish is developed by the Arts Resource Centre, University of Alberta.

## Prerequisites
Catfish development setup requres .NET Framework 4.6, Entity Framework 4.6, SQL Server (full or express version 2008 or later), and Apache Solr 7.5 or later. We also recommend Visual Studio 2017 or later as the IDE.

# Development Setup
## Solr Installation
Download and install Apache Solr: https://lucene.apache.org/solr/guide/7_0/installing-solr.html. 

The commands in the following steps assume you are in the folder where Solr is installed.

Start the Solr instance
```
bin\solr start
```
Create a Solr core for the project, where CoreName is the name of the solr core you want to create.
```
bin\solr create -c CoreName
```
Copy schema.xml and currency.xml in the SolrSetup folder of the git repository to the solr core config folder of the newly created Solr core.

In the solr core config folder, delete the file managed-schema.

Restart Solr by visiting the following URL on the browser. Please make sure you set the port to the correct solr port displayed on the commandline when you started Solr. Also, use the same Solr core name you selected for your application.
```
http://localhost:8983/solr/admin/cores?action=RELOAD&core=CoreName
```

### Update Solr Field Configuration
Update the definitions of multi-valued-string type solr fields if they should be search against for case insensitive words. If this is not done, they will only match case-sensitive full-string matchs.
```
	<field name="title_ss" type="text_general" indexed="true" stored="true" multiValued="true" required="false"/>
	<field name="content_ss" type="text_general" indexed="true" stored="true" multiValued="true" required="false"/>
```

## Source Code Setup
* Clone or download the source code from this GitHub repository. This solution contains the following projects:
   * Catfish - the primary web application
   * Catfish.Core - the library project which implement the core functionality of the Catfish repository
   * Catfish.DataHandler - a command-line tool developed for ingesting large batches of data to Catfish
   * Catfish.Tests - a test suite for Catfish
* Create an SQL database and a user for it for the project. The database user should have permission to create tables and edit contents in this database.
* Create configuration files
   * Copy to the `Web.example.config` file in the `Catfish` application folder and rename it as `Web.config`. 
   * Go to the `connectionStrings` element in `Web.config` and change its connection string named `piranha` to specify the MS SQL data base and the database user you create for the project.
   * You may comment other connection strings in this section of the Web.config file.
   * Go to the `appSettings` element and 
     - Update the `UploadRoot` value such that it will specify a target folder to store files uploaded by Catish.
     - Update the `LanguageCodes` value to specify the languages you want to use. Use `|` to separate language codes from each other.
   * Update the `SolrServer` value to specify the Solr core created for this catfish instance.
* Open the solution in Visual Studio, open Package Manager Console, select the "Defualt Project" to be Catfish.Core, and run the database migrations. 
```
PM> Update-Database
```

   
   
   
  


