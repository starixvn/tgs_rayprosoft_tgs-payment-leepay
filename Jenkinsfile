pipeline {
    agent any
	
	environment {
		gitlab_user_pwd=credentials('gitlab-user-pwd')
        BuildConfiguration="Debug"
	}

    stages {
        stage('Build') {
            steps {
                sh 'dotnet build -c $BuildConfiguration'
            }
        }
		
		stage('Push packages') {
			steps {
				sh 'dotnet pack -c $BuildConfiguration "Tgs.Payment.LeePay.Client/Tgs.Payment.LeePay.Client.csproj" --no-build'
				sh 'dotnet nuget push "Tgs.Payment.LeePay.Client/bin/$BuildConfiguration/*.nupkg" --source "raypro" --skip-duplicate'
			}
		}
	}
}
