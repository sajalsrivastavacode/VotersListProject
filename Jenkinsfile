pipeline {
    agent any

    environment {
        PATH = "/usr/local/bin:/usr/bin:/bin:${PATH}"
    }

    stages {

        stage('Build') {
            steps {
                sh '/usr/local/bin/dotnet build'
            }
        }

        stage('Docker Build') {
            steps {
                sh 'docker build -t voterslistproject .'
            }
        }

        stage('Docker Run') {
            steps {
                sh 'docker stop voterapp || true'
                sh 'docker rm voterapp || true'
                sh 'docker run -d -p 8081:8080 --name voterapp voterslistproject'
            }
        }
    }
}
