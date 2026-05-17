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
    }
}