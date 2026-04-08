pipeline {
    agent any

    stages {
        stage('Build') {
            steps {
                echo 'Building the app...'
                sh 'echo build done'
            }
        }
        stage('Test') {
            steps {
                echo 'Running tests...'
                sh 'echo all tests passed'
            }
        }
        stage('Deploy') {
            steps {
                echo 'Deploying...'
                sh 'echo deployed successfully'
            }
        }
    }

    post {
        success {
            echo 'Pipeline completed successfully!'
        }
        failure {
            echo 'Something went wrong!'
        }
    }
}
