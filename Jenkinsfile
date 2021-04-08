
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
				//git url: 'https://github.com/arcualberta/Catfish.git', branch: 'Catfish-2.0-master'
				git branch: 'Catfish-2.0-master'
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
		stage('Copy Config Files'){
		   	steps{
				bat 'copy ..\\_ConfigFiles\\catfish_appsettings.json Catfish\\appsettings.json /Q/Y' //Restoring the appsettings.json file
			}
		}		
		stage('Testing'){
		   	steps{
				//bat 'cp ..\\_ConfigFiles\\catfish_appsettings.json Catfish\\appsettings.json' //Restoring the appsettings.json file
				bat 'cd Catfish && dotnet run' //Publishing the code to the default folder
			}
		}		
	}
 }
