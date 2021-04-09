
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
			  	bat "dotnet build Catfish.sln --configuration Release"
		   	}
		}
		stage('Publish'){
		     	steps{
			     bat "dotnet publish Catfish\\Catfish.csproj "
		     	}
		}		
		/* stage('Stop Test Site'){
		     	steps{
			     //bat 'C:\\Windows\\System32\\inetsrv\\appcmd recycle apppool catfish-test.artsrn.ualberta.ca'
			     //bat 'C:\\Windows\\System32\\inetsrv\\appcmd stop site catfish-test.artsrn.ualberta.ca' //Stopping the catfish-test site
		     	}
		} */		
		stage('Deploy'){
		    	 steps{
				//bat 'xcopy Catfish\\bin\\Release\\netcoreapp3.1 E:\\inetpub\\wwwroot2\\catfish-test.artsrn.ualberta.ca\\ /Q /Y /S' //copy all published files
				bat '"C:\\Program Files\\IIS\\Microsoft Web Deploy V3\\msdeploy.exe" -source:contentPath="Catfish\\bin\\Release\\netcoreapp3.1" -dest:contentPath="E:\\inetpub\\wwwroot2\\catfish-test.artsrn.ualberta.ca"' //copy all published files
		     }
		}		
		stage('Start Test Site'){
		     	steps{
				bat 'C:\\Windows\\System32\\inetsrv\\appcmd start site catfish-test.artsrn.ualberta.ca' //Starting the catfish-test site
		     	}
		}		
	}
 }
