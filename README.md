# Catfish
Catfish is an open-source data repository solution developed based on the Portlan Commons Data Model (PCDM). Currently, it is implemented on top of the Pirahna CMS (.NET 4.6 version), thus Catfish can be used as a hybrid solution with CMS and data repository functionality. Catfish is developed by the Arts Resource Centre, University of Alberta.

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

## Source Code Setup
* Clone or download the source code from this GitHub repository. This solution contains the following projects:
   ** Catfish - the primary web application
   ** Catfish.Core - the library project which implement the core functionality of the Catfish repository
   ** Catfish.DataHandler - a command-line tool developed for ingesting large batches of data to Catfish
   ** Catfish.Tests - a test suite for Catfish
* 



