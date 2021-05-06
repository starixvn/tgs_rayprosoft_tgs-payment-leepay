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
				sh 'dotnet build "Tgs.Payment.LeePay/Tgs.Payment.LeePay.csproj" -c $BuildConfiguration -t:ContainerBuild -p:DockerfileTag="$ImageName:v${BUILD_NUMBER}" --no-restore '
			}
		}

		stage('Push Docker Image') {
			steps {
				sh 'sudo docker push $ImageName:v${BUILD_NUMBER}'
			}
		}

		stage('Deploy to dev') {
			input {
				message 'Ready to go?'
				ok 'yes'
			}

			steps {
				configFileProvider([
					configFile(
						fileId: 'leepay.dev.json', 
						variable: 'leepaydev')]) {
					script {
						def json = readJSON file: "$leepaydev";

						env.TGS = json.tgs;
                        env.PaymentApiUrl = json.paymentApiUrl;
                        env.NatsUri = json.nats;

						withKubeConfig([credentialsId: 'k8s-kube-config', serverUrl: 'https://192.168.50.105:6443']) {
							sh '''
								helm template k8s/leepay \
									--set image.tag=v${BUILD_NUMBER} \
									--set namespace=dev \
									--set "connectionStrings.tgs=${TGS}" \
									--set nats=${NatsUri}
							'''
							sh '''
								helm template k8s/leepay \
									--set image.tag=v${BUILD_NUMBER} \
									--set namespace=dev \
									--set "connectionStrings.tgs=${TGS}" \
									--set nats=${NatsUri} | kubectl -n dev apply -f -
							'''
						}
					}
				}
			}
		}
	}
}
