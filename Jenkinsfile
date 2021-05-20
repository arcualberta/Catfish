
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
				bat 'copy ..\\_ConfigFiles\\catfish_appsettings.test.json Catfish.Test\\appsettings.test.json' //Restoring the appsettings.test.json file
			}
		}	
		stage('Build'){
		   	steps{
			  	bat "dotnet build Catfish\\Catfish.csproj --configuration Release"
				bat "cd Catfish & npm install & npm run build & npm run copy"
		   	}
		}
		stage('Build Vue'){
		   	steps{
				bat "cd Catfish & npm install & npm run build & npm run copy"
		   	}
		}
		stage('Publish'){
		     steps{
			     bat "dotnet publish Catfish\\Catfish.csproj -c Release --no-build"
		     }
		}		
		stage('Deploy'){
//		     when {
//				branch 'Catfish-2.0-calendar-el-block'
//             }
		    steps{
				script{
					if (env.BRANCH_NAME == 'Catfish-2.0-calendar-el-block'){
						bat "del Catfish\\appsettings.json"
						bat """ "C:\\Program Files\\IIS\\Microsoft Web Deploy V3\\msdeploy.exe"  -verb:sync -source:iisApp="${WORKSPACE}\\Catfish\\bin\\Release\\netcoreapp3.1\\publish" -dest:iisApp="catfish-dev.artsrn.ualberta.ca" -enableRule:AppOffline """   	
					}
					if (env.BRANCH_NAME == 'Catfish-2.0-Interface-Testing'){
						bat """ "C:\\Program Files\\IIS\\Microsoft Web Deploy V3\\msdeploy.exe"  -verb:sync -source:iisApp="${WORKSPACE}\\Catfish\\bin\\Release\\netcoreapp3.1\\publish" -dest:iisApp="catfish-test.artsrn.ualberta.ca" -enableRule:AppOffline """   
					}					
				}
		    }
//		    when {
//				branch 'Catfish-2.0-Interface-Testing'
//             }
//		    steps{
//				bat """ "C:\\Program Files\\IIS\\Microsoft Web Deploy V3\\msdeploy.exe"  -verb:sync -source:iisApp="${WORKSPACE}\\Catfish\\bin\\Release\\netcoreapp3.1\\publish" -dest:iisApp="catfish-test.artsrn.ualberta.ca" -enableRule:AppOffline """   
//				//bat """ "C:\\Program Files\\IIS\\Microsoft Web Deploy V3\\msdeploy.exe"  -verb:sync -source:contentPath="${WORKSPACE}\\Catfish\\bin\\Release\\netcoreapp3.1\\publish" -dest:contentPath="E:\\inetpub\\wwwroot2\\catfish-test.artsrn.ualberta.ca" -enableRule:AppOffline """   
//		    }
		}		
		stage('Test'){
		     when {
				branch 'Catfish-2.0-dev'
             }
		     steps{
			     echo 'Testing ...'
			     //bat "dotnet test Catfish.Test"
		     }
		}		
	}
 }
