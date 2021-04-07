
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
				bat "dotnet restore Catfish\\Catfish.csproj"
			}
		}
		stage('Clean'){
			steps{
				bat "dotnet clean Catfish\\Catfish.csproj"
			}
		}
		stage('Debug Build'){
		   steps{
			  bat "dotnet build Catfish\\Catfish.csproj --configuration Debug"
		   }
		}		
	}
 }