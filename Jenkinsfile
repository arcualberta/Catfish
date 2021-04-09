
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
		stage('Copy Config Files'){
		   	steps{
				bat 'copy ..\\_ConfigFiles\\catfish_appsettings.json Catfish\\appsettings.json' //Restoring the appsettings.json file
			}
		}		
		stage('Build'){
		   	steps{
			  	bat "dotnet build Catfish\\Catfish.csproj --configuration Release"
		   	}
		}
		stage('Publish'){
		     	steps{
			     bat "dotnet publish Catfish\\Catfish.csproj -c Release --no-build"
		     	}
		}		
		stage('Deploy'){
		    	 steps{
				bat """ "C:\\Program Files\\IIS\\Microsoft Web Deploy V3\\msdeploy.exe"  -verb:sync -source:iisApp="${WORKSPACE}\\Catfish\\bin\\Release\\netcoreapp3.1\\publish" -dest:iisApp="catfish-test.artsrn.ualberta.ca" -enableRule:AppOffline """   
				//bat """ "C:\\Program Files\\IIS\\Microsoft Web Deploy V3\\msdeploy.exe"  -verb:sync -source:contentPath="${WORKSPACE}\\Catfish\\bin\\Release\\netcoreapp3.1\\publish" -dest:contentPath="E:\\inetpub\\wwwroot2\\catfish-test.artsrn.ualberta.ca" -enableRule:AppOffline """   
		     }
		}		
		stage('Load Homepage'){
		     	steps{
				def response = httpRequest "https://catfish-test.artsrn.ualberta.ca:
				println("Status: "+response.status)
		     	}
		}		
	}
 }
