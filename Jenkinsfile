pipeline {
    agent any
	
	environment {
		gitlab_user_pwd=credentials('gitlab-user-pwd')
        BuildConfiguration="Debug"
		ImageName="registry.gitlab.com/rayprosoft/turnkey-gaming-system/leepay"
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

		stage('Build Docker Image') {
			steps {
				sh 'dotnet build "Tgs.Payment.LeePay/Tgs.Payment.LeePay.csproj" -c $BuildConfiguration -t:ContainerBuild -p:DockerfileTag="$ImageName:v${currentBuild.number}" --no-restore '
			}
		}

		stage('Push Docker Image') {
			steps {
				sh 'sudo docker push $ImageName:v${currentBuild.number}'
			}
		}
	}
}
