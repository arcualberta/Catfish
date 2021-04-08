
/*
 * Kamal Ranaweera
 * Reference: https://faun.pub/jenkins-ci-cd-to-deploy-an-asp-net-core-application-6145b5308bff
*/

pipeline{
    agent any
    
    environment {
        dotnet ='C:\\Program Files\\dotnet\\'
        }
		        
    triggers {
        pollSCM 'H * * * *'
    }
	
	stages{
		stage('Checkout'){
			steps{
				git url: 'https://github.com/arcualberta/Catfish.git', branch: 'Catfish-2.0-master'
			}
		}
		stage('Restore packages'){
			steps{
				bat "dotnet restore Catfish.sln"
			}
		}
		stage('Clean'){
			steps{
				bat "dotnet clean Catfish.sln"
			}
		}
		stage('Debug Build'){
		   steps{
			  bat "dotnet build Catfish.sln --configuration Debug"
		   }
		}
		stage('Publish'){
		   	steps{
				bat 'dotnet publish Catfish\\Catfish.csproj -c Release' //Publishing the code to the default folder
				bat 'del Catfish\\bin\\Release\\netcoreapp3.1\\appsettings.json' //Deleting the published appsettings.json file since we don't want to coppy it to the deployed site.
				
			}
		}		
	}
 }
