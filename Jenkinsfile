pipeline {
    agent any

    stages {

        stage('Clone') {
            steps {
                git 'https://github.com/sajalsrivastavacode/VotersListProject.git'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build'
            }
        }

        stage('Docker Build') {
            steps {
                bat 'docker build -t voterslistproject .'
            }
        }

        stage('Docker Run') {
            steps {
                bat 'docker stop voterapp'
                bat 'docker rm voterapp'
                bat 'docker run -d -p 8081:8080 --name voterapp voterslistproject'
            }
        }
    }
}